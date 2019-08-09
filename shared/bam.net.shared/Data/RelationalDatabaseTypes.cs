using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Bam.Net.Data
{
    [JsonConverter(typeof(StringEnumConverter))]
    public enum RelationalDatabaseTypes
    {
        SQLite,
        MsSql,
        MySql,
        Npgsql
    }
}