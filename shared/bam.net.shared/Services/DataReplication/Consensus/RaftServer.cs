using Bam.Net.Server.Streaming;

namespace Bam.Net.Services.DataReplication.Consensus
{
    public class RaftServer : SecureStreamingServer<RaftRequest, RaftResponse>
    {
        public override RaftResponse ProcessDecryptedRequest(RaftRequest request)
        {
            throw new System.NotImplementedException();
        }
    }
}