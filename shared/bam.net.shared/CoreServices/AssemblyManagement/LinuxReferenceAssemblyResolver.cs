
using System;
using System.IO;
using System.Reflection;
using Bam.Net.Logging;
using Org.BouncyCastle.Bcpg.OpenPgp;

namespace Bam.Net.CoreServices.AssemblyManagement
{
    public class LinuxReferenceAssemblyResolver: IReferenceAssemblyResolver
    {
        private const string _dotnetAssemblyPath = "/usr/share/dotnet/shared/Microsoft.NETCore.App";
        public LinuxReferenceAssemblyResolver()
        { 
            NugetReferenceAssemblyResolver = new NugetReferenceAssemblyResolver(_dotnetAssemblyPath);
            RuntimeSettingsConfigReferenceAssemblyResolver = new RuntimeSettingsConfigReferenceAssemblyResolver();
        }

        private NugetReferenceAssemblyResolver NugetReferenceAssemblyResolver { get; set; }
        private RuntimeSettingsConfigReferenceAssemblyResolver RuntimeSettingsConfigReferenceAssemblyResolver { get; set; }
        public Assembly ResolveReferenceAssembly(Type type)
        {
            return Assembly.LoadFrom(ResolveReferenceAssemblyPath(type));
        }

        public string ResolveReferenceAssemblyPath(Type type)
        {
            FileInfo assemblyFile = new FileInfo(Path.Combine(_dotnetAssemblyPath, OSInfo.CoreVersion, $"{type.Namespace}.{type.Name}.dll"));
            if (assemblyFile.Exists)
            {
                return assemblyFile.FullName;
            }
            throw new ReferenceAssemblyNotFoundException(type); 
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

        public string ResolveNetStandardPath()
        {
            return ReferenceAssemblyResolver.ResolveNetStandardPath(ResolveSystemRuntimePath());
        }

        public string ResolveReferenceAssemblyPath(string assemblyName)
        {
            FileInfo runtime = new FileInfo(ResolveSystemRuntimePath());
            return Path.Combine(runtime.Directory.FullName, assemblyName);
        }

        public string ResolveReferencePackage(string packageName)
        {
            return NugetReferenceAssemblyResolver.ResolveReferencePackage(packageName);
        }
    }
}