using System;
using Bam.Net.Testing;

namespace Bam.Net.ServiceProxy
{
    public class ServiceCompilationExceptionReporter : CommandLineTestInterface, IServiceCompilationExceptionReporter
    {
        public ServiceCompilationExceptionReporter()
        {
            Reporter = DefaultReporter;
        }

        public ServiceCompilationExceptionReporter(Action<object, ServiceCompilationEventArgs> reporter)
        {
            Reporter = reporter;
        }

        private static Action<object, ServiceCompilationEventArgs> _defaultReporter;
        private static object _defaultReporterLock = new object();
        public static Action<object, ServiceCompilationEventArgs> DefaultReporter
        {
            get 
            { 
                return _defaultReporterLock.DoubleCheckLock(ref _defaultReporter, () =>
                {
                    return (o, args) => OutLineFormat("*** APP=({0} [{1}]) Service Compilation Exception ***\r\n\r\n{2}\r\n\r\n{3}", args?.AppConf?.Name, args?.AppConf?.DisplayName, args?.Exception?.Message, args?.Exception?.StackTrace);
                }); 
            }
        }

        public Action<object, ServiceCompilationEventArgs> Reporter { get; set; } 
        
        public void ReportCompilationException(object sender, ServiceCompilationEventArgs args)
        {
            Reporter(sender, args);
        }
    }
}