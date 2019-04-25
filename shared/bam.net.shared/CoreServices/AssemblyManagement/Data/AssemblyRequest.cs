using Bam.Net.Data.Repositories;

namespace Bam.Net.CoreServices.AssemblyManagement.Data
{
    public class AssemblyRequest : RepoData
    {
        /// <summary>
        /// The hash of the requested assembly or null if specific version is not required.
        /// </summary>
        public string FileHash { get; set; }
        
        /// <summary>
        /// The full name of the assembly as reported by Assembly.FullName
        /// </summary>
        public string AssemblyFullName { get; set; }
        
        /// <summary>
        /// The name of the assembly file as reported by Assembly.GetFileInfo().Name
        /// (where GetFileInfo() is the extension method Bam.Net.Extensions.GetFileInfo(Assembly assembly)
        /// which returns a FileInfo instance representing the specified assembly)
        /// </summary>
        public string Name { get; set; }
        
        /// <summary>
        /// The full name of the requesting assembly
        /// </summary>
        public string RequestingAssemblyFullName { get; set; }
    }
}