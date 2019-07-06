using Bam.Net.Services.DataReplication.Consensus.Data;

namespace Bam.Net.Services.DataReplication.Consensus
{
    public class RaftLogEntryWriteRequest
    {
        public RaftLogEntryWriteRequest(object key, object value)
        {
            LogEntry = new RaftLogEntry()
            {
                Base64Key = key.ToBinaryBytes().ToBase64(),
                Base64Value = value.ToBinaryBytes().ToBase64()
            };
        }
        
        public RaftLogEntry LogEntry { get; set; }
        
        public RaftNodeType TargetNodeType { get; set; }
    }
}