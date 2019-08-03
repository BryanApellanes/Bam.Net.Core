using System;
using System.IO;
using System.Reflection;
using Bam.Net.Server.Streaming;
using Bam.Net.Services.DataReplication.Consensus.Data;
using Bam.Net.Services.DataReplication.Consensus.Data.Dao.Repository;

namespace Bam.Net.Services.DataReplication.Consensus
{
    public class RaftClient
    {
        public RaftClient(string hostName, int port)
        {
            StreamingClient = new SecureStreamingClient<RaftRequest, RaftResponse>(hostName, port);
            HostName = hostName;
            Port = port;
        }

        public RaftClient(StreamingClient<RaftRequest, RaftResponse> streamingClient)
        {
            StreamingClient = streamingClient;
            HostName = StreamingClient.HostName;
            Port = StreamingClient.Port;
        }
        
        /// <summary>
        /// The host that this is a client of.
        /// </summary>
        public string HostName { get; set; }
        
        /// <summary>
        /// The port that the server host is listening on.
        /// </summary>
        public int Port { get; set; }
        
        public StreamingClient<RaftRequest, RaftResponse> StreamingClient { get; set; }

        public void SendFollowerWriteRequest(RaftLogEntryWriteRequest writeRequest)
        {
            Args.ThrowIfNull(writeRequest, "writeRequest");
            Args.ThrowIfNull(writeRequest.LogEntry, "writeRequest.LogEntry");
            Args.ThrowIf(writeRequest.LogEntry.State != RaftLogEntryState.Uncommitted, "RaftLogEntry already committed");
            Args.ThrowIf(writeRequest.TargetNodeState != RaftNodeState.Follower, "{0} called for RaftLogEntryWriteRequest not intended for follower.", nameof(SendFollowerWriteRequest));

            StreamingClient.SendRequest(CreateRaftRequest(writeRequest, RaftRequestType.WriteValue));
        }

        public void SendFollowerCommitRequest(RaftLogEntryWriteRequest writeRequest)
        {
            Args.ThrowIfNull(writeRequest, "writeRequest");
            Args.ThrowIfNull(writeRequest.LogEntry, "writeRequest.LogEntry");
            Args.ThrowIf(writeRequest.LogEntry.State != RaftLogEntryState.Committed, "RaftLogEntry not committed");
            Args.ThrowIf(writeRequest.TargetNodeState != RaftNodeState.Follower, "{0} called for RaftLogEntryWriteRequest not intended for follower.", nameof(SendFollowerCommitRequest));

            StreamingClient.SendRequest(CreateRaftRequest(writeRequest, RaftRequestType.NotifyFollowerLeaderValueCommitted));
        }

        public void ForwardWriteRequestToLeader(RaftLogEntryWriteRequest writeRequest)
        {
            Args.ThrowIfNull(writeRequest, "writeRequest");
            Args.ThrowIfNull(writeRequest.LogEntry, "writeRequest.LogEntry");
            Args.ThrowIf(writeRequest.LogEntry.State != RaftLogEntryState.Uncommitted, "RaftLogEntry already committed");
            Args.ThrowIf(writeRequest.TargetNodeState != RaftNodeState.Leader, "{0} called for RaftLogEntryWriteRequest not intended for leader.", nameof(ForwardWriteRequestToLeader));

            StreamingClient.SendRequest(CreateRaftRequest(writeRequest, RaftRequestType.WriteValue));
        }

        public void NotifyLeaderFollowerValueWritten(RaftLogEntryWriteRequest writeRequest)
        {
            Args.ThrowIfNull(writeRequest, "writeRequest");
            Args.ThrowIfNull(writeRequest.LogEntry, "writeRequest.LogEntry");

            StreamingResponse<RaftResponse> response =
                StreamingClient.SendRequest(CreateRaftRequest(writeRequest,
                    RaftRequestType.NotifyLeaderFollowerValueWritten));

            ResponseHandler?.Invoke(response);
        }

        public void SendLogSyncRequest(ulong sinceSequence)
        {
            StreamingResponse<RaftResponse> response = StreamingClient.SendRequest(CreateLogSyncRequest(sinceSequence));
            ResponseHandler?.Invoke(response);
        }

        public void SendLogSyncResponse(RaftReplicationLog log)
        {
            throw new NotImplementedException(); // use StreamingClient SendRequest(CreateLogSyncResponse(log));
            // follow existing server pattern
        }
        
        public RaftResponse SendJoinRaftRequest()
        {
            StreamingResponse<RaftResponse> response = StreamingClient.SendRequest(CreateJoinRaftRequest());
            ResponseHandler?.Invoke(response);
            return response.Body;
        }

        public void SendHeartbeat()
        {
            StreamingResponse<RaftResponse> response = StreamingClient.SendRequest(CreateHeartbeatRequest());
            ResponseHandler?.Invoke(response);
        }
        
        public void SendVoteResponse(int term, RaftVote vote)
        {
            StreamingResponse<RaftResponse> response = StreamingClient.SendRequest(CreateVoteResponse(term, vote));
            ResponseHandler?.Invoke(response);
        }
        
        public void SendVoteRequest(int term)
        {
            StreamingResponse<RaftResponse> response = StreamingClient.SendRequest(CreateVoteRequest(term));
            ResponseHandler?.Invoke(response);
        }

        protected RaftRequest CreateHeartbeatRequest()
        {
            return CreateRaftRequest(null, RaftRequestType.Heartbeat);
        }

        protected  RaftRequest CreateJoinRaftRequest()
        {
            return CreateRaftRequest(null, RaftRequestType.JoinRaft);
        }
        
        protected RaftRequest CreateVoteRequest(int term)
        {
            RaftRequest voteRequest = CreateRaftRequest(null, RaftRequestType.VoteRequest);
            voteRequest.ElectionTerm = term;
            return voteRequest;
        }

        protected RaftRequest CreateVoteResponse(int term, RaftVote vote)
        {
            RaftRequest voteResponse = CreateRaftRequest(null, RaftRequestType.VoteResponse);
            voteResponse.ElectionTerm = term;
            voteResponse.VoteResponse = vote;
            return voteResponse;
        }

        protected RaftRequest CreateLogSyncRequest(ulong sinceSeq)
        {
            RaftRequest request = CreateRaftRequest(null, RaftRequestType.LogSyncRequest);
            request.CommitSeq = sinceSeq;
            return request;
        }
        
        protected RaftRequest CreateRaftRequest(RaftLogEntryWriteRequest writeRequest, RaftRequestType requestType)
        {
            RaftNodeIdentifier current = RaftNodeIdentifier.ForCurrentProcess();
            return new RaftRequest()
            {
                RequesterHostName = current.HostName,
                RequesterPort = current.Port,
                RequestType = requestType,
                WriteRequest = writeRequest
            };
        }
        
        /// <summary>
        /// Optionally analyse responses.
        /// </summary>
        public Action<StreamingResponse<RaftResponse>> ResponseHandler { get; set; }
    }
}