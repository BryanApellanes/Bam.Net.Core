using System;
using Bam.Net.Server.Streaming;
using Bam.Net.Services.DataReplication.Consensus.Data;

namespace Bam.Net.Services.DataReplication.Consensus
{
    public class RaftProtocolServer : SecureStreamingServer<RaftRequest, RaftResponse>
    {
        public RaftProtocolServer(RaftRing ring, int port = RaftNodeIdentifier.DefaultPort)
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
                        response.Data = Ring.ReceiveWriteRequest(request);
                        break;
                    case RaftRequestType.NotifyLeaderFollowerValueWritten:
                        response.Data = Ring.ReceiveFollowerWriteValueNotification(request);                        
                        break;
                    case RaftRequestType.NotifyFollowerLeaderValueCommitted:
                        response.Data = Ring.ReceiveLeaderCommittedValueNotification(request);
                        break;
                    case RaftRequestType.VoteRequest:
                        response.Data = Ring.ReceiveVoteRequest(request);
                        break;
                    case RaftRequestType.VoteResponse:
                        response.Data = Ring.ReceiveVoteResponse(request);
                        break;
                    case RaftRequestType.Heartbeat:
                        response.Data = Ring.ReceiveHeartbeat(request);
                        break;
                    case RaftRequestType.JoinRaft:
                        response.Data = Ring.ReceiveJoinRaftRequest(request);
                        break;
                    case RaftRequestType.LogSyncRequest:
                        response.Data = Ring.ReceiveLogSyncRequest(request);
                        break;
                    case RaftRequestType.LogSyncResponse:
                        response.Data = Ring.ReceiveLogSyncResponse(request);
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

    }
}