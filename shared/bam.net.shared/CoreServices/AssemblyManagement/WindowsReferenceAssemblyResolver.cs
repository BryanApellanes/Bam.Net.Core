using System;
using System.IO;
using System.Reflection;

namespace Bam.Net.CoreServices.AssemblyManagement
{
    
    public class WindowsReferenceAssemblyResolver: IReferenceAssemblyResolver
    {
        public Assembly ResolveReferenceAssembly(Type type)
        {
            Args.ThrowIfNull(type, "type");
            return type.Assembly;
        }

        public string ResolveReferenceAssemblyPath(Type type)
        {
            Args.ThrowIfNull(type, "type");
            return type.Assembly.GetFilePath();
        }

        public string ResolveReferenceAssemblyPath(string nameSpace, string typeName)
        {
            return Type.GetType($"{nameSpace}.{typeName}").Assembly.GetFilePath();
        }

        public string ResolveSystemRuntimePath()
        {
            return typeof(object).Assembly.GetFilePath();
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
            return ResolveReferenceAssemblyPath(packageName);
        }
    }
}