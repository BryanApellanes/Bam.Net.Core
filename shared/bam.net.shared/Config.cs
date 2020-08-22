using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Reflection;
using System.Security.Permissions;
using Bam.Net.Data;
using Bam.Net.Logging;
using Bam.Net.Presentation.Handlebars;
using Bam.Net.Server;
using Bam.Net.Services;

namespace Bam.Net
{
    public class Config: Loggable
    {
        static Config()
        {
            ApplicationNameProvider = ProcessApplicationNameProvider.Current;
        }

        public Config() : this(true)
        {
        }

        private Config(bool subscribeToChanges)
        {
            ApplicationName = ProcessApplicationNameProvider.Current.GetApplicationName();
            AppSettings = Read(out FileInfo file);
            File = file;

            if(subscribeToChanges)
            {
                ConfigChangeWatcher = File.OnChange((o, a) =>
                {
                    Config oldConfig = new Config(false)
                    {
                        AppSettings = AppSettings
                    };
                    AppSettings = Read();
                    Config newConfig = this;
                    ConfigChangedEventArgs args = new ConfigChangedEventArgs()
                    {
                        OldConfig = oldConfig,
                        NewConfig = newConfig
                    };
                    FireEvent(ConfigChanged, this, args);
                });
            }
        }

        public Config(string applicationName) : this(applicationName, true)
        {
        }

        private Config(string applicationName, bool subscribeToChanges)
        {
            ApplicationName = applicationName;
            AppSettings = Read(applicationName, out FileInfo file);
            File = file;

            if (subscribeToChanges)
            {
                ConfigChangeWatcher = File.OnChange((o, a) =>
                {
                    Config oldConfig = new Config(applicationName, false)
                    {
                        AppSettings = AppSettings
                    };
                    AppSettings = Read(applicationName);
                    Config newConfig = this;
                    ConfigChangedEventArgs args = new ConfigChangedEventArgs()
                    {
                        OldConfig = oldConfig,
                        NewConfig = newConfig
                    };
                    FireEvent(ConfigChanged, this, args);
                });
            }
        }
        
        public string ApplicationName { get; private set; }
        
        static Config _current;
        static readonly object _currentLock = new object();
        
        /// <summary>
        /// Config for the current process; may be overwritten.
        /// </summary>
        public static Config Current
        {
            get { return _currentLock.DoubleCheckLock(ref _current, () => new Config()); }
            set => _current = value;
        }
        
        public static Config For(string applicationName)
        {
            return new Config(applicationName);
        }
        
        protected FileSystemWatcher ConfigChangeWatcher { get; set; }
        public event EventHandler ConfigChanged;
        public FileInfo File { get; set; }
        
        public Workspace Workspace => Workspace.Current;

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
                    AppSettings.Add(key, defaultValue);
                    Write(AppSettings);
                }

