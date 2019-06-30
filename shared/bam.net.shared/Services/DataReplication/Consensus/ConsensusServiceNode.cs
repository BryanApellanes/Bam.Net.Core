using System;
using System.Collections.Generic;
using System.Reflection.Metadata.Ecma335;
using System.Threading.Tasks;
using Bam.Net.CoreServices;
using Bam.Net.Data.Repositories;
using Bam.Net.Logging;
using Bam.Net.Services.DataReplication.Consensus.Data;
using Bam.Net.Services.DataReplication.Data;
using Lucene.Net.Index;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using LogEntry = Bam.Net.Services.DataReplication.Consensus.Data.LogEntry;

namespace Bam.Net.Services.DataReplication.Consensus
{
    public class ConsensusServiceNode : ApplicationProxyableService, IDistributedRepository
    {
        public ConsensusServiceNode()
        {
            NodeState = ConsensusNodeState.Follower;
            
            LocalRepository = new DaoRepository();
            LocalRepository.AddType<LogEntry>();
            LocalRepository.AddType<Vote>();
            LocalRepository.AddType<LeaderElection>();

            
            
            ReplicationLog = new ReplicationLog();
        }
        
        public override object Clone()
        {
            ConsensusServiceNode clone = new ConsensusServiceNode();
            clone.CopyProperties(this);
            clone.CopyEventHandlers(this);
            return clone;
        }
        
        public ConsensusRing ConsensusRing { get; set; }
        public ILogReplicationProvider LogReplicationProvider { get; set; }
        public string LeaderIdentifier { get; set; }
        public string NodeIdentifier { get; set; }
        
        public Ring<ConsensusServiceNode> ParticipantNodes { get; set; }
        
        public DaoRepository LocalRepository { get; set; }

        public ConsensusNodeState NodeState { get; set; }
        
        public ReplicationLog ReplicationLog { get; set; }

        public void BeginLifeCycle()
        {
            
        }

        protected void HeartBeat()
        {
            
        }

        protected void BecomeCandidate()
        {
            NodeState = ConsensusNodeState.Candidate;
        }

        public virtual void RequestVotes()
        {
            throw new NotImplementedException();
        }

        public virtual void ReceiveVote(Vote vote)
        {
            LocalRepository.SaveAsync(vote);
        }

        /// <summary>
        /// Accept a new value for the specified key if this is a Leader node.  Otherwise ignore the request.
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public virtual void WriteValue(object key, object value)
        {
            if (NodeState == ConsensusNodeState.Leader)
            {
                ReplicationLogEntry newEntry = new ReplicationLogEntry() {Key = key, Value = value};
                ReplicateValue(new ReplicateLogEntryRequest() {LogEntry = newEntry});
            }
        }

        public virtual void ReplicateValue(ReplicateLogEntryRequest replicationRequest)
        {
            LogReplicationProvider.ReplicateValue(replicationRequest);
        }

        public virtual void ReceiveReplicateValueResponse(ReplicateLogEntryResponse replicateLogEntryResponse)
        {
            LogReplicationProvider.ReceiveReplicateValueResponse(replicateLogEntryResponse);
        }

        protected bool MajorityOfFollowersHaveCommitted(ReplicateLogEntryResponse replicateLogEntryResponse)
        {
            return LogReplicationProvider.MajorityOfFollowersHaveCommitted(replicateLogEntryResponse);
        }

        public virtual void LeaderCommit(LogEntry logEntry)
        {
            LogReplicationProvider.LeaderCommit(logEntry);
        }

        public virtual void FollowerReceiveCommit(LogEntry logEntry)
        {
            LogReplicationProvider.FollowerReceiveCommit(logEntry);
        }
        
        
        // -- IDistributedRepository methods
        public object Save(SaveOperation value)
        {
            throw new NotImplementedException();
        }

        public object Create(CreateOperation value)
        {
            throw new NotImplementedException();
        }

        public object Retrieve(RetrieveOperation value)
        {
            throw new NotImplementedException();
        }

        public object Update(UpdateOperation value)
        {
            throw new NotImplementedException();
        }

        public bool Delete(DeleteOperation value)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<object> Query(QueryOperation query)
        {
            throw new NotImplementedException();
        }

        public ReplicationOperation Replicate(ReplicationOperation operation)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<object> NextSet(ReplicationOperation operation)
        {
            throw new NotImplementedException();
        }
    }
}