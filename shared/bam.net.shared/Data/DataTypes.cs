/*
	Copyright © Bryan Apellanes 2015  
*/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Bam.Net.Data
{
    [JsonConverter(typeof(StringEnumConverter))]
    public enum DataTypes
    {
        Default,
        Boolean,
        Int,
        UInt,
        ULong,
        Long,
        Decimal,
        String,
        ByteArray,
        DateTime
    }
}
