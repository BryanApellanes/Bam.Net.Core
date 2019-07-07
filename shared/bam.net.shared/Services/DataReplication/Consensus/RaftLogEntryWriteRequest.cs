using System.Collections.Generic;
using Bam.Net.Data.Repositories;
using Bam.Net.Services.DataReplication.Consensus.Data;

namespace Bam.Net.Services.DataReplication.Consensus
{
    public class RaftLogEntryWriteRequest
    {
        public RaftLogEntryWriteRequest()
        {
        }

        public static IEnumerable<RaftLogEntryWriteRequest> FromData(CompositeKeyAuditRepoData data)
        {
            foreach (RaftLogEntry raftLogEntry in RaftLogEntry.FromInstance(data))
            {
                yield return new RaftLogEntryWriteRequest()
                {
                    LogEntry = raftLogEntry
                };
            }
        } 
        
        public RaftLogEntry LogEntry { get; set; }
        
        public RaftNodeType TargetNodeType { get; set; }

        public RaftLogEntryWriteRequest LeaderCopy(RaftLogEntryState state = RaftLogEntryState.Uncommitted)
        {
            Args.ThrowIfNull(LogEntry, "LogEntry");
            
            RaftLogEntryWriteRequest copy = this.CopyAs<RaftLogEntryWriteRequest>();
            copy.TargetNodeType = RaftNodeType.Leader;
            copy.LogEntry = copy.LogEntry.CopyAs<RaftLogEntry>();
            copy.LogEntry.State = state;
            return copy;
        }

        public RaftLogEntryWriteRequest FollowerCopy(RaftLogEntryState state = RaftLogEntryState.Uncommitted)
        {
            Args.ThrowIfNull(LogEntry, "LogEntry");
            
            RaftLogEntryWriteRequest copy = this.CopyAs<RaftLogEntryWriteRequest>();
            copy.TargetNodeType = RaftNodeType.Follower;
            copy.LogEntry = copy.LogEntry.CopyAs<RaftLogEntry>();
            copy.LogEntry.State = state;
            return copy;
        }
    }
}