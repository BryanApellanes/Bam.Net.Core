using System;
using Bam.Net.Server.Streaming;
using Bam.Net.Services.DataReplication.Consensus.Data;

namespace Bam.Net.Services.DataReplication.Consensus
{
    public class RaftServer : SecureStreamingServer<RaftRequest, RaftResponse>
    {
        public RaftServer(RaftRing ring, int port = RaftNodeIdentifier.DefaultPort)
        {
            Port = port;
            Ring = ring;
        }
        
        public RaftRing Ring { get; set; }
        
        public override RaftResponse ProcessDecryptedRequest(RaftRequest request)
        {
            RaftResponse response = new RaftResponse() {Request = request};
            try
            {
                Args.ThrowIfNull(request, "RaftRequest");
                
                switch (request.RequestType)
                {
                    case RaftRequestType.WriteValue:
                        HandleWriteRequest(request.WriteRequest);
                        break;
                    case RaftRequestType.NotifyLeaderFollowerValueWritten:
                        Ring.ReceiveFollowerWriteValueNotification(request.WriteRequest);                        
                        break;
                    case RaftRequestType.NotifyFollowerLeaderValueCommitted:
                        Ring.ReceiveLeaderCommittedValueNotification(request.WriteRequest);
                        break;
                    case RaftRequestType.VoteRequest:
                        Ring.ReceiveVoteRequest(request);
                        break;
                    case RaftRequestType.VoteResponse:
                        Ring.ReceiveVoteResponse(request);
                        break;
                    case RaftRequestType.Heartbeat:
                        Ring.ReceiveHeartbeat(request);
                        break;
                    case RaftRequestType.JoinRaft:
                        Ring.ReceiveJoinRaftRequest(request);
                        break;
                    case RaftRequestType.Invalid:
                        Logger.AddEntry("Invalid raft request received: {0}", request?.ToString() ?? "[null]");
                        throw new InvalidOperationException(
                            $"Invalid raft request received: {request?.ToString() ?? "[null]"}");
                        break;
                    default:
                        break;
                }
                response.Success = true;
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = ex.Message;
            }

            return response;
        }

        protected virtual void HandleWriteRequest(RaftLogEntryWriteRequest writeRequest)
        {
            if (writeRequest.TargetNodeState == RaftNodeState.Leader)
            {
                Ring.LeaderWriteValue(writeRequest);
            }

            if (writeRequest.TargetNodeState == RaftNodeState.Follower)
            {
                Ring.FollowerWriteValue(writeRequest);
            }
        }
    }
}