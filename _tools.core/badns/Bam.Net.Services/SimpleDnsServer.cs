using Bam.Net.Logging;
using Bam.Net.Services;
using Bam.Net.Server;
using DNS.Server;

namespace Bam.Net.Services
{
    public class SimpleDnsServer : SimpleServer<DnsResponder>
    {
        public SimpleDnsServer() : this(new DnsResponder(), Log.Default)
        {
        }

        public SimpleDnsServer(DnsResponder responder, ILogger logger) : base(responder, logger)
        {
        }

        public void Listen(int port = 53)
        {
            MasterFile masterFile = new MasterFile();
            
            DnsServer server = new DnsServer();
        }
    }
}