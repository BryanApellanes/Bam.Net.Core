using Bam.Net.Data.Repositories;

namespace Bam.Net.Services.Catalog.Data
{
    public class ItemOption: KeyedAuditRepoData
    {
        public ulong CatalogItemId { get; set; }
        public virtual CatalogItem CatalogItem { get; set; }
        
        [CompositeKey]
        public ulong CatalogKey { get; set; }
        [CompositeKey]
        public ulong ItemDefinitionKey { get; set; }
        [CompositeKey]
        public string Name { get; set; }
        
        public string Description { get; set; }
        public decimal PriceDifference { get; set; }
    }
}