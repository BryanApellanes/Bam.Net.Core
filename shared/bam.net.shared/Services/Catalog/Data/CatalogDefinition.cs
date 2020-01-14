using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Bam.Net.Data.Repositories;
//using Bam.Net.Schema.Org.Things;

namespace Bam.Net.Services.Catalog.Data
{
    [Serializable]
    public class CatalogDefinition: KeyedAuditRepoData
    {
        public const string DefaultCatalogName = "PUBLIC-DEFAULT";
        
        [CompositeKey]
        public ulong OrganizationKey { get; set; }
        
        [CompositeKey]
        public ulong ApplicationKey { get; set; }
        
        [CompositeKey]
        public string Name { get; set; }
        public virtual List<ItemDefinition> Items { get; set; }
        public string KindOfCatalog { get; set; } // should implicitly map to KindsOfCatalogs

        static CatalogDefinition _defaultCatalog;
        static readonly object _defaultCatalogLock = new object();
        public static CatalogDefinition GetDefault(IRepository repository)
        {
            return _defaultCatalogLock.DoubleCheckLock(ref _defaultCatalog, () =>
            {
                ulong orgKey = Bam.Net.CoreServices.ApplicationRegistration.Data.Organization.Public.Key;
                CatalogDefinition defaultCatalog = new CatalogDefinition{OrganizationKey = orgKey, Name = DefaultCatalogName};
                defaultCatalog = repository.Query<CatalogDefinition>(new {Key = defaultCatalog.Key}).FirstOrDefault();
                if (defaultCatalog == null)
                {
                    defaultCatalog = new CatalogDefinition() {Name = DefaultCatalogName};
                    defaultCatalog = repository.Save(defaultCatalog);
                }

                return defaultCatalog;
            });
        }
    }
}
