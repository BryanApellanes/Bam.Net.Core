
using System;
using System.Reflection;

namespace Bam.Net.CoreServices.AssemblyManagement
{
    public class OsxReferenceAssemblyResolver : IReferenceAssemblyResolver
    {
        public OsxReferenceAssemblyResolver()
        {
            NugetReferenceAssemblyResolver = new NugetReferenceAssemblyResolver("/usr/local/share/dotnet/sdk/NugetFallbackFolder");
        }

        protected NugetReferenceAssemblyResolver NugetReferenceAssemblyResolver { get; set; }

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
            return NugetReferenceAssemblyResolver.ResolveSystemRuntimePath();
        }

        public string ResolvePackageRootDirectory(string typeNamespace, string typeName)
        {
            return NugetReferenceAssemblyResolver.ResolvePackageRootDirectory(typeNamespace, typeName);
        }
    }
}