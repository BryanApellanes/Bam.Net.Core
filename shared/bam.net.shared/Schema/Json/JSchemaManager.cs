using System;
using System.Collections.Generic;
using System.Linq;
using Bam.Net.Data.Schema;
using Bam.Net.Schema.Json;
using Markdig.Helpers;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Schema;

namespace Bam.Net.Application.Json
{
    public class JSchemaManager : IJSchemaManager
    {
        private readonly List<string> _classNameProperties;
        public JSchemaManager():this("className")
        {
        }

        /// <summary>
        /// Instantiate a new JSchemaManager.
        /// </summary>
        /// <param name="classNameProperties">The property names to look for the class name in.  JsonSchema doesn't specify where or how to declare
        /// the strongly typed name for a given schema, so we must specify that as part of our custom/proprietary implementation.</param>
        public JSchemaManager(params string[] classNameProperties)
        {
            _classNameProperties = new List<string>(classNameProperties);
        }

        /// <summary>
        /// A function used to further parse a class name when it is found.  This is intended
        /// to apply any conventions to the name that are not enforced in the JSchema.  Parse the
        /// inbound string and return an appropriate class name based on it.
        /// </summary>
        public Func<string, string> ParseClassNameFunction { get; set; }
        
        public Func<JSchema, string> ExtractClassName { get; set; }
        
        public Func<JSchema, string> ExtractEnumName { get; set; }
        /// <summary>
        /// A function used to further parse a property name when it is found.  This is intended to
        /// apply any conventions to the name that are not enforced in the JSchema.  Parse the
        /// inbound string and return an appropriate property name based on it.
        /// </summary>
        public Func<string, string> ParsePropertyNameFunction { get; set; }
        public virtual string GetObjectClassName(JSchema jSchema)
        {
            Args.ThrowIfNull(jSchema, nameof(jSchema));
            string schemaType = jSchema.Type?.ToString() ?? "object"; // if no type is specified try to parse the class name properties
            
            if (!schemaType.Equals("object", StringComparison.InvariantCultureIgnoreCase))
            {
                Args.Throw<InvalidOperationException>("Specified JSchema not an object: Actual type is ({0})", schemaType);
            }
            
            foreach (string tableNameProperty in _classNameProperties)
            {
                if (jSchema.HasJSchemaProperty(tableNameProperty, out object value))
                {
                    return ParseClassNameFunction == null ? value.ToString(): ParseClassNameFunction(value.ToString());
                }
            }
            
            string className = string.Empty;
            if (ExtractClassName != null)
            {
                className = ExtractClassName(jSchema);
            }
            
            return className;
        }

        /// <summary>
        /// Get the JSchema for the item definition of the specified array JSchema.
        /// </summary>
        /// <param name="jSchema"></param>
        /// <returns></returns>
        public JSchema GetArrayItemSchema(JSchema jSchema)
        {
            Args.ThrowIfNull(jSchema, nameof(jSchema));
            Args.ThrowIfNull(jSchema.Type, nameof(jSchema.Type));
            string schemaType = jSchema.Type.ToString();

            if (!schemaType.Equals("array", StringComparison.InvariantCultureIgnoreCase))
            {
                Args.Throw<InvalidOperationException>("Specified JSchema not an array: Actual type is ({0})", schemaType);
            }

            return jSchema.Items.FirstOrDefault();
        }
        
        public string GetArrayClassName(JSchema jSchema)
        {
            JSchema arraySchema = GetArrayItemSchema(jSchema);
            if (arraySchema == null)
            {
                Args.Throw<ArgumentNullException>("The items property of the specified array schema was null: \r\n{0}", jSchema);
            }

            return GetObjectClassName(arraySchema);
        }

        public string[] GetAllClassNames(JSchema jSchema)
        {
            HashSet<string> tableNames = new HashSet<string> {GetObjectClassName(jSchema)};

            foreach (string objectPropertyName in GetObjectPropertyNames(jSchema))
            {
                tableNames.Add(GetPropertyClassName(jSchema, objectPropertyName));
            }
            return tableNames.ToArray();
        }

        public string[] GetPropertyColumnNames(JSchema jSchema, string propertyName)
        {
            JSchema propertySchema = GetPropertySchema(jSchema, propertyName);
            return GetPropertyNames(propertySchema);
        }

        public string[] GetPropertyNames(JSchema jSchema)
        {
            List<string> results = new List<string>();
            results.AddRange(GetPrimitivePropertyNames(jSchema));
            results.AddRange(GetObjectPropertyNames(jSchema).Select(op=> $"{op}Id")); // objects are referenced by id
            
            if (ParsePropertyNameFunction != null)
            {
                return results.Select(ParsePropertyNameFunction).ToArray();
            }
            return results.ToArray();
        }

