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

namespace Bam.Net.Services.DataReplication.Consensus
{
    /// <summary>
    /// Represents a single node in a raft ring.
    /// </summary>
    public class RaftNode : ApplicationProxyableService, IDistributedRepository
    {
        public RaftNode()
        {
            NodeType = RaftNodeType.Follower;
            
            LocalRepository = new DaoRepository();
            LocalRepository.AddType<RaftLogEntry>();
            LocalRepository.AddType<RaftVote>();
            LocalRepository.AddType<RaftLeaderElection>();
            
            RaftReplicationLog = new RaftReplicationLog();
        }

        public static RaftNode FromIdentifier(RaftNodeIdentifier identifier)
        {
            return new RaftNode()
            {
                Identifier = identifier
            };
        }
        
        public override object Clone()
        {
            RaftNode clone = new RaftNode();
            clone.CopyProperties(this);
            clone.CopyEventHandlers(this);
            return clone;
        }
        
        public RaftRing RaftRing { get; set; }

        public RaftNodeIdentifier Identifier { get; set; }
        
        /// <summary>
        /// Get a RaftClient for the current node.  
        /// </summary>
        /// <returns></returns>
        public RaftClient GetClient()
        {
            return new RaftClient(Identifier.HostName, Identifier.Port);
        }
        
        public DaoRepository LocalRepository { get; set; }

        public RaftNodeType NodeType { get; set; }
        
        public RaftReplicationLog RaftReplicationLog { get; set; }

        /// <summary>
        /// Accept a new value for the specified key if this is a Leader node.  Otherwise ignore the request.
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public virtual void LeaderWriteValue(object key, object value)
        {
            Args.ThrowIfNull(key, "key");
            Args.ThrowIfNull(value, "value");
            
            if (NodeType == RaftNodeType.Leader)
            {;
                FollowerWriteValue(new RaftLogEntryWriteRequest()
                {
                    LogEntry = new RaftLogEntry()
                    {
                        Base64Key = key.ToBinaryBytes().ToBase64(),
                        Base64Value = value.ToBinaryBytes().ToBase64()
                    }
                });
            }
        }

        public virtual RaftLogEntry FollowerWriteValue(RaftLogEntryWriteRequest writeWriteRequest)
        {
            throw new NotImplementedException();
        }

        public virtual void LeaderReceiveWriteResponse(RaftLogEntryWriteResponse raftLogEntryWriteResponse)
        {
            throw new NotImplementedException();
        }

        public virtual void LeaderCommit(RaftLogEntry raftLogEntry)
        {
            throw new NotImplementedException();
        }

        public virtual void FollowerReceiveCommit(RaftLogEntry raftLogEntry)
        {
            throw new NotImplementedException();
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