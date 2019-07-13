using System;
using System.Reflection;
using Bam.Net.Server.Streaming;
using Bam.Net.Services.DataReplication.Consensus.Data;

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
        
        public string HostName { get; set; }
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
        }

        protected RaftRequest CreateRaftRequest(RaftLogEntryWriteRequest writeRequest, RaftRequestType requestType)
        {
            return new RaftRequest()
            {
                RequesterNodeIdentifier = RaftNodeIdentifier.For(HostName, Port),
                RequestType = requestType,
                WriteRequest = writeRequest
            };
        }
    }
}