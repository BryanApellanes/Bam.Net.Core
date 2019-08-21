using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Bam.Net.CoreServices;
using Bam.Net.Data.Repositories;
using Bam.Net.Server;
using Bam.Net.Services.Catalog.Data;

namespace Bam.Net.Services
{
    [Proxy("catalogSvc")]
    public class CatalogService : AsyncProxyableService, ICatalogService
    {
        protected CatalogService() { }
        public CatalogService(IRepository catalogRepo, AsyncCallbackService callbackService, DaoRepository repo, AppConf conf) : base(callbackService, repo, conf)
        {
            Repository = catalogRepo;
            DaoRepository.WarningsAsErrors = false;
            DaoRepository.AddReferenceAssemblies(typeof(CoreExtensions).Assembly);
        }

        public override object Clone()
        {
            CatalogService svc = new CatalogService(Repository, CallbackService, DaoRepository, AppConf);
            svc.CopyProperties(this);
            svc.CopyEventHandlers(this);
            return svc;
        }

        public virtual CatalogDefinition GetDefaultCatalog()
        {
            throw new NotImplementedException();
        }
        
        public CatalogDefinition CreateCatalog(string name)
        {
            CatalogDefinition catalog = new CatalogDefinition { Name = name };
            return Repository.Save(catalog);
        }

        public CatalogDefinition FindCatalog(string name)
        {
            CatalogDefinition catalog = Repository.Query<CatalogDefinition>(new { Name = name }).FirstOrDefault();
            return Repository.Retrieve<CatalogDefinition>(catalog.Uuid);
        }

        public CatalogDefinition RenameCatalog(ulong catalogKey, string name)
        {
            CatalogDefinition catalog = Repository.Query<CatalogDefinition>(new { Key = catalogKey }).FirstOrDefault();
            Args.ThrowIf(catalog == null, "Catalog ({0}) was not found", catalogKey);
            catalog.Name = name;
            return Repository.Save(catalog);
        }

        public CatalogDefinition GetCatalog(ulong catalogKey)
        {
            CatalogDefinition catalog = Repository.Query<CatalogDefinition>(new { Key = catalogKey }).FirstOrDefault();
            if(catalog != null)
            {
                catalog = Repository.Retrieve<CatalogDefinition>(catalog.Uuid);
                catalog.Items = GetCatalogItems(catalog.Cuid).ToList();
                return catalog;
            }
            return null;
        }

        protected IEnumerable<ItemDefinition> GetCatalogItems(string catalogCuid)
        {
            foreach(CatalogItem ci in Repository.Query<CatalogItem>(new { CatalogCuid = catalogCuid }))
            {
                ItemDefinition item = Repository.Query<ItemDefinition>(new { Cuid = ci.ItemKey }).FirstOrDefault();
                if(item != null)
                {
                    ItemDefinition result = Repository.Retrieve<ItemDefinition>(item.Uuid);
                    result.Properties = GetItemProperties(result.Cuid).ToList();
                    yield return result;
                }
            }
        }

        protected IEnumerable<ItemProperty> GetItemProperties(string itemCuid)
        {
            return Repository.Query<ItemProperty>(new { ItemDefinitionCuid = itemCuid });
        }

        public bool DeleteCatalog(ulong catalogKey)
        {
            return Repository.DeleteWhere(typeof(CatalogDefinition), new { Key = catalogKey });
        }

        public ItemDefinition CreateItem(string name)
        {
            return Repository.Create(new ItemDefinition { Name = name });
        }

        public ItemDefinition AddItem(ulong catalogKey, ulong itemKey)
        {
            CatalogDefinition catalog = GetCatalog(catalogKey);
            Args.ThrowIf(catalog == null, "Catalog not found ({0})", catalogKey);
            ItemDefinition item = GetItem(itemKey);
            Args.ThrowIf(item == null, "Item not found ({0})", itemKey);
            CatalogItem xref = new CatalogItem { CatalogKey = catalogKey, ItemKey = itemKey };
            Repository.Save(xref);
            return item;
        }

        public bool RemoveItem(ulong catalogKey, ulong itemKey)
        {
            try
            {
                CatalogItem xref = Repository.Query<CatalogItem>(new { CatalogKey = catalogKey, ItemKey = itemKey }).FirstOrDefault();
                if(xref != null)
                {
                    Repository.Delete(xref);
                }
                return true;
            }
            catch (Exception ex)
            {
                Logger.AddEntry("Error removing item ({0}) from catalog ({1})", ex, itemKey.ToString(), catalogKey.ToString());
                return false;
            }
        }

        public ItemDefinition RenameItem(ulong itemKey, string name)
        {
            ItemDefinition item = GetItem(itemKey);
            item.Name = name;
            return Repository.Save(item);
        }

        public ItemDefinition GetItem(ulong itemKey)
        {
            ItemDefinition item = Repository.Query<ItemDefinition>(new { Key = itemKey }).FirstOrDefault();
            if(item != null)
            {
                return Repository.Retrieve<ItemDefinition>(item.Uuid);
            }
            return null;
        }

        public bool DeleteItem(ulong itemKey)
        {
            return Repository.DeleteWhere<ItemDefinition>(new { Key = itemKey });
        }

        public ulong[] FindItemCatalogs(ulong itemKey)
        {
            return Repository.Query<CatalogItem>(new { CatalogKey = itemKey }).Select(x=> x.CatalogKey).ToArray();
        }
    }
}
