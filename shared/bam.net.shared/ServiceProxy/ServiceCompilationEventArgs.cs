using System;
using Bam.Net.Server;

namespace Bam.Net.ServiceProxy
{
    public class ServiceCompilationEventArgs : EventArgs
    {
        public Exception Exception { get; set; }
        public ApplicationServiceAssembly ApplicationServiceAssembly { get; set; }
        public AppConf AppConf { get; set; }
    }
}