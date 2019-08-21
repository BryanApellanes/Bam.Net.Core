using System;
using System.Collections.Generic;
using Bam.Net.Data.Repositories;
using Bam.Net.Services.DataReplication.Data;

namespace Bam.Net.Services.Catalog.Data
{
    [Serializable]
    public class ItemDefinition: KeyedAuditRepoData
    {
        [CompositeKey]
        public string Name { get; set; }
        public string Description { get; set; }
        public virtual List<ItemProperty> Properties { get; set; }
        public ItemDefinition Set(string propertyName, object value)
        {
            Properties.Add(new ItemProperty { Name = propertyName, Value = value });
            return this;
        }
    }
}
