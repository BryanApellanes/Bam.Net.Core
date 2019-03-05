using Bam.Net.Logging;
using Bam.Net.Services.Clients;
using System;
using System.Collections.Generic;
using System.Text;

namespace Bam.Net.Configuration
{
    public class CompositeConfigurationService : Loggable, IConfigurationService
    {
        public CompositeConfigurationService()
        {
            ConfigurationServices = new HashSet<IConfigurationService>
            {
                new EnvironmentConfigurationService(),
                new DefaultConfigurationService(),
                new ApplicationConfigurationProvider(CoreClient.Heart)
            };
        }

        public HashSet<IConfigurationService> ConfigurationServices
        {
            get;
        }

        public event EventHandler ConfigOverridden;
        public Dictionary<string, string> GetApplicationConfiguration(string applicationName, string configurationName = "")
        {
            Dictionary<string, string> config = new Dictionary<string, string>();
            IConfigurationService current = null;
            foreach(IConfigurationService configService in ConfigurationServices)
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
            foreach(IConfigurationService configService in ConfigurationServices)
            {
                configService.SetApplicationConfiguration(applicationName, configuration, configurationName);
            }
        }
    }
}
