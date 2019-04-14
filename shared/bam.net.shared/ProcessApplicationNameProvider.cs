using System.IO;
using System.Reflection;
using Bam.Net.CoreServices.ApplicationRegistration.Data;

namespace Bam.Net
{
    /// <summary>
    /// Gets the Application name from the environment variable BAM_ApplicationName
    /// or the name of the entry assembly if it is not set.  Will set the environment variable BAM_APPLICATION_NAME
    /// to the name of the entry assembly if BAM_ApplicationName is not set or set to
    /// a value prefixed by "UNKNOWN".
    /// </summary>
    public class ProcessApplicationNameProvider: IApplicationNameProvider
    {
        public const string ApplicationName = "ApplicationName";
        public string GetApplicationName()
        {
            string name = BamEnvironmentVariables.GetBamVariable("ApplicationName");
            if (string.IsNullOrEmpty(name) || name.StartsWith("UNKNOWN"))
            {
                name = Config.GetHostServiceName();
                BamEnvironmentVariables.SetApplicationName(name);
            }
            return name;
        }

        static ProcessApplicationNameProvider _applicationNameProvider;
        static object _applicationNameProviderLock = new object();
        public static ProcessApplicationNameProvider Current
        {
            get
            {
                return _applicationNameProviderLock.DoubleCheckLock(ref _applicationNameProvider,
                    () => new ProcessApplicationNameProvider());
            }
        }
    }
}