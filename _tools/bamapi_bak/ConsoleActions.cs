using Bam.Net.CommandLine;
using Bam.Net.Configuration;
using Bam.Net.CoreServices;
using Bam.Net.CoreServices.ServiceRegistration.Data.Dao.Repository;
using Bam.Net.Data.Repositories;
using Bam.Net.Logging;
using Bam.Net.Server;
using Bam.Net.ServiceProxy;
using Bam.Net.Testing;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using Bam.Net.Yaml;
using Bam.Net.CoreServices.ServiceRegistration.Data;

namespace Bam.Net.Application
{
    [Serializable]
    public class ConsoleActions : CommandLineTestInterface
    {
        static string contentRootConfigKey = "ContentRoot";
        static string defaultContentRoot = BamHome.Content;
        static string defaultSvcScriptsSrcPath = BamHome.SvcScriptsSrcPath;

        static BamSvcServer _bamSvcServer;
        
        [ConsoleAction("startBamSvcServer", "Start the BamSvc server")]
        public void StartBamSvcServerAndPause()
        {
            ConsoleLogger logger = GetLogger();
            StartBamRpcServer(logger);
            Pause("BamRpc is running");
        }

        [ConsoleAction("killBamSvcServer", "Kill the BamSvc server")]
        public void StopBamSvcServer()
        {
            if (_bamSvcServer != null)
            {
                _bamSvcServer.Stop();
                Pause("BamRpc stopped");
            }
            else
            {
                OutLine("BamRpc server not running");
            }
        }

        [ConsoleAction("serve", "Start the BamSvc server serving a specific service class")]
        public void Serve()
        {
            try
            {
                string serviceClassName = GetArgument("serve", "Enter the name of the class to serve ");
                string contentRoot = GetArgument("ContentRoot", $"Enter the path to the content root (default: {defaultContentRoot}) ");                
                Type serviceType = GetServiceType(serviceClassName, out Assembly assembly);
                if(serviceType == null)
                {
                    throw new InvalidOperationException($"The type {serviceClassName} was not found in the assembly {assembly.GetFilePath()}");
                }
                HostPrefix[] prefixes = ServiceConfig.GetConfiguredHostPrefixes();
                if (serviceType.HasCustomAttributeOfType(out ServiceSubdomainAttribute attr))
                {
                    foreach(HostPrefix prefix in prefixes)
                    {
                        prefix.HostName = $"{attr.Subdomain}.{prefix.HostName}";
                    }
                }

                ServeServiceTypes(contentRoot, prefixes, null, serviceType);
                Pause($"Gloo server is serving service {serviceClassName}");
            }
            catch (Exception ex)
            {
                Args.PopMessageAndStackTrace(ex, out StringBuilder message, out StringBuilder stackTrace);
                OutLineFormat("An error occurred: {0}", ConsoleColor.Red, message.ToString());
                OutLineFormat("{0}", stackTrace.ToString());
                Thread.Sleep(1500);
            }
        }

        [ConsoleAction("registries", "Start the BamSvc server serving the registries of the specified names")]
        public void ServeRegistries()
        {
            ConsoleLogger logger = GetLogger();
            string registries = GetArgument("registries", "Enter the registry names to serve in a comma separated list ");
            ServeRegistries(logger, registries);
        }
        
        [ConsoleAction("app", "Start the svc server serving the registry for the current application (determined by the default configuration file ApplicationName value)")]
        public void ServeApplicationRegistry()
        {
            ConsoleLogger logger = GetLogger();
            ServeRegistries(logger, DefaultConfigurationApplicationNameProvider.Instance.GetApplicationName());
        }

        [ConsoleAction("createRegistry", "Menu driven Service Registry creation")]
        public void CreateRegistry()
        {
            DataProvider dataProvider = DataProvider.Instance;
            IApplicationNameProvider appNameProvider = DefaultConfigurationApplicationNameProvider.Instance;
            ServiceRegistryService serviceRegistryService = ServiceRegistryService.GetLocalServiceRegistryService(dataProvider, appNameProvider, GetArgument("AssemblySearchPattern", "Please specify the AssemblySearchPattern to use"), GetLogger());

            List<dynamic> types = new List<dynamic>();
            string assemblyPath = "\r\n";
            DirectoryInfo sysData = DataProvider.Current.GetSysDataDirectory(nameof(ServiceRegistry).Pluralize());
            ServiceRegistryRepository repo = DataProvider.Current.GetSysDaoRepository<ServiceRegistryRepository>();
            ServiceRegistryDescriptor registry = new ServiceRegistryDescriptor();
            while (!assemblyPath.Equals(string.Empty))
            {
                if (!string.IsNullOrEmpty(assemblyPath.Trim()))
                {
                    Assembly assembly = Assembly.LoadFrom(assemblyPath);
                    if(assembly == null)
                    {
                        OutLineFormat("Assembly not found: {0}", ConsoleColor.Magenta, assemblyPath);
                    }
                    else
                    {
                        OutLineFormat("Storing assembly file chunks: {0}", ConsoleColor.Cyan, assembly.FullName);
                        serviceRegistryService.FileService.StoreFileChunks(assembly.GetFileInfo(), assembly.FullName);
                        string className = "\r\n";
                        while (!className.Equals(string.Empty))
                        {
                            if (!string.IsNullOrEmpty(className.Trim()))
                            {
                                Type type = GetType(assembly, className);
                                if(type == null)
                                {
                                    Thread.Sleep(300);
                                    OutLineFormat("Specified class was not found in the current assembly: {0}", assembly.FullName);
                                }
                                else
                                {
                                    registry.AddService(type, type);
                                }
                            }
                            Thread.Sleep(300);
                            className = Prompt("Enter the name of a class to add to the service registry (leave blank to finish)");
                        }
                    }
                }
                Thread.Sleep(300);
                assemblyPath = Prompt("Enter the path to an assembly file containing service types (leave blank to finish)");
            }
            string registryName = Prompt("Enter a name for the registry");
            string path = Path.Combine(sysData.FullName, $"{registryName}.json");
            registry.Name = registryName;
            registry.Save(repo);
            registry.ToJsonFile(path);           
        }

