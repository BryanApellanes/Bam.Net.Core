namespace Bam.Net.Services.DataReplication.Consensus
{
    public class RaftResult
    {
        public RaftResult()
        {
            Status = RaftResultStatus.Success;
        }

        public RaftResult(RaftRequest request) : this()
        {
            CollationPath = request.CollationPath;
        }

        public RaftResultStatus Status { get; set; }
        public string Message { get; set; }
        public string CollationPath { get; set; }
        public object Data { get; set; }
    }
}