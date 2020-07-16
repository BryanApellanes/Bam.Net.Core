using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Bam.Net.CoreServices;
using Bam.Net.ServiceProxy.Secure;
using Bam.Net.CoreServices.Auth.Data;
using Bam.Net.CoreServices.Auth.Data.Dao.Repository;
using Bam.Net.CoreServices.Auth;
using Bam.Net.ServiceProxy;
using Bam.Net.CoreServices.ApplicationRegistration.Data.Dao.Repository;

namespace Bam.Net.CoreServices
{
    [Proxy("authSettingsSvc")]    
    [ApiKeyRequired]
    [Authenticated]
    public class AuthSettingsService : ApplicationProxyableService
    {
        protected AuthSettingsService() { }

        public AuthSettingsService(AuthRepository oauthRepo, ApplicationRegistrationRepository applicationRegistrationRepo)
        {
            OAuthSettingsRepository = oauthRepo;
            ApplicationRegistrationRepository = applicationRegistrationRepo;
        }

        public AuthRepository OAuthSettingsRepository { get; set; }

        [RoleRequired("/", "Admin")]
        public virtual CoreServiceResponse<List<AuthClientSettings>> GetClientSettings(bool includeSecret = false)
        {
            try
            {
                ApplicationRegistration.Data.Application app = ClientApplication;
                return new CoreServiceResponse<List<AuthClientSettings>>
                    (
                        OAuthSettingsRepository
                            .AuthProviderSettingsesWhere(c => c.ApplicationIdentifier == app.Cuid && c.ApplicationName == app.Name)
                            .Select(os =>
                            {
                                AuthClientSettings setting = os.CopyAs<AuthClientSettings>();
                                if (!includeSecret)
                                {
                                    setting.ClientSecret = string.Empty;
                                }
                                return setting;
                            })
                            .ToList()
                    )
                {
                    Success = true
                };
            }
            catch(Exception ex)
            {
                return new CoreServiceResponse<List<AuthClientSettings>> { Success = false, Message = ex.Message };
            }
        }

        [RoleRequired("/", "Admin")]
        public virtual CoreServiceResponse<AuthClientSettings> SetProvider(string providerName, string clientId, string clientSecret)
        {
            try
            {
                ApplicationRegistration.Data.Application app = GetClientApplicationOrDie();

                AuthProviderSettings data = new AuthProviderSettings()
                {
                    ApplicationName = app.Name,
                    ApplicationIdentifier = app.Cuid,
                    ProviderName = providerName,
                    ClientId = clientId,
                    ClientSecret = clientSecret
                };
                AuthClientSettings settings = OAuthSettingsRepository.Save(data).CopyAs<AuthClientSettings>();
                return new CoreServiceResponse<AuthClientSettings> { Success = true, Data = settings };
            }
            catch (Exception ex)
            {
                return new CoreServiceResponse<AuthClientSettings> { Success = false, Message = ex.Message };
            }
        }

        [RoleRequired("/", "Admin")]
        public virtual CoreServiceResponse RemoveProvider(string providerName)
        {
            try
            {
                ApplicationRegistration.Data.Application app = GetClientApplicationOrDie();
                AuthProviderSettings data = OAuthSettingsRepository.OneAuthProviderSettingsWhere(c => c.ApplicationIdentifier == app.Cuid && c.ApplicationName == app.Name && c.ProviderName == providerName);
                if(data != null)
                {
                    bool success = OAuthSettingsRepository.Delete(data);
                    if (!success)
                    {
                        throw OAuthSettingsRepository.LastException;
                    }
                    return new CoreServiceResponse { Success = success };
                }
                throw new InvalidOperationException($"OAuthSettings not found: AppId={app.Cuid}, AppName={app.Name}, Provider={providerName}");
            }
            catch (Exception ex)
            {
                return new CoreServiceResponse { Success = false, Message = ex.Message };
            }
        }

        public override object Clone()
        {
            AuthSettingsService clone = new AuthSettingsService(OAuthSettingsRepository, ApplicationRegistrationRepository);
            clone.CopyProperties(this);
            clone.CopyEventHandlers(this);
            return clone;
        }
    }
}
