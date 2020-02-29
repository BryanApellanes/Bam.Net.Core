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
    public abstract class FileSystemJSchemaResolver: JSchemaResolver
    {
        public FileSystemJSchemaResolver(string path): this(new DirectoryInfo(path))
        {
        }
        
        public FileSystemJSchemaResolver(DirectoryInfo rootDirectory)
        {
            RootDirectory = rootDirectory;
        }
        
        public static FileSystemJSchemaResolver Default { get; set; }
        
        public JSchemaLoader JSchemaLoader { get; set; }
        
        public DirectoryInfo RootDirectory { get; set; }
        
        public override JSchema GetSubschema(SchemaReference reference, JSchema rootSchema)
        {
            if (JSchemaLoader != null && reference?.BaseUri != null)
            {
                return JSchemaLoader.LoadSchema(Path.Combine(RootDirectory.FullName, reference.BaseUri.ToString()));
            }
            if (reference?.SubschemaId == null)
            {
                foreach (string key in rootSchema.ExtensionData.Keys)
                {
                    JObject jObject = rootSchema.ExtensionData[key] as JObject;
                    jObject?.ConvertJSchemaPropertyTypes();
                }
                return rootSchema;
            }
            
            string subSchemaId = reference.SubschemaId.ToString(); // path in file
            object[] pathSegments = subSchemaId.DelimitSplit("/");
            if (pathSegments.First().Equals("#"))
            {
                pathSegments = pathSegments.Skip(1).ToArray();
            }

            JObject node = null;
            foreach (string segment in pathSegments)
            {
                if (node == null)
                {
                    node = rootSchema.ExtensionData[segment] as JObject;
                }
                else
                {
                    node = node[segment] as JObject;
                }
            }

            node.ConvertJSchemaPropertyTypes();
            return JSchema.Parse(node.ToJson(), this);
        }

        private void ConvertValueTypes(Dictionary<object, object> schema)
        {
            schema.Keys.BackwardsEach(key =>
            {
                if (schema[key] is string value)
                {
                    if (int.TryParse(value, out int intValue))
                    {
                        schema[key] = intValue;
                    }
                    else if (bool.TryParse(value, out bool boolValue))
                    {
                        schema[key] = boolValue;
                    }
                    else if (value.IsAffirmative())
                    {
                        schema[key] = true;
                    }
                    else if (value.IsNegative())
                    {
                        schema[key] = false;
                    }
                }
                else if (schema[key] is Dictionary<object, object> dictionary)
                {
                    ConvertValueTypes(dictionary);
                }
            });
        }
    }
}