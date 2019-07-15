using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;
using Bam.Net.Data;
using Bam.Net.Data.Repositories;
using Bam.Net.Logging;
using Bam.Net.Presentation.Handlebars;
using Bam.Net.Server.Streaming;
using Bam.Net.Services.DataReplication.Consensus.Data.Dao;
using Bam.Net.Services.DataReplication.Consensus.Data.Dao.Repository;
using RaftFollowerWriteLog = Bam.Net.Services.DataReplication.Consensus.Data.RaftFollowerWriteLog;
using RaftLeaderElection = Bam.Net.Services.DataReplication.Consensus.Data.RaftLeaderElection;
using RaftNodeIdentifier = Bam.Net.Services.DataReplication.Consensus.Data.RaftNodeIdentifier;
using RaftVote = Bam.Net.Services.DataReplication.Consensus.Data.RaftVote;

namespace Bam.Net.Services.DataReplication.Consensus
{
    /// <summary>
    /// RaftRing provides local state tracking for a raft consensus implementation.  It also manages communication between RaftNodes.
    /// </summary>
    public class RaftRing : Ring<RaftNode>
    {
        public RaftRing()
        {
            Logger = Log.Default;
            ElectionTimeout = RandomNumber.Between(150, 300);
            Server = new RaftServer(this);
            StartHeartbeat();
        }
        
        public string HostName { get; set; }
        public int Port { get; set; }

        public ILogger Logger { get; }
        
        public RaftConsensusRepository RaftConsensusRepository { get; set; }

        public RaftNodeIdentifier AddNode(string hostName, int port)
        {
            RaftNode newNode = RaftNode.ForHost(this, hostName, port);
            newNode.FollowerValueWritten += OnFollowerValueWritten;
            newNode.LeaderValueWritten += OnLeaderValueWritten;
            newNode.LeaderValueCommitted += OnLeaderValueCommitted;
            
            AddArc(newNode);            
            return newNode.Identifier;
        }
        
        public int ElectionTimeout { get; set; }
        public int Heartbeats { get; set; }
        public RaftLeaderElection LatestElection { get; private set; }

        public Task FollowerHeartbeat { get; private set; }

        bool _stopHeartbeat;
        public void StartHeartbeat()
        {
            _stopHeartbeat = false;
            FollowerHeartbeat = Task.Run(FollowerHeartbeatLoop);
        }

        public void StopHeartbeat()
        {
            _stopHeartbeat = true;
        }

