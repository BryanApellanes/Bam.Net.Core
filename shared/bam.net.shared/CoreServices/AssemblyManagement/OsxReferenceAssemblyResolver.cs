
using System;
using System.IO;
using System.Reflection;
using Bam.Net.Logging;

namespace Bam.Net.CoreServices.AssemblyManagement
{
    public class OsxReferenceAssemblyResolver : IReferenceAssemblyResolver
    {
        public OsxReferenceAssemblyResolver()
        {
            NugetReferenceAssemblyResolver = new NugetReferenceAssemblyResolver("/usr/local/share/dotnet/sdk/NugetFallbackFolder");
            RuntimeSettingsConfigReferenceAssemblyResolver = new RuntimeSettingsConfigReferenceAssemblyResolver();
        }

        protected NugetReferenceAssemblyResolver NugetReferenceAssemblyResolver { get; set; }
        protected RuntimeSettingsConfigReferenceAssemblyResolver RuntimeSettingsConfigReferenceAssemblyResolver { get; set; }

        public Assembly ResolveReferenceAssembly(Type type)
        {
            return NugetReferenceAssemblyResolver.ResolveReferenceAssembly(type);
        }

        public string ResolveReferenceAssemblyPath(Type type)
        {
            return NugetReferenceAssemblyResolver.ResolveReferenceAssemblyPath(type);
        }

        public string ResolveReferenceAssemblyPath(string nameSpace, string typeName)
        {
            return NugetReferenceAssemblyResolver.ResolveReferenceAssemblyPath(nameSpace, typeName);
        }

        public string ResolveSystemRuntimePath()
        {
            string netCoreDir = new FileInfo(typeof(object).Assembly.GetFilePath()).Directory.FullName;
            string systemRuntime = Path.Combine(netCoreDir, "System.Runtime.dll");
            if (!File.Exists(systemRuntime))
            {
                try
                {
                    systemRuntime = RuntimeSettingsConfigReferenceAssemblyResolver.ResolveSystemRuntimePath();
                }
                catch (Exception ex)
                {
                    Log.Error("Error trying to resolve `System.Runtime.dll` path");
                
                    return NugetReferenceAssemblyResolver.ResolveSystemRuntimePath();
                }
            }

            return systemRuntime;
        }

        public string ResolvePackageRootDirectory(string typeNamespace, string typeName)
        {
            return NugetReferenceAssemblyResolver.ResolvePackageRootDirectory(typeNamespace, typeName);
        }
    }
}