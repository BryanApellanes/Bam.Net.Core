using Bam.Net.Data.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bam.Net.Services.Catalog.Data
{
    [Serializable]
    public class CatalogItem: KeyedAuditRepoData
    {
        public ulong CatalogKey { get; set; }
        public ulong ItemKey { get; set; }
    }
}
