using Bam.Net.Data.Repositories;
using Bam.Net.Logging;
using Bam.Net.Server;
using Bam.Net.Testing.Data;

namespace Bam.Net.Application
{
    public class BamDbServer: SimpleServer<BamDbResponder>
    {
        public BamDbServer(BamConf conf, ILogger logger, IRepository repository, bool enableSqlPassThrough = false)
            : base(new BamDbResponder(conf, logger, repository), logger)
        {
            this.Responder.Initialize();
            EnableSqlPassThrough = enableSqlPassThrough;
        }

        public bool EnableSqlPassThrough
        {
            get { return Responder.SqlPassThroughEnabled; }
            set { Responder.SqlPassThroughEnabled = value; }
        }
    }
}
