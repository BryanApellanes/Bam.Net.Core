using Bam.Net;
using Bam.Net.CoreServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Bam.Net.Incubation;
using Bam.Net.Application;

namespace gloo.Test
{
    [Serializable]
    [ServiceRegistryContainer]
    public class BamRpcTestRegistry
    {
        [ServiceRegistryLoader("BamRpcTestRegistry", ProcessModes.Dev, ProcessModes.Test)]
        public static ServiceRegistry CreateTestRegistry()
        {
            return ServiceRegistry.Create()
                .For<BamRpcTestService>().Use<BamRpcTestService>()
                .For<BamRpcEncryptedTestService>().Use<BamRpcEncryptedTestService>()
                .For<BamRpcApiKeyRequiredTestService>().Use<BamRpcApiKeyRequiredTestService>()
                .Cast<ServiceRegistry>();
        }
    }
}
