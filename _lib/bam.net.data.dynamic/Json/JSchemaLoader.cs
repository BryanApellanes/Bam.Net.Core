using System;
using Newtonsoft.Json.Schema;

namespace Bam.Net.Schema.Json
{
    public abstract class JSchemaLoader : IJSchemaLoader
    {
        public JSchemaLoader(JSchemaResolver jSchemaResolver)
        {
            Format = SerializationFormat.Json;
            JSchemaResolver = jSchemaResolver;
        }
        
        static readonly object _defaultResolverLock = new object();
        private static JSchemaResolver _jSchemaResolver;

        public static JSchemaResolver DefaultJSchemaResolver
        {
            get
            {
                return _defaultResolverLock.DoubleCheckLock(ref _jSchemaResolver, () => new FileSystemYamlJSchemaResolver("./JsonSchemas"));
            }
            set => _jSchemaResolver = value;
        }

        public SerializationFormat Format { get; protected set; }
        public JSchemaResolver JSchemaResolver { get; set; }
        public abstract JSchema LoadSchema(string filePath);

        public static JSchemaLoader ForFormat(SerializationFormat format)
        {
            switch (format)
            {
                case SerializationFormat.Json:
                    return new JsonJSchemaLoader(DefaultJSchemaResolver);
                    break;
                case SerializationFormat.Yaml:
                    return new YamlJSchemaLoader(DefaultJSchemaResolver);
                    break;
                default:
                    throw new InvalidOperationException($"Unsupported SerializationFormat: {format.ToString()}");
            }
        }

        public static JSchema LoadJsonJSchema(string filePath)
        {
            return LoadJSchema(filePath, SerializationFormat.Json);
        }
        
        public static JSchema LoadYamlJSchema(string filePath)
        {
            return LoadJSchema(filePath, SerializationFormat.Yaml);
        }
        
        public static JSchema LoadJSchema(string filePath, SerializationFormat format)
        {
            JSchemaLoader loader = ForFormat(format);
            return loader.LoadSchema(filePath);
        }
    }
}