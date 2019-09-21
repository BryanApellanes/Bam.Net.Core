using Bam.Net.ServiceProxy;

namespace Bam.Net.Server
{
    
    public class RouteInfo
    {
        public IRequest Request { get; set; }
        public bool IsHomeRequest { get; set; }
        public bool CanRender { get; set; }
        public RequestRoute RequestRoute { get; set; }
    }
}