        protected void FollowerHeartbeatLoop()
        {
            try
            {
                while (!_stopHeartbeat)
                {
                    int currentBeats = Heartbeats;
                    Thread.Sleep(ElectionTimeout);
                    if (currentBeats == Heartbeats && !_stopHeartbeat) // didn't increment = didn't receive a leader heartbeat
                    {
                        if (LocalNode.NodeState == RaftNodeState.Follower)
                        {
                            // become candidate after election timeout expires then start election term
                            StartElection();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.AddEntry("Exception in {0}: {1}", ex, nameof(FollowerHeartbeatLoop), ex.Message);
            }
        }

        protected virtual void ReceiveHeartbeat()
        {
            ++Heartbeats;
        }
        
        protected virtual void StartElection()
        {
            LocalNode.NodeState = RaftNodeState.Candidate;
            
            CastVoteForSelf();
            BroadcastVoteRequest();
        }

        protected RaftLeaderElection GetLatestElection()
        {
            if (LatestElection == null)
            {
                LatestElection = RaftConsensusRepository.TopRaftLeaderElectionsWhere(1, e => e.Term > 0,
                    Order.By<RaftLeaderElectionColumns>(c => c.Term, SortOrder.Descending)).FirstOrDefault();

                if (LatestElection == null)
                {
                    LatestElection = new RaftLeaderElection {Term = 1};
                    LatestElection = RaftConsensusRepository.Save(LatestElection);
                }
            }

            return LatestElection;
        }

        protected RaftLeaderElection GetElectionForTerm(int term)
        {
            return RaftLeaderElection.ForTerm(term, RaftConsensusRepository);
        }
        
        protected virtual void CastVoteForSelf()
        {
            CastVoteFor(GetLatestElection().Term + 1, new RaftNodeIdentifier(HostName, Port));
        }
        
        protected virtual void CastVoteFor(int term, RaftNodeIdentifier voteFor)
        {
            RaftLeaderElection leaderElection = GetElectionForTerm(term);
            RaftVote.Cast(RaftConsensusRepository, term, voteFor);
        }

        
        public RaftServer Server { get; set; }

        RaftNode _raftNode;
        readonly object _raftNodeLock = new object();
        public RaftNode LocalNode
        {
            get
            {
                return _raftNodeLock.DoubleCheckLock(ref _raftNode,
                    () => RaftNode.FromIdentifier(this, RaftNodeIdentifier.ForCurrentProcess()));
            }
        }

        public override string GetHashString(object value)
        {
            return CompositeKeyHashProvider.GetStringKeyHash(value);
        }

        public DaoRepository LocalRepository
        {
            get { return LocalNode.LocalRepository; }
        }
        
        /// <summary>
        /// Write the specified key value pair by delegating to the local node.
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public void WriteValue(CompositeKeyAuditRepoData data)
        {
            foreach (RaftLogEntryWriteRequest writeRequest in RaftLogEntryWriteRequest.FromData(data).ToList())
            {
                WriteValue(writeRequest);
            }
        }

        /// <summary>
        /// Write the specified request by delegating to the local node.
        /// </summary>
        /// <param name="writeRequest"></param>
        /// <returns></returns>
        public void WriteValue(RaftLogEntryWriteRequest writeRequest)
        {
            LocalNode.WriteValue(writeRequest);
        }

        /// <summary>
        /// Write the specified value to the local repo if we are the leader.
        /// </summary>
        /// <param name="writeRequest"></param>
        public virtual void LeaderWriteValue(RaftLogEntryWriteRequest writeRequest)
        {
            LocalNode.LeaderWriteValue(writeRequest);
        }
        
        /// <summary>
        /// Write the specified value to the local repo if we are a follower.
        /// </summary>
        /// <param name="writeRequest"></param>
        public virtual void FollowerWriteValue(RaftLogEntryWriteRequest writeRequest)
        {
            LocalNode.FollowerWriteValue(writeRequest);
        }

        public virtual void ReceiveFollowerWriteValueNotification(RaftLogEntryWriteRequest writeRequest)
        {
            RaftFollowerWriteLog followerWriteLog = RaftFollowerWriteLog.For(HostName, Port, writeRequest);
            // track the follower write
            RaftFollowerWriteLog existing = LocalRepository.First<RaftFollowerWriteLog>(
                                                Filter.Where(nameof(RaftFollowerWriteLog.NodeIdentifier)) == followerWriteLog.NodeIdentifier &&
                                                Filter.Where(nameof(RaftFollowerWriteLog.LogEntryIdentifier)) == followerWriteLog.LogEntryIdentifier
                                            ) ?? LocalRepository.Save(followerWriteLog);

            List<RaftFollowerWriteLog> allFollowerWrites = LocalRepository.Query<RaftFollowerWriteLog>(
                Filter.Where(nameof(RaftFollowerWriteLog.LogEntryIdentifier)) == followerWriteLog.LogEntryIdentifier).ToList();
            // determine if majority of followers have written
            if (allFollowerWrites.Count >= GetMajority())
            {
                // LeaderCommit
                LocalNode.LeaderCommitValue(writeRequest);
            }
        }

        public virtual void ReceiveLeaderCommittedValueNotification(RaftLogEntryWriteRequest writeRequest)
        {
            LocalNode.FollowerCommitValue(writeRequest);
        }
        
        public virtual void BroadcastFollowerWriteRequest(RaftLogEntryWriteRequest writeRequest)
        {
            Parallel.ForEach(GetFollowers(),
                (follower) => follower.GetClient().SendFollowerWriteRequest(writeRequest.FollowerCopy()));
        }

        public virtual void BroadcastFollowerCommitRequest(RaftLogEntryWriteRequest writeRequest)
        {
            Parallel.ForEach(GetFollowers(),
                (follower) => follower.GetClient()
                    .SendFollowerCommitRequest(writeRequest.FollowerCopy(RaftLogEntryState.Committed)));
        }
        
        public virtual void ForwardWriteRequestToLeader(RaftLogEntryWriteRequest writeRequest)
        {
            // forward to the leaders ring using an appropriate client
            ValidateWriteRequest(writeRequest, RaftNodeState.Leader);

            RaftClient raftClient = GetLeaderNode().GetClient();
            Task.Run(() => raftClient.ForwardWriteRequestToLeader(writeRequest.LeaderCopy()));
        }

        public virtual void ReceiveVoteRequest(RaftRequest request)
        {
            RaftVote vote = RaftVote.Cast(RaftConsensusRepository, request.ElectionTerm, request);
            RaftClient voteResponseClient = request.GetResponseClient();
            voteResponseClient.SendVoteResponse(request.ElectionTerm, vote);
        }

        public virtual void ReceiveVoteResponse(RaftRequest request)
        {
            
        }
        
        protected virtual void BroadcastVoteRequest()
        {
            Parallel.ForEach(GetAllOtherNodes(), (other) => other.GetClient().SendVoteRequest(GetLatestElection().Term));
        }
       
        protected decimal GetMajority()
        {
            return (decimal) Math.Ceiling(ArcCount * .51);
        }

        protected void OnFollowerValueWritten(object sender, EventArgs e)
        {
            try
            {
                RaftLogEntryWrittenEventArgs args = (RaftLogEntryWrittenEventArgs) e;
                NotifyLeaderFollowerValueWritten(args.WriteRequest.LeaderCopy());
            }
            catch (Exception ex)
            {
                Logger.Warning("Exception handling follower value write event: {0}", ex.Message);
            }
        }

        protected void OnLeaderValueWritten(object sender, EventArgs e)
        {
            try
            {
                RaftLogEntryWrittenEventArgs args = (RaftLogEntryWrittenEventArgs) e;
                BroadcastFollowerWriteRequest(args.WriteRequest);
            }
            catch (Exception ex)
            {
                Logger.Warning("Exception handling leader value write event: {0}", ex.Message);
            }
        }

        protected void OnLeaderValueCommitted(object sender, EventArgs e)
        {
            try
            {
                RaftLogEntryWrittenEventArgs args = (RaftLogEntryWrittenEventArgs) e;
                BroadcastFollowerCommitRequest(args.WriteRequest);
            }
            catch (Exception ex)
            {
                Logger.Warning("Exception handling leader value committed event: {0}", ex.Message);
            }
        }
        
        protected void NotifyLeaderFollowerValueWritten(RaftLogEntryWriteRequest writeRequest)
        {
            RaftClient raftClient = GetLeaderNode().GetClient();
            raftClient.NotifyLeaderFollowerValueWritten(writeRequest.LeaderCopy());
        }
        
        protected internal RaftNode GetLeaderNode()
        {
            return FirstArcWhere(a => a.GetTypedServiceProvider().NodeState == RaftNodeState.Leader)
                .GetTypedServiceProvider();
        }

        protected internal List<RaftNode> GetFollowers()
        {
            return ArcsWhere(a => a.GetTypedServiceProvider().NodeState == RaftNodeState.Follower)
                .Select(a => a.GetTypedServiceProvider()).ToList();
        }

        protected internal List<RaftNode> GetAllOtherNodes()
        {
            return ArcsWhere(a => a.GetTypedServiceProvider() != LocalNode).Select(a => a.GetTypedServiceProvider())
                .ToList();
        }
        
        protected internal override Arc CreateArc()
        {
            return new Arc<RaftNode>();
        }

        public string GetHashString(object value, HashAlgorithms algorithm = HashAlgorithms.Invalid)
        {
            Args.ThrowIfNull(value);
            if (algorithm == HashAlgorithms.Invalid)
            {
                algorithm = HashAlgorithms.SHA256;
            }
            return CompositeKeyHashProvider.GetStringKeyHash(value, ",",
                CompositeKeyHashProvider.GetCompositeKeyProperties(value.GetType()), algorithm);
        }

        public override int GetObjectKey(object value)
        {
            return CompositeKeyHashProvider.GetStringKeyHash(value).ToSha256Int();
        }

        protected override Arc FindArcByObjectKey(int key)
        {
            double slotIndex = Math.Floor((double)(key / ArcSize));
            Arc result = null;
            if (slotIndex < Arcs.Length)
            {
                result = Arcs[(int)slotIndex];
            }

            return result;
        }
        
        private static void ValidateWriteRequest(RaftLogEntryWriteRequest writeRequest, RaftNodeState nodeState)
        {
            Args.ThrowIfNull(writeRequest);
            Args.ThrowIfNull(writeRequest.LogEntry);
            Args.ThrowIf(writeRequest.TargetNodeState != nodeState, "Write request not intended for {0}.", nodeState.ToString());
        }
    }
}