using Bam.Net.Server;
using Bam.Net.Server.PathHandlers;

namespace Bam.Net.Application
{
    public class AppRouteHandlerManager: RouteHandlerManager
    {
        public AppRouteHandlerManager(AppConf appConf)
        {
            AppConf = appConf;
        }
        
        public AppConf AppConf { get; private set; }
    }
}