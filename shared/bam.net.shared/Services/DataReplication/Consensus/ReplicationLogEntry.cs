using System;

namespace Bam.Net.Services.DataReplication.Consensus
{
    [Serializable]
    public class ReplicationLogEntry
    {
        public LogEntryState State { get; set; }
        public object Key { get; set; }
        public object Value { get; set; }
    }
}