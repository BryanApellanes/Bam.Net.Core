/*
	Copyright © Bryan Apellanes 2015  
*/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Reflection;
using Bam.Net.UserAccounts;
using Bam.Net.Data;
using Bam.Net.Logging;
using Bam.Net.ServiceProxy;
using Bam.Net.Server;
using Newtonsoft.Json;
using System.Xml.Serialization;
using Bam.Net.Incubation;
using Bam.Net.Configuration;
using Bam.Net.ServiceProxy.Secure;
using YamlDotNet.Serialization;

namespace Bam.Net.Server
{
    /// <summary>
    /// Configuration for a Bam Application
    /// </summary>
    public class AppConf : IApplicationNameProvider
    {
		public const string DefaultPageConst = "start";
		public const string DefaultLayoutConst = "basic";
        public const string DefaultHtmlDirConst = "pages";
        public const string DefaultBinDirConst = "bin";

        public AppConf()
        {
            _serviceTypeNames = new List<string>();
            _schemaInitializers = new List<SchemaInitializer>();
            _serviceTypeNames.Add(typeof(Echo).AssemblyQualifiedName);
            _serviceTypeNames.Add(typeof(EncryptedEcho).AssemblyQualifiedName);

            AppSettings = new AppSetting[] { };
			RenderLayoutBody = true;
			DefaultLayout = DefaultLayoutConst;
			DefaultPage = DefaultPageConst;
			ServiceSearchPattern = new string[] { "*Services.dll", "*Proxyables.dll" };
            ServiceReferences = new string[] { };
            ProcessMode = ProcessModes.Dev;
            ServerConf = new AppServerConf(ServerKinds.Bam);
            Name = ApplicationNameProvider.Default.GetApplicationName();
        }

        public AppConf(string name, int port = 8080, bool ssl = false)
            : this()
        {
            Name = name;
            GenerateDao = true;
            Bindings = new HostPrefix[] { new HostPrefix { HostName = name, Port = port, Ssl = ssl } };
        }

        public AppConf(BamConf serverConf, string name)
            : this(name)
        {
            BamConf = serverConf;
        }

        public static AppConf FromDefaultConfig()
        {
            return new AppConf { Name = DefaultConfiguration.GetAppSetting("ApplicationName", ServiceProxy.Secure.Application.Unknown.Name) };
        }

        public static AppConf FromConfig()
        {
            return new AppConf{ Name = Config.Current.ApplicationName};
        }
        
        Fs _appRoot;
        readonly object _appRootLock = new object();
        [JsonIgnore]
        [XmlIgnore]
        [YamlIgnore]
        public Fs AppRoot
        {
            get
            {
                return _appRootLock.DoubleCheckLock(ref _appRoot, () => new Fs(Path.Combine(BamConf.ContentRoot, "apps", Name)));
            }
        }

        public BamServer GetServer()
        {
            return BamConf.Server;
        }

        public void AddServices(Incubator incubator)
        {
            BamConf.Server.AddAppServices(Name, incubator);
        }

        /// <summary>
        /// Add a service of the specified type to be 
        /// instantiated by the specified function
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="serviceInstanciator"></param>
        public void AddService<T>(Func<T> serviceInstanciator)
        {
            BamConf.Server.AddAppService<T>(Name, serviceInstanciator);
        }

        /// <summary>
        /// Add the specified instance as a service to 
        /// be exposed to the client
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="instance"></param>
        public void AddService<T>(T instance)
        {
            BamConf.Server.AddAppService<T>(Name);
        }

        /// <summary>
        /// The name of the application.  This will typically be the name of the 
        /// folder being served from the "apps" folder of the root
        /// of the BamServer depending on how the AppConf was instantiated.
        /// </summary>
        public string Name { get; set; }

        public string GetApplicationName()
        {
            return Name;
        }

        string _displayName;
        public string DisplayName 
        {
            get 
            {
                if (string.IsNullOrEmpty(_displayName)) 
                {
                    _displayName = Name?.PascalSplit(" ") ?? "AppConf.DisplayName not specified";
                }

                return _displayName;
            }
            set => _displayName = value;
        }
        
        public Workspace GetAppWorkspace()
        {
            return Workspace.ForApplication(new StaticApplicationNameProvider(Name));
        }

        public Workspace GetAppContentWorkspace()
        {
            return Workspace.ForApplication();
        }
        
        [JsonIgnore]
        [XmlIgnore]
        [YamlIgnore]
        public bool IsProd => ProcessMode == ProcessModes.Prod;
        
        [JsonIgnore]
        [XmlIgnore]
        [YamlIgnore]
        public bool IsTest => ProcessMode == ProcessModes.Test;
        
        [JsonIgnore]
        [XmlIgnore]
        [YamlIgnore]
        public bool IsDev => ProcessMode == ProcessModes.Dev;

        public ProcessModes ProcessMode { get; set; } 

