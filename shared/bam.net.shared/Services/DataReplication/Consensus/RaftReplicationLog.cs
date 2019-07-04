using System.Collections.Generic;
using Bam.Net.Services.DataReplication.Consensus.Data;

namespace Bam.Net.Services.DataReplication.Consensus
{
    public class RaftReplicationLog
    {
        public RaftReplicationLog()
        {
            Entries = new List<RaftLogEntry>();
        }
        
        public List<RaftLogEntry> Entries { get; } 
    }
}