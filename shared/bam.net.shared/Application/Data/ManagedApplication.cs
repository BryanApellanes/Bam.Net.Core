﻿using Bam.Net.Data.Repositories;
using System;
using System.Collections.Generic;
using System.Text;

namespace Bam.Net.Application.Data
{
    public class ManagedApplication : KeyedAuditRepoData
    {
        public virtual List<ManagedPage> Pages { get; set; }
    }
}
