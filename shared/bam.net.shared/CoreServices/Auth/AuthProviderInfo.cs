using System;
using System.Collections.Generic;
using System.Text;

namespace Bam.Net.CoreServices.Auth
{
    public class AuthProviderInfo
    {
        public string ProviderName { get; set; }
        public string ClientId { get; set; }
        public string ClientSecret { get; set; }
        public string AuthorizationEndpoint { get; set; }
        public override int GetHashCode()
        {
            return $"{nameof(AuthProviderInfo)}.{ProviderName}".GetHashCode();
        }

        public override bool Equals(object obj)
        {
            if (obj is AuthProviderInfo provider)
            {
                return provider.ProviderName.Equals(ProviderName);
            }
            return false;
        }
    }
}
