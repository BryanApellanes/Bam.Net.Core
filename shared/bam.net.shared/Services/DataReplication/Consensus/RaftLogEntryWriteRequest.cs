using System;
using System.Collections.Generic;
using Bam.Net.Data.Repositories;
using Bam.Net.Services.DataReplication.Consensus.Data;
using Bam.Net.Services.DataReplication.Data;

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

        public static IEnumerable<RaftLogEntryWriteRequest> FromCreateOperation(CreateOperation createOperation)
        {
            return FromWriteOperation(createOperation); 
        }
        
        public static IEnumerable<RaftLogEntryWriteRequest> FromSaveOperation(SaveOperation saveOperation)
        {
            return FromWriteOperation(saveOperation);
        }

        public static IEnumerable<RaftLogEntryWriteRequest> FromWriteOperation(WriteOperation writeOperation)
        {
            foreach (DataProperty dataProperty in writeOperation.Properties)
            {
                yield return new RaftLogEntryWriteRequest()
                {
                    LogEntry = RaftLogEntry.FromDataProperty(dataProperty)
                };
            }
        }
        
        public RaftLogEntry LogEntry { get; set; }
        
        public RaftNodeState TargetNodeState { get; set; }
        
        public string LeaderIdentifier { get; set; }

        public RaftLogEntryWriteRequest LeaderCopy(RaftLogEntryState state = RaftLogEntryState.Uncommitted)
        {
            Args.ThrowIfNull(LogEntry, "LogEntry");
            
            RaftLogEntryWriteRequest copy = this.CopyAs<RaftLogEntryWriteRequest>();
            copy.TargetNodeState = RaftNodeState.Leader;
            copy.LogEntry = copy.LogEntry.CopyAs<RaftLogEntry>();
            copy.LogEntry.State = state;
            return copy;
        }

        public RaftLogEntryWriteRequest FollowerCopy(RaftLogEntryState state = RaftLogEntryState.Uncommitted, string leaderIdentifier = null)
        {
            Args.ThrowIfNull(LogEntry, "LogEntry");
            
            RaftLogEntryWriteRequest copy = this.CopyAs<RaftLogEntryWriteRequest>();
            copy.TargetNodeState = RaftNodeState.Follower;
            copy.LogEntry = copy.LogEntry.CopyAs<RaftLogEntry>();
            copy.LogEntry.State = state;
            copy.LeaderIdentifier = leaderIdentifier;
            return copy;
        }
    }
}