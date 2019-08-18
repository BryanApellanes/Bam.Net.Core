using Bam.Net.Data.Repositories;

namespace Bam.Net.CoreServices.AssemblyManagement.Data
{
    public class AssemblyQualifiedTypeDescriptor: TypeDescriptor
    {
        [CompositeKey]
        public string AssemblyPath { get; set; }
    }
}