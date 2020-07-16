using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json.Schema;

namespace Bam.Net.Schema.Json
{
    /// <summary>
    /// A resolver for json schema entities defined in yaml files in the file system.
    /// </summary>
    public class FileSystemYamlJSchemaResolver: FileSystemJSchemaResolver
    {
        public FileSystemYamlJSchemaResolver(HomePath path) : this(path.Resolve())
        {
        }

        public FileSystemYamlJSchemaResolver(string path): this(new DirectoryInfo(path))
        {
        }
        
        public FileSystemYamlJSchemaResolver(DirectoryInfo rootDirectory): base(rootDirectory)
        {
            RootDirectory = rootDirectory;
        }
        
        public override Stream GetSchemaResource(ResolveSchemaContext context, SchemaReference reference)
        {
            string baseUri = reference.BaseUri.ToString(); // path to the file

            string filePath = Path.Combine(RootDirectory.FullName, baseUri);
            Dictionary<object, object> schema = filePath.FromYamlFile() as Dictionary<object, object>; 
            schema.ConvertJSchemaPropertyTypes();
            
            return schema.ToJsonStream();
        }
    }
}