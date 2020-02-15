using System.Collections.Generic;
using Bam.Net.Application.Json;
using Newtonsoft.Json.Schema;

namespace Bam.Net.Schema.Json
{
    public class JSchemaSchemaDefinitionGenerationSettings
    {
        public string Name { get; set; }
        public IEnumerable<JSchema> JSchemas{ get; set; }
        public SerializationFormat SerializationFormat { get; set; }
        public JSchemaManager JSchemaManager { get; set; }

        public JSchemaLoader GetLoader()
        {
            return JSchemaLoader.ForFormat(SerializationFormat);
        }
    }
}