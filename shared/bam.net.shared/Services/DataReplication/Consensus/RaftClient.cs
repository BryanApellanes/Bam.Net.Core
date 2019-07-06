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

        public void WriteFollowerRequest(RaftLogEntryWriteRequest writeRequest)
        {
            Args.ThrowIfNull(writeRequest, "writeRequest");
            Args.ThrowIfNull(writeRequest.LogEntry, "writeRequest.LogEntry");
            Args.ThrowIf(writeRequest.LogEntry.State == RaftLogEntryState.Committed, "RaftLogEntry already committed");
            Args.ThrowIf(writeRequest.TargetNodeType == RaftNodeType.Leader, "{0} called for RaftLogEntryWriteRequest intended for leader.", MethodInfo.GetCurrentMethod().Name);
            
            StreamingClient.SendRequest(new RaftRequest() { RequestType = RaftRequestType.WriteValue, WriteRequest = writeRequest});
        }

        public void ForwardWriteRequestToLeader(RaftLogEntryWriteRequest writeRequest)
        {
            Args.ThrowIfNull(writeRequest, "writeRequest");
            Args.ThrowIfNull(writeRequest.LogEntry, "writeRequest.LogEntry");
            Args.ThrowIf(writeRequest.LogEntry.State == RaftLogEntryState.Committed, "RaftLogEntry already committed");
            Args.ThrowIf(writeRequest.TargetNodeType == RaftNodeType.Follower, "{0} called for RaftLogEntryWriteRequest intended for follower.");

            StreamingClient.SendRequest(new RaftRequest() {RequestType = RaftRequestType.WriteValue, WriteRequest = writeRequest});
        }

        public void NotifyLeaderValueWrittenAsFollower(RaftLogEntryWriteRequest writeRequest)
        {
            Args.ThrowIfNull(writeRequest, "writeRequest");
            Args.ThrowIfNull(writeRequest.LogEntry, "writeRequest.LogEntry");

            StreamingResponse<RaftResponse> response = StreamingClient.SendRequest(new RaftRequest()
            {
                SenderNodeIdentifier = RaftNodeIdentifier.IdFor(HostName, Port),
                RequestType = RaftRequestType.NotifyLeaderFollowerValueWritten, WriteRequest = writeRequest
            });
        }
    }
}