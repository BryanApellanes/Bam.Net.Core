using System.Collections.Generic;
using Bam.Net.Data.Schema;

namespace Bam.Net.Schema.Json
{
    public class JSchemaSchemaDefinition
    {
        public static implicit operator SchemaDefinition(JSchemaSchemaDefinition jSchemaSchemaDefinition)
        {
            return jSchemaSchemaDefinition.SchemaDefinition;
        }
        
        public JSchemaSchemaDefinition(SchemaDefinition schemaDefinition, HashSet<JSchemaClass> classes)
        {
            this.SchemaDefinition = schemaDefinition;
            this.Classes = classes;
        }
        
        public SchemaDefinition SchemaDefinition { get; set; }
        public HashSet<JSchemaClass> Classes { get; set; }
    }
}