        [ConsoleAction("downloadProxyCode", "Generate proxies from a running service proxy host")]
        public void GenerateProxies()
        {
            ConsoleLogger logger = new ConsoleLogger();
            ProxyFactory proxyFactory = new ProxyFactory(logger);
            string host = GetArgument("host", "Please specify the host to download proxy code from");
            int port = GetArgument("port", "Please specify the host to download proxy code from").ToInt(80);
            string nameSpace = GetArgument("nameSpace", "Please specify the namespace of the type to get code for");
            string typeName = GetArgument("typeName", "Please specify the name of the type to get code for");
            string directory = GetArgument("output", "Please specify the directory to write downloaded source to");
            ProxyAssemblyGeneratorService genSvc = proxyFactory.GetProxy<ProxyAssemblyGeneratorService>(host, port, logger);
            Services.ServiceResponse response = genSvc.GetProxyCode(nameSpace, typeName);
            if (!response.Success)
            {
                Warn(response.Message);
                Exit(1);
            }
            string filePath = Path.Combine(directory, $"{typeName}_{proxyFactory.DefaultSettings.Protocol.ToString()}_{host}_{port}_Proxy.cs");
            response.Data.ToString().SafeWriteToFile(filePath);
            OutLineFormat("Wrote file {0}", filePath);
        }

        [ConsoleAction("csBamSvc", "Start the BamSvc server serving the compiled results of the specified csBamRpc files")]
        public void ServeCsService()
        {
            string csglooDirectoryPath = GetArgument("BamRpcSrc", $"Enter the path to the BamSvc source directory (default: {defaultSvcScriptsSrcPath})");
            Assembly bamRpcAssembly = CompileBamSvcSource(csglooDirectoryPath, "csBamRpc");

            string contentRoot = GetArgument("ContentRoot", $"Enter the path to the content root (default: {defaultContentRoot} ");

            HostPrefix[] prefixes = ServiceConfig.GetConfiguredHostPrefixes();
            Type[] glooTypes = bamRpcAssembly.GetTypes().Where(t => t.HasCustomAttributeOfType<ProxyAttribute>()).ToArray();
            ServeServiceTypes(contentRoot, prefixes, null, glooTypes);
            Pause($"BamRpc server is serving cs types: {string.Join(", ", glooTypes.Select(t => t.Name).ToArray())}");
        }

        public static Assembly CompileBamSvcSource(string bamSvcDirectoryPath, string extension)
        {
            string binPath = Path.Combine(BamHome.ToolkitPath, "BamSvc_bin");
            DirectoryInfo bamSvcSrcDirectory = new DirectoryInfo(bamSvcDirectoryPath.Or(defaultSvcScriptsSrcPath)).EnsureExists();
            DirectoryInfo bamSvcBnDirectory = new DirectoryInfo(binPath).EnsureExists();
            FileInfo[] files = bamSvcSrcDirectory.GetFiles($"*.{extension}", SearchOption.AllDirectories);
            StringBuilder src = new StringBuilder();
            foreach (FileInfo file in files)
            {
                src.AppendLine(file.ReadAllText());
            }
            string hash = src.ToString().Sha1();
            string assemblyName = $"{hash}.dll";
            Assembly bamSvcAssembly = null;
            string svcAssemblyBinPath = Path.Combine(bamSvcBnDirectory.FullName, assemblyName);
            if (!File.Exists(svcAssemblyBinPath))
            {
                bamSvcAssembly = files.ToAssembly(assemblyName);
                FileInfo assemblyFile = bamSvcAssembly.GetFileInfo();
                FileInfo targetPath = new FileInfo(svcAssemblyBinPath);
                if (!targetPath.Directory.Exists)
                {
                    targetPath.Directory.Create();
                }
                File.Copy(assemblyFile.FullName, svcAssemblyBinPath);
            }
            else
            {
                bamSvcAssembly = Assembly.LoadFile(svcAssemblyBinPath);
            }

            return bamSvcAssembly;
        }

