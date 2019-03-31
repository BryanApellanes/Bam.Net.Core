using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using Bam.Net;
using Bam.Net.CommandLine;
using Bam.Net.Data.Dynamic;
using Bam.Net.Data.Repositories;
using Bam.Net.Logging;
using Bam.Net.Testing;
using CsQuery.ExtensionMethods;
using Lucene.Net.Analysis.Hunspell;
using Lucene.Net.Analysis.Standard;
using Microsoft.CodeAnalysis.Operations;

namespace Bam.Shell
{
    /// <summary>
    /// A class representing the first argument to the entry assembly.  Exists primarily to allow
    /// processing of arguments without a prefix and to route execution to a method specified on
    /// the command line.
    /// </summary>
    public class ArgZero : CommandLineTestInterface
    {
        static object _targetsLock = new object();
        static Dictionary<string, MethodInfo> _targets; 
        public static Dictionary<string, MethodInfo> Targets
        {
            get
            {
                return _targetsLock.DoubleCheckLock(ref _targets,
                    () => new Dictionary<string, MethodInfo>());
            }
        }

        public static void Register(string arg, MethodInfo method)
        {
            if (!Targets.AddMissing(arg, method))
            {
                Args.Throw<InvalidOperationException>("The specified ArgZero is already registered: {0}", arg);
            }
        }

        /// <summary>
        /// Scan for ArgZero methods.
        /// </summary>
        public static void Scan<T>() where T: IRegisterArguments
        {   
            Assembly current = Assembly.GetExecutingAssembly();
            
            current.GetTypes().ForEach(type =>
            {
                if (type.ExtendsType<T>())
                {
                    if (!type.Name.EndsWith("Provider"))
                    {
                        OutLineFormat("For clarity and convention, the name of type {0} should end with 'Provider'", ConsoleColor.Yellow);
                    }
                    type.Construct<T>().RegisterArguments();
                    string providerName = type.Name.Truncate("Provider".Length);
                    ProviderTypes.AddMissing(providerName, type);
                }
                
                type.GetMethods().ForEach(m =>
                {
                    if (m.HasCustomAttributeOfType<ArgZeroAttribute>(out ArgZeroAttribute arg))
                    {
                        Register(arg.Argument, m);
                    }
                });
            });
        }
        
        private static Dictionary<string, Type> _providerTypes;
        static object _providerTypesLock = new object();
        public static Dictionary<string, Type> ProviderTypes
        {
            get { return _providerTypesLock.DoubleCheckLock(ref _providerTypes, () => new Dictionary<string, Type>()); }
        }
        
        /// <summary>
        /// Execute any ArgZero arguments specified on the command line then exit.  Has no effect if no relevant arguments
        /// are detected.
        /// </summary>
        public static void ExecuteArgZero<T>(string[] arguments) where T: IRegisterArguments
        {
            if (arguments.Length == 0)
            {
                return;
            }
            
            Scan<T>();
            ShellProviderDelegator.Register(arguments);
            if (Targets.ContainsKey(arguments[0]))
            {
                List<string> targetArguments = new List<string>();
                List<ArgumentInfo> argumentInfos = new List<ArgumentInfo>();
                arguments.Rest(1, (val) =>
                {
                    targetArguments.Add(val);
                    if (val.StartsWith("/"))
                    {
                        string argName = val.TruncateFront(1).ReadUntil(':', out string argVal);
                        argumentInfos.Add(new ArgumentInfo(argName, true));
                    }
                });
                Arguments = new ParsedArguments(targetArguments.ToArray(), argumentInfos.ToArray());
                MethodInfo method = Targets[arguments[0]];
                object instance = null;
                if (!method.IsStatic)
                {
                    instance = method.DeclaringType.Construct();
                }

                try
                {
                    method.Invoke(instance, null);
                }
                catch (Exception ex)
                {
                    OutLineFormat("Exception executing ArgZero: {0}", ConsoleColor.Magenta, ex.Message);
                }
                Exit(0);
            }
        }
        
        private static IEnumerable<Assembly> FindAssemblies()
        {
            Config config = Config.Current;
            string arg0DirPath = Path.Combine(Workspace.Current.Directory("arg0").FullName);
            string argZeroAssemblyFolders = config.AppSettings["ArgZeroAssemblyFolders"].Or($".,{arg0DirPath}");
            string argZeroScanPattern = config.AppSettings["ArgZeroScanPattern"].Or("*-arg0.dll");

            string[] assemlbyFolderPaths = argZeroAssemblyFolders.DelimitSplit(",");
            foreach (string assemblyFolderPath in assemlbyFolderPaths)
            {
                foreach (Assembly assembly in FindAssemblies(new DirectoryInfo(assemblyFolderPath), argZeroScanPattern))
                {
                    yield return assembly;
                }
            }
        }
        
        private static IEnumerable<Assembly> FindAssemblies(DirectoryInfo directoryInfo, string searchPattern)
        {
            FileInfo[] fileInfos = directoryInfo.GetFiles(searchPattern);
            foreach (FileInfo fileInfo in fileInfos)
            {
                Assembly next = null;
                try
                {
                    next = Assembly.LoadFile(fileInfo.FullName);
                }
                catch (Exception ex)
                {
                    Log.Warn("Error finding assemblies in directory {0}: {1}", directoryInfo.FullName, ex.Message);
                    continue;
                }

                if (next != null)
                {
                    yield return next;   
                }
            }
        }
    }
}