using Bam.Net.Application;
using Bam.Net.Logging;
using Bam.Net.Services;
using Bam.Net.Server;
using DNS.Server;
using Bam.Net.CoreServices.NameResolution;

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

        public override void Start()
        {
            base.Start();
            Listen(Config.Current["Port", "53"].ToInt());
        }

        public DnsServer DnsServer { get; private set; }
        
        public void Listen(int port = 53)
        {   
            DnsServer server = new DnsServer(new DnsRootServerRequestResolver());
            server.Listen(port);
            DnsServer = server;
        }
    }
}