        private object _schemaNameMapLock = new object();

        protected HashSet<string> PrimitiveTypes
        {
            get { return new HashSet<string>(new[] {"number", "string", "boolean"}); }
        }
        
        public string[] GetPrimitivePropertyNames(JSchema jSchema)
        {
            HashSet<string> primitiveTypes = PrimitiveTypes;
            List<string> propertyNames = new List<string>();
            foreach (string propertyName in jSchema.Properties.Keys)
            {
                JSchema property = jSchema.Properties[propertyName];

                if (primitiveTypes.Contains(property.Type.ToString().ToLowerInvariant()))
                {
                    propertyNames.Add(propertyName.PascalCase());
                }
            }

            return propertyNames.ToArray();
        }

        /// <summary>
        /// Get the names of the properties that are of type object.
        /// </summary>
        /// <param name="jSchema"></param>
        /// <returns></returns>
        public string[] GetObjectPropertyNames(JSchema jSchema)
        {
            return GetPropertyNamesOfType(jSchema, "object");
        }

        public string[] GetArrayPropertyNames(JSchema jSchema)
        {
            return GetPropertyNamesOfType(jSchema, "array");
        }

        public string GetArrayPropertyClassName(JSchema jSchema, string propertyName)
        {
            JSchema propertySchema = GetPropertySchema(jSchema, propertyName);
            return GetArrayClassName(propertySchema);
        }

        public string GetPropertyClassName(JSchema rootSchema, string propertyName)
        {
            JSchema propertySchema = GetPropertySchema(rootSchema, propertyName);
            return GetObjectClassName(propertySchema);
        }
        
        public JSchema GetPropertySchema(JSchema jSchema, string propertyName)
        {
            Args.ThrowIfNullOrEmpty(propertyName, nameof(propertyName));
            string key = propertyName;
            if (!jSchema.Properties.ContainsKey(key) && IsAlphaUpper(propertyName[0]))
            {
                key = propertyName.CamelCase();
            }

            if (!jSchema.Properties.ContainsKey(key))
            {
                Args.Throw<InvalidOperationException>("Specified property not found in specified schema: {0}\r\n{1}", key, jSchema.ToString());
            }
            return jSchema.Properties[key];
        }

        public IEnumerable<JSchema> GetSubSchemas(JSchema jSchema)
        {
            return GetSubSchemas(jSchema, "definitions");
        }
        
        public IEnumerable<JSchema> GetSubSchemas(JSchema jSchema, string key)
        {
            foreach (JToken token in jSchema.ExtensionData[key])
            {
                Dictionary<object, object> result= new Dictionary<object, object>();
                result.AddMissing("$schema", "http://json-schema.org/draft-04/schema#");
                foreach (JObject child in token.Children<JObject>())
                {
                    foreach (JProperty property in child.Properties())
                    {
                        result.Add(property.Name, child[property.Name]);
                    }
                }
                result.ConvertJSchemaPropertyTypes();
                yield return JSchema.Parse(result.ToJson());
            }
        }

        public string[] GetEnumValues(JSchema enumSchema)
        {
            List<string> values = new List<string>();
            foreach (JValue val in enumSchema.Enum)
            {
                values.Add(val.Value?.ToString());
            }

            return values.ToArray();
        }

        public string GetEnumName(JSchema enumDefinition)
        {
            if (ExtractEnumName != null)
            {
                return ExtractEnumName(enumDefinition);
            }

            return string.Empty;
        }

        public Dictionary<string, JSchema> GetEnumProperties(JSchema jSchema)
        {
            Dictionary<string, JSchema> enumProperties = new Dictionary<string, JSchema>();
            foreach (string propertyName in jSchema.Properties.Keys)
            {
                JSchema property = jSchema.Properties[propertyName];
                if (property.Enum.Count > 0)
                {
                    enumProperties.Add(propertyName, property);
                }
            }

            return enumProperties;
        }
        
        
        protected string[] GetPropertyNamesOfType(JSchema jSchema, string typeName)
        {
            List<string> propertyNames = new List<string>();
            foreach(string propertyName in jSchema.Properties.Keys)
            {
                JSchema property = jSchema.Properties[propertyName];
                if (property.Type.ToString().Equals(typeName, StringComparison.InvariantCultureIgnoreCase))
                {
                    propertyNames.Add(propertyName);
                }
            }

            return propertyNames.ToArray();
        }
        
        private static bool IsAlphaUpper(char c)
        {
            return c >= 'A' && c <= 'Z';
        }
    }
}