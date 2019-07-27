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
using Bam.Net.Server;
using Bam.Net.Server.Streaming;
using Bam.Net.Services.DataReplication.Consensus.Data.Dao;
using Bam.Net.Services.DataReplication.Consensus.Data.Dao.Repository;
using MongoDB.Driver.Core.WireProtocol.Messages.Encoders.JsonEncoders;
using UnityEngine.SocialPlatforms;
using RaftFollowerWriteLog = Bam.Net.Services.DataReplication.Consensus.Data.RaftFollowerWriteLog;
using RaftLeaderElection = Bam.Net.Services.DataReplication.Consensus.Data.RaftLeaderElection;
using RaftNodeIdentifier = Bam.Net.Services.DataReplication.Consensus.Data.RaftNodeIdentifier;
using RaftVote = Bam.Net.Services.DataReplication.Consensus.Data.RaftVote;

namespace Bam.Net.Services.DataReplication.Consensus
{
    /// <summary>
    /// RaftRing provides local state tracking for a raft consensus implementation by managing communication between a local RaftNode and other RaftNodes participating in the protocol.
    /// </summary>
    public class RaftRing : Ring<RaftNode>
    {
        public RaftRing(RaftConsensusRepository raftConsensusRepository, int port = RaftNodeIdentifier.DefaultPort, AppConf appConf = null)
        {
            RaftConsensusRepository = raftConsensusRepository;
            HeartbeatTimeout = 75;
            Logger = Log.Default;
            AppConf = appConf ?? AppConf.FromConfig();
            EventSource = new RaftEventSource(raftConsensusRepository, AppConf, Logger);
            Server = new RaftServer(this, port);
        }

        public static RaftRing FromConfig(RaftConfig config)
        {
            RaftRing result = new RaftRing(new RaftConsensusRepository());
            HashSet<RaftNodeIdentifier> nodeIdentifiers = new HashSet<RaftNodeIdentifier>();
            foreach (RaftNodeInfo nodeInfo in config.ServerNodes)
            {
                if (!string.IsNullOrEmpty(nodeInfo.HostName))
                {
                    nodeIdentifiers.Add(new RaftNodeIdentifier(nodeInfo.HostName, nodeInfo.Port));
                }
            }

            foreach (RaftNodeIdentifier identifier in nodeIdentifiers)
            {
                result.AddNode(identifier);
            }

            return result;
        }
        
        public AppConf AppConf { get; private set; }
        public RaftEventSource EventSource { get; private set; }

        public string HostName => LocalNode?.Identifier?.HostName;
        public int Port => (LocalNode?.Identifier?.Port).Value;

        public ILogger Logger { get; }
        
        public RaftConsensusRepository RaftConsensusRepository { get; set; }

        public RaftNodeIdentifier AddNode(RaftNodeIdentifier identifier)
        {
            return AddNode(identifier.HostName, identifier.Port);
        }
        
        public RaftNodeIdentifier AddNode(string hostName, int port)
        {
            RaftNode newNode = RaftNode.ForHost(this, hostName, port);
            if (!LocalNode.Identifier.HostName.Equals(hostName) ||
                LocalNode.Identifier.Port != port)
            {
                AddArc(newNode);
                JoinRaft(newNode);
            }
            
            return RaftConsensusRepository.GetByCompositeKey<RaftNodeIdentifier>(newNode.Identifier);
        }

        public static RaftRing StartFromConfig(RaftConfig config)
        {
            RaftRing fromConfig = RaftRing.FromConfig(config);
            fromConfig.StartRaftProtocol();
            return fromConfig;
        }
        
        public void StartRaftProtocol()
        {
            Server.Start();
            ResetElectionTimeout();
        }

        public RaftResponse JoinRaft(RaftNode raftNode)
        {
            Args.ThrowIfNull(raftNode, "raftNode");
            return JoinRaft(raftNode.Identifier.HostName, raftNode.Identifier.Port);
        }

