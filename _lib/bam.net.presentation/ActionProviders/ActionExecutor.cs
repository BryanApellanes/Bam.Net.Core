using System;
using Bam.Net.Logging;
using Bam.Net.ServiceProxy;
using Bam.Net.ServiceProxy.Secure;

namespace Bam.Net.Presentation.ActionProviders
{
    [Serializable]
    [Proxy("actionExecutor")]
    [ApiKeyRequired]
    public class ActionExecutor : Loggable
    {
        public ActionExecutor(ILogger logger = null)
        {
            if (logger != null)
            {
                Subscribe(logger);
            }
        }
        
        public BackgroundThreadQueue<ExecutionRequest> ExecutionQueue { get; }

        public ActionExecutor RegisterViewModel(ViewModel viewModel)
        {
            throw new NotImplementedException();
        }
    }
}