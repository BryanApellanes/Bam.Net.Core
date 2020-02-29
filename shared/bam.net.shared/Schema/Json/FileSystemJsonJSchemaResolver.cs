using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Bam.Net.Schema.Json;
using MongoDB.Bson;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Schema;

namespace Bam.Net.Application.Json
{
    /// <summary>
    /// A resolver for json schema entities defined in yaml files in the file system.
    /// </summary>
    public class FileSystemJsonJSchemaResolver: FileSystemJSchemaResolver
    {
        public FileSystemJsonJSchemaResolver(string path): this(new DirectoryInfo(path))
        {
        }
        
        public FileSystemJsonJSchemaResolver(DirectoryInfo rootDirectory): base(rootDirectory)
        {
            RootDirectory = rootDirectory;
        }
        
        public override Stream GetSchemaResource(ResolveSchemaContext context, SchemaReference reference)
        {
            string baseUri = reference.BaseUri.ToString(); // path to the file

            string filePath = Path.Combine(RootDirectory.FullName, baseUri);
            return File.OpenRead(filePath);
        }
    }
}