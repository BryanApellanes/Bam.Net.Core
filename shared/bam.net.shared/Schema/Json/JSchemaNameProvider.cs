using System;
using System.Collections.Generic;
using System.Linq;
using Bam.Net.Data.Schema;
using Bam.Net.Schema.Json;
using CsQuery.ExtensionMethods.Internal;
using Newtonsoft.Json.Schema;

namespace Bam.Net.Application.Json
{
    public class JSchemaNameProvider : IJSchemaNameProvider
    {
        private readonly Dictionary<ulong, SchemaNameMap> _schemaNameMapsByJSchemaHash;
        private readonly List<string> _tableNameProperties;
        public JSchemaNameProvider():this("className")
        {
        }

        /// <summary>
        /// Instantiate a new JSchemaNameProvider.
        /// </summary>
        /// <param name="classNameProperties">The property names to look for the class name in.  JsonSchema doesn't specify where or how to declare
        /// the strongly typed name for a given schema, so we must specify that as part of our custom/proprietary implementation.</param>
        public JSchemaNameProvider(params string[] classNameProperties)
        {
            _schemaNameMapsByJSchemaHash = new Dictionary<ulong, SchemaNameMap>();
            _tableNameProperties = new List<string>(classNameProperties);
        }

        public Func<string, string> ParseRootTableNameFunction { get; set; }
        public Func<string, string> ParsePropertyNameForColumnNameFunction { get; set; }
        public string GetTableName(JSchema jSchema)
        {
            return GetClassName(jSchema);
        }

        public string GetClassName(JSchema jSchema)
        {
            Args.ThrowIfNull(jSchema.Type, "jSchema.Type");
            
            if (!jSchema.Type.ToString().Equals("object", StringComparison.InvariantCultureIgnoreCase))
            {
                Args.Throw<InvalidOperationException>("Specified JSchema not an object: {0}", jSchema);
            }
            
            foreach (string tableNameProperty in _tableNameProperties)
            {
                if (jSchema.HasJSchemaProperty(tableNameProperty, out object value))
                {
                    return ParseRootTableNameFunction == null ? value.ToString(): ParseRootTableNameFunction(value.ToString());
                }
            }

            return string.Empty;
        }
        
        public string[] GetAllTableNames(JSchema jSchema)
        {
            HashSet<string> tableNames = new HashSet<string> {GetTableName(jSchema)};

            foreach (string objectPropertyName in GetObjectPropertyNames(jSchema))
            {
                tableNames.Add(GetPropertyTableName(jSchema, objectPropertyName));
            }
            return tableNames.ToArray();
        }

        public string[] GetPropertyColumnNames(JSchema jSchema, string propertyName)
        {
            JSchema propertySchema = GetPropertySchema(jSchema, propertyName);
            return GetColumnNames(propertySchema);
        }

        public string[] GetColumnNames(JSchema jSchema)
        {
            List<string> results = new List<string>();
            results.AddRange(GetPrimitivePropertyNames(jSchema));
            results.AddRange(GetObjectPropertyNames(jSchema).Select(op=> $"{op}Id")); // objects are referenced by id
            
            if (ParsePropertyNameForColumnNameFunction != null)
            {
                return results.Select(ParsePropertyNameForColumnNameFunction).ToArray();
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
                    propertyNames.Add(propertyName);
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

        public string GetPropertyTableName(JSchema rootSchema, string propertyName)
        {
            JSchema propertySchema = GetPropertySchema(rootSchema, propertyName);
            return GetTableName(propertySchema);
        }
        
        public JSchema GetPropertySchema(JSchema rootSchema, string propertyName)
        {
            return rootSchema.Properties[propertyName];
        }

        protected string[] GetPropertyNamesOfType(JSchema jSchema, string typeName)
        {List<string> propertyNames = new List<string>();
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
    }
}