using Bam.Net.Configuration;
using Bam.Net.CoreServices;
using Bam.Net.Logging;
using Bam.Net.Presentation;
using Bam.Net.Services.Clients;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Bam.Net.Data.Repositories;
using Bam.Net.Server;
using Bam.Net.ServiceProxy;

namespace Bam.Net.Services
{
    /// <summary>
    /// A service registry (or dependency injection container) for the currently running application process.  
    /// </summary>
    public class ApplicationServiceRegistry: ServiceRegistry
    {
        protected CoreClient CoreClient { get; set; }

        static ApplicationServiceRegistry _appRegistry;
        static readonly object _appRegistryLock = new object();
        public static ApplicationServiceRegistry Current
        {
            get
            {
                return _appRegistryLock.DoubleCheckLock(ref _appRegistry, () => Configure(Configurer ?? ((reg) => { })));
            }
            set
            {
                _appRegistry = value;
            }
        }

        public static Action<ApplicationServiceRegistry> Configurer { get; set; }

        public static ApplicationServiceRegistry ForProcess(Action<ApplicationServiceRegistry> configureMore = null)
        {
            return ForApplication(ProcessApplicationNameProvider.Current.GetApplicationName(), configureMore);
        }
        
        public static ApplicationServiceRegistry ForApplication()
        {
            return ForApplication(DefaultConfigurationApplicationNameProvider.Instance.GetApplicationName());
        }

        static readonly Dictionary<string, ApplicationServiceRegistry> _appRegistries = new Dictionary<string, ApplicationServiceRegistry>();
        static readonly Dictionary<string, Action<ApplicationServiceRegistry>> _applicationConfigurers = new Dictionary<string, Action<ApplicationServiceRegistry>>();
        public static ApplicationServiceRegistry ForApplication(string applicationName, Action<ApplicationServiceRegistry> configureMore = null)
        {
            try
            {
                if (!_appRegistries.ContainsKey(applicationName))
                {
                    if (configureMore == null && _applicationConfigurers.ContainsKey(applicationName))
                    {
                        configureMore = _applicationConfigurers[applicationName];
                    }
                    else if (configureMore != null)
                    {
                        _applicationConfigurers.Set(applicationName, configureMore);
                    }

                    ApplicationServiceRegistry applicationServiceRegistry = Configure((appSvcReg) =>
                    {
                        Workspace workspace = Workspace.ForApplication(applicationName);
                        DirectoryInfo services = workspace.Directory("services");
                        if (services.Exists)
                        {
                            Parallel.ForEach(services.GetFiles("*.dll"), fileInfo => { TryAddTypes(appSvcReg, fileInfo); });
                        }
                    });
                    configureMore?.Invoke(applicationServiceRegistry);
                    _appRegistries.Add(applicationName, applicationServiceRegistry);
                }
                return _appRegistries[applicationName];
            }
            catch (Exception ex)
            {
                Log.Warn("Exception discovering application services: {0}", ex, ex.Message);
            }
            return Configure(a => { });
        }


        [ServiceRegistryLoader]
        public static ApplicationServiceRegistry Configure(Action<ApplicationServiceRegistry> configure, bool setConfigurer = true)
        {
            return Configure(null, configure, setConfigurer);
        }
        
        [ServiceRegistryLoader]
        public static ApplicationServiceRegistry Configure(AppConf appConf, Action<ApplicationServiceRegistry> configure, bool setConfigurer = true)
        {
            IApplicationNameProvider applicationNameProvider = new DefaultConfigurationApplicationNameProvider();
            if (appConf != null)
            {
                applicationNameProvider = appConf;
            }
            
            if (setConfigurer && Configurer != configure)
            {
                Configurer = configure;
            }
            ApplicationServiceRegistry appRegistry = new ApplicationServiceRegistry();
            appRegistry.CombineWith(CoreClientServiceRegistryContainer.Current);
            appRegistry
                .For<ApplicationServiceRegistry>().Use(appRegistry)
                .For<AppConf>().Use(appConf)
                .For<IRepositoryResolver>().Use<DefaultRepositoryResolver>()
                .For<IApplicationNameProvider>().Use(applicationNameProvider)
                .For<IIncludesResolver>().Use<IncludesResolver>()
                .For<IViewModelProvider>().Use<DefaultViewModelProvider>()
                .For<IPersistenceModelProvider>().Use<DefaultPersistenceModelProvider>()
                .For<IExecutionRequestResolver>().Use<ExecutionRequestResolver>()
                .For<ApplicationModel>().Use<ApplicationModel>();

            configure(appRegistry);
            appRegistry.CoreClient = appRegistry.Get<CoreClient>();
            Current = appRegistry;
            SetAppRegistry(applicationNameProvider.GetApplicationName(), appRegistry);
            return appRegistry;
        }

        private static void TryAddTypes(ApplicationServiceRegistry appServiceRegistry, FileInfo file)
        {
            TryAddTypes(appServiceRegistry, file, t => t.HasCustomAttributeOfType<AppServiceAttribute>());
        }

        private static void TryAddTypes(ApplicationServiceRegistry appServiceRegistry, FileInfo file, Func<Type, bool> predicate)
        {
            try
            {
                Assembly assembly = Assembly.LoadFile(file.FullName);
                Type[] types = assembly.GetTypes();
                foreach (Type type in types.Where(predicate))
                {
                    appServiceRegistry.Set(type, type);
                }
            }
            catch (Exception ex)
            {
                Log.Warn("Exception loading file for service discovery {0}: {1}", ex, file.FullName, ex.Message);
            }
        }

        private static readonly object _lockAppRegistry = new object();
        private static void SetAppRegistry(string applicationName, ApplicationServiceRegistry applicationServiceRegistry)
        {
            lock (_lockAppRegistry)
            {
                if (_appRegistries.ContainsKey(applicationName))
                {
                    _appRegistries[applicationName] = applicationServiceRegistry;
                }
                else
                {
                    _appRegistries.Add(applicationName, applicationServiceRegistry);
                }
            }
        }
    }
}
