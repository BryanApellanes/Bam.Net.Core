namespace Bam.Net.Services.DataReplication.Consensus
{
    public class RaftResult
    {
        public RaftResult()
        {
            Success = true; 
        }

        public RaftResult(RaftRequest request) : this()
        {
            CollationPath = request.CollationPath;
        }

        public bool Success { get; set; }
        public string Message { get; set; }
        public string CollationPath { get; set; }
    }
}