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
                BamHomeDir = BamHomeDir,
                BamDir = BamDir,
                ProcessHomeDir = ProcessHomeDir
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

        public static string GetSystemRuntimePath()
        {
            return Path.Combine(GetConfig().ReferenceAssembliesDir, SystemRuntime);
        }
        
        public static string ReferenceAssembliesDir
        {
            get { return Path.Combine(BinDir, "ReferenceAssemblies"); }
        }

        public static string GenDir
        {
            get { return Path.Combine(BinDir, "gen"); }
        }
        
        public static string BinDir
        {
            get { return Path.Combine(BamHomeDir, "bin"); }
        }
        
        public static string BamHomeDir
        {
            get
            {
                return Path.Combine(ProcessHomeDir, ".bam");
            }
        }

        public static string BamDir
        {
            get
            {
                return Path.Combine(Environment.CurrentDirectory, ".bam");
            }
        }

        public static string ProcessHomeDir
        {
            get
            {
                return IsUnix ? Environment.GetEnvironmentVariable("HOME") : Environment.ExpandEnvironmentVariables("%HOMEDRIVE%%HOMEPATH%");
            }
        }

        public static bool IsWindows
        {
            get
            {
                return RuntimeInformation.IsOSPlatform(OSPlatform.Windows);
            }
        }

        public static bool IsUnix
        {
            get
            {
                return RuntimeInformation.IsOSPlatform(OSPlatform.OSX) || RuntimeInformation.IsOSPlatform(OSPlatform.Linux);
            }
        }

        public static bool IsLinux
        {
            get
            {
                return RuntimeInformation.IsOSPlatform(OSPlatform.Linux);
            }
        }

        /// <summary>
        /// Gets a vlaue indicating if the current runtime environment is a mac, same as IsOSX
        /// </summary>
        public static bool IsMac
        {
            get
            {
                return IsOSX;
            }
        }

        /// <summary>
        /// Gets a vlaue indicating if the current runtime environment is a mac, same as IsMac
        /// </summary>
        public static bool IsOSX
        {
            get
            {
                return RuntimeInformation.IsOSPlatform(OSPlatform.OSX);
            }
        }
    }
}
