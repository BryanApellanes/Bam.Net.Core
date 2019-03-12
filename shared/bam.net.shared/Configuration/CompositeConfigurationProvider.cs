using Bam.Net.Logging;
using Bam.Net.Services.Clients;
using System;
using System.Collections.Generic;
using System.Text;

namespace Bam.Net.Configuration
{
    public class CompositeConfigurationProvider : Loggable, IConfigurationProvider
    {
        public CompositeConfigurationProvider()
        {
            ConfigurationServices = new HashSet<IConfigurationProvider>
            {
                new EnvironmentConfigurationProvider(),
                new DefaultConfigurationProvider(),
                new ApplicationConfigurationProvider(CoreClient.Heart)
            };
        }

        public HashSet<IConfigurationProvider> ConfigurationServices
        {
            get;
        }

        public event EventHandler ConfigOverridden;
        public Dictionary<string, string> GetApplicationConfiguration(string applicationName, string configurationName = "")
        {
            Dictionary<string, string> config = new Dictionary<string, string>();
            IConfigurationProvider current = null;
            foreach(IConfigurationProvider configService in ConfigurationServices)
            {
                Dictionary<string, string> currentConfig = configService.GetApplicationConfiguration(applicationName, configurationName);
                foreach(string key in currentConfig.Keys)
                {
                    if (config.ContainsKey(key))
                    {
                        FireEvent(ConfigOverridden, new ConfigurationConflictEventArgs { Key = key, WinningValue = currentConfig[key], OverriddenValue = config[key], WinningConfigurationServiceType = configService.GetType(), OverriddenConfigurationServiceType = current?.GetType() });
                    }
                }
                current = configService;
            }
            return config;
        }

        public void SetApplicationConfiguration(string applicationName, Dictionary<string, string> configuration, string configurationName)
        {
            foreach(IConfigurationProvider configService in ConfigurationServices)
            {
                configService.SetApplicationConfiguration(applicationName, configuration, configurationName);
            }
        }
    }
}
