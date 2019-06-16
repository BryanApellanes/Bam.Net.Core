using Bam.Net.Data.Repositories;

namespace Bam.Net.Services.Data
{
    public class Vouch : CompositeKeyAuditRepoData
    {
        public virtual DnsServerDescriptor DnsServerDescriptor { get; set; }
        public string User { get; set; }
        public int Value { get; set; }
    }
}