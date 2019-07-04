using Bam.Net.Services.DataReplication.Consensus.Data;

namespace Bam.Net.Services.DataReplication.Consensus
{
    public class RaftLogEntryWriteRequest
    {
        public RaftLogEntry LogEntry { get; set; }
    }
}