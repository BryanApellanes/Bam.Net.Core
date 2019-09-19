using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Bam.Net.ServiceProxy;
using Bam.Net.Incubation;

namespace Bam.Net.ServiceProxy
{
    public class JsClientProxyGenerator : ClientProxyGenerator
    {
        public override void BeforeWriteProxyCode(Incubator serviceProvider, IHttpContext context)
        {
            IResponse response = context.Response;
            response.ContentType = "application/javascript";
        }
        
        public override string GetProxyCode(Incubator serviceProvider, IHttpContext context)
        {
            return ServiceProxySystem.GenerateJsProxyScript(serviceProvider, serviceProvider.ClassNames, ShouldIncludeLocalMethods(context), context.Request).ToString();
        }

        protected bool ShouldIncludeLocalMethods(IHttpContext context)
        {
            Args.ThrowIfNull(context, "context");

            return context.Request.UserHostAddress.StartsWith("127.0.0.1");
        }
    }
}
