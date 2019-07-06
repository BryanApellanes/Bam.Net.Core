namespace Bam.Net.Services.DataReplication.Consensus
{
    public class RaftRequest
    {
        public ulong SenderNodeIdentifier { get; set; }
        public RaftRequestType RequestType { get; set; }
        public RaftLogEntryWriteRequest WriteRequest { get; set; }
        public RaftLogEntryWriteResponse WriteResponse { get; set; }
    }
}