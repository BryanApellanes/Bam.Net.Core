using Bam.Net.Incubation;

namespace Bam.Net.ServiceProxy
{
    public class JsWebServiceProxyGenerator : JsClientProxyGenerator
    {
        public override string GetProxyCode(Incubator serviceProvider, IHttpContext context)
        {
            return ServiceProxySystem.GenerateWebServiceProxyScript(serviceProvider, serviceProvider.ClassNames, ShouldIncludeLocalMethods(context), context.Request).ToString();
        }
    }
}