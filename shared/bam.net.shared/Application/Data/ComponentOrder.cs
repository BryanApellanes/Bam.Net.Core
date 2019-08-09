using Bam.Net.Data.Repositories;
using System;
using System.Collections.Generic;
using System.Text;

namespace Bam.Net.Application.Data
{
    public class ComponentOrder : KeyedAuditRepoData
    {
        [CompositeKey]
        public string PageCuid { get; set; }

        [CompositeKey]
        public string ComponentCuid { get; set; }

        public int Index { get; set; }
    }
}
