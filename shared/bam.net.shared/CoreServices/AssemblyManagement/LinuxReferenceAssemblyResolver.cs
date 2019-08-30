
using System;
using System.Reflection;

namespace Bam.Net.CoreServices.AssemblyManagement
{
    public class LinuxReferenceAssemblyResolver: IReferenceAssemblyResolver
    {
        public Assembly ResolveReferenceAssembly(Type type)
        {
            throw new NotImplementedException();
        }

        public string ResolveReferenceAssemblyPath(Type type)
        {
            throw new NotImplementedException();
        }

        public string ResolveReferenceAssemblyPath(string nameSpace, string typeName)
        {
            throw new NotImplementedException();
        }

        public string ResolveSystemRuntimePath()
        {
            throw new NotImplementedException();
        }
    }
}