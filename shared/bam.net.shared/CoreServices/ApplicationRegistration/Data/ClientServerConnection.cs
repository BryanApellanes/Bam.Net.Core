﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Bam.Net.Data.Repositories;

namespace Bam.Net.CoreServices.ApplicationRegistration.Data
{
    [Serializable]
    public class ClientServerConnection: AuditRepoData
    {
        public ulong ClientId { get; set; }
        public virtual ProcessDescriptor Client { get; set; }
        public ulong ServerId { get; set; }
        public virtual ProcessDescriptor Server { get; set; }
    }
}
