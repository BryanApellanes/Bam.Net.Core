using System.IO;
using Bam.Net.Data.Schema;
using Newtonsoft.Json;
using Newtonsoft.Json.Schema;

namespace Bam.Net.Application.Json
{
    public class JsonSchemaSchemaManager: SchemaManager
    {

        
        public static JSchema LoadJsonSchema(string path)
        {
            
            using (StreamReader file = File.OpenText(path))
            using (JsonTextReader reader = new JsonTextReader(file))
            {
                return JSchema.Load(reader);
            }
        }
    }
}