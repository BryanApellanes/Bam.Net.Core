namespace Bam.Net.Services.DataReplication.Consensus
{
    public class RaftResponse
    {
        public string Message { get; set; }
        public bool Success { get; set; }
        public RaftRequest Request { get; set; }
        public object Data { get; set; }
    }
}