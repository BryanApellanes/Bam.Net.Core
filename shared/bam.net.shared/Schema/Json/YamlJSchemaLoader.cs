using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json.Schema;

namespace Bam.Net.Schema.Json
{
    public class YamlJSchemaLoader : JSchemaLoader
    {
        private readonly Dictionary<string, JSchema> _fileSchemas;
        public YamlJSchemaLoader(string rootDirectory) : this(new FileSystemYamlJSchemaResolver(rootDirectory))
        {
        }

        public YamlJSchemaLoader(JSchemaResolver jSchemaResolver) : base(jSchemaResolver)
        {
            _fileSchemas = new Dictionary<string, JSchema>();
            Format = SerializationFormat.Yaml;
        }
        
        public SerializationFormat Format { get; protected set; }

        readonly object loadLock = new object();
        public override JSchema LoadSchema(string filePath)
        {
            if (!_fileSchemas.ContainsKey(filePath))
            {
                lock (loadLock)
                {
                    FileInfo fileInfo = new FileInfo(filePath);
                    Dictionary<object, object> schemaAsDictionary = filePath.FromYamlFile() as Dictionary<object, object>;
                    schemaAsDictionary.ConvertJSchemaPropertyTypes();
                    if (JSchemaResolver is FileSystemJSchemaResolver fileSystemJSchemaResolver)
                    {
                        fileSystemJSchemaResolver.JSchemaLoader = this;
                        fileSystemJSchemaResolver.RootDirectory = fileInfo.Directory;
                    }
                    JSchemaResolver resolver = JSchemaResolver ?? new FileSystemYamlJSchemaResolver(fileInfo.Directory.FullName)
                    {
                        JSchemaLoader = this
                    };
                    _fileSchemas.Add(filePath, JSchema.Parse(schemaAsDictionary.ToJson(), resolver));
                }
            }
            return _fileSchemas[filePath];
        }
    }
}