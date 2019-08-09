using System;
using Bam.Net.Services.DataReplication.Consensus.Data;

namespace Bam.Net.Services.DataReplication.Consensus
{
    public class RaftLogEntryCommittedEventArgs : EventArgs
    {
        public RaftLogEntryCommittedEventArgs()
        {
            
        }
        
        public RaftNodeIdentifier CommittingNode { get; set; }
        public RaftNodeIdentifier DataSourceNode { get; set; }
        public RaftLogEntry LogEntry { get; set; }
    }
}