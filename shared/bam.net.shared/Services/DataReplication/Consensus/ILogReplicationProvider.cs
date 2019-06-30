
using Bam.Net.Services.DataReplication.Consensus.Data;

namespace Bam.Net.Services.DataReplication.Consensus
{
    public interface ILogReplicationProvider
    {
        void ReplicateValue(ReplicateLogEntryRequest replicateLogEntryRequest);

        void ReceiveReplicateValueResponse(ReplicateLogEntryResponse replicateLogEntryResponse);

        bool MajorityOfFollowersHaveCommitted(ReplicateLogEntryResponse replicateLogEntryResponse);

        void LeaderCommit(LogEntry logEntry);

        void FollowerReceiveCommit(LogEntry logEntry);
    }
}