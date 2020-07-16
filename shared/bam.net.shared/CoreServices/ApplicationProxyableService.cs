using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Bam.Net.Server;
using Bam.Net.Data.Repositories;
using Bam.Net.CoreServices.Auth;
using Bam.Net.CoreServices.ApplicationRegistration.Data;
using Bam.Net.ServiceProxy;
using Bam.Net.CoreServices.ApplicationRegistration.Data.Dao.Repository;
using Bam.Net.Services;
using Bam.Net.Web;

namespace Bam.Net.CoreServices
{
    /// <summary>
    /// Base class for a proxyable service that provides functionality for 
    /// a specific named distributed application.
    /// </summary>
    /// <seealso cref="Bam.Net.CoreServices.ProxyableService" />
    public abstract class ApplicationProxyableService: ProxyableService
    {
        public ApplicationProxyableService() { }
        public ApplicationProxyableService(DaoRepository repository, AppConf appConf) 
            : base(repository, appConf)
        { }

        public ApplicationProxyableService(IRepository genericRepo, DaoRepository daoRepo, AppConf appConf) 
            : base(genericRepo, daoRepo, appConf)
        { }
        
        public ApplicationRegistrationRepository ApplicationRegistrationRepository { get; set; } // should get set by deriving classes
        
        public IApplicationNameProvider ApplicationNameProvider { get; set; }

        /// <summary>
        /// Gets the name of the server application.  This is the logical name provided by ApplicationNameProvider.
        /// </summary>
        /// <value>
        /// The name of the server application.
        /// </value>
        public string ServerApplicationName
        {
            get
            {
                if(ApplicationNameProvider != null && ApplicationNameProvider != this)
                {
                    return ApplicationNameProvider.GetApplicationName();
                }
                return ApplicationRegistration.Data.Application.Unknown.Name;
            }
        }

        /// <summary>
        /// Gets the name of the client application.
        /// </summary>
        /// <value>
        /// The name of the client application.  This value comes from the custom HTTP header "X-Bam-AppName".
        /// </value>
        public string ClientApplicationName => base.ApplicationName;

        public ProcessModes ProcessMode
        {
            get
            {
                string modeString = HttpContext?.Request?.Headers[Headers.ProcessMode];
                if (!string.IsNullOrEmpty(modeString))
                {
                    return modeString.ToEnum<ProcessModes>();
                }
                return ProcessModes.Prod;
            }
        }

        /// <summary>
        /// The organization that is owner of the server application.
        /// </summary>
        public ApplicationRegistration.Data.Organization ServerOrganization => ServerApplication?.Organization ?? Organization.Public;

        private ApplicationRegistration.Data.Application _serverApplication;
        private readonly object _serverApplicationLock = new object();
        /// <summary>
        /// The application representing what the server thinks it is.
        /// </summary>
        public ApplicationRegistration.Data.Application ServerApplication
        {
            get
            {
                return _serverApplicationLock.DoubleCheckLock(ref _serverApplication,
                    () => ApplicationRegistrationRepository.GetOneApplicationWhere(c =>
                        c.Name == ServerApplicationName));
            }
        }

        private ApplicationRegistration.Data.User _serverApplicationUser;
        private readonly object _serverApplicationUserLock = new object();
        public ApplicationRegistration.Data.User ServerApplicationUser
        {
            get
            {
                return _serverApplicationUserLock.DoubleCheckLock(ref _serverApplicationUser,
                    () => ApplicationRegistrationRepository.GetOneUserWhere(c => c.Email == CurrentUser.Email));
            }
        }

        public ApplicationRegistration.Data.Organization ClientOrganization => ClientApplication?.Organization ?? Organization.Public;

        private ApplicationRegistration.Data.Application _clientApplication;
        private readonly object _clientApplicationLock = new object();
        /// <summary>
        /// The application that the client requested.
        /// </summary>
        public ApplicationRegistration.Data.Application ClientApplication
        {
            get
            {
                return _clientApplicationLock.DoubleCheckLock(ref _clientApplication,
                    () => ApplicationRegistrationRepository.GetOneApplicationWhere(c =>
                        c.Name == ClientApplicationName));
            }
        }

        private ApplicationRegistration.Data.User _clientApplicationUser;
        private readonly object _clientApplicationUserLock = new object();
        /// <summary>
        /// Contains information about the user and their organizational relationships; for security, see CurrentUser.
        /// </summary>
        public ApplicationRegistration.Data.User ClientApplicationUser
        {
            get
            {
                return _clientApplicationUserLock.DoubleCheckLock(ref _clientApplicationUser,
                    () => ApplicationRegistrationRepository.GetOneUserWhere(c => c.Email == CurrentUser.Email));
            }
        }
        
