using Bam.Net.CoreServices;
using Bam.Net.Data.Schema;
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
                .For<JSchemaLoader>().Use(JSchemaLoader.ForFormat(format))
                .For<JavaJSchemaClassManager>().Use(new JavaJSchemaClassManager() {JSchemaResolver = resolver});
        }
        
        public static JSchemaManagementRegistry CreateForYaml(string rootData, params string[] classNameProperties)
        {
            return Create(rootData, SerializationFormat.Yaml, classNameProperties);
        }
        
        public static JSchemaManagementRegistry Create(string rootData, SerializationFormat format, params string[] classNameProperties)
        {
            if (classNameProperties == null || classNameProperties.Length == 0)
            {
                classNameProperties = new string[] {"class", "className", "@type", "javaType"};
            }
            JSchemaManagementRegistry registry = new JSchemaManagementRegistry(rootData, format);
            JSchemaResolver resolver = FileSystemJSchemaResolver.ForFormat(rootData, format);
            JSchemaLoader loader = JSchemaLoader.ForFormat(format);
            SchemaManager schemaManager = new SchemaManager(new UnixPath($"~/.bam/data/JSchema_{nameof(SchemaManager)}.json"));
            registry
                .For<SchemaManager>().Use(schemaManager)
                .For<JSchemaResolver>().Use(resolver)
                .For<JSchemaLoader>().Use(loader)
                .For<JSchemaClassManager>().Use(new JSchemaClassManager(classNameProperties) {JSchemaResolver = resolver})
                .For<JavaJSchemaClassManager>().Use(new JavaJSchemaClassManager() {JSchemaResolver = resolver})
                .For<JSchemaDaoAssemblyGenerator>().Use<JSchemaDaoAssemblyGenerator>();

            return registry;
        }
    }
}