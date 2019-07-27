using System.Collections.Generic;

namespace Bam.Net.Services.DataReplication.Consensus
{
    public class RaftConfig
    {
        public int HeartbeatTimeout { get; set; }
        public List<RaftNodeInfo> ServerNodes { get; set; }
    }
}