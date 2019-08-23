using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Bam.Net.CoreServices;
using Bam.Net.Data.Repositories;
using Bam.Net.Server;
using Bam.Net.Services.Catalog.Data;
using GraphQL;

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
            AddCatalogTypes();
        }

        public override object Clone()
        {
            CatalogService svc = new CatalogService(Repository, CallbackService, DaoRepository, AppConf);
            svc.CopyProperties(this);
            svc.CopyEventHandlers(this);
            return svc;
        }

        protected void AddCatalogTypes()
        {
            string nameSpace = typeof(CatalogDefinition).Namespace;
            Repository.AddNamespace(typeof(CatalogDefinition).Assembly, nameSpace, type => type.ExtendsType<RepoData>());
        }
        
        public virtual CatalogDefinition GetDefaultCatalog()
        {
            return CatalogDefinition.GetDefault(Repository);
        }
        
        /// <summary>
        /// Creates a catalog with the specified name or returns the existing catalog if one exists with the specified name.
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public CatalogDefinition CreateCatalog(string name)
        {
            Args.ThrowIfNullOrEmpty(name, "name");
            
            CatalogDefinition catalog = FindCatalog(name);
            if (catalog == null)
            {
                catalog = new CatalogDefinition
                    {OrganizationKey = ClientOrganization.Key, ApplicationKey = ClientApplication.Key, Name = name};
                return catalog.SaveByKey<CatalogDefinition>(Repository);
            }
            throw new InvalidOperationException($"A catalog named ({name}) already exists (Org={ClientOrganization.Name}, App={ClientApplication.Name})");
        }

        public CatalogDefinition FindCatalog(string name)
        {
            CatalogDefinition catalog = Repository.Query<CatalogDefinition>(new { OrganizationKey = ClientOrganization.Key, ApplicationKey = ClientApplication.Key, Name = name }).FirstOrDefault();
            if (catalog != null)
            {
                return Repository.Retrieve<CatalogDefinition>(catalog.Uuid);
            }

            return null;
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
                catalog.Items = GetCatalogItems(catalog.Key).ToList();
                return catalog;
            }
            return null;
        }

        protected IEnumerable<ItemDefinition> GetCatalogItems(ulong catalogKey)
        {
            foreach(CatalogItem ci in Repository.Query<CatalogItem>(new { CatalogKey = catalogKey }))
            {
                ItemDefinition item = Repository.Query<ItemDefinition>(new { Key = ci.ItemDefinitionKey }).FirstOrDefault();
                if(item != null)
                {
                    ItemDefinition result = Repository.Retrieve<ItemDefinition>(item.Uuid);
                    result.Properties = GetItemProperties(result.Key).ToList();
                    yield return result;
                }
            }
        }

        protected IEnumerable<ItemProperty> GetItemProperties(ulong itemKey)
        {
            return Repository.Query<ItemProperty>(new { ItemDefinitionKey = itemKey });
        }

        public bool DeleteCatalog(ulong catalogKey)
        {
            return Repository.DeleteWhere(typeof(CatalogDefinition), new { Key = catalogKey });
        }

        public ItemDefinition AddItem(ulong catalogKey, string itemName)
        {
            return AddItem(catalogKey, itemName, out CatalogItem ignore);
        }
        
        public ItemDefinition AddItem(ulong catalogKey, string itemName, out CatalogItem catalogItem)
        {
            CatalogDefinition catalog = GetCatalog(catalogKey);
            Args.ThrowIf(catalog == null, "Catalog not found ({0})", catalogKey);
            ItemDefinition item = new ItemDefinition()
            {
                Name = itemName
            };
            item = item.LoadByKey<ItemDefinition>(Repository) ?? item.SaveByKey<ItemDefinition>(Repository);

            CatalogItem xref = new CatalogItem { CatalogKey = catalogKey, ItemDefinitionKey = item.Key };
            catalogItem = Repository.Save(xref);
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
