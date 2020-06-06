using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Bam.Net;
using Bam.Net.ServiceProxy.Secure;

namespace Bam.Net.Application
{
    [Proxy("glooTestSvc")]
    public class BamRpcTestService
    {
        public BamRpcMonkey GetMonkey(string name)
        {
            return new BamRpcMonkey(name);
        }
    }

    [Encrypt]
    [Proxy("glooEncryptedTestSvc")]
    public class BamRpcEncryptedTestService
    {
        public BamRpcMonkey GetMonkey(string name)
        {
            return new BamRpcMonkey(string.Format("From Encrypted Test Service: {0}", name));
        }
    }

    [ApiKeyRequired]
    [Proxy("glooApiKeyRequiredSvc")]
    public class BamRpcApiKeyRequiredTestService
    {
        public BamRpcMonkey GetMonkey(string name)
        {
            return new BamRpcMonkey(string.Format("From ApiKeyRequired Test Service: {0}", name));
        }
    }
}
