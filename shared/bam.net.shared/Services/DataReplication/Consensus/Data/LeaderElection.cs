using System.Collections.Generic;
using System.Linq;
using Bam.Net.Data.Repositories;

namespace Bam.Net.Services.DataReplication.Consensus.Data
{
    public class LeaderElection : CompositeKeyAuditRepoData
    {
        public LeaderElection()
        {
            
        }
        
        public int Term { get; set; }

        HashSet<Vote> _votes;

        public List<Vote> Votes
        {
            get
            {
                return _votes.ToList();
            }
            set
            {
                _votes = new HashSet<Vote>(value);
            }
        }
    }
}