using System.Collections.Generic;
using System.Threading.Tasks;
using Bam.Net.Data.Repositories;
using Bam.Net.Services.DataReplication.Consensus.Data;

namespace Bam.Net.Services.DataReplication.Consensus
{
    public class RaftReplicationLog
    {
        public RaftReplicationLog()
        {
            Entries = new List<RaftLogEntryCommit>();
        }
        
        public RaftNodeIdentifier SourceNode { get; set; }
        
        public List<RaftLogEntryCommit> Entries { get; internal set; }

        public void AddEntry(IRepository repository, ulong commitSeq, RaftLogEntry raftLogEntry)
        {
            RaftLogEntryCommit commit = new RaftLogEntryCommit()
            {
                Seq = commitSeq,
                RaftLogEntryId = raftLogEntry.CompositeKeyId
            };
            Task.Run(() => Entries.Add(repository.Save(commit)));
        }
    }
}