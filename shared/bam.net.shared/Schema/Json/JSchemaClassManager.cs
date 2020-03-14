using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Bam.Net.Data.Schema;
using Bam.Net.Logging;
using Bam.Net.Schema.Json;
using CsQuery.ExtensionMethods.Internal;
using Markdig.Helpers;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Schema;

namespace Bam.Net.Application.Json
{
    public class JSchemaClassManager : IJSchemaClassManager
    {
        private readonly List<string> _classNameProperties;

        public JSchemaClassManager(JSchemaResolver jSchemaResolver):this("className")
        {
            JSchemaResolver = jSchemaResolver;
            Logger = Log.Default;
        }

        /// <summary>
        /// Instantiate a new JSchemaManager.
        /// </summary>
        /// <param name="classNameProperties">The property names to look for the class name in.  JsonSchema doesn't specify where or how to declare
        /// the strongly typed name for a given schema, so we must specify that as part of our custom/proprietary implementation.</param>
        public JSchemaClassManager(params string[] classNameProperties)
        {
            JSchemaNameParser = new JSchemaNameParser();
            _classNameProperties = new List<string>(classNameProperties);
            Func<JSchema, string> defaultClassNameExtractor = JSchemaNameParser.ExtractClassName;
            JSchemaNameParser.ExtractClassName = jSchema =>
            {
                string defaultClassName = defaultClassNameExtractor(jSchema);
                string fromClassNameProperties = ExtractClassNameFromClassNameProperties(jSchema);
                if (!defaultClassName.Equals(fromClassNameProperties))
                {
                    return fromClassNameProperties;
                }

                return defaultClassName;
            };
        }

        protected string ExtractClassNameFromClassNameProperties(JSchema jSchema)
        {
            foreach (string tableNameProperty in _classNameProperties)
            {
                if (jSchema.HasProperty(tableNameProperty, out object value))
                {
                    return MungeClassName == null ? value.ToString(): MungeClassName(value.ToString());
                }
            }

            return string.Empty;
        }
        public JSchemaResolver JSchemaResolver { get; set; }
        public JSchemaNameParser JSchemaNameParser { get; set; }
        
        public ILogger Logger { get; set; }
        /// <summary>
        /// A function used to further parse a class name when it is found.  This is intended
        /// to apply any conventions to the name that are not enforced in the JSchema.  Parse the
        /// inbound string and return an appropriate class name based on it.
        /// </summary>
        public Func<string, string> MungeClassName
        {
            get => JSchemaNameParser.MungeClassName;
            set => JSchemaNameParser.MungeClassName = value;
        }


        public Func<JSchema, string> ExtractClassName
        {
            get => JSchemaNameParser.ExtractClassName;
            set => JSchemaNameParser.ExtractClassName = value;
        }

        public Func<string, string> MungeEnumName
        {
            get => JSchemaNameParser.MungeEnumName;
            set => JSchemaNameParser.MungeEnumName = value;
        }

        /// <summary>
        /// A function used to further parse a property name when it is found.  This is intended to
        /// apply any conventions to the name that are not enforced in the JSchema.  Parse the
        /// inbound string and return an appropriate property name based on it.
        /// </summary>
        public Func<string, string> ParsePropertyName
        {
            get => JSchemaNameParser.ParsePropertyName;
            set => JSchemaNameParser.ParsePropertyName = value;
        }
        
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
                if (jSchema.HasProperty(tableNameProperty, out object value))
                {
                    return MungeClassName == null ? value.ToString(): MungeClassName(value.ToString());
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
            
            if (ParsePropertyName != null)
            {
                return results.Select(ParsePropertyName).ToArray();
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


        public JSchemaClass LoadJSchemaClass(string filePath)
        {
            JSchemaLoader loader = GetJSchemaLoader(new FileInfo(filePath));
            if (loader == null)
            {
                string msgFormat = "No loader for file {0}";
                Logger.Warning(msgFormat, filePath);
                return new JSchemaClass(this, string.Format(msgFormat, filePath));
            }
            
            loader.JSchemaResolver = JSchemaResolver;
            JSchema result = loader.LoadSchema(filePath);
            JSchemaClass resultClass = new JSchemaClass(result, this);
            return resultClass;
        }

        public HashSet<JSchema> LoadJSchemas(DirectoryInfo directoryInfo, JSchemaResolver jSchemaResolver, out List<JSchemaLoadResult> loadResults)
        {
            Args.ThrowIfNull(directoryInfo, "directoryInfo");
            loadResults = new List<JSchemaLoadResult>();

            HashSet<JSchema> results = new HashSet<JSchema>();
            foreach (FileInfo fileInfo in directoryInfo.GetFiles())
            {
                JSchemaLoader loader = GetJSchemaLoader(fileInfo);

                if (loader == null)
                {
                    Logger.Warning("No loader for file {0}", fileInfo.FullName);
                    continue;
                }

                loader.JSchemaResolver = jSchemaResolver;
                try
                {
                    JSchema jSchema = loader.LoadSchema(fileInfo.FullName);
                    results.Add(jSchema);
                    loadResults.Add(new JSchemaLoadResult(fileInfo.FullName, jSchema));
                }
                catch (Exception ex)
                {
                    loadResults.Add(new JSchemaLoadResult(fileInfo.FullName, ex));
                }
            }

            return results;
        }

        private static JSchemaLoader GetJSchemaLoader(FileInfo fileInfo)
        {
            JSchemaLoader loader = null;
            if (fileInfo.Extension.Equals(".yaml", StringComparison.InvariantCultureIgnoreCase))
            {
                loader = JSchemaLoader.ForFormat(SerializationFormat.Yaml);
            }
            else if (fileInfo.Extension.Equals(".json", StringComparison.InvariantCultureIgnoreCase))
            {
                loader = JSchemaLoader.ForFormat(SerializationFormat.Json);
            }

            return loader;
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