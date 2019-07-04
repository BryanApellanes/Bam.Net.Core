using Bam.Net.Server.Streaming;

namespace Bam.Net.Services.DataReplication.Consensus
{
    public class RaftClient
    {
        public RaftClient(string hostName, int port)
        {
            StreamingClient = new SecureStreamingClient<RaftRequest, RaftResponse>(hostName, port);
        }

        public RaftClient(StreamingClient<RaftRequest, RaftResponse> streamingClient)
        {
            StreamingClient = streamingClient;
        }
        
        public StreamingClient<RaftRequest, RaftResponse> StreamingClient { get; set; }
        
    }
}