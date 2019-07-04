using Bam.Net.Data.Repositories;

namespace Bam.Net.Services.DataReplication.Consensus.Data
{
    public class RaftVote : AuditRepoData
    {
        public string FromNodeIdentifier { get; set; }
        public string ForNodeIdentifier { get; set; }
        
        public ulong LeaderElectionId { get; set; }
        public virtual RaftLeaderElection RaftLeaderElection { get; set; }
    }
}