using System;
using System.Collections.Generic;
using Bam.Net.Data.FirebirdSql;
using Bam.Net.Data.Schema;
using Bam.Net.Logging;
using Newtonsoft.Json.Schema;

namespace Bam.Net.Schema.Json
{
    public class JSchemaSchemaDefinition // it is unclear whether SchemaDefinition will properly clone if this class extends it, so treating this as a wrapper to SchemaDefinition
    {
        public IEnumerable<JSchema> JSchemas { get; set; }
        public Dictionary<string, HashSet<string>> DiscoveredEnums { get; set; }
        public SchemaDefinition SchemaDefinition { get; set; }

        public JSchemaSchemaDefinition CombineWith(SchemaDefinition schemaDefinition)
        {
            SchemaDefinition = SchemaDefinition.CombineWith(schemaDefinition);
            return this;
        }
        
        public JSchemaSchemaDefinition CombineWith(JSchemaSchemaDefinition jSchemaSchemaDefinition, Action<HashSet<string>> onEnumCollision = null)
        {
            List<JSchema> jSchemas = new List<JSchema>();
            jSchemas.AddRange(JSchemas);
            jSchemas.AddRange(jSchemaSchemaDefinition.JSchemas);
            JSchemas = jSchemas;

            foreach (string key in jSchemaSchemaDefinition.DiscoveredEnums.Keys)
            {
                if (DiscoveredEnums.ContainsKey(key))
                {
                    if (onEnumCollision != null)
                    {
                        onEnumCollision(jSchemaSchemaDefinition.DiscoveredEnums[key]);
                    }
                    else
                    {
                        Log.Error("Duplicate enum names found: {0}", key);
                    }
                }
            }

            SchemaDefinition = SchemaDefinition.CombineWith(jSchemaSchemaDefinition.SchemaDefinition);
            
            return this;
        }
    }
}