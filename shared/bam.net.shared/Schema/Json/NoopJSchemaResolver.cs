using System.IO;
using Newtonsoft.Json.Schema;

namespace Bam.Net.Schema.Json
{
    public class NoopJSchemaResolver: JSchemaResolver
    {
        public override Stream GetSchemaResource(ResolveSchemaContext context, SchemaReference reference)
        {
            return Stream.Null;
        }
    }
}