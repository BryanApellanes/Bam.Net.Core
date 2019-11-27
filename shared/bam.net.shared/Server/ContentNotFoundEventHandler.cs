using Bam.Net.Server;
using Bam.Net.ServiceProxy;

namespace DefaultNamespace
{
    public delegate void ContentNotFoundEventHandler(IResponder responder, IHttpContext context, string[] checkedPaths);
}