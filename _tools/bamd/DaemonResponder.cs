using Bam.Net.Application;
using Bam.Net.Data.Repositories;
using Bam.Net.Logging;
using Bam.Net.Server;
using Bam.Net.Server.Renderers;
using Bam.Net.ServiceProxy;
using Bam.Net.Services.Automation;

namespace Bam.Net.System
{
    public class DaemonResponder : HttpHeaderResponder
    {
        public DaemonResponder(BamConf conf, DaemonProcessMonitorService monitorService, ILogger logger, bool verbose = false) 
            : base(conf, logger)
        {
            RendererFactory = new WebRendererFactory(logger);
            ServiceProxyResponder = new ServiceProxyResponder(conf, logger);
            ServiceProxyResponder.AddCommonService(new CommandService());
            ServiceProxyResponder.AddCommonService(monitorService);
            DataProvider.Current.SetRuntimeAppDataDirectory();
            if (verbose)
            {
                WireResponseLogging(ServiceProxyResponder, logger);
            }
        }

        public ServiceProxyResponder ServiceProxyResponder
        {
            get;
            private set;
        }

        public override bool Respond(IHttpContext context)
        {
            if (!ServiceProxyResponder.TryRespond(context))
            {
                SendResponse(context, 404, new { BamServer = "BamDaemon" });
            }
            context.Response.Close();
            return true;
        }
    }
}
