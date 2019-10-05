using System;

namespace Bam.Net.CoreServices.AssemblyManagement
{
    public class LocalReferenceAssemblyResolver: ReferenceAssemblyResolver
    {
        public override string ResolveReferencePackage(string packageName)
        {
            return ReferenceAssemblyResolver.Current.ResolveReferencePackage(packageName);
        }
    }
}