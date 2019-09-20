using Bam.Net.Server;

namespace Bam.Net.ServiceProxy
{
    public interface IApplicationStartupHandler
    {
        void Execute(AppConf appConf);
    }
}