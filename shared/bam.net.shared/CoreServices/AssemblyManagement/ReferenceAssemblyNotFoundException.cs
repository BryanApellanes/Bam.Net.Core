using System;

namespace Bam.Net.CoreServices.AssemblyManagement
{
    public class ReferenceAssemblyNotFoundException : Exception
    {
        public ReferenceAssemblyNotFoundException(Type type) : base(
            $"Unable to find reference assembly for type ({type.AssemblyQualifiedName})")
        {
        }

        public ReferenceAssemblyNotFoundException(string typeName) : base(
            $"Unable to find reference assembly for type ({typeName})")
        {
        }
    }
}