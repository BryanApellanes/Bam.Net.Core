using System.Net;
using Bam.Net.CoreServices.NameResolution.Data;
using DNS.Client.RequestResolver;

namespace Bam.Net.CoreServices.NameResolution
{
    /// <summary>
    /// A Dns client that requests name resolution form a specified DnsServerDescriptor 
    /// </summary>
    public class DnsServerDescriptorClient : DNS.Client.DnsClient
    {
        public DnsServerDescriptorClient(DnsServerDescriptor rootDnsServerDescriptor) : this(rootDnsServerDescriptor.Ipv4Address)
        {
            DnsServerDescriptor = rootDnsServerDescriptor;
        }

        public DnsServerDescriptorClient(IPEndPoint dns) : base(dns)
        {
        }

        public DnsServerDescriptorClient(IPAddress ip, int port = 53) : base(ip, port)
        {
        }

        public DnsServerDescriptorClient(string ip, int port = 53) : base(ip, port)
        {
        }

        public DnsServerDescriptorClient(IRequestResolver resolver) : base(resolver)
        {
        }
        
        public DnsServerDescriptor DnsServerDescriptor { get; set; }
    }
}