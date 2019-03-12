using Bam.Net.Configuration;
using Bam.Net.CoreServices;
using Bam.Net.Logging;
using Bam.Net.Presentation;
using Bam.Net.Services.Clients;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Bam.Net.Data.Repositories;
using Bam.Net.Server;
using Bam.Net.ServiceProxy;

namespace Bam.Net.Services
{
    /// <summary>
    /// A service registry (or dependency injection container) for the currently running application process.  The application name is
    /// determined by the default configuration file (app.config or web.config).
    /// </summary>
    public class ApplicationServiceRegistry: ServiceRegistry
    {
        protected CoreClient CoreClient { get; set; }

        static ApplicationServiceRegistry _appRegistry;
        static object _appRegistryLock = new object();
        public static ApplicationServiceRegistry Current
        {
            get
            {
                return _appRegistryLock.DoubleCheckLock(ref _appRegistry, () => Configure(Configurer ?? ((reg) => { })));
            }
            private set
            {
                _appRegistry = value;
            }
        }

        public static Task<ApplicationServiceRegistry> Discovered
        {
            get
            {
                return Task.Run(() => Discover());
            }
        }

        public static Action<ApplicationServiceRegistry> Configurer { get; set; }

        public static ApplicationServiceRegistry Discover()
        {
            return Discover(Assembly.GetEntryAssembly().GetFileInfo().Directory.FullName);
        }

        public static ApplicationServiceRegistry Discover(string directoryPath)
        {
            return Discover(new DirectoryInfo(directoryPath));
        }

        public static ApplicationServiceRegistry ForApplication()
        {
            return ForApplication(DefaultConfigurationApplicationNameProvider.Instance.GetApplicationName());
        }

        static Dictionary<string, ApplicationServiceRegistry> _appRegistries = new Dictionary<string, ApplicationServiceRegistry>();
        public static ApplicationServiceRegistry ForApplication(string applicationName)
        {
            try
            {
                if (!_appRegistries.ContainsKey(applicationName))
                {
                    DirectoryInfo directoryInfo = Assembly.GetEntryAssembly().GetFileInfo().Directory;
                    _appRegistries.Add(applicationName, Configure((appSvcReg) =>
                    {
                        ForEachAssemblyIn(directoryInfo, file => TryAddTypes(appSvcReg, file, (t) =>
                        {
                            return t.HasCustomAttributeOfType(out AppProviderAttribute attr) && (attr?.ApplicationName?.Equals(applicationName)).Value;
                        }));
                    }));
                }
                return _appRegistries[applicationName];
            }
            catch (Exception ex)
            {
                Log.Warn("Exception discovering application services: {0}", ex, ex.Message);
            }
            return Configure(a => { });
        }

        static object _discoverLock = new object();
        static ApplicationServiceRegistry _discoveredApplicationServiceRegistry;
        public static ApplicationServiceRegistry Discover(DirectoryInfo directoryInfo)
        {
            try
            {
                return _discoverLock.DoubleCheckLock(ref _discoveredApplicationServiceRegistry, () =>
                {
                    return Configure((appServiceRegistry) =>
                    {
                        ForEachAssemblyIn(directoryInfo, file => TryAddTypes(appServiceRegistry, file));
                    });
                });                
            }
            catch (Exception ex)
            {
                Log.Warn("Exception discovering services: {0}", ex, ex.Message);
            }
            return Configure(a => { });
        }

        [ServiceRegistryLoader]
        public static ApplicationServiceRegistry Configure(Action<ApplicationServiceRegistry> configure)
        {
            Configurer = configure;
            ApplicationServiceRegistry appRegistry = new ApplicationServiceRegistry();
            appRegistry.CombineWith(CoreClientServiceRegistryContainer.Current);
            appRegistry
                .For<IDataProviderResolver>().Use<DefaultDataProvider>()
                .For<IApplicationNameProvider>().Use<DefaultConfigurationApplicationNameProvider>()
                .For<IIncludesResolver>().Use<IncludesResolver>()
                .For<ProxyAssemblyGeneratorService>().Use<ProxyAssemblyGeneratorServiceProxy>()
                .For<ApplicationServiceRegistry>().Use(appRegistry)
                .For<IViewModelProvider>().Use<DefaultViewModelProvider>()
                .For<IPersistenceModelProvider>().Use<DefaultPersistenceModelProvider>()
                .For<IExecutionRequestResolver>().Use<ExecutionRequestResolver>()
                .For<ApplicationModel>().Use<ApplicationModel>();

            configure(appRegistry);
            appRegistry.CoreClient = appRegistry.Get<CoreClient>();
            Current = appRegistry;
            return appRegistry;
        }

        private static void ForEachAssemblyIn(DirectoryInfo directoryInfo, Action<FileInfo> action)
        {
            foreach (FileInfo file in directoryInfo.GetFiles().Where(file =>
            {
                return (file.Extension?.Equals(".dll", StringComparison.InvariantCultureIgnoreCase)).Value || (file.Extension?.Equals(".exe", StringComparison.InvariantCultureIgnoreCase)).Value;
            }))
            {
                action(file);
            }
        }

        private static void TryAddTypes(ApplicationServiceRegistry appServiceRegistry, FileInfo file)
        {
            TryAddTypes(appServiceRegistry, file, t => t.HasCustomAttributeOfType<AppProviderAttribute>());
        }

        private static void TryAddTypes(ApplicationServiceRegistry appServiceRegistry, FileInfo file, Func<Type, bool> predicate)
        {
            try
            {
                Assembly assembly = Assembly.LoadFile(file.FullName);
                Type[] types = assembly.GetTypes();
                foreach (Type type in types.Where(predicate))
                {
                    appServiceRegistry.Set(type, type);
                }
            }
            catch (Exception ex)
            {
                Log.Warn("Exception loading file for service discovery {0}: {1}", ex, file.FullName, ex.Message);
            }
        }
    }
}
