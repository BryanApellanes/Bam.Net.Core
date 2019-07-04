using Bam.Net.Services.DataReplication.Consensus.Data;

namespace Bam.Net.Services.DataReplication.Consensus
{
    public class RaftLogEntryWriteResponse
    {
        public string Message { get; set; }
        public bool Success { get; set; }
        public RaftNodeIdentifier NodeIdentifier { get; set; }
        public RaftLogEntry LogEntry { get; set; }
    }
}