        public RaftResponse JoinRaft(string hostName, int port)
        {
            Args.ThrowIfNullOrEmpty(hostName);
            Args.ThrowIf(port <= 0, "port must be a valid network port"); 
            RaftClient client = new RaftClient(hostName, port);
            return client.SendJoinRaftRequest();
        }
        
        public event Action<RaftRequest> RequestReceived;
        
        public int ElectionTimeout { get; set; }
        public int HeartbeatTimeout { get; set; }
        public int Heartbeats { get; set; }
        
        /// <summary>
        /// Gets the LatestElection.
        /// </summary>
        public RaftLeaderElection LatestElection { get; private set; }

        public Task LeaderHeartBeat { get; private set; }
        public Task FollowerHeartbeatCheck { get; private set; }
        
        bool _stopLeaderHeartbeat;
        public void StartLeaderHeartbeat()
        {
            _stopLeaderHeartbeat = false;
            LeaderHeartBeat = Task.Run(LeaderHeartbeatLoop);
        }

        public void StopLeaderHeartbeat()
        {
            _stopLeaderHeartbeat = true;
        }

        public void RestartLeaderHeartbeat()
        {
            StopLeaderHeartbeat();
            StartLeaderHeartbeat();
        }

        protected void ReceiveRequestAsync(RaftRequest request)
        {
            Task.Run(() => RequestReceived?.Invoke(request));
        }
        
        protected void LeaderHeartbeatLoop()
        {
            try
            {
                while (!_stopLeaderHeartbeat && LocalNode.NodeState == RaftNodeState.Leader)
                {
                    Parallel.ForEach(GetAllOtherNodes(), node => node.GetClient().SendHeartbeat());
                    Thread.Sleep(HeartbeatTimeout);
                }
            }
            catch (Exception ex)
            {
                Logger.AddEntry("Exception in {0}: {1}", ex, nameof(LeaderHeartbeatLoop), ex.Message);
                RestartLeaderHeartbeat();
            }
        }
        
        bool _stopFollowerHeartbeatCheck;
        public void StartFollowerHeartbeatCheck()
        {
            _stopFollowerHeartbeatCheck = false;
            FollowerHeartbeatCheck = Task.Run(FollowerHeartbeatCheckLoop);
        }

        public void RestartFollowerHeartbeatCheck()
        {
            StopFollowerHeartbeatCheck();
            Thread.Sleep(ElectionTimeout);
            StartFollowerHeartbeatCheck();
        }
        
        public void StopFollowerHeartbeatCheck()
        {
            _stopFollowerHeartbeatCheck = true;
        }

        protected void FollowerHeartbeatCheckLoop()
        {
            try
            {
                while (!_stopFollowerHeartbeatCheck)
                {
                    int currentBeats = Heartbeats;
                    Thread.Sleep(ElectionTimeout);
                    if (currentBeats == Heartbeats && !_stopFollowerHeartbeatCheck) // didn't increment = didn't receive a leader heartbeat
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
                Logger.AddEntry("Exception in {0}: {1}", ex, nameof(FollowerHeartbeatCheckLoop), ex.Message);
                RestartFollowerHeartbeatCheck();
            }
        }

        protected virtual void ResetElectionTimeout()
        {
            AddHeartbeat();
            StopFollowerHeartbeatCheck();
            ElectionTimeout = RandomNumber.Between(150, 300);
            RestartFollowerHeartbeatCheck();
        }

        protected virtual void AddHeartbeat()
        {
            ++Heartbeats;
        }
        
        protected virtual void StartElection()
        {
            LocalNode.NodeState = RaftNodeState.Candidate;
            
            RaftLeaderElection election = CastVoteForSelf();
            BroadcastVoteRequest(election.Term);
        }

        object _latestElectionLock = new object();
        /// <summary>
        /// Thread safe way of accessing the latest election.
        /// </summary>
        /// <returns></returns>
        protected RaftLeaderElection GetLatestElection()
        {
            lock (_latestElectionLock)
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
            }

            return LatestElection;
        }

        protected RaftLeaderElection GetElectionForTerm(int term)
        {
            return RaftLeaderElection.ForTerm(term, RaftConsensusRepository);
        }
        
