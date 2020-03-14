using System.Collections.Generic;
using System.Linq;
using Bam.Net.Application.Json;
using Newtonsoft.Json.Schema;

namespace Bam.Net.Schema.Json
{
    public class JSchemaSchemaDefinitionGenerationSettings
    {
        public JSchemaSchemaDefinitionGenerationSettings()
        {
        }

        public JSchemaSchemaDefinitionGenerationSettings(JSchemaClassManager jSchemaClassManager, JSchemaResolver resolver, string schemaName, params string[] dataPaths)
        {
            Name = schemaName;
            JSchemas = dataPaths.Select(JSchemaLoader.LoadYamlJSchema).ToList();
            SerializationFormat = SerializationFormat.Yaml;
            JSchemaClassManager = jSchemaClassManager;
            JSchemaResolver = resolver;
        }

    public string Name { get; set; }
        public IEnumerable<JSchema> JSchemas{ get; set; }
        public SerializationFormat SerializationFormat { get; set; }
        public JSchemaClassManager JSchemaClassManager { get; set; }

        public JSchemaResolver JSchemaResolver
        {
            get => JSchemaClassManager.JSchemaResolver;
            set => JSchemaClassManager.JSchemaResolver = value;
        }

        public JSchemaLoader GetLoader()
        {
            return JSchemaLoader.ForFormat(SerializationFormat);
        }
    }
}