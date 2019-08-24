using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Bam.Net.Services.DataReplication
{
    [JsonConverter(typeof(StringEnumConverter))]
    public enum QueryOperator
    {
        Invalid,
        Equals,
        NotEqualTo,
        GreaterThan,
        LessThan,
        StartsWith,
        DoesntStartWith,
        EndsWith,
        DoesntEndWith,
        Contains,
        DoesntContain,
        In
    }
}