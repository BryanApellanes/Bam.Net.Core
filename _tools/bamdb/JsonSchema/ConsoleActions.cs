using System;
using System.Collections.Generic;
using System.IO;
using Bam.Net.CommandLine;
using Bam.Net.Testing;
using Newtonsoft.Json;
using Newtonsoft.Json.Schema;

namespace Bam.Net.Application.Json
{
    [Serializable]
    public class ConsoleActions: CommandLineTestInterface
    {
        [ConsoleAction]
        public void LoadSchemaTest()
        {

        }

    }
}