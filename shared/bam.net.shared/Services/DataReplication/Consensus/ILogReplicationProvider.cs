
using Bam.Net.Services.DataReplication.Consensus.Data;

namespace Bam.Net.Services.DataReplication.Consensus
{
    public interface ILogReplicationProvider
    {
        void FollowerWriteValue(RaftLogEntryWriteRequest writeRequest);

        void LeaderReceiveWriteResponse(RaftLogEntryWriteResponse raftLogEntryWriteResponse);

        bool MajorityOfFollowersHaveWritten(RaftLogEntryWriteResponse raftLogEntryWriteResponse);

        void LeaderCommit(RaftLogEntry raftLogEntry);

        void FollowerReceiveCommit(RaftLogEntry raftLogEntry);
    }
}