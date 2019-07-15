using Bam.Net.Services.DataReplication.Consensus.Data;

namespace Bam.Net.Services.DataReplication.Consensus
{
    public class RaftRequest
    {
        public string RequesterHostName { get; set; }
        public int RequesterPort { get; set; }
        public RaftRequestType RequestType { get; set; }
        public int ElectionTerm { get; set; }
        
        /// <summary>
        /// For write RequestTypes 
        /// </summary>
        public RaftLogEntryWriteRequest WriteRequest { get; set; }

        /// <summary>
        /// Response to a vote request or null for non vote requests.
        /// </summary>
        public RaftVote VoteResponse { get; set; }
        
        public RaftNodeIdentifier RequesterIdentifier()
        {
            return new RaftNodeIdentifier(RequesterHostName, RequesterPort);
        }

        public RaftClient GetResponseClient()
        {
            return new RaftClient(RequesterHostName, RequesterPort);
        }
    }
}