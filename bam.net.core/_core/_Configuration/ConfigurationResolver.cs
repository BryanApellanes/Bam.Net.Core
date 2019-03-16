using Bam.Net.Logging;
using Bam.Net.Services;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration;
using System.IO;
using System.Reflection;
using System.Text;
using Lucene.Net.Analysis.Hunspell;

namespace Bam.Net.Configuration
{
    public partial class ConfigurationResolver: Loggable
    {
        public ConfigurationResolver(ILogger logger = null)
        {
            DefaultConfiguration = ConfigurationManager.AppSettings;
            ConfigurationProvider = new DefaultConfigurationProvider();
            AppSettings = Config.Read();
        }

        public ConfigurationResolver(IConfiguration configuration, ILogger logger = null)
        {
            Logger = logger ?? Log.Default;
            NetCoreConfiguration = configuration;
            DefaultConfiguration = ConfigurationManager.AppSettings;
            AppSettings = Config.Read();
        }
        
        public Dictionary<string, string> AppSettings { get; set; }

        public IConfiguration NetCoreConfiguration { get; set; }
        public NameValueCollection DefaultConfiguration { get; set; }

        [Inject]
        public ILogger Logger { get; set; }

        [Inject]
        public IConfigurationProvider ConfigurationProvider { get; set; }

        public string this[string key, string defaultValue = null, bool callConfigService = false]
        {
            get
            {
                string value = NetCoreConfiguration?[key];
                if (string.IsNullOrEmpty(value))
                {
                    value = DefaultConfiguration[key];
                }

                if (string.IsNullOrEmpty(value) && AppSettings.ContainsKey(key))
                {
                    value = AppSettings[key];
                }
                if (string.IsNullOrEmpty(value))
                {
                    value = BamEnvironmentVariables.GetBamVariable(key);
                }

                if (!string.IsNullOrEmpty(defaultValue))
                {
                    value = defaultValue;
                }
                if(string.IsNullOrEmpty(value) && callConfigService)
                {
                    value = FromService(key);
                }
                if (string.IsNullOrEmpty(value))
                {
                    FireEvent(ConfigurationValueNotFound, new ConfigurationEventArgs { Key = key });
                }
                if (!string.IsNullOrEmpty(value))
                {
                    AppSettings.AddMissing(key, value);
                    Config.Write(AppSettings);
                }
                return value;
            }       
        }

        public static void Startup(IConfiguration configuration)
        {
            Current = new ConfigurationResolver(configuration);
        }
        
        protected string ApplicationName
        {
            get
            {
                return this["ApplicationName"];
            }
        }

        public event EventHandler CallingConfigService;
        public event EventHandler CalledConfigService;

        public event EventHandler RetrievingFromService;
        public event EventHandler RetrievedFromCache;
        public event EventHandler RetrievedFromService;

        public event EventHandler ConfigurationValueNotFound;

        Dictionary<string, string> _config;
        private string FromService(string key)
        {
            FireEvent(CallingConfigService, new ConfigurationEventArgs { Key = key });
            if (_config != null)
            {
                if (_config.ContainsKey(key))
                {
                    string value = _config[key];
                    FireEvent(RetrievedFromCache, new ConfigurationEventArgs { Key = key });
                    return value;
                }
            }
            if (ConfigurationProvider != null && !string.IsNullOrEmpty(ApplicationName))
            {
                FireEvent(RetrievingFromService, new ConfigurationEventArgs { Key = key });
                _config = ConfigurationProvider.GetApplicationConfiguration(ApplicationName);
                if(_config != null && _config.ContainsKey(key))
                {
                    string value = _config[key];
                    FireEvent(RetrievedFromCache, new ConfigurationEventArgs { Key = key, Value = value });
                    return value;
                }
            }            
            return string.Empty;
        }
    }
}
