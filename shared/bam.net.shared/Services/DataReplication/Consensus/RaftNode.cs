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
        public RaftNode(RaftRing ring)
        {
            RaftRing = ring;
            NodeType = RaftNodeType.Follower;
            
            LocalRepository = new DaoRepository();
            LocalRepository.AddType<RaftLogEntry>();
            LocalRepository.AddType<RaftVote>();
            LocalRepository.AddType<RaftLeaderElection>();
            
            RaftReplicationLog = new RaftReplicationLog();
        }

        public static RaftNode ForCurrentProcess(RaftRing ring)
        {
            return FromIdentifier(ring, RaftNodeIdentifier.ForCurrentProcess());
        }
        
        public static RaftNode ForHost(RaftRing ring, string hostName, int port = RaftNodeIdentifier.DefaultPort)
        {
            return FromIdentifier(ring, new RaftNodeIdentifier() {HostName = hostName, Port = port});
        }
        
        public static RaftNode FromIdentifier(RaftRing ring, RaftNodeIdentifier identifier)
        {
            return new RaftNode(ring)
            {
                Identifier = identifier
            };
        }
        
        public override object Clone()
        {
            RaftNode clone = new RaftNode(RaftRing);
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
        public virtual RaftClient GetClient()
        {
            return new RaftClient(Identifier.HostName, Identifier.Port);
        }
        
        public DaoRepository LocalRepository { get; set; }

        public RaftNodeType NodeType { get; set; }
        
        public RaftReplicationLog RaftReplicationLog { get; set; }


        public virtual void WriteValue(RaftLogEntryWriteRequest writeRequest)
        {
            if (NodeType == RaftNodeType.Leader)
            {
                LeaderWriteValue(writeRequest);
            }
            else
            {
                RaftRing?.BroadcastWriteRequestToFollowers(writeRequest);
            }
        }

        public event EventHandler ValueWrittenAsLeader;
        public event EventHandler ValueCommittedAsLeader;
        public event EventHandler ValueWrittenAsFollower;
        public event EventHandler ValueCommittedAsFollower;
        /// <summary>
        /// Write a new value for the specified key if this is a Leader node.  Otherwise ignore the request.
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public virtual void LeaderWriteValue(RaftLogEntryWriteRequest writeRequest)
        {
            Args.ThrowIfNull(writeRequest, "writeRequest");
            Args.ThrowIfNull(writeRequest.LogEntry, "writeRequest.LogEntry");
            
            if (NodeType == RaftNodeType.Leader)
            {
                LocalRepository.SaveAsync(writeRequest.LogEntry);
                FireEvent(ValueWrittenAsLeader, new RaftLogEntryWrittenEventArgs() {WriteRequest = writeRequest});
            }
        }

        public virtual void LeaderCommitValue(RaftLogEntryWriteRequest writeRequest)
        {
            Args.ThrowIfNull(writeRequest, "writeRequest");
            Args.ThrowIfNull(writeRequest.LogEntry, "writeRequest.LogEntry");

            if (NodeType == RaftNodeType.Leader)
            {
                RaftLogEntry logEntry = writeRequest.LogEntry;
                logEntry.State = RaftLogEntryState.Committed;
                LocalRepository.SaveAsync(logEntry);
                FireEvent(ValueCommittedAsLeader, new RaftLogEntryWrittenEventArgs(){WriteRequest = writeRequest});
            }
        }
        
        public virtual void FollowerWriteValue(RaftLogEntryWriteRequest writeRequest)
        {
            if (!FollowerWriteRequestIsValid(writeRequest))
            {
                return;
            }

            if (writeRequest.LogEntry.State != RaftLogEntryState.Uncommitted)
            {
                Info("FollowerWriteValue called but the specified LogEntry is already committed: {0}", writeRequest.LogEntry.GetId().ToString());
                return;
            }

            LocalRepository.SaveAsync(writeRequest.LogEntry);
            FireEvent(ValueWrittenAsFollower, new RaftLogEntryWrittenEventArgs() {WriteRequest = writeRequest});
        }

        public virtual void FollowerCommitValue(RaftLogEntryWriteRequest writeRequest)
        {
            if (!FollowerWriteRequestIsValid(writeRequest))
            {
                return;
            }

            writeRequest.LogEntry.State = RaftLogEntryState.Committed;
            LocalRepository.SaveAsync(writeRequest.LogEntry);
            
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
        
        private bool FollowerWriteRequestIsValid(RaftLogEntryWriteRequest writeRequest)
        {
            Args.ThrowIfNull(writeRequest, "writeRequest");
            Args.ThrowIfNull(writeRequest.LogEntry, "writeRequest.LogEntry");

            if (NodeType != RaftNodeType.Follower)
            {
                Info("FollowerWriteValue called but this node is not a follower: {0}", this.ToString());
                return false;
            }

            return true;
        }
    }
}