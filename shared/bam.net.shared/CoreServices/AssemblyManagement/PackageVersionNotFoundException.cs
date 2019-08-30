using System;

namespace Bam.Net.CoreServices.AssemblyManagement
{
    public class PackageVersionNotFoundException : Exception
    {
        public PackageVersionNotFoundException(Type type) : base(
            $"Unable to resolve package version for type ({type.AssemblyQualifiedName})")
        {
        }

        public PackageVersionNotFoundException(string typeName) : base(
            $"Unable to resolve package version for type ({typeName})")
        {
        }
    }
}