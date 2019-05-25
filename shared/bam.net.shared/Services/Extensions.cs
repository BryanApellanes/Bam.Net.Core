using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Bam.Net.CoreServices;
using Bam.Net.Server;
using Bam.Net.Incubation;
using Bam.Net.Logging;

namespace Bam.Net.Services
{
    public static class Extensions
    {
        public static ServiceProxyServer ServeType<T>(this T service, HostPrefix hostPrefix, ILogger logger = null, BamConf conf = null) where T : ProxyableService
        {
            return service.ServeType<T>(logger, hostPrefix.HostName, hostPrefix.Port, hostPrefix.Ssl, conf);
        }

        public static ServiceProxyServer ServeType<T>(this T service, ILogger logger = null, string hostName = "localhost", int port = 8080, bool ssl = false, BamConf conf = null) where T: ProxyableService
        {
            CoreServices.ServiceRegistry reg = new CoreServices.ServiceRegistry();
            reg.For<T>().Use(service);
            return reg.ServeRegistry(logger, hostName, port, ssl, conf);
        }

        public static ServiceProxyServer ServeRegistry(this CoreServices.ServiceRegistry registry, ILogger logger = null, string hostName = "localhost", int port = 8080, bool ssl = false, BamConf conf = null)
        {
            return ServeRegistry(registry, new HostPrefix[]{ new HostPrefix { HostName = hostName, Port = port, Ssl = ssl } }, logger, conf);
        }

        public static ServiceProxyServer ServeType<T>(this T service, HostPrefix[] hostPrefixes, ILogger logger = null, BamConf conf = null)
        {
            CoreServices.ServiceRegistry reg = new CoreServices.ServiceRegistry();
            reg.For<T>().Use(service);
            return reg.ServeRegistry(hostPrefixes, logger, conf);
        }

        public static ServiceProxyServer ServeRegistry(this CoreServices.ServiceRegistry registry, HostPrefix[] hostPrefixes, ILogger logger = null, BamConf conf = null)
        {
            conf = conf ?? BamConf.Load(ServiceConfig.ContentRoot);
            logger = logger ?? Log.Default;

            ServiceProxyServer server = new ServiceProxyServer(registry, new ServiceProxyResponder(conf, logger), logger);
            foreach(HostPrefix prefix in hostPrefixes)
            {
                server.HostPrefixes.Add(prefix);
            }
            server.Start();
            return server;
        }
    }
}
