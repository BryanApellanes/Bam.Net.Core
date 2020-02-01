using Newtonsoft.Json.Schema;

namespace Bam.Net.Schema.Json
{
    public interface IJSchemaLoader
    {
        SerializationFormat Format { get; }
        JSchema LoadSchema(string filePath);
    }
}