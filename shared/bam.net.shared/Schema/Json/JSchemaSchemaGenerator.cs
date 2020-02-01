using System;
using System.Collections.Generic;
using System.IO;
using Bam.Net.Data.Schema;
using Bam.Net.Schema.Json;
using Newtonsoft.Json;
using Newtonsoft.Json.Schema;

namespace Bam.Net.Application.Json
{
    public class JSchemaSchemaGenerator
    {
        public JSchemaSchemaGenerator(SchemaManager schemaManager, SerializationFormat serializationFormat, IJSchemaNameProvider jSchemaNameProvider = null)
        {
            SchemaManager = schemaManager;
            JSchemaLoader = JSchemaLoader.ForFormat(serializationFormat);
            JSchemaNameProvider = jSchemaNameProvider ?? new JSchemaNameProvider();
        }

        private static SchemaManager _schemaManager;
        private static readonly object _schemaManagerLock = new object();
        public static SchemaManager DefaultSchemaManager
        {
            get { return _schemaManagerLock.DoubleCheckLock(ref _schemaManager, () => new SchemaManager()); }
            set => _schemaManager = value;
        }

        public SchemaManager SchemaManager { get; set; }
        public JSchemaLoader JSchemaLoader { get; set; }
        public IJSchemaNameProvider JSchemaNameProvider { get; set; }

        public SchemaDefinition GenerateSchemaDefinition(JSchema rootSchema)
        {
            AddJSchema(SchemaManager.CurrentSchema, rootSchema);
            throw new NotImplementedException("This implementation is not complete");
            return SchemaManager.CurrentSchema;
        }

        public void AddJSchema(SchemaDefinition schemaDefinition, JSchema rootSchema)
        {
            SchemaManager.CurrentSchema = schemaDefinition;
            string rootTableName = JSchemaNameProvider.GetTableName(rootSchema);
            SchemaManager.AddTable(rootTableName);
            string[] objectPropertyNames = JSchemaNameProvider.GetObjectPropertyNames(rootSchema);
            foreach (string objectPropertyName in objectPropertyNames)
            {
                JSchema propertySchema = JSchemaNameProvider.GetPropertySchema(rootSchema, objectPropertyName);
                string propertyTableName = JSchemaNameProvider.GetTableName(propertySchema);
                SchemaManager.AddTable(propertyTableName);
                
                string[] columnNames = JSchemaNameProvider.GetPropertyColumnNames(rootSchema, objectPropertyName);
                foreach (string columnName in columnNames)
                {
                    SchemaManager.AddColumn(propertyTableName, columnName);
                }
            }
        }
        
        /// <summary>
        /// Generate a SchemaDefinition from the specified json schema file. 
        /// </summary>
        /// <param name="jSchemaFilePath">The path to the json schema</param>
        /// <param name="serializationFormat">The format that the json schema is in; either yaml or json</param>
        /// <returns></returns>
        public static SchemaDefinition GenerateSchemaDefinition(string jSchemaFilePath, SerializationFormat serializationFormat, JSchemaNameProvider jSchemaNameProvider = null)
        {
            JSchema jSchema = JSchemaLoader.LoadJSchema(jSchemaFilePath, serializationFormat);
            return GenerateSchemaDefinition(jSchema, serializationFormat, jSchemaNameProvider);
        }

        public static SchemaDefinition GenerateSchemaDefinition(JSchema jSchema, SerializationFormat serializationFormat, JSchemaNameProvider jSchemaNameProvider)
        {
            JSchemaSchemaGenerator generator = new JSchemaSchemaGenerator(new SchemaManager(), serializationFormat)
            {
                JSchemaNameProvider = jSchemaNameProvider
            };
            return generator.GenerateSchemaDefinition(jSchema);
        }
    }
}