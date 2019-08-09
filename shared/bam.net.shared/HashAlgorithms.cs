/*
	Copyright © Bryan Apellanes 2015  
*/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Bam.Net
{
    [JsonConverter(typeof(StringEnumConverter))]
    public enum HashAlgorithms
    {
        Invalid,
        MD5,
        RIPEMD160,
        SHA1,
        SHA256,
        SHA384,
        SHA512
    }
}
