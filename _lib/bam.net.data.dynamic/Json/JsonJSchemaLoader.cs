using System.Collections.Generic;
using Newtonsoft.Json.Schema;

namespace Bam.Net.Schema.Json
{
    public class JsonJSchemaLoader : JSchemaLoader
    {
        public JsonJSchemaLoader(JSchemaResolver jSchemaResolver) : base(jSchemaResolver)
        {
            Format = SerializationFormat.Json;
        }

        public override JSchema LoadSchema(string filePath)
        {
            return JSchema.Parse(filePath.SafeReadFile(), new JSchemaReaderSettings {Resolver = this.JSchemaResolver});
        }
    }
}