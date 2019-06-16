using Bam.Net.Data.Repositories;

namespace Bam.Net.Services.Data
{
    public class DnsServerDescriptor: CompositeKeyAuditRepoData
    {
        public string Ipv4Address { get; set; }
        public string Ipv6Address { get; set; }
        public string Operator { get; set; }
    }
}