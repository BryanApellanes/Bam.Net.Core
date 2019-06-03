using Bam.Net.Data.Repositories;
using System;
using System.Collections.Generic;
using System.Text;

namespace Bam.Net.Application.Data
{
    public class ManagedApplication : NamedAuditRepoData
    {
        public virtual List<ManagedPage> Pages { get; set; }
    }
}
