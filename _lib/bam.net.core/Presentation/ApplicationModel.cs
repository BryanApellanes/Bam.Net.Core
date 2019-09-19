using Bam.Net.Data.Dynamic;
using Bam.Net.Logging;
using Bam.Net.ServiceProxy;
using Bam.Net.Services;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Bam.Net.Configuration;
using Bam.Net.CoreServices;
using Bam.Net.Data.Repositories;
using Bam.Net.Server;

namespace Bam.Net.Presentation
{
    [Singleton]
    public class ApplicationModel
    {
        public ApplicationModel(AppConf appConf)
        {
            AppConf = appConf;
            ApplicationServiceRegistry = ApplicationServiceRegistry.Current;
            Log = ApplicationServiceRegistry.Get<ILog>();
            ApplicationServiceRegistry.Get("Startup", out Type startupType);
            if(startupType != null)
            {
                ApplicationNameSpace = startupType.Namespace;
            }
            WebServiceRegistry = ApplicationServiceRegistry.Get<WebServiceRegistry>();
            ApplicationName = ApplicationServiceRegistry.Get<IApplicationNameProvider>().GetApplicationName();
            ApplicationServiceRegistry.SetInjectionProperties(this);
            RepositoryProviderResolver = ApplicationServiceRegistry.Get<IRepositoryResolver>();
            ApplicationNameProvider = new StaticApplicationNameProvider(appConf.Name);
        }

        protected IRepositoryResolver RepositoryProviderResolver { get; set; }
        protected IApplicationNameProvider ApplicationNameProvider { get; set; }
        public AppConf AppConf { get; set; }
        public string ApplicationNameSpace { get; set; }
        public string ApplicationName { get; set; }

        public ILog Log { get; set; }

        public DataProvider DataProvider => DataProvider.Current;

        public DirectoryInfo DataDirectory => AppConf.AppRoot.GetDirectory("~/data");

        public DirectoryInfo JsonDirectory => new DirectoryInfo(Path.Combine(DataDirectory.FullName, "json"));
        
        public DirectoryInfo YamlDirectory => new DirectoryInfo(Path.Combine(DataDirectory.FullName, "yaml"));

        public DirectoryInfo CsvDirectory => new DirectoryInfo(Path.Combine(DataDirectory.FullName, "csv"));

        public Dictionary<string, FileInfo> JsonFiles => JsonDirectory?.GetFiles("*.json", SearchOption.TopDirectoryOnly).ToDictionary(f => Path.GetFileNameWithoutExtension(f.Name));

        public Dictionary<string, FileInfo> YamlFiles => YamlDirectory?.GetFiles("*.yaml", SearchOption.TopDirectoryOnly).ToDictionary(f=> Path.GetFileNameWithoutExtension(f.Name));

        public Dictionary<string, FileInfo> CsvFiles => CsvDirectory?.GetFiles("*.csv", SearchOption.TopDirectoryOnly).ToDictionary(f => Path.GetFileNameWithoutExtension(f.Name));

        /// <summary>
        /// Get a FileInfo instance for the specified path.  The file may not exist, check the Exists property.
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public FileInfo GetDataFile(params string[] path)
        {
            List<string> segments = new List<string>(path);
            segments.Insert(0, DataDirectory.FullName);
            string filePath = Path.Combine(segments.ToArray());
            return new FileInfo(filePath);
        }

        /// <summary>
        /// Get a DirectoryInfo instance for the specified path.  The directory may not exist, check the Exists property.
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public DirectoryInfo GetDataSubdirectory(params string[] path)
        {
            List<string> segments = new List<string>(path);
            segments.Insert(0, DataDirectory.FullName);
            string dirPath = Path.Combine(segments.ToArray());
            return new DirectoryInfo(dirPath);
        }
        
        public ApplicationServiceRegistry ApplicationServiceRegistry { get; set; }
        public WebServiceRegistry WebServiceRegistry { get; set; }

        public DynamicTypeManager DynamicTypeManager { get; set; }

        [Inject]
        public IViewModelProvider ViewModelProvider { get; set; }

        [Inject]
        public IPersistenceModelProvider PersistenceModelProvider { get; set; }

        [Inject]
        public IExecutionRequestResolver ExecutionRequestResolver { get; set; }

        public PersistenceModel GetPersistenceModel(string persistenceModelName)
        {
            if (string.IsNullOrEmpty(persistenceModelName))
            {
                return null;
            }
            return PersistenceModelProvider.GetPersistenceModel(ApplicationName, persistenceModelName);
        }
        
        public ViewModel GetViewModel(string viewModelName, string persistenceModelName = null)
        {
            return ViewModelProvider.GetViewModel(viewModelName, GetPersistenceModel(persistenceModelName));
        }
    }
}
