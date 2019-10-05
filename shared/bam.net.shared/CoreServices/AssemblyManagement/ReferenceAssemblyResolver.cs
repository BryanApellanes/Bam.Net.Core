
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices.WindowsRuntime;

namespace Bam.Net.CoreServices.AssemblyManagement
{
    public abstract class ReferenceAssemblyResolver: IReferenceAssemblyResolver
    {
        private static Dictionary<OSNames, IReferenceAssemblyResolver> _referenceAssemblyResolvers;
        static readonly object _referenceAssemblyResolversLock = new object();

        private static Dictionary<OSNames, IReferenceAssemblyResolver> ReferenceAssemblyResolvers
        {
            get
            {
                return _referenceAssemblyResolversLock.DoubleCheckLock<Dictionary<OSNames, IReferenceAssemblyResolver>>(ref _referenceAssemblyResolvers, ()=> new Dictionary<OSNames, IReferenceAssemblyResolver>()
                {
                    {OSNames.Windows, new RuntimeSettingsConfigReferenceAssemblyResolver()},
                    {OSNames.OSX, new OsxReferenceAssemblyResolver()},
                    {OSNames.Linux, new LinuxReferenceAssemblyResolver()}
                });
            }
        }

        public static IReferenceAssemblyResolver Current => ReferenceAssemblyResolvers[OSInfo.Current];

        public virtual Assembly ResolveReferenceAssembly(Type type)
        {
            return Assembly.Load(ResolveReferenceAssemblyPath(type));
        }

        public virtual string ResolveReferenceAssemblyPath(Type type)
        {
            return ReferenceAssemblyResolvers[OSInfo.Current].ResolveReferenceAssemblyPath(type);
        }

        public virtual string ResolveReferenceAssemblyPath(string nameSpace, string typeName)
        {
            return ReferenceAssemblyResolvers[OSInfo.Current].ResolveReferenceAssemblyPath(nameSpace, typeName);
        }

        public virtual string ResolveSystemRuntimePath()
        {
            return RuntimeSettings.GetSystemRuntimePath();
        }

        public static string GetReferenceAssemblyPath(Type type)
        {
            return ReferenceAssemblyResolvers[OSInfo.Current].ResolveReferenceAssemblyPath(type);
        }

        public string ResolveNetStandardPath()
        {
            return ResolveNetStandardPath(ResolveSystemRuntimePath());
        }

        public string ResolveReferenceAssemblyPath(string assemblyName)
        {
            FileInfo runtime = new FileInfo(ResolveSystemRuntimePath());
            return Path.Combine(runtime.Directory.FullName, assemblyName);
        }

        public abstract string ResolveReferencePackage(string packageName);

        protected internal static string ResolveNetStandardPath(string systemRuntimePath = "")
        {
            if (string.IsNullOrEmpty(systemRuntimePath))
            {
                systemRuntimePath = ReferenceAssemblyResolvers[OSInfo.Current].ResolveSystemRuntimePath();
            }

            FileInfo systemRuntime = new FileInfo(systemRuntimePath);
            return Path.Combine(systemRuntime.Directory.FullName, "netstandard.dll");
        }
    }
}