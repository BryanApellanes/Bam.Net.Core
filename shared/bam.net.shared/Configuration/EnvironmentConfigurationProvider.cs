using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace Bam.Net.Configuration
{
    public class EnvironmentConfigurationProvider : IConfigurationProvider
    {
        public Dictionary<string, string> GetApplicationConfiguration(string applicationName, string configurationName = "")
        {
            IDictionary environmentVariables = Environment.GetEnvironmentVariables();
            Dictionary<string, string> config = new Dictionary<string, string>();
            foreach(object key in environmentVariables.Keys)
            {
                config.Add(key?.ToString(), environmentVariables[key]?.ToString());
            }
            return config;
        }

        public void SetApplicationConfiguration(string applicationName, Dictionary<string, string> configuration, string configurationName)
        {
            Environment.SetEnvironmentVariable("BAM_APPLICATION", applicationName);
            foreach(string key in configuration.Keys)
            {
                Environment.SetEnvironmentVariable(key, configuration[key]);
            }
        }
    }
}
