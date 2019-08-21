using Bam.Net.Data.Repositories;

namespace Bam.Net.Services.Catalog.Data
{
    public class ItemOption: KeyedAuditRepoData
    {
        [CompositeKey]
        public ulong ItemKey { get; set; }
        [CompositeKey]
        public string Name { get; set; }
        
        public string Description { get; set; }
        public decimal PriceDifference { get; set; }
    }
}