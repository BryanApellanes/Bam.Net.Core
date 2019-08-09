namespace Bam.Net.Services.DataReplication.Consensus
{
    public enum RaftRequestType
    {
        Invalid,
        WriteValue,
        NotifyLeaderFollowerValueWritten,
        NotifyFollowerLeaderValueCommitted,
        
        VoteRequest,
        VoteResponse,
        
        Heartbeat,
        
        JoinRaft,
        
        LogSyncRequest,
        LogSyncResponse
    }
}