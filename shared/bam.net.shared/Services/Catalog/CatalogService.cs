using System;
using System.Collections.Generic;
using System.Linq;
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
            return catalog != null ? Repository.Retrieve<CatalogDefinition>(catalog.Uuid) : null;
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
                ItemDefinition item = Repository.Query<ItemDefinition>(new { Key = ci.ItemKey }).FirstOrDefault();
                if(item != null)
                {
                    ItemDefinition result = Repository.Retrieve<ItemDefinition>(item.Uuid);
                    result.Properties = GetItemProperties(result.Key).ToList();
                    yield return result;
                }
            }
        }

        protected ItemDefinition GetItemDefinition(ulong itemDefinitionKey)
        {
            ItemDefinition itemDefinition = Repository.Query<ItemDefinition>(new {Key = itemDefinitionKey}).FirstOrDefault();
            return itemDefinition != null ? Repository.Retrieve<ItemDefinition>(itemDefinition.Uuid) : null;
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
            var item = CreateItem(itemName);

            CatalogItem xref = new CatalogItem { CatalogKey = catalogKey, ItemKey = item.Key };
            catalogItem = Repository.Save(xref);
            return item;
        }

        public ItemDefinition CreateItem(string itemName)
        {
            ItemDefinition item = new ItemDefinition()
            {
                Name = itemName
            };
            item = item.LoadByKey<ItemDefinition>(Repository) ?? item.SaveByKey<ItemDefinition>(Repository);
            return item;
        }

        public IEnumerable<ItemProperty> AddItemProperties(ulong itemKey, object properties)
        {
            Dictionary<string, string> propDictionary = properties.ToDictionary<string>(o => o.ToString());
            return AddItemProperties(itemKey, propDictionary);
        }

        public IEnumerable<ItemProperty> AddItemProperties(ulong itemKey, Dictionary<string, string> properties)
        {
            ItemDefinition itemDefinition = GetItemDefinition(itemKey);
            Args.ThrowIf(itemDefinition == null, "ItemDefinition not found ({0})", itemKey);
            foreach (ItemProperty itemProperty in AddItemProperties(itemDefinition, properties))
            {
                yield return itemProperty;
            }
        }

        protected IEnumerable<ItemProperty> AddItemProperties(ItemDefinition itemDefinition, Dictionary<string, string> properties)
        {
            foreach (string propertyName in properties.Keys)
            {
                ItemProperty itemProperty = new ItemProperty
                {
                    ItemDefinitionId = itemDefinition.Id, ItemKey = itemDefinition.Key, Name = propertyName,
                    Value = properties[propertyName]
                };
                itemProperty = itemProperty.SaveByKey<ItemProperty>(Repository);
                yield return itemProperty;
            }
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
        
        protected IEnumerable<ItemProperty> GetItemProperties(ulong itemKey)
        {
            return Repository.Query<ItemProperty>(new { ItemKey = itemKey });
        }
    }
}
