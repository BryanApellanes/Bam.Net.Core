﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Bam.Net.Services.Catalog
{
    [JsonConverter(typeof(StringEnumConverter))]
    public enum KindsOfCatalogs
    {
        None,
        List,
        Inventory,
        Shopping,
        Menu
    }
}
