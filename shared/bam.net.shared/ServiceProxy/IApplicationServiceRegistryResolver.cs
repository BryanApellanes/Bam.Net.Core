using System;
using Bam.Net.Server;
using Bam.Net.Services;

namespace Bam.Net.ServiceProxy
{
    public interface IApplicationServiceRegistryResolver
    {
        ApplicationServiceRegistry ResolveApplicationServiceRegistry(AppConf appConf, Action<ApplicationServiceRegistry> configure = null);
    }
}