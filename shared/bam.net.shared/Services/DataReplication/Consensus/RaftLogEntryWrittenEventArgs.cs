using System;

namespace Bam.Net.Services.DataReplication.Consensus
{
    public class RaftLogEntryWrittenEventArgs : EventArgs
    {
        public RaftLogEntryWriteRequest WriteRequest { get; set; }
    }
}