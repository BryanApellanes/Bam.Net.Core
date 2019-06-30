using System;
using Bam.Net.Data.Repositories;
using Bam.Net.Services.DataReplication.Consensus.Data;

namespace Bam.Net.Services.DataReplication.Consensus
{
    public class ConsensusLogReplicationProvider : ILogReplicationProvider
    {
        public ConsensusLogReplicationProvider(ConsensusRing ring)
        {
            ConsensusRing = ring;
        }
        
        public ConsensusRing ConsensusRing { get; set; }
        public ConsensusServiceNode ConsensusServiceNode { get; set; }
        public DaoRepository LocalRepository { get; set; }
        public string LeaderIdentifier { get; set; }
        
        public void ReplicateValue(ReplicateLogEntryRequest replicationRequest)
        {
            Args.ThrowIfNull(replicationRequest, "replicationRequest");
            Args.ThrowIfNull(replicationRequest.LogEntry, "replicationRequest.LogEntry");
            
            if (replicationRequest.RequesterNodeIdentifier.Equals(LeaderIdentifier)
            && ConsensusServiceNode.NodeIdentifier.Equals(LeaderIdentifier) &&
            ConsensusServiceNode.NodeState == ConsensusNodeState.Leader)
            {
                LogEntry toSave = new LogEntry()
                {
                    Base64Key = replicationRequest.LogEntry.Key.ToBinaryBytes().ToBase64(),
                    Base64Value = replicationRequest.LogEntry.Value.ToBinaryBytes().ToBase64()
                };
                LogEntry logEntry = LocalRepository.Save(toSave);
                ConsensusRing.ReportCommittedLogEntry(replicationRequest, logEntry);
            }
        }

        public void ReceiveReplicateValueResponse(ReplicateLogEntryResponse replicateLogEntryResponse)
        {
            if (ConsensusServiceNode.NodeState == ConsensusNodeState.Leader)
            {
                // determine if majority of service nodes have committed the specified entry
                // then LeaderCommit
            }

            throw new NotImplementedException();
        }

        public bool MajorityOfFollowersHaveCommitted(ReplicateLogEntryResponse replicateLogEntryResponse)
        {
            // determine if the majority of followers have committed the specified response
            throw new NotImplementedException();
        }

        public void LeaderCommit(LogEntry logEntry)
        {
            if (ConsensusServiceNode.NodeState == ConsensusNodeState.Leader)
            {
                // commit after ReceiveReplicateValueResponse has determined that the majority
                // of followers have committed the entry
            }
            
            throw new NotImplementedException();
        }

        public void FollowerReceiveCommit(LogEntry logEntry)
        {
            // after the leader has received a majority of follower commits the leader notifies
            // followers that the entry is committed by calling this method
            throw new NotImplementedException();
        }
    }
}