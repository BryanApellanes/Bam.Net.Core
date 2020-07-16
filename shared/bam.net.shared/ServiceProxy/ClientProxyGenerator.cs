using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Bam.Net.ServiceProxy;
using Bam.Net.Incubation;
using Bam.Net.Logging;

namespace Bam.Net.ServiceProxy
{
    public abstract class ClientProxyGenerator : IClientProxyGenerator
    {
        public ClientProxyGenerator()
        {
            Encoding = Encoding.UTF8;
        }

        public Encoding Encoding { get; set; }
        public ILogger Logger { get; set; }
        public abstract string GetProxyCode(Incubator serviceProvider, IHttpContext context);

        public virtual void BeforeWriteProxyCode(Incubator serviceProvider, IHttpContext context)
        {
        }

        public virtual void AfterWriteProxyCode(Incubator serviceProvider, IHttpContext context)
        {
        }

        private Dictionary<string, string> _cachedProxyCode;
        private readonly object _cachedProxyCodeLock = new object();
        public string GetCachedProxyCode(Incubator serviceProvider, IHttpContext context)
        {
            _cachedProxyCodeLock.DoubleCheckLock(ref _cachedProxyCode, () => new Dictionary<string, string>());
            lock (_cachedProxyCodeLock)
            {
                string key = context.Request.Url.ToString();
                if (!_cachedProxyCode.ContainsKey(key))
                {
                    _cachedProxyCode.Add(key, GetProxyCode(serviceProvider, context));
                }

                return _cachedProxyCode[key];
            }
        }

        public void SendProxyCode(Incubator serviceProvider, IHttpContext context)
        {
            Args.ThrowIfNull(context, "context");
            Args.ThrowIfNull(context.Response, "context.Response");
            BeforeWriteProxyCode(serviceProvider, context);
            IResponse response = context.Response;
            byte[] data = Encoding.GetBytes(GetCachedProxyCode(serviceProvider, context));
            response.OutputStream.Write(data, 0, data.Length);
            AfterWriteProxyCode(serviceProvider, context);
        }
    }
}
