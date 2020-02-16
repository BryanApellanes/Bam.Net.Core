using Bam.Net.Data.Repositories;

namespace Bam.Net.CoreServices.Auth.Data
{
    public class AccessToken : AuditRepoData
    {
        public string ProviderId { get; set; }
        public string ProviderName { get; set; }
        public string Bearer => $"Bearer {Value}";
        public string Value { get; set; }
    }
}