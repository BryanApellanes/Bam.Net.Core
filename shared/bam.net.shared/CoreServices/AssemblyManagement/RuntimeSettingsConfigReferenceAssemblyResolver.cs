using System;
using System.IO;

namespace Bam.Net.CoreServices.AssemblyManagement
{
    public class RuntimeSettingsConfigReferenceAssemblyResolver : ReferenceAssemblyResolver
    {
        public override string ResolveReferenceAssemblyPath(Type type)
        {
            return ResolveReferenceAssemblyPath(type.Namespace, type.Name);
        }

        public virtual string ResolveReferenceAssemblyPath(string typeNamespace, string typeName)
        {
            string referenceAssembliesDir = RuntimeSettings.GetConfig().ReferenceAssembliesDir;

            string filePath = Path.Combine(referenceAssembliesDir, $"{typeNamespace}.dll");
            if (!File.Exists(filePath))
            {
                filePath = Path.Combine(referenceAssembliesDir, $"{typeNamespace}.{typeName}.dll");
            }

            if (!File.Exists(filePath))
            {
                throw new ReferenceAssemblyNotFoundException($"{typeNamespace}.{typeName}");
            }

            return filePath;
        }

        /// <summary>
        /// Resolves the specified package by reading the current RuntimeConfig
        /// </summary>
        /// <param name="packageName"></param>
        /// <returns></returns>
        public override string ResolveReferencePackage(string packageName)
        {
            return ResolveReferenceAssemblyPath(ResolveReferenceAssemblyPath(packageName, packageName));
        }
    }
}