                return defaultValue;
            }
            set
            {
                if (AppSettings.ContainsKey(key))
                {
                    AppSettings[key] = string.IsNullOrEmpty(value) ? defaultValue: value;
                }
                else
                {
                    AppSettings.Add(key, string.IsNullOrEmpty(value) ? defaultValue: value);
                }
                Save();
            }
        }

        static IApplicationNameProvider _applicationNameProvider;
        public static IApplicationNameProvider ApplicationNameProvider
        {
            get => _applicationNameProvider ??= ProcessApplicationNameProvider.Current;

            set => _applicationNameProvider = value;
        }

        public static Dictionary<string, string> ReadFromProfile()
        {
            return ReadFromProfile(out FileInfo ignore);
        }

        public static Dictionary<string, string> ReadFromProfile(out FileInfo configFile)
        {
            configFile = GetBamProfileConfigFile();
            return configFile.FromYamlFile<Dictionary<string, string>>() ?? new Dictionary<string, string>();
        }
        
        public static Dictionary<string, string> Read()
        {
            return Read(out FileInfo ignore);
        }

        public static Dictionary<string, string> Read(out FileInfo configFile)
        {
            configFile = GetBamHomeConfigFile();
            return configFile.FromYamlFile<Dictionary<string, string>>() ?? new Dictionary<string, string>();
        }

        public static Dictionary<string, string> Read(string applicationName)
        {
            return Read(applicationName, out FileInfo ignore);
        }
        
        public static Dictionary<string, string> Read(string applicationName, out FileInfo configFile)
        {
            configFile = GetBamHomeConfigFile(applicationName);
            return configFile.FromYamlFile<Dictionary<string, string>>() ?? new Dictionary<string, string>();
        }

        public void Save()
        {
            if (File == null)
            {
                throw new InvalidOperationException("File not set");
            }
            AppSettings.ToYaml().SafeWriteToFile(File.FullName, true);
        }
        
        public static void Write(Dictionary<string, string> appSettings, bool setBamEnvironmentVariables = true)
        {
            if (setBamEnvironmentVariables)
            {
                foreach (string key in appSettings.Keys)
                {
                    BamEnvironmentVariables.SetBamVariable(key, appSettings[key]);
                }
            }
            FileInfo configFile = GetBamHomeConfigFile();
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
            FileInfo configFile = GetBamHomeConfigFile(applicationNameProvider);
            Config config = new Config
            {
                AppSettings = configFile.FromYamlFile<Dictionary<string, string>>() ?? new Dictionary<string, string>()
            };
            Current = config;
            return config;
        }

        /// <summary>
        /// Load an instance of T from bam configs, creating the config file if necessary.
        /// </summary>
        /// <param name="applicationNameProvider"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static T Load<T>(IApplicationNameProvider applicationNameProvider = null) where T : class, new()
        {
            DirectoryInfo processDir = GetDirectory(applicationNameProvider);
            string fileName = $"{typeof(T).Namespace}.{typeof(T).Name}.config.yaml";
            FileInfo file = new FileInfo(Path.Combine(processDir.FullName, fileName));
            if (!file.Exists)
            {
                (new T()).ToYamlFile(file);
            }
            return file.FromYamlFile<T>();
        }
        
        public static Dictionary<string, string> AppSettingsFor<T>(IApplicationNameProvider applicationNameProvider = null)
        {
            string fileName = $"{typeof(T).Namespace}.{typeof(Type).Name}.appsettings.yaml";
            FileInfo file = new FileInfo(Path.Combine(GetDirectory(applicationNameProvider).FullName, fileName));
            if (file.Exists)
            {
                return file.FromYamlFile<Dictionary<string, string>>() ?? new Dictionary<string, string>();
            }
            return new Dictionary<string, string>();
        }
        
        /// <summary>
        /// Gets the configuration directory for the specified application name provider
        /// </summary>
        /// <param name="applicationNameProvider"></param>
        /// <returns></returns>
        public static DirectoryInfo GetDirectory(IApplicationNameProvider applicationNameProvider = null)
        {
            DirectoryInfo configDir = GetBamHomeConfigFile().Directory;
            applicationNameProvider ??= ProcessApplicationNameProvider.Current;
            string typeConfigsFolderName = applicationNameProvider.GetApplicationName();
            if (string.IsNullOrEmpty(typeConfigsFolderName))
            {
                typeConfigsFolderName = Bam.Net.CoreServices.ApplicationRegistration.Data.Application.Unknown.Name;
            }
            
            return new DirectoryInfo(Path.Combine(configDir.FullName, typeConfigsFolderName));
        }
        
        public static FileInfo GetBamHomeConfigFile(IApplicationNameProvider applicationNameProvider = null)
        {
            applicationNameProvider ??= ProcessApplicationNameProvider.Current;
            Log.Trace("Config using applicationNameProvider of type ({0})", applicationNameProvider?.GetType().Name);
            string providedAppName = applicationNameProvider.GetApplicationName();
            return GetBamHomeConfigFile(providedAppName);
        }
        
        /// <summary>
        /// Get the config file for the specified application from the `.bam` directory of the process
        /// owner's profile.
        /// </summary>
        /// <param name="applicationNameProvider"></param>
        /// <returns></returns>
        public static FileInfo GetBamProfileConfigFile(IApplicationNameProvider applicationNameProvider = null)
        {
            applicationNameProvider ??= ProcessApplicationNameProvider.Current;
            Log.Trace("Config using applicationNameProvider of type ({0})", applicationNameProvider?.GetType().Name);
            string providedAppName = applicationNameProvider.GetApplicationName();
            return GetBamProfileConfigFile(providedAppName);
        }

        public static FileInfo GetBamProfileConfigFile(string appName)
        {
            string assemblyFile = Assembly.GetEntryAssembly().GetFileInfo().FullName;
            string assemblyName = Path.GetFileNameWithoutExtension(assemblyFile);
            string path = !appName.StartsWith("UNKNOWN")
                ? Path.Combine(BamProfile.Config, appName, $"{appName}.appsettings.yaml")
                : Path.Combine(BamProfile.Config, assemblyName, $"{assemblyName}.appsettings.yaml");
            Log.Trace("config file path = {0}", path);
            FileInfo configFile = EnsureFile(path);

            return configFile;
        }
        
        public static FileInfo GetBamHomeConfigFile(string appName)
        {
            string assemblyFile = Assembly.GetEntryAssembly().GetFileInfo().FullName;
            string assemblyName = Path.GetFileNameWithoutExtension(assemblyFile);
            string path = !appName.StartsWith("UNKNOWN")
                ? Path.Combine(BamHome.Config, appName, $"{appName}.appsettings.yaml")
                : Path.Combine(BamHome.Config, assemblyName, $"{assemblyName}.appsettings.yaml");
            Log.Trace("config file path = {0}", path);
            FileInfo configFile = EnsureFile(path);

            return configFile;
        }
        
        /// <summary>
        /// Get the name of the entry assembly without extension.
        /// </summary>
        /// <returns></returns>
        public static string GetHostServiceName()
        {
            string assemblyFile = Assembly.GetEntryAssembly().GetFileInfo().FullName;
            return Path.GetFileNameWithoutExtension(assemblyFile);
        }
        
        protected static FileInfo EnsureFile(string path)
        {
            FileInfo configFile = new FileInfo(path);
            if (!configFile.Exists)
            {
                if (!configFile.Directory.Exists)
                {
                    configFile.Directory.Create();
                }

                System.IO.File.Create(configFile.FullName).Dispose();
            }

            return configFile;
        }
    }
}