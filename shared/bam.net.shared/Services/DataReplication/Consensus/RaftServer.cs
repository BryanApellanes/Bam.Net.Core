using System;
using Bam.Net.Server.Streaming;

namespace Bam.Net.Services.DataReplication.Consensus
{
    public class RaftServer : SecureStreamingServer<RaftRequest, RaftResponse>
    {
        public RaftServer(RaftRing ring)
        {
            Ring = ring;
        }
        
        public RaftRing Ring { get; set; }
        
        public override RaftResponse ProcessDecryptedRequest(RaftRequest request)
        {
            RaftResponse response = new RaftResponse() {Request = request};
            try
            {
                Args.ThrowIfNull(request, "RaftRequest");
                Args.ThrowIfNull(request.WriteRequest, "RaftRequest.WriteRequest");
                
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
                    case RaftRequestType.Invalid:
                        Logger.AddEntry("Invalid raft request received: {0}", request?.ToString() ?? "[null]");
                        break;
                    default:
                        break;
                }
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