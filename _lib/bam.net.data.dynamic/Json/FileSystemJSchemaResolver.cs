using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Schema;

namespace Bam.Net.Schema.Json
{
    /// <summary>
    /// A resolver for json schema entities defined in yaml files in the file system.
    /// </summary>
    public abstract class FileSystemJSchemaResolver: JSchemaResolver
    {
        public FileSystemJSchemaResolver(string path): this(new DirectoryInfo(path))
        {
            Collected = new HashSet<JSchema>();
        }
        
        public FileSystemJSchemaResolver(DirectoryInfo rootDirectory)
        {
            RootDirectory = rootDirectory;
            Collected = new HashSet<JSchema>();
        }
        
        public static FileSystemJSchemaResolver Default { get; set; }

        public static FileSystemJSchemaResolver ForFormat(string rootDirectory, SerializationFormat format)
        {
            switch (format)
            {
                case SerializationFormat.Json:
                    return new FileSystemJsonJSchemaResolver(rootDirectory);
                case SerializationFormat.Yaml:
                    return new FileSystemYamlJSchemaResolver(rootDirectory);
                case SerializationFormat.Xml:
                case SerializationFormat.Binary:
                case SerializationFormat.Invalid:
                    throw new UnsupportedSerializationFormatException(format);
            }
            throw new UnsupportedSerializationFormatException(format);
        }
        public JSchemaLoader JSchemaLoader { get; set; }
        
        public DirectoryInfo RootDirectory { get; set; }
        
        public HashSet<JSchema> Collected { get; set; }
        
        public override JSchema GetSubschema(SchemaReference reference, JSchema rootSchema)
        {
            if (JSchemaLoader != null && reference?.BaseUri != null)
            {
                JSchema referenced = JSchemaLoader.LoadSchema(Path.Combine(RootDirectory.FullName, reference.BaseUri.ToString()));
                Collected.Add(referenced);
                return referenced;
            }
            if (reference?.SubschemaId == null)
            {
                foreach (string key in rootSchema.ExtensionData.Keys)
                {
                    JObject jObject = rootSchema.ExtensionData[key] as JObject;
                    jObject?.ConvertJSchemaPropertyTypes();
                }

                Collected.Add(rootSchema);
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
            string refValue = string.Empty;
            if (node["$ref"] != null)
            {
                refValue = node["$ref"].ToString();
                node = JObject.Parse(GetSubschema(CreateSchemaReference(reference, refValue), rootSchema).ToString());
            }
            else if (node["type"] != null)
            {
                JSchemaType type = Enum.Parse<JSchemaType>(node["type"].ToString(), true);
                if (type == JSchemaType.Array)
                {
                    if (node["items"]?["$ref"] != null)
                    {
                        refValue = node["items"]["$ref"].ToString();
                        node["items"] = JObject.Parse(GetSubschema(CreateSchemaReference(reference, refValue), rootSchema).ToString());
                    }
                }
                else if (type == JSchemaType.Object)
                {
                    node = ResolveRefs(node, reference, rootSchema);
                }
            }
            
            JSchema result = JSchema.Parse(node.ToJson(), this);
            Collected.Add(result);
            return result;
        }

        private static SchemaReference CreateSchemaReference(SchemaReference rootReference, string refValue)
        {
            SchemaReference schemaReference = new SchemaReference();
            if (refValue.StartsWith("#"))
            {
                schemaReference.BaseUri = rootReference.BaseUri;
                schemaReference.SubschemaId = new Uri(refValue, UriKind.Relative);
            }
            else
            {
                string[] split = refValue.DelimitSplit("#");
                if (split.Length == 2)
                {
                    schemaReference.BaseUri = new Uri(split[0], UriKind.Relative);
                    schemaReference.SubschemaId = new Uri(split[1], UriKind.Relative);
                }
            }

            return schemaReference;
        }

        private JObject ResolveRefs(JObject node, SchemaReference rootReference, JSchema rootSchema)
        {
            JObject result = JObject.Parse(node.ToString());
            foreach (JProperty property in ((JObject) node["properties"]).Properties())
            {
                if (property.Value["$ref"] != null)
                {
                    SchemaReference subRef = CreateSchemaReference(rootReference, property.Value["$ref"].ToString());
                    JSchema jSchema = GetSubschema(subRef, rootSchema);
                    result["properties"][property.Name] = JObject.Parse(jSchema.ToString());
                }
            }

            return result;
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