using System;
using Bam.Net.Data.Repositories;
using Bam.Net.Services.DataReplication.Consensus.Data;

namespace Bam.Net.Services.DataReplication.Consensus
{
    public class RaftLogReplicationProvider : ILogReplicationProvider
    {
        public RaftLogReplicationProvider(RaftRing ring, RaftNode node)
        {
            RaftRing = ring;
        }
        
        public RaftRing RaftRing { get; set; }
        public RaftNode Node { get; set; }
        
        public void FollowerWriteValue(RaftLogEntryWriteRequest writeRequest)
        {
            Args.ThrowIfNull(writeRequest, "replicationRequest");
            Args.ThrowIfNull(writeRequest.LogEntry, "replicationRequest.LogEntry");
            Args.ThrowIf(writeRequest.LogEntry.State == RaftLogEntryState.Committed, "Specified log entry must be in Uncommitted state.");

            RaftLogEntry raftLogEntry = Node.FollowerWriteValue(writeRequest);
            RaftRing.NotifyLeaderLogEntryCommitted(raftLogEntry);
        }

        public void LeaderReceiveWriteResponse(RaftLogEntryWriteResponse writeResponse)
        {
            // determine if majority of service nodes have committed the specified entry
            // then LeaderCommit
            if (MajorityOfFollowersHaveWritten(writeResponse))
            {
                LeaderCommit(writeResponse.LogEntry);
            }
        }

        public bool MajorityOfFollowersHaveWritten(RaftLogEntryWriteResponse writeResponse)
        {
            // determine if the majority of followers have committed the specified response
            
            throw new NotImplementedException();
        }

        public void LeaderCommit(RaftLogEntry raftLogEntry)
        {
            // commit after ReceiveReplicateValueResponse has determined that the majority
            // of followers have committed the entry
            
            throw new NotImplementedException();
        }

        public void FollowerReceiveCommit(RaftLogEntry raftLogEntry)
        {
            // after the leader has received a majority of follower commits the leader notifies
            // followers that the entry is committed by calling this method
            throw new NotImplementedException();
        }
    }
}