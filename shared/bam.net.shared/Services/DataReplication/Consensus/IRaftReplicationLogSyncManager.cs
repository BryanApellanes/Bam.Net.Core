namespace Bam.Net.Services.DataReplication.Consensus
{
    public interface IRaftReplicationLogSyncManager
    {
        void HandleReplicationLog(RaftReplicationLog log);
    }
}