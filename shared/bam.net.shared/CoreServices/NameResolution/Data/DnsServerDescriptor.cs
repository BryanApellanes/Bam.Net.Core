using Bam.Net.Data.Repositories;

namespace Bam.Net.CoreServices.NameResolution.Data
{
    public class DnsServerDescriptor: CompositeKeyAuditRepoData
    {
        [CompositeKey]
        public string Ipv4Address { get; set; }
        
        [CompositeKey]
        public string Ipv6Address { get; set; }
        
        [CompositeKey]
        public string Operator { get; set; }
    }
}