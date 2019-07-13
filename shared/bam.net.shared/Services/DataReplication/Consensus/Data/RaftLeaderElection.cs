using System;
using System.Collections.Generic;
using System.Linq;
using Bam.Net.Data.Repositories;

namespace Bam.Net.Services.DataReplication.Consensus.Data
{
    public class RaftLeaderElection : CompositeKeyAuditRepoData
    {
        public RaftLeaderElection()
        {
        }
        
        public int Term { get; set; }

        HashSet<RaftVote> _votes;

        public virtual List<RaftVote> Votes
        {
            get
            {
                return _votes.ToList();
            }
            set
            {
                _votes = new HashSet<RaftVote>(value);
            }
        }
    }
}