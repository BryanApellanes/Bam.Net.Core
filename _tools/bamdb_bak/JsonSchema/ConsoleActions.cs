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
    public class ConsoleActions: CommandLineTool
    {
        [ConsoleAction]
        public void LoadSchemaTest()
        {
            // see bam.net/_tests/core/bam.net.schema.json.tests
        }

    }
}