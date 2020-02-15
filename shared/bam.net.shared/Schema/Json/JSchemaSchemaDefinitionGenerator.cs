using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Bam.Net.Data.Schema;
using Bam.Net.Logging;
using Bam.Net.Schema.Json;
using CsQuery.ExtensionMethods;
using CsQuery.ExtensionMethods.Internal;
using Newtonsoft.Json;
using Newtonsoft.Json.Schema;

namespace Bam.Net.Application.Json
{
    /// <summary>
    /// A class used to generate a SchemaDefinition from one or more JSchemas
    /// </summary>
    public class JSchemaSchemaDefinitionGenerator
    {
        public JSchemaSchemaDefinitionGenerator()
        {
            JSchemaManager = new JSchemaManager();
            DiscoveredEnums = new Dictionary<string, HashSet<string>>();
            SchemaManager = new SchemaManager();
            JSchemaLoader = JSchemaLoader.ForFormat(SerializationFormat.Json);
            Logger = Log.Default;
        }

        public JSchemaSchemaDefinitionGenerator(string schemaName):this()
        {
            SchemaManager = new SchemaManager(new SchemaDefinition(schemaName));
        }
        
        public JSchemaSchemaDefinitionGenerator(JSchemaSchemaDefinitionGenerationSettings settings) : this(new SchemaManager(new SchemaDefinition(settings.Name)), settings.SerializationFormat, settings.JSchemaManager)
        {
        }

        public JSchemaSchemaDefinitionGenerator(SchemaManager schemaManager, SerializationFormat serializationFormat, JSchemaManager jSchemaManager = null)
        {
            SchemaManager = schemaManager;
            JSchemaLoader = JSchemaLoader.ForFormat(serializationFormat);
            JSchemaManager = jSchemaManager ?? new JSchemaManager();
            DiscoveredEnums = new Dictionary<string, HashSet<string>>();
            Logger = Log.Default;
        }

        private static SchemaManager _schemaManager;
        private static readonly object _schemaManagerLock = new object();
        public static SchemaManager DefaultSchemaManager
        {
            get { return _schemaManagerLock.DoubleCheckLock(ref _schemaManager, () => new SchemaManager()); }
            set => _schemaManager = value;
        }
        
        public ILogger Logger { get; set; }
        public SchemaManager SchemaManager { get; set; }
        public JSchemaLoader JSchemaLoader { get; set; }
        public JSchemaManager JSchemaManager { get; set; }

        public Dictionary<string, HashSet<string>> DiscoveredEnums { get; set; }

        public void AddEnum(JSchema jSchema, string enumName, string[] enumValues)
        {
            if (!DiscoveredEnums.ContainsKey(enumName))
            {
                DiscoveredEnums.Add(enumName, new HashSet<string>());
            }
            DiscoveredEnums[enumName].AddRange(enumValues);
        }

        public JSchemaSchemaDefinition GenerateSchemaDefinition(string directoryPath)
        {
            return GenerateSchemaDefinition(new DirectoryInfo(directoryPath));
        }

        public JSchemaSchemaDefinition GenerateSchemaDefinition(DirectoryInfo jsonSchemaContainingFolder)
        {
            return GenerateSchemaDefinition(jsonSchemaContainingFolder, out List<JSchemaLoadResult> ignore);
        }
        
        public JSchemaSchemaDefinition GenerateSchemaDefinition(DirectoryInfo jsonSchemaContainingFolder, out List<JSchemaLoadResult> loadResults)
        {
            List<JSchema> jSchemas = LoadJSchemas(jsonSchemaContainingFolder, out loadResults);

            return GenerateSchemaDefinition(jSchemas.ToArray());
        }

        public List<JSchema> LoadJSchemas(DirectoryInfo jsonSchemaContainingFolder, out List<JSchemaLoadResult> loadResults)
        {
            FileInfo[] files = jsonSchemaContainingFolder.GetFiles();
            List<JSchema> jSchemas = new List<JSchema>();
            loadResults = new List<JSchemaLoadResult>();
            foreach (FileInfo file in files)
            {
                JSchemaLoader loader = null;
                if (file.Extension.Equals(".yaml", StringComparison.InvariantCultureIgnoreCase))
                {
                    loader = JSchemaLoader.ForFormat(SerializationFormat.Yaml);
                }
                else if (file.Extension.Equals(".json", StringComparison.InvariantCultureIgnoreCase))
                {
                    loader = JSchemaLoader.ForFormat(SerializationFormat.Json);
                }

                if (loader == null)
                {
                    Logger.Warning("No loader for file {0}", file.FullName);
                    continue;
                }

                try
                {
                    JSchema jSchema = loader.LoadSchema(file.FullName);
                    jSchemas.Add(jSchema);
                    loadResults.Add(new JSchemaLoadResult(file.FullName, jSchema));
                }
                catch (Exception ex)
                {
                    loadResults.Add(new JSchemaLoadResult(file.FullName, ex));
                }
            }

            return jSchemas;
        }

