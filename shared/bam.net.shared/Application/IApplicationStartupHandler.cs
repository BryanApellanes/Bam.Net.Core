using Bam.Net.Server;

namespace Bam.Net.Application
{
    public interface IApplicationStartupHandler
    {
        void Execute(AppConf appConf);
    }
}