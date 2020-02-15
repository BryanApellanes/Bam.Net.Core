using System.Collections.Generic;
using System.IO;
using Bam.Net.Application.Json;
using Bam.Net.CoreServices;
using Newtonsoft.Json.Schema;

namespace Bam.Net.Schema.Json
{
    public class YamlJSchemaLoader : JSchemaLoader
    {
        public YamlJSchemaLoader(string rootDirectory) : this(new FileSystemYamlJSchemaResolver(rootDirectory))
        {
        }

        public YamlJSchemaLoader(JSchemaResolver jSchemaResolver) : base(jSchemaResolver)
        {
            Format = SerializationFormat.Yaml;
        }
        
        public SerializationFormat Format { get; protected set; }
        
        public override JSchema LoadSchema(string filePath)
        {
            FileInfo fileInfo = new FileInfo(filePath);
            Dictionary<object, object> schemaAsDictionary = filePath.FromYamlFile() as Dictionary<object, object>;
            return JSchema.Parse(schemaAsDictionary.ToJson(), new FileSystemYamlJSchemaResolver(fileInfo.Directory.FullName));
        }
    }
}