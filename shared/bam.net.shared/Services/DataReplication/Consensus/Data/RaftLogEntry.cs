using System;
using Bam.Net.Data.Repositories;

namespace Bam.Net.Services.DataReplication.Consensus.Data
{
    [Serializable]
    public class RaftLogEntry: AuditRepoData
    {
        public RaftLogEntryState State { get; set; }
        public string Base64Key { get; set; }
        public string Base64Value { get; set; }
    }
}