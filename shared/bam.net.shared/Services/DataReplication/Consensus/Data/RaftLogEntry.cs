using System;
using Bam.Net.Data.Repositories;

namespace Bam.Net.Services.DataReplication.Consensus.Data
{
    [Serializable]
    public class RaftLogEntry: CompositeKeyAuditRepoData
    {
        public RaftLogEntryState State { get; set; }
        
        [CompositeKey]
        public string Base64Key { get; set; }
        
        [CompositeKey]
        public string Base64Value { get; set; }

        public ulong GetId()
        {
            return GetULongKeyHash();
        }
    }
}