using System;

namespace Bam.Net.ServiceProxy
{
    public interface IServiceCompilationExceptionReporter
    {
        Action<object, ServiceCompilationEventArgs> Reporter { get; set; } 
        void ReportCompilationException(object sender, ServiceCompilationEventArgs args);
    }
}