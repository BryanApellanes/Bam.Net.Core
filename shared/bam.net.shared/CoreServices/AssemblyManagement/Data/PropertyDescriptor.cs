using Bam.Net.Data;
using Bam.Net.Data.Repositories;

namespace Bam.Net.CoreServices.AssemblyManagement.Data
{
    public class PropertyDescriptor : CompositeKeyAuditRepoData
    {
        /// <summary>
        /// The composite key of the parent TypeDescriptor
        /// </summary>
        public ulong ParentTypeDescriptorKey { get; set; }
        public string PropertyName { get; set; }
        public DataTypes DataType { get; set; }
    }
}