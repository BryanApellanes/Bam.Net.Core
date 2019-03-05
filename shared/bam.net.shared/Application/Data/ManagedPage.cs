using Bam.Net.Configuration;
using Bam.Net.Data.Repositories;
using System;
using System.Collections.Generic;
using System.Text;

namespace Bam.Net.Application.Data
{
    public class ManagedPage: CompositeKeyRepoData
    {
        public ManagedPage()
        {
            OrgnizationName = DefaultConfigurationOrganizationNameProvider.Instance.GetOrganizationName();
        }
        
        [CompositeKey]
        public string OrgnizationName { get; set; }

        [CompositeKey]
        public string ApplicationName { get; set; }

        [CompositeKey]
        public string Name { get; set; }

        public ulong ManagedApplicationId { get; set; }
        public virtual ManagedApplication ManagedApplication { get; set; }
        public virtual List<ManagedComponent> Components { get; set; }
    }
}
