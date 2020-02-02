using System.Collections.Generic;
using Bam.Net.Data.Schema;
using Newtonsoft.Json.Schema;

namespace Bam.Net.Schema.Json
{
    public class JSchemaSchemaDefinition // it is unclear whether SchemaDefinition will properly clone if this class extends it, so treating this as a wrapper to SchemaDefinition
    {
        public IEnumerable<JSchema> JSchemas { get; set; }
        public Dictionary<string, HashSet<string>> DiscoveredEnums { get; set; }
        public SchemaDefinition SchemaDefinition { get; set; }
    }
}