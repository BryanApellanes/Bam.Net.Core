using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
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
using Bam.Net.Services.DataReplication.Data;
using CsQuery.ExtensionMethods;
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
    public class RaftRing : Ring<RaftNode>, IDistributedRepository
    {
        public RaftRing(IRepository dataReplicationRepository, RaftConsensusRepository raftConsensusRepository, IRaftReplicationLogSyncManager replicationLogSyncManager = null, TypeMap typeMap = null, IRaftLogEntryPropertyHandler raftLogEntryPropertyHandler = null, int port = RaftNodeIdentifier.DefaultPort, AppConf appConf = null, ILogger logger = null)
        {
            DataReplicationRepository = dataReplicationRepository;
            RaftConsensusRepository = raftConsensusRepository;
            HeartbeatTimeout = 75;
            Logger = logger ?? Log.Default;
            TypeMap = typeMap ?? new TypeMap();
            AppConf = appConf ?? AppConf.FromConfig();
            EventSource = new RaftEventSource(raftConsensusRepository, AppConf, Logger);
            ProtocolServer = new RaftProtocolServer(this, port);
            ReplicationLogSyncManager = replicationLogSyncManager ?? new RaftReplicationLogSyncManager(this);
            RaftLogEntryPropertyHandler = raftLogEntryPropertyHandler ?? new RaftLogEntryPropertyHandler()
            {
                TypeMap = TypeMap,
                Logger = Logger
            };
        }
        
        protected internal IRaftLogEntryPropertyHandler RaftLogEntryPropertyHandler { get; set; }
        protected internal TypeMap TypeMap { get; private set; }
        protected internal ITypeResolver TypeResolver { get; private set; }
        protected internal IRaftReplicationLogSyncManager ReplicationLogSyncManager { get; set; }
        public RaftProtocolServer ProtocolServer { get; set; }

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
        public AppConf AppConf { get; private set; }
        public RaftEventSource EventSource { get; private set; }

        public string HostName => LocalNode?.Identifier?.HostName;
        public int Port => (LocalNode?.Identifier?.Port).Value;

        public ILogger Logger { get; }
        
        public RaftConsensusRepository RaftConsensusRepository { get; set; }
        
        public IRepository DataReplicationRepository { get; set; }
        
        /// <summary>
        /// The event that fires when a RaftRequest is received.  Handlers of this event
        /// are executed asynchronously; this event is intended for diagnostics and debugging.
        /// </summary>
        public event Action<RaftRequest> RequestReceived;
        
        public int ElectionTimeout { get; set; }
        public int HeartbeatTimeout { get; set; }
        public int Heartbeats { get; set; }
        
        /// <summary>
        /// Gets the LatestElection, may be null if GetLatestElection has not been called.
        /// </summary>
        public RaftLeaderElection LatestElection { get; private set; }

        public Task LeaderHeartBeat { get; private set; }
        public Task FollowerHeartbeatCheck { get; private set; }
        
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
            }
            
            return RaftConsensusRepository.GetByCompositeKey<RaftNodeIdentifier>(newNode.Identifier);
        }
        
        public static RaftRing FromConfig(RaftConfig config)
        {
            RaftRing result = new RaftRing(null, new RaftConsensusRepository());
            HashSet<RaftNodeIdentifier> nodeIdentifiers = new HashSet<RaftNodeIdentifier>();
            if (config.IncludeLocalNode)
            {
                nodeIdentifiers.Add(RaftNodeIdentifier.ForCurrentProcess());
            }
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

        public void ForEachRaftNode(Action<RaftNode> action)
        {
            ForEachArcService(action);
        }

        public static RaftRing StartFromConfig(RaftConfig config)
        {
            RaftRing fromConfig = RaftRing.FromConfig(config);
            fromConfig.StartRaftProtocol();
            return fromConfig;
        }
        
        public void StartRaftProtocol()
        {
            ProtocolServer.Start();
            ResetElectionTimeout();
        }

        /// <summary>
        /// Join the raft protocol where the specified node is already a member.
        /// </summary>
        /// <param name="raftNode"></param>
        /// <returns></returns>
        public RaftResponse JoinRaft(RaftNode raftNode)
        {
            Args.ThrowIfNull(raftNode, "raftNode");
            return JoinRaft(raftNode.Identifier.HostName, raftNode.Identifier.Port);
        }

        /// <summary>
        /// Join the raft protocol where the specified node is already a member.
        /// </summary>
        /// <param name="hostName"></param>
        /// <param name="port"></param>
        /// <returns></returns>
        public RaftResponse JoinRaft(string hostName, int port)
        {
            Args.ThrowIfNullOrEmpty(hostName);
            Args.ThrowIf(port <= 0, "port must be a valid network port"); 
            RaftProtocolClient protocolClient = new RaftProtocolClient(hostName, port);
            return protocolClient.SendJoinRaftRequest();
        }
        
        bool _stopLeaderHeartbeat;
        public void StartLeaderHeartbeat()
        {
            _stopLeaderHeartbeat = false;
            LeaderHeartBeat = Task.Run((Action)LeaderHeartbeatLoop);
        }

        public void StopLeaderHeartbeat()
        {
            _stopLeaderHeartbeat = true;
        }

        public void RestartLeaderHeartbeat()
        {
            StopLeaderHeartbeat();
            try
            {
                LeaderHeartBeat.Dispose();
            }
            catch
            {
                // swallow
            }

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
            FollowerHeartbeatCheck = Task.Run((Action)FollowerHeartbeatCheckLoop);
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

        readonly object _latestElectionLock = new object();
        /// <summary>
        /// Thread safe way of accessing the latest election.
        /// </summary>
        /// <returns></returns>
        protected RaftLeaderElection GetLatestElection(bool reload = false)
        {
            lock (_latestElectionLock)
            {
                if (LatestElection == null || reload)
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

        public override string GetHashString(object value)
        {
            return CompositeKeyHashProvider.GetStringKeyHash(value);
        }

        public RaftConsensusRepository LocalRepository => LocalNode.LocalRepository;
        
        /// <summary>
        /// Write the specified data by delegating to the local node.  This request is distributed to the other nodes in the raft.
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public void WriteValue(CompositeKeyAuditRepoData data)
        {
            foreach (RaftLogEntryWriteRequest writeRequest in RaftLogEntryWriteRequest.FromData(data))
            {
                WriteValue(writeRequest);
            }
        }

        /// <summary>
        /// Write the specified request asynchronously by delegating to the local node.  This request is distributed to the other nodes in the raft.
        /// </summary>
        /// <param name="writeRequest"></param>
        /// <returns></returns>
        public void WriteValue(RaftLogEntryWriteRequest writeRequest)
        {
            Task.Run(() =>
            {
                try
                {
                    LocalNode.WriteValue(writeRequest);
                }
                catch (Exception ex)
                {
                    Logger.AddEntry("Error writing value: {0}", ex, ex.Message);
                }
            });
        }

        /// <summary>
        /// Receive the specified request and write as leader or follower as appropriate for the current LocalNode.
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public virtual RaftResult ReceiveWriteRequest(RaftRequest request)
        {
            RaftResult result = new RaftResult(request);
            try
            {
                Args.ThrowIfNull(request, "request");
                Args.ThrowIfNull(request.WriteRequest, "request.WriteRequest");
                
                if (request.WriteRequest.TargetNodeState == RaftNodeState.Leader)
                {
                    LeaderWriteValue(request);
                }
                else if (request.WriteRequest.TargetNodeState == RaftNodeState.Follower)
                {
                    FollowerWriteValue(request.WriteRequest);
                }
            }
            catch (Exception ex)
            {
                HandleException(request, ex, result);
            }

            return result;
        }

        /// <summary>
        /// Write the specified value to the local repo if we are the leader for the term specified in the request, otherwise become a
        /// follower and write the value as a follower.
        /// </summary>
        /// <param name="request"></param>
        protected virtual void LeaderWriteValue(RaftRequest request)
        {
            LatestElection = GetLatestElection(true);
            if (request.ElectionTerm > LatestElection.Term)
            {
                BecomeFollower();
                LocalNode.FollowerWriteValue(request.WriteRequest);
            }
            else
            {
                LocalNode.LeaderWriteValue(request.WriteRequest);
            }
        }
        
        /// <summary>
        /// Write the specified value to the local repo if we are a follower.
        /// </summary>
        /// <param name="writeRequest"></param>
        protected virtual void FollowerWriteValue(RaftLogEntryWriteRequest writeRequest)
        {
            LocalNode.FollowerWriteValue(writeRequest);
        }

        public virtual RaftResult ReceiveFollowerWriteValueNotification(RaftRequest request)
        {
            RaftResult result = new RaftResult(request);
            try
            {
                Args.ThrowIfNull(request, "request");
                ReceiveFollowerWriteValueNotification(request.WriteRequest);
            }
            catch (Exception ex)
            {
                HandleException(request, ex, result);
            }

            return result;
        }

        protected virtual void ReceiveFollowerWriteValueNotification(RaftLogEntryWriteRequest writeRequest)
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
            if (allFollowerWrites.Count >= GetMajority() && LocalNode.NodeState == RaftNodeState.Leader)
            {
                // LeaderCommit
                LocalNode.LeaderCommitValue(writeRequest);
            }
        }

        public virtual RaftResult ReceiveLeaderCommittedValueNotification(RaftRequest request)
        {
            Args.ThrowIfNull(request, "request");
            RaftResult result = new RaftResult(request);
            try
            {
                ReceiveLeaderCommittedValueNotification(request.WriteRequest);
            }
            catch (Exception ex)
            {
                HandleException(request, ex, result);
            }

            return result;
        }

        protected virtual void ReceiveLeaderCommittedValueNotification(RaftLogEntryWriteRequest writeRequest)
        {
            LocalNode.FollowerCommitValue(writeRequest);
        }
        
        /// <summary>
        /// Notify followers that the current node as leader has written a value (uncommitted).
        /// </summary>
        /// <param name="writeRequest"></param>
        public virtual void BroadcastFollowerWriteRequest(RaftLogEntryWriteRequest writeRequest)
        {
            Parallel.ForEach(GetFollowers(),
                (follower) => follower.GetClient().SendFollowerWriteRequest(writeRequest.FollowerCopy()));
        }

        public virtual void BroadcastLeaderWriteRequest(RaftLogEntryWriteRequest writeRequest)
        {
            Parallel.ForEach(GetAllOtherNodes(),
                (node) => node.GetClient().ForwardWriteRequestToLeader(writeRequest.LeaderCopy()));
        }

        public virtual void BroadcastNotifyLeaderFollowerValueWritten(RaftLogEntryWriteRequest writeRequest)
        {
            Parallel.ForEach(GetAllOtherNodes(),
                (node) => node.GetClient().NotifyLeaderFollowerValueWritten(writeRequest));
        }

        public virtual void BroadcastLogSyncRequest(ulong sinceSequence)
        {
            Parallel.ForEach(GetAllOtherNodes(), node => node.GetClient().SendLogSyncRequest(sinceSequence));
        }
        
        public virtual void BroadcastFollowerCommitRequest(RaftLogEntryWriteRequest writeRequest)
        {
            Parallel.ForEach(GetFollowers(),
                (follower) => follower.GetClient()
                    .SendFollowerCommitRequest(writeRequest.FollowerCopy(RaftLogEntryState.Committed)));
        }
        
        public virtual void ForwardWriteRequestToLeader(RaftLogEntryWriteRequest writeRequest)
        {
            ValidateWriteRequest(writeRequest, RaftNodeState.Leader);

            RaftNode leaderNode = GetLeaderNode();
            if (leaderNode != null)
            {
                RaftProtocolClient raftProtocolClient = leaderNode.GetClient();
                Task.Run(() => raftProtocolClient.ForwardWriteRequestToLeader(writeRequest.LeaderCopy()));
            }
            else
            {
                BroadcastLeaderWriteRequest(writeRequest);
            }
        }

        public virtual RaftResult ReceiveLogSyncRequest(RaftRequest request)
        {
            Args.ThrowIfNull(request, "request");
            RaftResult result = new RaftResult(request);
            try
            {
                ReceiveRequestAsync(request);
                Task.Run(() => LocalNode.SendLogSyncResponse(request));
            }
            catch (Exception ex)
            {
                HandleException(request, ex, result);
            }

            return result;
        }

        public virtual RaftResult ReceiveLogSyncResponse(RaftRequest request)
        {
            Args.ThrowIfNull(request, "request");
            RaftResult result = new RaftResult(request);
            try
            {
                ReceiveRequestAsync(request);
                Task.Run(() => ReplicationLogSyncManager.HandleReplicationLog(request.LogSyncResponse));
            }
            catch (Exception ex)
            {
                HandleException(request, ex, result);
            }

            return result;
        }

        /// <summary>
        /// Records the requester as a node in the current raft ring.
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public virtual RaftResult ReceiveJoinRaftRequest(RaftRequest request)
        {
            Args.ThrowIfNull(request, "request");
            RaftResult result = new RaftResult(request);
            try
            {
                ReceiveRequestAsync(request);
                AddNode(request.OriginHostName, request.OriginPort);
            }
            catch (Exception ex)
            {
                HandleException(request, ex, result);
            }

            return result;
        }
        
        public virtual RaftResult ReceiveHeartbeat(RaftRequest request)
        {
            Args.ThrowIfNull(request, "request");
            RaftResult result = new RaftResult(request);
            try
            {
                ReceiveRequestAsync(request);
                SetLeaderAsync(request);
                ResetElectionTimeout();
            }
            catch (Exception ex)
            {
                HandleException(request, ex, result);
            }

            return result;
        }

        protected Task SetLeaderAsync(RaftRequest request)
        {
            return Task.Run(() =>
            {   
                Arc<RaftNode> leader = ArcsWhere(arc =>
                {
                    RaftNode node = arc.GetTypedServiceProvider();
                    return node.Identifier.CompositeKey.Equals(request.RequesterIdentifier());
                }).FirstOrDefault();

                if (leader == null)
                {
                    Logger.Warning("Unable to locate leader Arc for specified request: {0}:{1}/{2}", request.OriginHostName, request.OriginPort, request.RequesterIdentifier().CompositeKey);
                }
                else
                {
                    ForEachRaftNode(node =>
                    {
                        node.NodeState = node.Identifier.CompositeKey.Equals(request.RequesterIdentifier().CompositeKey) ? RaftNodeState.Leader : RaftNodeState.Follower;
                    });                    
                }
            });
        }
        
        public virtual RaftResult ReceiveVoteRequest(RaftRequest request)
        {
            Args.ThrowIfNull(request, "request");
            RaftResult result = new RaftResult(request);
            try
            {
                Task.Run(() =>
                {
                    ReceiveRequestAsync(request);
                    RaftVote vote = RaftVote.Cast(RaftConsensusRepository, request.ElectionTerm, request);
                    RaftProtocolClient voteResponseProtocolClient = request.GetResponseClient();
                    voteResponseProtocolClient.SendVoteResponse(request.ElectionTerm, vote);
                });
            }
            catch (Exception ex)
            {
                HandleException(request, ex, result);
            }

            return result;
        }

        public virtual RaftResult ReceiveVoteResponse(RaftRequest request)
        {
            Args.ThrowIfNull(request, "request");
            Args.ThrowIfNull(request.VoteResponse, "request.VoteResponse");
            RaftResult result = new RaftResult(request);
            try
            {
                ReceiveRequestAsync(request);
                RaftLeaderElection LatestElection = GetLatestElection();
                RaftLeaderElection electionForRequestedTerm = GetElectionForTerm(request.ElectionTerm);
                if (LatestElection.Term == electionForRequestedTerm.Term)
                {
                    // get the local vote for the response if it exists
                    RaftVote vote = LocalRepository.LoadByCompositeKey<RaftVote>(request.VoteResponse.CompositeKeyId);
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
            catch (Exception ex)
            {
                HandleException(request, ex, result);
            }

            return result;
        }

        protected void BecomeLeader()
        {
            if (LocalNode.NodeState != RaftNodeState.Leader)
            {
                LocalNode.NodeState = RaftNodeState.Leader;
                ForEachRaftNode(node =>
                {
                    node.NodeState = node.Equals(LocalNode) ? RaftNodeState.Leader : RaftNodeState.Follower;
                });
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
                SendLogSyncRequest();
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
                BroadcastFollowerWriteRequest(args.WriteRequest.FollowerCopy(RaftLogEntryState.Uncommitted, LocalNode.Identifier.CompositeKey));
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

        protected void SendLogSyncRequest()
        {
            ulong sinceSequence = GetLatestCommitSequence();
            RaftProtocolClient raftProtocolClient = GetLeaderNode()?.GetClient();
            if (raftProtocolClient != null)
            {
                raftProtocolClient.SendLogSyncRequest(sinceSequence);
            }
            else
            {
                BroadcastLogSyncRequest(sinceSequence);
            }
        }

        protected void NotifyLeaderFollowerValueWritten(RaftLogEntryWriteRequest writeRequest)
        {
            RaftProtocolClient raftProtocolClient = GetLeaderNode()?.GetClient();
            if (raftProtocolClient != null)
            {
                raftProtocolClient.NotifyLeaderFollowerValueWritten(writeRequest.LeaderCopy());
            }
            else
            {
                BroadcastNotifyLeaderFollowerValueWritten(writeRequest.LeaderCopy());
            }
        }
        
        protected internal RaftNode GetLeaderNode()
        {
            return FirstArcWhere(a => a.GetTypedServiceProvider().NodeState == RaftNodeState.Leader)
                ?.GetTypedServiceProvider();
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
        
        private void HandleException(RaftRequest request, Exception ex, RaftResult result)
        {
            Logger.Error("Error handling write request: \r\n{0}\r\n{1}", ex.Message, request?.ToJson());
            result.Message = ex.Message;
            result.Status = RaftResultStatus.Error;
        }
        
        private ulong GetLatestCommitSequence()
        {
            Bam.Net.Services.DataReplication.Consensus.Data.RaftLogEntryCommit commit = LocalRepository.TopRaftLogEntryCommitsWhere(1, c => c.Seq > -1,
                Order.By<RaftLogEntryCommitColumns>(c => c.Seq, SortOrder.Descending)).FirstOrDefault();

            return commit.Seq;
        }
        
        public void WriteValue(CreateOperation createOperation)
        {
            foreach (RaftLogEntryWriteRequest writeRequest in RaftLogEntryWriteRequest.FromCreateOperation(createOperation))
            {
                WriteValue(writeRequest);
            }
        }
        
        public void WriteValue(SaveOperation saveOperation)
        {
            foreach (RaftLogEntryWriteRequest writeRequest in RaftLogEntryWriteRequest.FromSaveOperation(saveOperation))
            {
                WriteValue(writeRequest);
            }
        }
        
        // delegate write operations to RaftRing.WriteValue
        // delegate read operations to the LocalNode, if no values are found delegate to leader if leader is known otherwise broadcast read request
        public object Save(SaveOperation saveOperation)
        {
            Expect.AreEqual(OperationIntent.Save, saveOperation.Intent);
            
            WriteValue(saveOperation);

            return DataPoint.FromSaveOperation(saveOperation);
        }

        public object Create(CreateOperation createOperation)
        {
            Expect.AreEqual(OperationIntent.Create, createOperation.Intent);
            
            WriteValue(createOperation);

            return DataPoint.FromCreateOperation(createOperation);
        }

        public object Update(UpdateOperation updateOperation)
        {
            // transform the operation into an appropriate RaftWriteRequest
            throw new NotImplementedException();
        }

        public bool Delete(DeleteOperation deleteOperation)
        {
            return LocalNode.Delete(deleteOperation);
        }

        public IEnumerable<object> QueryLocal(QueryOperation queryOperation)
        {
            return LocalNode.Query(queryOperation);
        }
        
        public object RetrieveLocal(RetrieveOperation retrieveOperation)
        {
            return LocalNode.Retrieve(retrieveOperation);
        }
        
        public object Retrieve(RetrieveOperation retrieveOperation)
        {
            object retrieved = RetrieveLocal(retrieveOperation);
            if (retrieved == null)
            {
                // request from leader 
                RaftProtocolClient leaderClient = GetLeaderNode()?.GetClient();
                if (leaderClient != null)
                {
                    retrieved = leaderClient.SendRetrieveRequest(retrieveOperation);
                }
                else
                {
                    // if leader is not known broadcast request
                    retrieved = BroadcastRetrieveRequest(retrieveOperation);
                }
            }

            return retrieved;
        }

        public void ForEachQueryResult(QueryOperation query, Action<object> action)
        {
            Task.Run(() => Parallel.ForEach(LocalQuery(query), action));
            Task.Run(() => Parallel.ForEach(BroadcastQuery(query), action));
        }
        
        public IEnumerable<object> Query(QueryOperation queryOperation)
        {
            foreach (object localObject in LocalQuery(queryOperation))
            {
                yield return localObject;
            }

            foreach (object broadcastResponse in BroadcastQuery(queryOperation))
            {
                yield return broadcastResponse;
            }
        }
        
        private IEnumerable<object> LocalQuery(QueryOperation query)
        {
            return LocalNode.Query(query);
        }

        private object BroadcastRetrieveRequest(RetrieveOperation retrieveOperation)
        {
            object firstResponse = null;
            if (Exec.TakesTooLong(() =>
            {
                AutoResetEvent resetEvent = new AutoResetEvent(false);
                foreach (RaftNode raftNode in GetAllOtherNodes())
                {
                    Task.Run(() =>
                    {
                        firstResponse = raftNode.GetClient()?.SendRetrieveRequest(retrieveOperation);
                        resetEvent.Set();
                    });
                }

                resetEvent.WaitOne();
            }, 3000))
            {
                Logger.Warning("BroadcastRetrieveRequest timed out for RetrieveOperation ({0})", retrieveOperation.Cuid);
            }

            return firstResponse;
        }
        
        private IEnumerable<object> BroadcastQuery(QueryOperation query)
        {
            throw new NotImplementedException();
        }


        public ReplicationOperation Replicate(ReplicationOperation operation)
        {
            return LocalNode.Replicate(operation);
        }

        public IEnumerable<object> NextSet(ReplicationOperation operation)
        {
            return LocalNode.NextSet(operation);
        }
    }
}