        /// <summary>
        /// Returns true if the ApplicationNameProvider returns the same 
        /// ApplicationName as is specified by the request headers
        /// </summary>
        /// <returns></returns>
        [Local]
        public bool RequestIsForCurrentApplication(bool dieOnWarning = false)
        {
            if(ApplicationNameProvider == null)
            {
                Logger.Warning("{0} was null, '{1}' will always return true in this case", nameof(ApplicationNameProvider), nameof(RequestIsForCurrentApplication));
                if (dieOnWarning)
                {
                    throw new InvalidOperationException($"{nameof(ApplicationNameProvider)} was null, '{nameof(RequestIsForCurrentApplication)}' will always return true in this case");
                }
            }
            return ClientApplicationName.Equals(ServerApplicationName);
        }

        [Local]
        public bool UserIsInApplicationOrganization()
        {
            return ServerApplication.Organization.Users.Contains(GetApplicationUser());
        }

        [Local]
        public bool UserIsInOrganization(string organizationName)
        {
            if (organizationName.Equals(Organization.Public.Name))
            {
                return true;
            }
            User user = GetApplicationUserOrDie();
            Organization org = ApplicationRegistrationRepository.OneOrganizationWhere(c => c.Name == organizationName);
            if(org != null)
            {
                return org.Users.Contains(user);
            }
            return false;
        }
        
        /// <summary>
        /// Returns true if the ServerApplication is authorized
        /// for the current domain requested as specified by a
        /// HostDomain entry for the current application
        /// </summary>
        /// <returns></returns>
        [Local]
        public bool HostDomainIsAuthorized()
        {
            IRequest request = HttpContext?.Request;
            string host = request.Url?.Host;
            int port = (request?.Url?.Port).Value;
            Bam.Net.CoreServices.ApplicationRegistration.Data.Application app = GetServerApplicationOrDie();
            if(app.HostDomains.Count > 0)
            {
                HostDomain hd = app.HostDomains.FirstOrDefault(h => h.DomainName.Equals(host) && h.Port.Equals(port));
                return hd?.Authorized ?? false;
            }
            else
            {                
                HostDomain hd = new HostDomain
                {
                    Authorized = true,
                    DomainName = host,
                    DefaultApplicationName = app.Name,
                    Port = port
                };
                app.HostDomains.Add(hd);
                Task.Run(() => ApplicationRegistrationRepository.Save(app));
                return true;
            }
        }

        protected internal ApplicationRegistration.Data.User GetApplicationUserOrDie()
        {
            ApplicationRegistration.Data.User user = GetApplicationUser();
            if(user == null)
            {
                throw new ApplicationUserNotFoundException(ApplicationName, UserName);
            }
            return user;
        }

        /// <summary>
        /// Get the User entry from the ApplicationRegistrationRepository for the current user
        /// </summary>
        /// <returns></returns>
        protected internal User GetApplicationUser()
        {
            UserIsLoggedInOrDie();
            return ApplicationRegistrationRepository.GetOneUserWhere(c => c.UserName == UserName);
        }

        protected internal bool UserIsLoggedIn()
        {
            return !CurrentUser.Equals(UserAccounts.Data.User.Anonymous);
        }

        protected internal void UserIsLoggedInOrDie()
        {
            if (CurrentUser.Equals(UserAccounts.Data.User.Anonymous))
            {
                throw new InvalidOperationException("User not logged in");
            }
        }
        
        protected internal Bam.Net.CoreServices.ApplicationRegistration.Data.Application GetServerApplicationOrDie()
        {
            if (ServerApplicationName.Equals(ApplicationRegistration.Data.Application.Unknown.Name))
            {
                throw new InvalidOperationException("Application is unknown");
            }
            Bam.Net.CoreServices.ApplicationRegistration.Data.Application app = ApplicationRegistrationRepository.OneApplicationWhere(c => c.Name == ServerApplicationName);
            if (app == null)
            {
                throw new InvalidOperationException("Application was not found");
            }
            return app;
        }

        protected internal Bam.Net.CoreServices.ApplicationRegistration.Data.Application GetClientApplicationOrDie()
        {
            if (ClientApplicationName.Equals(ApplicationRegistration.Data.Application.Unknown.Name))
            {
                throw new InvalidOperationException("Application is unknown");
            }
            Bam.Net.CoreServices.ApplicationRegistration.Data.Application app = ApplicationRegistrationRepository.OneApplicationWhere(c => c.Name == ClientApplicationName);
            if(app == null)
            {
                throw new InvalidOperationException("Application was not found");
            }
            return app;
        }
    }
}
