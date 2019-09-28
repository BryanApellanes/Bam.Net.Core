using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Bam.Net.Server
{
    [JsonConverter(typeof(StringEnumConverter))]
    public enum ServerKinds
    {
        Invalid,
        Bam,
        Node,
        Python,
        External
    }
}