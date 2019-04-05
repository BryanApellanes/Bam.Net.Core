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
        static RuntimeSettings()
        {
            SystemDotRuntimePath = Path.Combine(SysHomeDir, "bin", "System.Runtime.dll");
            NetStandardPath = Path.Combine(SysHomeDir, "bin", "netstandard.dll");
        }

        public static RuntimeConfig GetConfig()
        {
            string fileName = "runtime-config.yaml";
            FileInfo configFile = new FileInfo(Path.Combine(SysDir, fileName));
            if (configFile.Exists)
            {
                return configFile.FromYamlFile<RuntimeConfig>();
            }

            RuntimeConfig config = new RuntimeConfig
            {
                SystemDotRuntimePath = SystemDotRuntimePath,
                NetStandardPath = NetStandardPath,
                SysHomeDir = SysHomeDir,
                SysDir = SysDir,
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

        public static string SystemDotRuntimePath
        {
            get;
            set;
        }

        public static string NetStandardPath
        {
            get;
            set;
        }
        
        public static string SysHomeDir
        {
            get
            {
                return Path.Combine(ProcessHomeDir, ".bam");
            }
        }

        public static string SysDir
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
