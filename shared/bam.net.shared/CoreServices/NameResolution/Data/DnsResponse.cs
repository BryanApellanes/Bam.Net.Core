using System.Net;
using Bam.Net.CoreServices;
using Bam.Net.Data.Repositories;
using DNS.Protocol;
using DNS.Protocol.ResourceRecords;

namespace Bam.Net.CoreServices.NameResolution.Data
{
    public class DnsResponse : CompositeKeyAuditRepoData
    {
        public DnsResponse()
        {
            Code = Cuid;
        }
        
        public virtual DnsServerDescriptor DnsServerDescriptor { get; set; }
        
        [CompositeKey]
        public string HostName { get; set; }
        
        [CompositeKey]
        public string IpAddress { get; set; }

        /// <summary>
        /// A random value ensuring that the composite key calculation allows
        /// duplicate HostNames and IpAddresses.
        /// </summary>
        [CompositeKey]
        public string Code { get; set; }
        
        /// <summary>
        /// Indicates whether to consider this a valid dns response
        /// </summary>
        public bool IsActive { get; set; }
        public IPAddressResourceRecord ToResourceRecord()
        {
            return new IPAddressResourceRecord(new Domain(HostName), IPAddress.Parse(IpAddress));
        }
    }
}