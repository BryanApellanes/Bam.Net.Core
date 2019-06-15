using System.Net;
using Bam.Net.Services.Data;
using DNS.Client.RequestResolver;

namespace Bam.Net.Application
{
    public class BamDnsClient : DNS.Client.DnsClient
    {
        public BamDnsClient(RootDnsServer rootDnsServer) : this(rootDnsServer.Ipv4Address)
        {
            RootDnsServer = rootDnsServer;
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
        
        public RootDnsServer RootDnsServer { get; set; }
    }
}