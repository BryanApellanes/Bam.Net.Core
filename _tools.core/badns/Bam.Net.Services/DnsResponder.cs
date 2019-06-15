using System;
using Bam.Net.CoreServices;
using Bam.Net.Logging;
using Bam.Net.Server;
using Bam.Net.ServiceProxy;

namespace Bam.Net.Services
{
    public class DnsResponder: HttpHeaderResponder
    {
        public DnsResponder() : this(BamConf.Load())
        {
        }

        public DnsResponder(BamConf conf) : this(conf, Log.Default)
        {
        }

        public DnsResponder(BamConf conf, ILogger logger) : base(conf, logger)
        {
        }

        public override bool TryRespond(IHttpContext context)
        {
            try
            {
                WriteResponse(context.Response, DiagnosticInfo.Current.ToJson(true));
            }
            catch (Exception ex)
            {
                Log.Error("Exception responding to request: {0}", ex, ex.Message);
                return false;
            }
        }
    }
}