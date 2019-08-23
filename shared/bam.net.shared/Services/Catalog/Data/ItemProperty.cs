using Bam.Net.Data.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace Bam.Net.Services.Catalog.Data
{
    [Serializable]
    public class ItemProperty: KeyedAuditRepoData
    {
        public ulong ItemDefinitionId { get; set; }
        public virtual ItemDefinition ItemDefinition { get; set; }
        
        [CompositeKey]
        public ulong ItemKey { get; set; }
        
        [CompositeKey]
        public string Name { get; set; }
        public string Value { get; set; }
    }
}
