using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Bam.Net.Services.DataReplication.Consensus
{
    [JsonConverter(typeof(StringEnumConverter))]
    public enum RaftNodeState
    {
        Invalid,
        Follower,
        Candidate,
        Leader
    }
}