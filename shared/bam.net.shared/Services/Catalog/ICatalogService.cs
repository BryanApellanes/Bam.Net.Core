using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Bam.Net.Services.Catalog.Data;

namespace Bam.Net.Services
{
    public interface ICatalogService
    {
        CatalogDefinition GetDefaultCatalog();
        CatalogDefinition CreateCatalog(string name);
        CatalogDefinition FindCatalog(string name);
        CatalogDefinition RenameCatalog(ulong catalogKey, string name);
        CatalogDefinition GetCatalog(ulong catalogKey);
        bool DeleteCatalog(ulong catalogKey);
        ItemDefinition CreateItem(string name);
        ItemDefinition AddItem(ulong catalogKey, ulong itemKey);
        bool RemoveItem(ulong catalogKey, ulong itemKey);
        ItemDefinition RenameItem(ulong itemKey, string name);
        ItemDefinition GetItem(ulong itemKey);
        bool DeleteItem(ulong itemKey);
        ulong[] FindItemCatalogs(ulong itemKey);
    }
}
