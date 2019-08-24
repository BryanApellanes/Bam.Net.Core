using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Bam.Net.Services.DataReplication
{
    [JsonConverter(typeof(StringEnumConverter))]
    public enum UniversalIdentifiers
    {
        Uuid,
        Cuid,
        CKey // composite key
    }
}
