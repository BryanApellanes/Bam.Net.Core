using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bam.Net.CoreServices.Auth
{
    public class AuthClientSettings
    {
        public AuthClientSettings() { }
        
        /// <summary>
        /// The name of the authorization/authentication provider.  For example, okta, auth0, onelogin etc.
        /// </summary>
        public string ProviderName { get; set; }
        
        /// <summary>
        /// The identifier assigned to us by the provider.
        /// </summary>
        public string ClientId { get; set; }
        
        /// <summary>
        /// The secret assigned to us by the provider.  
        /// </summary>
        public string ClientSecret { get; set; }
        
        /// <summary>
        /// The endpoint at the provider to redirect unauthorized/unauthenticated requests to.
        /// </summary>
        public string AuthorizationEndpoint { get; set; }
    }
}