using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Bam.Net.Application
{

    [JsonConverter(typeof(StringEnumConverter))]
    public enum AppKind
    {
        Bam,
        Repo,
        Dao,
        Rest,
        Service,
        Razor,
        //Django,
        //Express
    }
}
