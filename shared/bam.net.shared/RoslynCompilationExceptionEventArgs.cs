using System;
using Bam.Net.Server;

namespace Bam.Net
{
    public class RoslynCompilationExceptionEventArgs : EventArgs
    {
        public RoslynCompilationExceptionEventArgs(AppConf appConf, RoslynCompilationException roslynCompilationException)
        {
            AppConf = appConf;
            Exception = roslynCompilationException;
        }

        public AppConf AppConf { get; set; }
        public RoslynCompilationException Exception { get; set; }
    }
}