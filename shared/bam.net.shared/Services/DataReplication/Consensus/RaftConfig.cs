using System.Collections.Generic;

namespace Bam.Net.Services.DataReplication.Consensus
{
    public class RaftConfig
    {
        public bool IncludeLocalNode { get; set; }
        public int HeartbeatTimeout { get; set; }
        public List<RaftNodeInfo> ServerNodes { get; set; }
    }
}