        public JSchemaSchemaDefinition GenerateSchemaDefinition(params JSchema[] schemas)
        {
            foreach (JSchema schema in schemas)
            {
                AddJSchema(SchemaManager.CurrentSchema, schema);
            }

            return new JSchemaSchemaDefinition()
            {
                JSchemas = schemas,
                DiscoveredEnums = DiscoveredEnums,
                SchemaDefinition = SchemaManager.CurrentSchema
            };
        }

        public void AddJSchema(SchemaDefinition schemaDefinition, JSchema schema)
        {
            SchemaManager.CurrentSchema = schemaDefinition;
            string className = JSchemaManager.GetObjectClassName(schema);
            string[] propertyNames = JSchemaManager.GetPropertyNames(schema);
            SchemaManager.AddTable(className);
            propertyNames.Each(pn => SchemaManager.AddColumn(className, pn));
                
            string[] objectPropertyNames = JSchemaManager.GetObjectPropertyNames(schema);
            foreach (string objectPropertyName in objectPropertyNames)
            {
                JSchema propertySchema = JSchemaManager.GetPropertySchema(schema, objectPropertyName);
                AddJSchema(schemaDefinition, propertySchema);
            }

            string[] arrayPropertyNames = JSchemaManager.GetArrayPropertyNames(schema);
            foreach (string arrayPropertyName in arrayPropertyNames)
            {
                JSchema arrayPropertySchema = JSchemaManager.GetPropertySchema(schema, arrayPropertyName);
                JSchema arrayItemSchema = JSchemaManager.GetArrayItemSchema(arrayPropertySchema);
                if (arrayItemSchema.IsObject())
                {
                    AddJSchema(schemaDefinition, arrayItemSchema);
                }
                else if (arrayItemSchema.IsEnum(out string[] enumValues))
                {
                    AddEnum(arrayItemSchema, arrayPropertyName.PascalCase(), enumValues);
                }
            }
        }

        public static JSchemaSchemaDefinition GenerateSchemaDefinition(JSchemaSchemaDefinitionGenerationSettings settings)
        {
            JSchemaSchemaDefinitionGenerator generator = Create(settings);
            return generator.GenerateSchemaDefinition(settings.JSchemas.ToArray());
        }

        public static JSchemaSchemaDefinitionGenerator Create(JSchemaSchemaDefinitionGenerationSettings settings)
        {
            SchemaManager schemaManager = new SchemaManager(new SchemaDefinition(settings.Name));
            JSchemaSchemaDefinitionGenerator generator = new JSchemaSchemaDefinitionGenerator(schemaManager, settings.SerializationFormat)
            {
                JSchemaManager = settings.JSchemaManager
            };
            return generator;
        }

        /// <summary>
        /// Generate a SchemaDefinition from the specified json schema file. 
        /// </summary>
        /// <param name="jSchemaFilePath">The path to the json schema</param>
        /// <param name="serializationFormat">The format that the json schema is in; either yaml or json</param>
        /// <returns></returns>
        public static JSchemaSchemaDefinition GenerateSchemaDefinition(string jSchemaFilePath, SerializationFormat serializationFormat, JSchemaManager jSchemaManager = null)
        {
            JSchema jSchema = JSchemaLoader.LoadJSchema(jSchemaFilePath, serializationFormat);
            return GenerateSchemaDefinition(jSchema, serializationFormat, jSchemaManager);
        }

        public static JSchemaSchemaDefinition GenerateSchemaDefinition(JSchema jSchema, SerializationFormat serializationFormat, JSchemaManager jSchemaNameProvider)
        {
            JSchemaSchemaDefinitionGenerator generator = new JSchemaSchemaDefinitionGenerator(new SchemaManager(), serializationFormat)
            {
                JSchemaManager = jSchemaNameProvider
            };
            return generator.GenerateSchemaDefinition(jSchema);
        }
    }
}