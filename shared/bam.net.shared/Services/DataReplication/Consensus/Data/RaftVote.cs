using System;
using Bam.Net.Data.Repositories;
using Bam.Net.Services.DataReplication.Consensus.Data.Dao.Repository;

namespace Bam.Net.Services.DataReplication.Consensus.Data
{
    public class RaftVote : CompositeKeyAuditRepoData
    {
        [CompositeKey]
        public string FromNodeIdentifier { get; set; }
        
        [CompositeKey]
        public string ForNodeIdentifier { get; set; }
        
        [CompositeKey]
        public string ElectionKey { get; set; }
        
        // DaoRepository relationship
        public virtual ulong RaftLeaderElectionId { get; set; }
        public virtual RaftLeaderElection RaftLeaderElection { get; set; }
        // -- DaoRepository relationship

        public static RaftVote Cast(RaftConsensusRepository repository, RaftRequest request)
        {
            return Cast(repository, request.ElectionTerm, request);
        }
        
        public static RaftVote Cast(RaftConsensusRepository repository, int term, RaftRequest request)
        {
            Data.RaftLeaderElection election = RaftLeaderElection.ForTerm(term, repository);
            RaftVote termVote = ForElection(repository, election.CompositeKey);
            if (termVote == null) // if the current node hasn't voted, vote for the requester
            {
                termVote = Cast(repository, term, request.RequesterIdentifier());
            }

            return termVote;
        }

        public static RaftVote ForElection(RaftConsensusRepository repository, Data.RaftLeaderElection election)
        {
            return ForElection(repository, election.CompositeKey);
        }
        
        public static RaftVote ForElection(RaftConsensusRepository repository, string electionKey)
        {
            return repository.OneRaftVoteWhere(v => v.ElectionKey == electionKey && v.FromNodeIdentifier == RaftNodeIdentifier.ForCurrentProcess().CompositeKey);
        }

        public static RaftVote Create(RaftLeaderElection election, RaftNodeIdentifier voter, RaftNodeIdentifier voteFor)
        {
            Args.ThrowIf(!election.GetIsPersisted(), "Specified election is not persisted");
            
            return new RaftVote
            {
                FromNodeIdentifier = voter.CompositeKey,
                ForNodeIdentifier = voteFor.CompositeKey,
                ElectionKey = election.CompositeKey,
                RaftLeaderElectionId = election.Id
            };
        }

        static readonly object _castLock = new object();
        public static RaftVote Cast(RaftConsensusRepository repository, int term, RaftNodeIdentifier voteFor)
        {
            Data.RaftLeaderElection election = RaftLeaderElection.ForTerm(term, repository);
            RaftNodeIdentifier voter = RaftNodeIdentifier.FromRepository(repository);
            return Cast(repository, election, voter, voteFor);
        }
        
        public static RaftVote Cast(RaftConsensusRepository repository, RaftLeaderElection election,
            RaftNodeIdentifier voter, RaftNodeIdentifier voteFor)
        {
            lock (_castLock)
            {
                RaftVote vote = Create(election, voter, voteFor);
                return repository.GetByCompositeKey<RaftVote>(vote);
            }
        }
    }
}