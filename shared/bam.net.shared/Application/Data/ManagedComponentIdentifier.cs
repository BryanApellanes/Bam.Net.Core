using Bam.Net.Data.Repositories;
using System;
using System.Collections.Generic;
using System.Text;

namespace Bam.Net.Application.Data
{
    public class ManagedComponentIdentifier: CompositeKeyRepoData
    {
        public string ManagedComponentCuid { get; set; }

        public virtual List<ManagedComponent> ManagedComponents { get; set; }
    }
}
