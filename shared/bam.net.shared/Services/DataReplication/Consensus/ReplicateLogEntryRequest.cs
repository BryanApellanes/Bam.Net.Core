namespace Bam.Net.Services.DataReplication.Consensus
{
    public class ReplicateLogEntryRequest
    {
        public string RequesterNodeIdentifier { get; set; }
        public ReplicationLogEntry LogEntry { get; set; }
    }
}