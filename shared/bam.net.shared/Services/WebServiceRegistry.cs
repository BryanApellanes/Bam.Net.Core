using Bam.Net.CoreServices;
using Bam.Net.Incubation;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using System.Linq;

namespace Bam.Net.Services
{
    /// <summary>
    /// A service registry for proxyable services.
    /// </summary>
    public class WebServiceRegistry: ServiceRegistry
    {
        public WebServiceRegistry() { }
        
        public WebServiceRegistry(Incubator registry)
        {
            CopyWebServices(registry);
        }

        public WebServiceRegistry CopyWebServices(Incubator registry)
        {
            foreach (string className in registry.ClassNames)
            {
                object instance = registry.Get(className, out Type type);
                if (type.HasCustomAttributeOfType<ProxyAttribute>())
                {
                    Set(type, () => registry.Get(type));
                }
            }
            return this;
        }

        public static WebServiceRegistry ForApplicationServiceRegistry(ApplicationServiceRegistry applicationServiceRegistry)
        {
            WebServiceRegistry fromAssembly = FromEntryAssembly(applicationServiceRegistry);
            return fromAssembly.CopyWebServices(applicationServiceRegistry);
        }

        public static WebServiceRegistry FromEntryAssembly(ServiceRegistry serviceRegistry = null)
        {
            WebServiceRegistry webServiceRegistry = new WebServiceRegistry();
            foreach (Type type in Assembly.GetEntryAssembly()
                .GetTypes()
                .Where(type => type.HasCustomAttributeOfType(out ProxyAttribute proxyAttribute)).ToArray())
            {
                if (serviceRegistry != null)
                {
                    webServiceRegistry.Set(type, () => serviceRegistry.Get(type));
                }
                else
                {
                    webServiceRegistry.Set(type, type.Construct());
                }
            }
            return webServiceRegistry;
        }

        public static WebServiceRegistry FromRegistry(ServiceRegistry registry)
        {
            return FromIncubator(registry);
        }

        public static WebServiceRegistry FromIncubator(Incubator incubator)
        {
            WebServiceRegistry result = new WebServiceRegistry();
            result.CopyWebServices(incubator);
            return result;
        }
    }
}
