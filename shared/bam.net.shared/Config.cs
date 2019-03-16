using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Security.Permissions;
using Bam.Net.Data;
using Bam.Net.Logging;
using Bam.Net.Server;
using Bam.Net.Services;
using Lucene.Net.Analysis.Hunspell;
using Lucene.Net.Queries.Function.ValueSources;

namespace Bam.Net
{
    public class Config: Loggable
    {
        static Config()
        {
            ApplicationNameProvider = ProcessApplicationNameProvider.Current;
        }
        
        public Config()
        {
            AppSettings = Read(out FileInfo file);
            File = file;
            ConfigChangeWatcher = File.OnChange((o, a) =>
            {
                Config oldConfig = this.CopyAs<Config>();
                AppSettings = Read();
                Config newConfig = this;
                ConfigChangedEventArgs args = new ConfigChangedEventArgs()
                {
                    OldConfig = oldConfig, NewConfig = newConfig
                };
                FireEvent(ConfigChanged, this, args);
            });
        }

        static Config _current;
        static object _currentLock = new object();
        public static Config Current
        {
            get { return _currentLock.DoubleCheckLock(ref _current, () => new Config()); }
            set { _current = value; }
        }
        
        protected FileSystemWatcher ConfigChangeWatcher { get; set; }
        public event EventHandler ConfigChanged;
        public FileInfo File { get; set; }
        public Dictionary<string, string> AppSettings { get; set; }

        public string this[string key, string defaultValue = null]
        {
            get
            {
                if (AppSettings.ContainsKey(key))
                {
                    return AppSettings[key];
                }

                if (!string.IsNullOrEmpty(defaultValue))
                {
                    BamEnvironmentVariables.SetBamVariable(key, defaultValue);
                    AppSettings.Add(key, defaultValue);
                }

                return defaultValue;
            }
        }

        public static IApplicationNameProvider ApplicationNameProvider
        {
            get;
            set;
        }
        
        public static Dictionary<string, string> Read()
        {
            return Read(out FileInfo ignore);
        }

        public static Dictionary<string, string> Read(out FileInfo configFile)
        {
            configFile = GetConfigFile();
            return configFile.FromYamlFile<Dictionary<string, string>>() ?? new Dictionary<string, string>();
        }
        
        public static void Write(Dictionary<string, string> appSettings)
        {
            foreach (string key in appSettings.Keys)
            {
                BamEnvironmentVariables.SetBamVariable(key, appSettings[key]);
            }
            FileInfo configFile = GetConfigFile();
            if (configFile.Exists)
            {
                Dictionary<string, string> existing = configFile.FullName.FromYamlFile<Dictionary<string, string>>() ?? new Dictionary<string, string>();
                foreach (string key in existing.Keys)
                {
                    appSettings.AddMissing(key, existing[key]);
                }
            }
            appSettings.ToYaml().SafeWriteToFile(configFile.FullName, true);
        }

        public static Config Set(IApplicationNameProvider applicationNameProvider)
        {
            Args.ThrowIfNull(applicationNameProvider, "applicationNameProvider");
            FileInfo configFile = GetConfigFile(applicationNameProvider);
            Config config = new Config
            {
                AppSettings = configFile.FromYamlFile<Dictionary<string, string>>()
            };
            Current = config;
            return config;
        }
        
        public static FileInfo GetConfigFile(IApplicationNameProvider applicationNameProvider = null)
        {
            string assemblyFile = Assembly.GetEntryAssembly().GetFileInfo().FullName;
            string configName = Path.GetFileNameWithoutExtension(assemblyFile);
            string path = applicationNameProvider != null
                ? Path.Combine(BamPaths.ConfPath, configName, $"{applicationNameProvider.GetApplicationName()}.yaml")
                : Path.Combine(BamPaths.ConfPath, configName, $"{configName}.yaml"); 
            FileInfo configFile = new FileInfo(path);
            if (!configFile.Exists)
            {
                if (!configFile.Directory.Exists)
                {
                    configFile.Directory.Create();
                }
                configFile.Create();
            }
            return configFile;
        }
    }
}