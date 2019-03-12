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
using Bam.Net.Data.Repositories;

namespace Bam.Net.Presentation
{
    [Singleton]
    public class ApplicationModel
    {
        public ApplicationModel(ApplicationServiceRegistry applicationServiceRegistry)
        {
            ApplicationServiceRegistry = applicationServiceRegistry;
            Log = ApplicationServiceRegistry.Get<ILog>();
            ApplicationServiceRegistry.Get("Startup", out Type startupType);
            if(startupType != null)
            {
                ApplicationNameSpace = startupType.Namespace;
            }
            WebServiceRegistry = applicationServiceRegistry.Get<WebServiceRegistry>();
            ApplicationName = ApplicationServiceRegistry.Get<IApplicationNameProvider>().GetApplicationName();
            ApplicationServiceRegistry.SetInjectionProperties(this);
            DataProviderResolver = ApplicationServiceRegistry.Get<IDataProviderResolver>();
            ApplicationNameProvider = ApplicationServiceRegistry.Get<IApplicationNameProvider>();
        }

        protected IDataProviderResolver DataProviderResolver { get; set; }
        protected IApplicationNameProvider ApplicationNameProvider { get; set; }
        
        public string ApplicationNameSpace { get; set; }
        public string ApplicationName { get; set; }

        public ILog Log { get; set; }

        public DefaultDataProvider DataProvider
        {
            get
            {
                //DataSettingsResolver.Resolve()
                throw new NotImplementedException();
            }
        }
        
        public DirectoryInfo DataDirectory
        {
            get
            {
                //return DataDirectoryProvider.Resolve()
                throw new NotImplementedException();
            }
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
