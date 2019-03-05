using System;
using System.Collections.Generic;
using System.Text;

namespace Bam.Net.Data.Schema
{
    public class DaoGenerationEventArgs: EventArgs
    {
        public DaoGenerationEventArgs()
        {

        }

        public SchemaDefinition SchemaDefinition { get; set; }

        public Table Table { get; set; }
    }
}
