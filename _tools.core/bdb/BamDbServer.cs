using Bam.Net.Data.Repositories;
using Bam.Net.Logging;
using Bam.Net.Server;

namespace Bam.Net.Application
{
    public class BamDbServer: SimpleServer<BamDbResponder>
    {
        public BamDbServer(BamConf conf, ILogger logger, IRepository repository)
            : base(new BamDbResponder(conf, logger, repository), logger)
        {
            this.Responder.Initialize();
        }
    }
}
