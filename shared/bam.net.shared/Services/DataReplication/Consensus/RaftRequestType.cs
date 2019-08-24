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
        LogSyncResponse,
        
        RetrieveRequest, // blocking operation, response sent in RaftResponse
        
        QueryRequest // blocking operation, response sent in RaftResponse
    }
}