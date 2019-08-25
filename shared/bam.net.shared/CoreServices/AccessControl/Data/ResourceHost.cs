using Bam.Net.Data.Repositories;
using System;
using System.Collections.Generic;
using System.Text;

namespace Bam.Net.CoreServices.AccessControl.Data
{
    [Serializable]
    public class ResourceHost: KeyedAuditRepoData
    {
        [CompositeKey]
        public string Name { get; set; }

        public string IpV4Address { get; set; }
        public string IpV6Address { get; set; }
        
        [CompositeKey]
        public string MacAddress { get; set; }
        
        public virtual List<Resource> Resources { get; set; }
    }
}