        public AppServerConf ServerConf { get; set; }
        
        List<HostPrefix> _bindings;
        readonly object _bindingsLock = new object();
        public HostPrefix[] Bindings
        {
            get
            {
                return _bindingsLock.DoubleCheckLock(ref _bindings, () =>
                {
                    List<HostPrefix> result = new List<HostPrefix>
                    {
                        new HostPrefix
                        {
                            HostName = $"{Name}.bamapps.net",
                            Port = 80,
                            Ssl = false
                        },
                        new HostPrefix
                        {
                            HostName = Name,
                            Port = 8080,
                            Ssl = false
                        }
                    };

                    return result;
                }).ToArray();
            }
            set => _bindings = new List<HostPrefix>(value);
        }

        public AppSetting[] AppSettings { get; set; }
        /// <summary>
        /// The name of the default layout 
        /// </summary>
        public string DefaultLayout { get; set; }

		string _defaultPage;
		/// <summary>
		/// The name of the page to serve when the 
		/// root is requested "/".
		/// </summary>
		public string DefaultPage
		{
			get => _defaultPage.Or(DefaultPageConst);
            set => _defaultPage = value.Or(DefaultPageConst);
        }

        string _defaultHtmlDir;
        public string HtmlDir
        {
            get => _defaultHtmlDir.Or(DefaultHtmlDirConst);
            set => _defaultHtmlDir = value.Or(DefaultHtmlDirConst);
        }

        private string _defaultBinDir;
        public string BinDir
        {
            get => _defaultBinDir.Or(DefaultBinDirConst);
            set => _defaultBinDir = value.Or(DefaultBinDirConst);
        }
        
        public bool GenerateDao { get; set; }

        public bool CheckDaoHashes { get; set; }

		public bool RenderLayoutBody { get; set; }

		public string[] ServiceSearchPattern { get; set; }

        /// <summary>
        /// An array of net core assembly names that must be referenced in order to compile application services.
        /// </summary>
        public string[] ServiceReferences { get; set; }
        
        List<string> _serviceTypeNames;
        public string[] ServiceTypeNames
        {
            get => _serviceTypeNames.ToArray();
            set => _serviceTypeNames = new List<string>(value);
        }

        List<SchemaInitializer> _schemaInitializers;
        public SchemaInitializer[] SchemaInitializers
        {
            get => _schemaInitializers.ToArray();
            set => _schemaInitializers = new List<SchemaInitializer>(value);
        }

        /// <summary>
        /// The assembly qualified name of an IAppInitializer
        /// implementation that is called on application 
        /// initialization.
        /// </summary>
        public string AppInitializer
        {
            get;
            set;
        }

		/// <summary>
		/// The file path to the assembly that contains
		/// the type specified by AppInitializer
		/// </summary>
        public string AppInitializerAssemblyPath
        {
            get;
            set;
        }

        UserManagerConfig _userManagerConfig;
        public UserManagerConfig UserManagerConfig
        {
            get => _userManagerConfig ?? (_userManagerConfig = new UserManagerConfig());
            set => _userManagerConfig = value;
        }

        UserManager _userManager;
        public UserManager GetUserManager()
        {
            return _userManager ?? (_userManager = UserManagerConfig.Create(Logger));
        }

        public string Setting(string settingName, string insteadIfNullOrEmpty)
        {
            AppSetting setting = AppSettings.FirstOrDefault(s => s.Name.Equals(settingName));
            if(setting != null)
            {
                return setting.Value;
            }
            Logger.AddEntry("AppConf.AppSettings[{0}]: Setting Name ({0}), not found; using value ({1}) instead", LogEventType.Warning, settingName, insteadIfNullOrEmpty);
            return insteadIfNullOrEmpty;
        }

        /// <summary>
        /// Get the application id used in the dom by parsing the appName.
        /// </summary>
        /// <param name="appName"></param>
        /// <returns></returns>
        public static string DomApplicationIdFromAppName(string appName)
        {
            string[] split = appName.DelimitSplit(".");
            string result = split[0];
            if (split.Length == 3)
            {
                result = split[1];
            }

            AppNamesByDomAppId[result] = appName;

            return result;
        }

        public override string ToString()
        {
            return $"{DisplayName}::{string.Join(" ", Bindings.Select(b => b.ToString()))}";
        }

        public ILogger GetLogger() // methods don't serialize
        {
            return Logger;
        }

        internal BamConf BamConf
        {
            get;
            set;
        }

        internal ILogger Logger => BamConf?.Server?.MainLogger ?? Log.Default;

        static Dictionary<string, string> _appNamesByDomAppId;
        static readonly object _domAppIdsSync = new object();
        protected internal static Dictionary<string, string> AppNamesByDomAppId
        {
            get
            {
                return _domAppIdsSync.DoubleCheckLock(ref _appNamesByDomAppId, () => new Dictionary<string, string>());
            }
        }
    }
}
