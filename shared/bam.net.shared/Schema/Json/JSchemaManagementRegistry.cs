using System;
using Bam.Net.Application.Json;
using Bam.Net.CoreServices;
using Newtonsoft.Json.Schema;

namespace Bam.Net.Schema.Json
{
    public class JSchemaManagementRegistry : ServiceRegistry
    {
        public JSchemaManagementRegistry(string fileSystemRoot, SerializationFormat format = SerializationFormat.Yaml) : this(
            new FileSystemYamlJSchemaResolver(fileSystemRoot), format)
        {
        }

        public JSchemaManagementRegistry(JSchemaResolver resolver, SerializationFormat format)
        {
            For<JSchemaResolver>().Use(resolver)
                .For<JSchemaLoader>().Use(JSchemaLoader.ForFormat(format));
            
        }

        public static JSchemaManagementRegistry ForJSchemasWithClassNamePropertiesOf(string rootData, params string[] classNameProperties)
        {
            JSchemaManagementRegistry registry = new JSchemaManagementRegistry(rootData, SerializationFormat.Yaml);
            JSchemaResolver resolver = new FileSystemYamlJSchemaResolver(rootData);
            JSchemaLoader loader = JSchemaLoader.ForFormat(SerializationFormat.Yaml);
            registry
                .For<JSchemaResolver>().Use(resolver)
                .For<JSchemaLoader>().Use(loader)
                .For<JSchemaClassManager>().Use(new JSchemaClassManager(classNameProperties) {JSchemaResolver = resolver});
            
            return registry;
        }
    }
}