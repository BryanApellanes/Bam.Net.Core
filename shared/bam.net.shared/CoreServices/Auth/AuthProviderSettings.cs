using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Bam.Net.Configuration;
using Bam.Net.Logging;
using Bam.Net.CoreServices.Auth.Data;

namespace Bam.Net.CoreServices.Auth.Data
{
    public partial class AuthProviderSettings
    {
        static readonly Dictionary<string, Type> _settingsTypeMap;
        static AuthProviderSettings()
        {
            _settingsTypeMap = new Dictionary<string, Type>
            {
                { "bamapps.net", typeof(AuthProviderSettings) },
                { "facebook", typeof(FacebookOAuthSettings) }
            };
        }

        public AuthProviderSettings() : base()
        {
            ProviderName = "bamapps.net";
            ClientId = "1282272511809831";
            AuthorizationEndpointFormat = "https://bamapps.net/oauth/authorize?clientId={ClientId}&callbackUrl={CallbackUrl}&code={Code}&state={State}";
            AuthorizationCallbackEndpointFormat = "https://bamapps.net/oauth/setaccesstoken?clientId={ClientId}&callbackUrl={TokenCallbackUrl}&clientSecret={ClientSecret}&code={Code}&state={State}";
        }

        public AuthProviderSettings(string clientId, string clientSecret) : this()
        {
            ClientId = clientId;
            ClientSecret = clientSecret;
        }

        /// <summary>
        /// The providers user login endpoint 
        /// </summary>
        public string GetAuthorizationUrl()
        {
            return AuthorizationEndpointFormat.NamedFormat(this);
        }

        public string GetAuthorizationCallbackUrl()
        {
            return AuthorizationCallbackEndpointFormat.NamedFormat(this);
        }

        public string Version { get; set; }        

        public string AccessToken { get; set; }

        public AuthProviderSettings WithAccessToken(string accessToken)
        {
            AuthProviderSettings result = GetType().Construct<AuthProviderSettings>();
            result.CopyProperties(this);
            result.AccessToken = accessToken;
            return result;
        }
    }
}
