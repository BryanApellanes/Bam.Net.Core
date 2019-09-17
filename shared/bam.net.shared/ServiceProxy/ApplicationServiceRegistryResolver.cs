using System;
using System.Collections.Generic;
using Bam.Net.Server;
using Bam.Net.Services;

namespace Bam.Net.ServiceProxy
{
    public class ApplicationServiceRegistryResolver : IApplicationServiceRegistryResolver
    {
        private static readonly Dictionary<string, ApplicationServiceRegistry> _applicationServiceRegistries = new Dictionary<string, ApplicationServiceRegistry>();
        private static object _applicationServiceRegistryLock = new object();
        public ApplicationServiceRegistry ResolveApplicationServiceRegistry(AppConf appConf, Action<ApplicationServiceRegistry> configure = null)
        {
            lock (_applicationServiceRegistryLock)
            {
                if (!_applicationServiceRegistries.ContainsKey(appConf.Name))
                {
                    _applicationServiceRegistries.Add(appConf.Name, ApplicationServiceRegistry.Configure(configure ?? (Action<ApplicationServiceRegistry>)((reg) => { })));
                }

                return _applicationServiceRegistries[appConf.Name];
            }
        }
    }
}