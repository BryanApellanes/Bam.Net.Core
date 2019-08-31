using System;
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
    }
}