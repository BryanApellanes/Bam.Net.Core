using Bam.Net.Data.Repositories;

namespace Bam.Net.CoreServices.AssemblyManagement.Data
{
    public class TypeDescriptor : CompositeKeyAuditRepoData
    {
        [CompositeKey]
        public string Namespace { get; set; }
        
        [CompositeKey]
        public string TypeName { get; set; }
    }
}