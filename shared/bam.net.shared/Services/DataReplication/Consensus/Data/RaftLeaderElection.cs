using System;
using System.Collections.Generic;
using System.Linq;
using Bam.Net.Data.Repositories;
using Bam.Net.Services.DataReplication.Consensus.Data.Dao.Repository;

namespace Bam.Net.Services.DataReplication.Consensus.Data
{
    public class RaftLeaderElection : CompositeKeyAuditRepoData
    {
        public RaftLeaderElection()
        {
        }
        
        [CompositeKey]
        public int Term { get; set; }

        HashSet<RaftVote> _votes;

        public virtual List<RaftVote> Votes
        {
            get => _votes.ToList();
            set => _votes = new HashSet<RaftVote>(value);
        }

        static readonly object _forTermLock = new object();
        public static RaftLeaderElection ForTerm(int term, RaftConsensusRepository repository)
        {
            lock (_forTermLock)
            {
                return repository.GetOneRaftLeaderElectionWhere(le => le.Term == term);
            }
        }
    }
}