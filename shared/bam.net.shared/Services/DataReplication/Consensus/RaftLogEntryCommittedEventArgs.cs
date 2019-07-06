namespace Bam.Net.Services.DataReplication.Consensus
{
    public class RaftLogEntryCommittedEventArgs
    {
        public RaftLogEntryCommittedEventArgs()
        {
            
        }
        
        public RaftLogEntryWriteRequest WriteRequest { get; set; }
    }
}