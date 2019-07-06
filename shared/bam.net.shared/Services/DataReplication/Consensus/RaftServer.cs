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
                switch (request.RequestType)
                {
                    case RaftRequestType.WriteValue:
                        if (request.WriteRequest != null)
                        {
                            HandleWriteRequest(request.WriteRequest);
                        }

                        break;
                    case RaftRequestType.NotifyLeaderFollowerValueWritten:
                        Ring.ReceiveFollowerWriteValueNotification(request.WriteRequest);
                        break;
                    case RaftRequestType.Invalid:
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
            if (writeRequest.TargetNodeType == RaftNodeType.Leader)
            {
                Ring.LeaderWriteValue(writeRequest);
            }

            if (writeRequest.TargetNodeType == RaftNodeType.Follower)
            {
                Ring.FollowerWriteValue(writeRequest);
            }
        }
    }
}