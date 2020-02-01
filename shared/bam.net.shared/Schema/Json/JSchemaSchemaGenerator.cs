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
            
        private void temp()
        {
            Dictionary<object, object> orgYaml = "/home/bryan/src/BamAppServices/_data/Vimly.Entity/v1/organization_v1.yaml".FromYamlFile() as Dictionary<object, object>;
            JSchema jSchema = JSchema.Parse(orgYaml.ToJson(), new FileSystemYamlJSchemaResolver("/home/bryan/src/BamAppServices/_data/Vimly.Entity/v1/"));
            //OutLine(jSchema.ToJson(), ConsoleColor.Blue);
            //OutLine(jSchema.ToString(), ConsoleColor.Cyan);
            
            // TODO: build dao schema 
            // read properties
            // read "javaType" property for class names
            // if type = "array" read items for type and setup foreign key
            // move this implementation to JsonSchemaYamlLoader
        }

        public SchemaDefinition GenerateSchemaDefinition(JSchema rootSchema)
        {
            AddJSchema(SchemaManager.CurrentSchema, rootSchema);
            return SchemaManager.CurrentSchema;
        }

        public void AddJSchema(SchemaDefinition schemaDefinition, JSchema rootSchema)
        {
            SchemaManager.CurrentSchema = schemaDefinition;
            
        }
        
        /// <summary>
        /// Generate a SchemaDefinition from the specified json schema file. 
        /// </summary>
        /// <param name="jSchemaFilePath">The path to the json schema</param>
        /// <param name="serializationFormat">The format that the json schema is in; either yaml or json</param>
        /// <returns></returns>
        public static SchemaDefinition GenerateSchemaDefinition(string jSchemaFilePath, SerializationFormat serializationFormat)
        {
            JSchema jSchema = JSchemaLoader.LoadJSchema(jSchemaFilePath, serializationFormat);
            JSchemaSchemaGenerator generator = new JSchemaSchemaGenerator(new SchemaManager(), serializationFormat);
            return generator.GenerateSchemaDefinition(jSchema);
        }
    }
}