        protected virtual RaftLeaderElection CastVoteForSelf()
        {
            return CastVoteFor(GetLatestElection().Term + 1, LocalNode.Identifier);
        }
        
        protected virtual RaftLeaderElection CastVoteFor(int term, RaftNodeIdentifier voteFor)
        {
            RaftLeaderElection leaderElection = GetElectionForTerm(term);
            RaftVote.Cast(RaftConsensusRepository, leaderElection, LocalNode.Identifier, voteFor);
            ResetElectionTimeout();
            return leaderElection;
        }

        
        public RaftServer Server { get; set; }

        RaftNode _raftNode;
        readonly object _raftNodeLock = new object();
        public RaftNode LocalNode
        {
            get
            {
                return _raftNodeLock.DoubleCheckLock(ref _raftNode,
                    () =>
                    {
                        RaftNode local = RaftNode.FromIdentifier(this, RaftNodeIdentifier.ForCurrentProcess());
                        local.FollowerValueWritten += OnFollowerValueWritten;
                        local.LeaderValueWritten += OnLeaderValueWritten;
                        local.LeaderValueCommitted += OnLeaderValueCommitted;
                        return local;
                    });
            }
        }

        public override string GetHashString(object value)
        {
            return CompositeKeyHashProvider.GetStringKeyHash(value);
        }

        public RaftConsensusRepository LocalRepository
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

        public virtual void ReceiveJoinRaftRequest(RaftRequest request)
        {
            ReceiveRequestAsync(request);
            AddNode(request.RequesterHostName, request.RequesterPort);
        }
        
        public virtual void ReceiveHeartbeat(RaftRequest request)
        {
            ReceiveRequestAsync(request);
            ResetElectionTimeout();
        }
        
        public virtual void ReceiveVoteRequest(RaftRequest request)
        {
            ReceiveRequestAsync(request);
            RaftVote vote = RaftVote.Cast(RaftConsensusRepository, request.ElectionTerm, request);
            RaftClient voteResponseClient = request.GetResponseClient();
            voteResponseClient.SendVoteResponse(request.ElectionTerm, vote);
        }

        public virtual void ReceiveVoteResponse(RaftRequest request)
        {
            ReceiveRequestAsync(request);
            RaftLeaderElection LatestElection = GetLatestElection();
            RaftLeaderElection electionForRequestedTerm = GetElectionForTerm(request.ElectionTerm);
            if (LatestElection.Term == electionForRequestedTerm.Term)
            {
                // get the local vote for the response if it exists
                RaftVote vote = RaftVote.ForElection(LocalRepository, electionForRequestedTerm);
                // save it if it doesn't
                if (vote == null)
                {
                    LocalRepository.Save(request.VoteResponse);
                }
                
                if (LocalNodeWonLatestElection())
                {
                    BecomeLeader();
                }
                else
                {
                    BecomeFollower();
                }
            }
        }

        protected void BecomeLeader()
        {
            if (LocalNode.NodeState != RaftNodeState.Leader)
            {
                LocalNode.NodeState = RaftNodeState.Leader;
                StopFollowerHeartbeatCheck();
                RestartLeaderHeartbeat();
            }
        }

        protected void BecomeFollower()
        {
            if (LocalNode.NodeState != RaftNodeState.Follower)
            {
                LocalNode.NodeState = RaftNodeState.Follower;
                StopLeaderHeartbeat();
                RestartFollowerHeartbeatCheck();
            }
        }
        
        protected bool LocalNodeWonLatestElection()
        {
            RaftLeaderElection election = GetLatestElection();
            decimal votesReceived = (decimal)LocalRepository.CountRaftVotesWhere(v =>
                v.ElectionKey == election.CompositeKey && v.ForNodeIdentifier == LocalNode.Identifier.CompositeKey);

            return votesReceived >= GetMajority();
        }
        
        protected virtual void BroadcastVoteRequest(int term)
        {
            Parallel.ForEach(GetAllOtherNodes(), (other) => other.GetClient().SendVoteRequest(term));
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