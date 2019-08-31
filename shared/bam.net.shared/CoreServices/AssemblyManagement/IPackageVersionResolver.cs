using System;

namespace Bam.Net.CoreServices.AssemblyManagement
{
    public interface IPackageVersionResolver
    {
        string ResolveVersion(Type type);
        string ResolveVersion(string nameSpace, string typeName);
    }
}