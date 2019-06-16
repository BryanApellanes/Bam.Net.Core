using System.Net;
using Bam.Net.Services.Data;
using DNS.Client.RequestResolver;

namespace Bam.Net.Application
{
    public class BamDnsClient : DNS.Client.DnsClient
    {
        public BamDnsClient(DnsServerDescriptor rootDnsServerDescriptor) : this(rootDnsServerDescriptor.Ipv4Address)
        {
            DnsServerDescriptor = rootDnsServerDescriptor;
        }

        public BamDnsClient(IPEndPoint dns) : base(dns)
        {
        }

        public BamDnsClient(IPAddress ip, int port = 53) : base(ip, port)
        {
        }

        public BamDnsClient(string ip, int port = 53) : base(ip, port)
        {
        }

        public BamDnsClient(IRequestResolver resolver) : base(resolver)
        {
        }
        
        public DnsServerDescriptor DnsServerDescriptor { get; set; }
    }
}