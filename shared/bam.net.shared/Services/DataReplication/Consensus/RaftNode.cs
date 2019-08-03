using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Threading.Tasks;
using Bam.Net.CoreServices;
using Bam.Net.Data;
using Bam.Net.Data.Repositories;
using Bam.Net.Logging;
using Bam.Net.Services.DataReplication.Consensus.Data.Dao;
using Bam.Net.Services.DataReplication.Consensus.Data.Dao.Repository;
using Bam.Net.Services.DataReplication.Data;
using Lucene.Net.Index;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using RaftLogEntry = Bam.Net.Services.DataReplication.Consensus.Data.RaftLogEntry;
using RaftLogEntryCommit = Bam.Net.Services.DataReplication.Consensus.Data.RaftLogEntryCommit;
using RaftNodeIdentifier = Bam.Net.Services.DataReplication.Consensus.Data.RaftNodeIdentifier;

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
            NodeState = RaftNodeState.Follower;
            
            LocalRepository = new RaftConsensusRepository();
            RaftReplicationLog = new RaftReplicationLog() {SourceNode = ring.LocalNode.Identifier};
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

        public override string ToString()
        {
            return $"{Identifier?.HostName}:{Identifier.Port}";
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
        
        public RaftConsensusRepository LocalRepository { get; set; }

        public RaftNodeState NodeState { get; set; }
        
        public RaftReplicationLog RaftReplicationLog { get; set; }

        public ulong LastCommitSeq
        {
            get
            {
                if (_lastCommit == null)
                {
                    return 0;
                }
                else
                {
                    return _lastCommit.Seq;
                }
            }
        }
        
        /// <summary>
        /// If the current node is the leader calls LeaderWriteValue with a leader copy of the request, otherwise forwards the request to the leader.
        /// </summary>
        /// <param name="writeRequest"></param>
        public virtual void WriteValue(RaftLogEntryWriteRequest writeRequest)
        {
            if (NodeState == RaftNodeState.Leader)
            {
                LeaderWriteValue(writeRequest.LeaderCopy());
            }
            else
            {
                FollowerWriteValue(writeRequest.FollowerCopy());
                RaftRing?.ForwardWriteRequestToLeader(writeRequest.LeaderCopy());
            }
        }

        public event EventHandler LeaderValueWritten;
        public event EventHandler LeaderValueCommitted;
        public event EventHandler FollowerValueWritten;
        public event EventHandler FollowerValueCommitted;

        protected internal virtual void SendLogSyncResponse(RaftRequest request)
        {
            // if we're the leader
            // get all RaftLogEntryCommits since the specified request.CommitSeq
            if (NodeState == RaftNodeState.Leader)
            {
                RaftReplicationLog log = new RaftReplicationLog()
                {
                    SourceNode = Identifier,
                    Entries = LocalRepository.RaftLogEntryCommitsWhere(c => c.Seq >= request.CommitSeq).ToList()
                };
                request.GetResponseClient().SendLogSyncResponse(log);
            }
        }

        /// <summary>
        /// Write a new value for the specified key if this is a Leader node.  Otherwise ignore the request.
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        protected internal virtual void LeaderWriteValue(RaftLogEntryWriteRequest writeRequest)
        {
            ValidateLeaderWriteRequest(writeRequest);

            RaftLogEntry logEntry = writeRequest.LeaderCopy().LogEntry;
            LocalRepository.SaveAsync(logEntry);
            FireEvent(LeaderValueWritten, new RaftLogEntryWrittenEventArgs() {WriteRequest = writeRequest});
        }

        protected internal virtual void LeaderCommitValue(RaftLogEntryWriteRequest writeRequest)
        {
            ValidateLeaderWriteRequest(writeRequest);

            RaftLogEntry logEntry = writeRequest.LeaderCopy(RaftLogEntryState.Committed).LogEntry;
            SetNextCommitEntry(writeRequest);
            RaftReplicationLog.AddEntry(LocalRepository, LastCommitSeq, logEntry);
            LocalRepository.SaveAsync(logEntry);
            FireEvent(LeaderValueCommitted, new RaftLogEntryWrittenEventArgs() {WriteRequest = writeRequest});
        }
        
        protected internal virtual void FollowerWriteValue(RaftLogEntryWriteRequest writeRequest)
        {
            ValidateFollowerWriteRequest(writeRequest);

            RaftLogEntry logEntry = writeRequest.FollowerCopy().LogEntry;
            LocalRepository.SaveAsync(logEntry);
            FireEvent(FollowerValueWritten, new RaftLogEntryWrittenEventArgs() {WriteRequest = writeRequest});
        }

        protected internal virtual void FollowerCommitValue(RaftLogEntryWriteRequest writeRequest)
        {
            ValidateFollowerWriteRequest(writeRequest);

            RaftLogEntry logEntry = writeRequest.FollowerCopy(RaftLogEntryState.Committed).LogEntry;
            LocalRepository.SaveAsync(logEntry);
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
        
        private static void ValidateFollowerWriteRequest(RaftLogEntryWriteRequest writeRequest)
        {
            Args.ThrowIfNull(writeRequest, "writeRequest");
            Args.ThrowIfNull(writeRequest.LogEntry, "writeRequest.LogEntry");
            Args.ThrowIf(writeRequest.TargetNodeState != RaftNodeState.Follower,
                "writeRequest.TargetNodeType != RaftNodeType.Leader");
        }
        
        private static void ValidateLeaderWriteRequest(RaftLogEntryWriteRequest writeRequest)
        {
            Args.ThrowIfNull(writeRequest, "writeRequest");
            Args.ThrowIfNull(writeRequest.LogEntry, "writeRequest.LogEntry");
            Args.ThrowIf(writeRequest.TargetNodeState != RaftNodeState.Leader,
                "writeRequest.TargetNodeType != RaftNodeType.Leader");
        }
        
        readonly object _commitSequenceLock = new object();
        RaftLogEntryCommit _lastCommit;
        private RaftLogEntryCommit SetNextCommitEntry(RaftLogEntryWriteRequest writeRequest)
        {
            lock (_commitSequenceLock)
            {
                if (_lastCommit == null)
                {
                    _lastCommit = LocalRepository.TopRaftLogEntryCommitsWhere(1, c => c.Seq > 0, Order.By<RaftLogEntryCommitColumns>(c=> c.Seq, SortOrder.Descending)).FirstOrDefault();
                    if (_lastCommit == null)
                    {
                        _lastCommit = LocalRepository.Save(new RaftLogEntryCommit()
                        {
                            RaftLogEntryId = writeRequest.LogEntry.CompositeKeyId,
                            Seq = 1
                        });
                    }
                }
                else
                {
                    _lastCommit = LocalRepository.Save(new RaftLogEntryCommit()
                    {
                        RaftLogEntryId = writeRequest.LogEntry.CompositeKeyId,
                        Seq = _lastCommit.Seq + 1
                    });
                }

                return _lastCommit;
            }
        }
    }
}