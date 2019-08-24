using System;
using Bam.Net.Data.Repositories;

namespace Bam.Shell.ShellGeneration.Data
{
    public class ShellDescriptor : CompositeKeyRepoData
    {
        public ShellDescriptor()
        {
        }

        public ShellDescriptor(Type type)
        {
            ProviderType = type;
            AssemblyName = type?.Assembly?.FullName;
            NameSpace = type.Namespace;
            ProviderTypeName = type.Name;
        }
        
        internal Type ProviderType { get; }

        [CompositeKey]
        public string AssemblyName { get; set; }
        
        [CompositeKey]
        public string NameSpace { get; set; }

        [CompositeKey]
        public string ProviderTypeName { get; set; }
    }
}