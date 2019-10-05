using Bam.Net.Data.Repositories;

namespace Bam.Shell.ShellGeneration.Data
{
    public class ShellWrapperAssembly: AuditRepoData
    {
        public ShellWrapperAssembly()
        {
        }
        
        /// <summary>
        /// The key of the ShellDescriptor this ShellWrapperAssembly was generated for.
        /// </summary>
        public ulong ShellDescriptorKey { get; set; }
        
        public string Base64Assembly { get; set; }
    }
}