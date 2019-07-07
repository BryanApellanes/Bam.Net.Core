namespace Bam.Net.Services.DataReplication.Consensus
{
    public class RaftRequest
    {
        public ulong RequesterNodeIdentifier { get; set; }
        public RaftRequestType RequestType { get; set; }
        public RaftLogEntryWriteRequest WriteRequest { get; set; }
    }
}