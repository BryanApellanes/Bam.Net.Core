using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Bam.Net.Data.Schema;
using Bam.Net.Logging;
using Bam.Net.Schema.Json;
using CsQuery.ExtensionMethods;
using CsQuery.ExtensionMethods.Internal;
using Microsoft.Extensions.DependencyInjection;
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
            JSchemaClassManager = new JSchemaClassManager();
            DiscoveredEnums = new Dictionary<string, HashSet<string>>();
            SchemaManager = new SchemaManager();
            JSchemaLoader = JSchemaLoader.ForFormat(SerializationFormat.Json);
            Logger = Log.Default;
        }

        public JSchemaSchemaDefinitionGenerator(JSchemaClassManager jSchemaClassManager) : this()
        {
            JSchemaClassManager = jSchemaClassManager;
        }

        public JSchemaSchemaDefinitionGenerator(string schemaName) : this()
        {
            SchemaManager = new SchemaManager(new SchemaDefinition(schemaName));
        }

        public JSchemaSchemaDefinitionGenerator(JSchemaSchemaDefinitionGenerationSettings settings) : this(new SchemaManager(new SchemaDefinition(settings.Name)), settings.SerializationFormat, settings.JSchemaClassManager, settings.JSchemaResolver)
        {
        }

        public JSchemaSchemaDefinitionGenerator(SchemaManager schemaManager, SerializationFormat serializationFormat, JSchemaClassManager jSchemaClassManager = null, JSchemaResolver resolver = null)
        {
            SchemaManager = schemaManager;
            JSchemaLoader = JSchemaLoader.ForFormat(serializationFormat);
            JSchemaClassManager = jSchemaClassManager ?? new JSchemaClassManager();
            JSchemaClassManager.JSchemaResolver = resolver ?? FileSystemJSchemaResolver.Default;
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
        public JSchemaClassManager JSchemaClassManager { get; set; }

        public Dictionary<string, HashSet<string>> DiscoveredEnums { get; set; }

        public void AddEnum(string enumName, JSchema enumProperty)
        {
            if (!DiscoveredEnums.ContainsKey(enumName))
            {
                DiscoveredEnums.Add(enumName, new HashSet<string>());
            }
            DiscoveredEnums[enumName].AddRange(JSchemaClassManager.GetEnumValues(enumProperty));
        }
        
        public JSchemaSchemaDefinition GenerateSchemaDefinition(string folderPathContainingJsonSchemas)
        {
            return GenerateSchemaDefinition(new DirectoryInfo(folderPathContainingJsonSchemas));
        }

        public JSchemaSchemaDefinition GenerateSchemaDefinition(DirectoryInfo folderContainingJsonSchemas)
        {
            return GenerateSchemaDefinition(folderContainingJsonSchemas, out List<JSchemaLoadResult> ignore);
        }
        
        public JSchemaSchemaDefinition GenerateSchemaDefinition(DirectoryInfo folderContainingJsonSchemas, out List<JSchemaLoadResult> loadResults)
        {
            List<JSchema> jSchemas = LoadJSchemas(folderContainingJsonSchemas, out loadResults);

            return GenerateSchemaDefinition(jSchemas.ToArray());
        }
        
        public List<JSchema> LoadJSchemas(DirectoryInfo folderContainingJsonSchemas, out List<JSchemaLoadResult> loadResults)
        {
            FileInfo[] files = folderContainingJsonSchemas.GetFiles();
            return LoadJSchemas(files, out loadResults);
        }

        public List<JSchema> LoadJSchemas(params string[] filePaths)
        {
            return LoadJSchemas(out List<JSchemaLoadResult> loadResults, filePaths);
        }
        
        public List<JSchema> LoadJSchemas(out List<JSchemaLoadResult> loadResults,  params string[] filePaths)
        {
            return LoadJSchemas(filePaths.Select(fp=> new FileInfo(fp)).ToArray(), out loadResults);
        }
        
        public List<JSchema> LoadJSchemas(FileInfo[] files, out List<JSchemaLoadResult> loadResults)
        {
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
                    // only add the jSchema if it is an object
                    if (jSchema.IsObject())
                    {
                        jSchemas.Add(jSchema);
                    }

                    //jSchemas.AddRange(AddSubJSchemas(loadResults, jSchema));
                }
                catch (Exception ex)
                {
                    loadResults.Add(new JSchemaLoadResult(file.FullName, ex));
                }
            }

            return jSchemas;
        }

        // public List<JSchema> AddSubJSchemas(List<JSchemaLoadResult> loadResults, JSchema jSchema)
        // {
        //     List<JSchema> subSchemas = JSchemaManager.ExtractDefinitionSchemas(jSchema).ToList();
        //     Queue<JSchema> subSchemaQueue = new Queue<JSchema>(subSchemas);
        //     while (subSchemaQueue.Count > 0)
        //     {
        //         JSchema subSchema = subSchemaQueue.Dequeue();
        //         try
        //         {
        //             if (subSchema.IsObject())
        //             {
        //                 subSchemas.Add(subSchema);
        //             }
        //             JSchemaManager.ExtractDefinitionSchemas(subSchema).Each(ss => subSchemaQueue.Enqueue(ss));
        //         }
        //         catch (Exception ex)
        //         {
        //             loadResults.Add(new JSchemaLoadResult(subSchema, ex));
        //         }
        //     }
        //
        //     // ensure we have unique JSchema instances
        //     HashSet<string> subSchemaJson = new HashSet<string>();
        //     subSchemas.Each(s => subSchemaJson.Add(s.ToJson()));
        //     return subSchemaJson.Select(JSchema.Parse).ToList();
        // }
        
        public JSchemaSchemaDefinition GenerateCombinedSchemaDefinition(SchemaDefinition schemaDefinition, params JSchema[] schemas)
        {
            JSchemaSchemaDefinition jSchemaSchemaDefinition = GenerateSchemaDefinition(schemas);
            return jSchemaSchemaDefinition.CombineWith(schemaDefinition);
        }
        
        public JSchemaSchemaDefinition GenerateSchemaDefinition(params JSchema[] schemas)
        {
            return GenerateSchemaDefinition("JSchemaSchemaDefinition", schemas);
        }
        
        public JSchemaSchemaDefinition GenerateSchemaDefinition(string schemaName, params JSchema[] schemas)
        {
            SchemaDefinition schemaDefinition = new SchemaDefinition(schemaName);
            
            foreach (JSchema schema in schemas)
            {
                AddJSchema(schemaDefinition, schema);
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
            string className = JSchemaClassManager.GetObjectClassName(schema);
            if (string.IsNullOrEmpty(className))
            {
                Log.Warn("Unable to determine class name for schema: \r\n{0}", schema.ToJson(true));
                return;
            }
            SchemaManager.CurrentSchema = schemaDefinition;

            string[] propertyNames = JSchemaClassManager.GetPropertyNames(schema);
            SchemaManager.AddTable(className);
            propertyNames.Each(pn => SchemaManager.AddColumn(className, pn));
                
            string[] objectPropertyNames = JSchemaClassManager.GetObjectPropertyNames(schema);
            foreach (string objectPropertyName in objectPropertyNames)
            {
                JSchema propertySchema = JSchemaClassManager.GetPropertySchema(schema, objectPropertyName);
                AddJSchema(schemaDefinition, propertySchema);
            }

            Dictionary<string, JSchema> enumProperties = JSchemaClassManager.GetEnumProperties(schema);
            foreach (string enumPropertyName in enumProperties.Keys)
            {
                AddEnum(enumPropertyName.PascalCase(), enumProperties[enumPropertyName]);
            }
            
            string[] arrayPropertyNames = JSchemaClassManager.GetArrayPropertyNames(schema);
            foreach (string arrayPropertyName in arrayPropertyNames)
            {
                JSchema arrayPropertySchema = JSchemaClassManager.GetPropertySchema(schema, arrayPropertyName);
                JSchema arrayItemSchema = JSchemaClassManager.GetArrayItemSchema(arrayPropertySchema);
                if (arrayItemSchema.IsObject())
                {
                    AddJSchema(schemaDefinition, arrayItemSchema);
                }
                else if (arrayItemSchema.IsEnum(out string[] enumValues))
                {
                    AddEnum(arrayPropertyName.PascalCase(), arrayItemSchema);
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
            JSchemaSchemaDefinitionGenerator generator = new JSchemaSchemaDefinitionGenerator(schemaManager, settings.SerializationFormat, settings.JSchemaClassManager, settings.JSchemaResolver)
            {
                JSchemaClassManager = settings.JSchemaClassManager
            };
            return generator;
        }

        /// <summary>
        /// Generate a SchemaDefinition from the specified json schema file. 
        /// </summary>
        /// <param name="jSchemaFilePath">The path to the json schema</param>
        /// <param name="serializationFormat">The format that the json schema is in; either yaml or json</param>
        /// <returns></returns>
        public static JSchemaSchemaDefinition GenerateSchemaDefinition(string jSchemaFilePath, SerializationFormat serializationFormat, JSchemaClassManager jSchemaClassManager = null)
        {
            JSchema jSchema = JSchemaLoader.LoadJSchema(jSchemaFilePath, serializationFormat);
            return GenerateSchemaDefinition(jSchema, serializationFormat, jSchemaClassManager);
        }

        public static JSchemaSchemaDefinition GenerateSchemaDefinition(JSchema jSchema, SerializationFormat serializationFormat, JSchemaClassManager jSchemaClassNameProvider)
        {
            JSchemaSchemaDefinitionGenerator generator = new JSchemaSchemaDefinitionGenerator(new SchemaManager(), serializationFormat)
            {
                JSchemaClassManager = jSchemaClassNameProvider
            };
            return generator.GenerateSchemaDefinition(jSchema);
        }
    }
}