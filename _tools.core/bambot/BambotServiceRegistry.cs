using System;
using System.Reflection;
using Bam.Net.Automation;
using Bam.Net.CoreServices;
using Bam.Net.Services;
using Bam.Net.Services.Automation;
using System.Linq;
using Bam.Net.Profiguration;

namespace Bam.Net.Application
{
    [ServiceRegistryContainer]
    public class BambotServiceRegistry : ApplicationServiceRegistry
    {
        public const string ApplicationName = "Bambot";

        public static ApplicationServiceRegistry ForCurrentProcessMode()
        {
            ProcessModes current = ProcessMode.Current.Mode;
            MethodInfo creator = typeof(BambotServiceRegistry).GetMethods().FirstOrDefault(m =>
            {
                if (m.HasCustomAttributeOfType(out ServiceRegistryLoaderAttribute attr))
                {
                    return attr.ProcessModes.Contains(current);
                }

                return false;
            });

            if (creator != null)
            {
                return (ApplicationServiceRegistry) creator.Invoke(null, null);
            }

            return Dev();
        }
        
        
        [ServiceRegistryLoader(ApplicationName, ProcessModes.Dev)]
        public static ApplicationServiceRegistry Dev()
        {
             // customize for Dev
            return Create();
        }
        
        [ServiceRegistryLoader(ApplicationName, ProcessModes.Test)]
        public static ApplicationServiceRegistry Test()
        {
            // customize for Test
            return Create();
        }
        
        [ServiceRegistryLoader(ApplicationName, ProcessModes.Prod)]
        public static ApplicationServiceRegistry Prod()
        {
            // customize for Prod
            return Create();
        }
        
        public static ApplicationServiceRegistry Create(Action<ApplicationServiceRegistry> configure = null)
        {
            ApplicationServiceRegistry result = ForProcess(appSvcReg =>
            {
                appSvcReg.For<CommandService>().Use<CommandService>();
                appSvcReg.For<IApplicationNameProvider>().Use<ProcessApplicationNameProvider>();
                appSvcReg.For<JobManagerService>().Use<JobManagerService>();
            });
            configure?.Invoke(result);

            return result;
        } 
    }
}