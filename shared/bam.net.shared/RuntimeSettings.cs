using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Bam.Net
{
    public static partial class RuntimeSettings
    {
        public const string SystemRuntime = "System.Runtime.dll";
        
        public static RuntimeConfig GetConfig()
        {
            string fileName = "runtime-config.yaml";
            FileInfo configFile = new FileInfo(Path.Combine(BamDir, fileName));
            if (configFile.Exists)
            {
                return configFile.FromYamlFile<RuntimeConfig>();
            }

            RuntimeConfig config = new RuntimeConfig
            {
                ReferenceAssembliesDir = ReferenceAssembliesDir,
                GenDir = GenDir,
                BamProfileDir = BamProfileDir,
                BamDir = BamDir,
                ProcessProfileDir = ProcessProfileDir
            };
            config.ToYamlFile(configFile);
            return config;
        }
        
        static string _appDataFolder;
        static object _appDataFolderLock = new object();
        
        public static Func<Type, bool> ClrTypeFilter
        {
            get
            {
                return (t) => !t.IsAbstract && !t.HasCustomAttributeOfType<CompilerGeneratedAttribute>()
                              && t.Attributes != (
                                      TypeAttributes.NestedPrivate |
                                      TypeAttributes.Sealed |
                                      TypeAttributes.Serializable |
                                      TypeAttributes.BeforeFieldInit
                                  );
            }
        }
        
        public static string GetBamAssemblyPath()
        {
            return typeof(BamPaths).Assembly.GetFilePath();
        }

        public static string GetMsCoreLibPath()
        {
            return Path.Combine(GetConfig().ReferenceAssembliesDir, "mscorlib.dll");
        }
        
        public static string GetSystemRuntimePath()
        {
            return Path.Combine(GetConfig().ReferenceAssembliesDir, SystemRuntime);
        }
        
        public static string ReferenceAssembliesDir => Path.Combine(BinDir, OSInfo.Current.ToString(), "ReferenceAssemblies");

        public static string GenDir => Path.Combine(BinDir, "gen");

        public static string BinDir => Path.Combine(BamProfileDir, "bin");

        /// <summary>
        /// The path to the '.bam' directory found in the home directory of the owner of the
        /// current process.
        /// </summary>
        public static string BamProfileDir => Path.Combine(ProcessProfileDir, ".bam");

        /// <summary>
        /// The path to the the '.bam' directory found in the current working directory. 
        /// </summary>
        public static string BamDir => Path.Combine(Environment.CurrentDirectory, ".bam");

        /// <summary>
        /// The path to the home directory of the user that owns the current process.
        /// </summary>
        public static string ProcessProfileDir => IsUnix ? Environment.GetEnvironmentVariable("HOME") : Environment.ExpandEnvironmentVariables("%HOMEDRIVE%%HOMEPATH%");

        /// <summary>
        /// Gets a value indicating if the current process is running on Windows.
        /// </summary>
        public static bool IsWindows => RuntimeInformation.IsOSPlatform(OSPlatform.Windows);

        /// <summary>
        /// Gets a value indicating if the current process is running on a unix platform such as, Linux, BSD or Mac OSX.
        /// </summary>
        public static bool IsUnix => RuntimeInformation.IsOSPlatform(OSPlatform.OSX) || RuntimeInformation.IsOSPlatform(OSPlatform.Linux);

        public static bool IsLinux => RuntimeInformation.IsOSPlatform(OSPlatform.Linux);

        /// <summary>
        /// Gets a value indicating if the current runtime environment is a mac, same as IsOSX
        /// </summary>
        public static bool IsMac => IsOSX;

        /// <summary>
        /// Gets a value indicating if the current runtime environment is a mac, same as IsMac
        /// </summary>
        public static bool IsOSX => RuntimeInformation.IsOSPlatform(OSPlatform.OSX);
    }
}