        private static void ServeRegistries(ILogger logger, string registries)
        {
            string contentRoot = GetArgument("ContentRoot", $"Enter the path to the content root (default: {defaultContentRoot}) ");
            DataProvider dataSettings = DataProvider.Current;
            IApplicationNameProvider appNameProvider = DefaultConfigurationApplicationNameProvider.Instance;
            ServiceRegistryService serviceRegistryService = ServiceRegistryService.GetLocalServiceRegistryService(dataSettings, appNameProvider, GetArgument("AssemblySearchPattern", "Please specify the AssemblySearchPattern to use"), logger);

            string[] requestedRegistries = registries.DelimitSplit(",");
            HashSet<Type> serviceTypes = new HashSet<Type>();
            ServiceRegistry allTypes = new ServiceRegistry();
            foreach (string registryName in requestedRegistries)
            {
                ServiceRegistry registry = serviceRegistryService.GetServiceRegistry(registryName);
                foreach (string className in registry.ClassNames)
                {
                    registry.Get(className, out Type type);
                    if (type.HasCustomAttributeOfType<ProxyAttribute>())
                    {
                        serviceTypes.Add(type);
                    }
                }
                allTypes.CombineWith(registry);
            }
            Type[] services = serviceTypes.ToArray();
            if(services.Length == 0)
            {
                throw new ArgumentException("No services were loaded");
            }

            HostPrefix[] hostPrefixes = ServiceConfig.GetConfiguredHostPrefixes();
            ServeServiceTypes(contentRoot, hostPrefixes, allTypes, services);
            hostPrefixes.Each(h => OutLine(h.ToString(), ConsoleColor.Blue));
            Pause($"BamRpc server is serving services\r\n\t{services.ToArray().ToDelimited(s => s.FullName, "\r\n\t")}");
        }

        public static void ServeServiceTypes(string contentRoot, HostPrefix[] prefixes, ServiceRegistry registry = null, params Type[] serviceTypes)
        {
            BamConf conf = BamConf.Load(contentRoot.Or(defaultContentRoot));
            if(registry != null && ServiceRegistry.Default == null)
            {
                ServiceRegistry.Default = registry;
            }
            _bamSvcServer = new BamSvcServer(conf, GetLogger(), GetArgument("verbose", "Log responses to the console?").IsAffirmative())
            {
                HostPrefixes = new HashSet<HostPrefix>(prefixes),
                MonitorDirectories = new string[] { }                
            };
            serviceTypes.Each(t => _bamSvcServer.ServiceTypes.Add(t));

            _bamSvcServer.Start();
        }
        
        public static void StartBamRpcServer(ConsoleLogger logger)
        {
            BamConf conf = BamConf.Load(DefaultConfiguration.GetAppSetting(contentRootConfigKey).Or(defaultContentRoot));
            _bamSvcServer = new BamSvcServer(conf, logger, GetArgument("verbose", "Log responses to the console?").IsAffirmative())
            {
                HostPrefixes = new HashSet<HostPrefix>(HostPrefix.FromDefaultConfiguration("localhost", 9100)),
                MonitorDirectories = DefaultConfiguration.GetAppSetting("MonitorDirectories").DelimitSplit(",", ";")
            };
            _bamSvcServer.Start();
        }

        private static ConsoleLogger GetLogger()
        {
            ConsoleLogger logger = new ConsoleLogger()
            {
                AddDetails = false,
                UseColors = true
            };
            logger.StartLoggingThread();
            return logger;
        }

        private Type GetServiceType(string className, out Assembly assembly)
        { 
            assembly = GetAssembly(className, out Type result);
            return result;
        }

        private Assembly GetAssembly(string className, out Type type)
        {
            type = null;
            Assembly result = null;
            Assembly[] assemblies = AppDomain.CurrentDomain.GetAssemblies();
            foreach (Assembly assembly in assemblies)
            {
                type = GetType(assembly, className);
                if(type != null)
                {
                    result = assembly;
                    break;
                }
            }
            if(result == null)
            {
                string assemblyPath = GetArgument("assemblyPath", true);
                if (!File.Exists(assemblyPath))
                {
                    assemblyPath = new FileInfo(assemblyPath).FullName;
                }

                if (File.Exists(assemblyPath))
                { 
                    result = Assembly.LoadFrom(assemblyPath);
                    type = GetType(result, className);
                    if (type == null)
                    {
                        type = result.GetType(className);
                    }
                }
            }

            return result;
        }

        private Type GetType(Assembly assembly, string className)
        {
            return assembly.GetTypes().FirstOrDefault(t => t.Name.Equals(className) || $"{t.Namespace}.{t.Name}".Equals(className));
        }
    }
}