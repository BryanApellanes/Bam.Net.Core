using System;
using Bam.Net.Application;

namespace Bam.Net
{
    public static class BamEnvironmentVariables
    {
        const string BAM_HOME = "BAM_HOME";
        const string BAM_APPLICATION_NAME = "BAM_APPLICATION_NAME";
        const string BAM_APP_KIND = "BAM_APP_KIND";
        const string BAM_PROCESS_MODE = "BAM_PROCESS_MODE";

        public static string Home(string value = null)
        {
            if (!string.IsNullOrEmpty(value))
            {
                Set(BAM_HOME, value);
            }

            return Get(BAM_HOME).Or(BamHome.Path);
        }

        public static void SetApplicationName(string applicationName)
        {
            Set(BAM_APPLICATION_NAME, applicationName);
        }
        
        public static string ApplicationName(string applicationName = null)
        {
            if (applicationName != null)
            {
                SetApplicationName(applicationName);
            }

            return Get(BAM_APPLICATION_NAME).Or(ApplicationNameProvider.Default.GetApplicationName());
        }

        public static AppKind AppKind()
        {
            return Get(BAM_APP_KIND).ToEnum<AppKind>();
        }
        
        public static void AppKind(AppKind appKind)
        {
            Set(BAM_APP_KIND, appKind.ToString());
        }
        
        public static ProcessModes ProcessMode()
        {
            return Get(BAM_PROCESS_MODE).ToEnum<ProcessModes>();
        }
        
        public static void ProcessMode(ProcessModes mode)
        {
            Set(BAM_PROCESS_MODE, mode.ToString());
        }

        /// <summary>
        /// Gets the bam environment variable for the specified name.  The environment
        /// variable name is prefixed by BAM_. 
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public static string GetBamVariable(string name)
        {
            return Get($"BAM_{name}");
        }

        public static void SetBamVariable(string name, string value)
        {
            Set($"BAM_{name}", value);
        }
        
        private static void Set(string name, string value)
        {
            Environment.SetEnvironmentVariable(name, value);
        }

        private static string Get(string name)
        {
            return Environment.GetEnvironmentVariable(name);
        }
    }
}