using System.Collections.Generic;

namespace Bam.Net.Services.DataReplication.Consensus
{
    public class ReplicationLog
    {
        public ReplicationLog()
        {
            Entries = new List<ReplicationLogEntry>();
        }
        
        public List<ReplicationLogEntry> Entries { get; } 
    }
}