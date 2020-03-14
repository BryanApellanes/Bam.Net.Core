using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Bam.Net.Application.Json;
using Bam.Net.CommandLine;
using Bam.Net.Data.Schema;
using Bam.Net.Logging;
using Bam.Net.Testing;
using Bam.Net.Testing.Unit;
using MongoDB.Bson.Serialization.Serializers;
using Newtonsoft.Json.Schema;

namespace Bam.Net.Schema.Json.Tests
{
    [Serializable]
    public class UnitTests : CommandLineTestInterface
    {
        private UnixPath RootData = new UnixPath("~/_data/JsonSchema/");
        private UnixPath ApplicationSchema = new UnixPath("~/_data/JsonSchema/application_v1.yaml");
        private UnixPath CensusSchema = new UnixPath("~/_data/JsonSchema/census_v1.yaml");
        private UnixPath CommonSchema = new UnixPath("~/_data/JsonSchema/common_v1.yaml");
        private UnixPath OrganizationDataPath => new UnixPath(Path.Combine(RootData, "organization_v1.yaml"));
        private UnixPath CompanyDataPath => new UnixPath(Path.Combine(RootData, "company_v1.yaml"));

        [UnitTest]
        [TestGroup("JSchema")]
        public void GeneratedSchemaHasSubTypes()
        {
            ConsoleLogger logger = new ConsoleLogger(){AddDetails = false, UseColors = true};
            logger.StartLoggingThread();
            Log.Default = logger;
            
            JSchemaSchemaDefinitionGenerator generator = new JSchemaSchemaDefinitionGenerator(GetJSchemaManager<JavaJSchemaClassManager>());
            List<JSchema> schemas = generator.LoadJSchemas(ApplicationSchema);
            (schemas.Count > 0).IsTrue();
        }
        
        [UnitTest]
        [TestGroup("JSchema")]
        public void CanLoadCensusSchema()
        {
            ConsoleLogger logger = new ConsoleLogger(){AddDetails = false, UseColors = true};
            logger.StartLoggingThread();
            Log.Default = logger;
            
            JSchemaSchemaDefinitionGenerator generator = new JSchemaSchemaDefinitionGenerator(GetJSchemaManager<JavaJSchemaClassManager>());
            List<JSchema> schemas = generator.LoadJSchemas(CensusSchema);
            (schemas.Count > 0).IsTrue();
        }
        
        [UnitTest]
        [TestGroup("JSchema")]
        public void GetsSubTypesFromCommon()
        {
            ConsoleLogger logger = new ConsoleLogger(){AddDetails = false, UseColors = true};
            logger.StartLoggingThread();
            Log.Default = logger;
            
            JSchemaSchemaDefinitionGenerator generator = new JSchemaSchemaDefinitionGenerator(new JavaJSchemaClassManager());
            List<JSchema> schemas = generator.LoadJSchemas(CommonSchema);
            (schemas.Count > 0).IsTrue();
        }
        
        [UnitTest]
        [TestGroup("JSchema")]
        public void CanGenerateFromDirectory()
        {
            ConsoleLogger logger = new ConsoleLogger(){AddDetails = false, UseColors = true};
            logger.StartLoggingThread();
            Log.Default = logger;
            
            JSchemaSchemaDefinitionGenerator generator = new JSchemaSchemaDefinitionGenerator(new JavaJSchemaClassManager());
            JSchemaSchemaDefinition schemaDefinition = generator.GenerateSchemaDefinition(new DirectoryInfo(RootData), out List<JSchemaLoadResult> loadResults);
            
            OutLineFormat("Load result count {0}", loadResults.Count);
            OutLineFormat("Load success count {0}", loadResults.Count(lr => lr.Success));
            
            OutLine(schemaDefinition.ToJson(true), ConsoleColor.Cyan);
        }
        
        [UnitTest]
        [TestGroup("JSchema")]
        public void CanResolveUnixPath()
        {
            UnixPath path = new UnixPath("~/src");
            path.Resolve().StartsWith("~").IsFalse();
            path.Path.StartsWith("~/").IsTrue();
            path.Resolve().StartsWith(BamHome.UserHome);
            OutLineFormat("Unix path: {0}", ConsoleColor.Cyan, path.Resolve());
        }
        
        [UnitTest]
        [TestGroup("JSchema")]
        public void CanLoadJSchemaWithoutExceptions()
        {
            JSchema jSchema = JSchemaLoader.LoadJSchema(OrganizationDataPath, SerializationFormat.Yaml);
            OutLine(jSchema.ToString(), ConsoleColor.Cyan);
        }

        [UnitTest]
        [TestGroup("JSchema")]
        public void CanGetAllClassNames()
        {
            JSchema jSchema = GetOrganizationJSchema(out JSchemaClassManager jSchemaSchemaManager);
            string[] tableNames = jSchemaSchemaManager.GetAllClassNames(jSchema);
            (tableNames.Length > 0).IsTrue("No table names returned");
            HashSet<string> names = new HashSet<string>(tableNames);
            names.Contains("Organization").IsTrue("Organization wasn't included");
            names.Contains("BusinessName").IsTrue("BusinessName wasn't included");
            names.Contains("TaxIds").IsTrue("TaxIds wasn't included");
            names.Contains("IndustryCodes").IsTrue("IndustryCodes wasn't included");
        }

        [UnitTest]
        [TestGroup("JSchema")]
        public void CanGetClassManagerFromRegistry()
        {
            JSchemaManagementRegistry registry = new JSchemaManagementRegistry(RootData);
            JSchemaClassManager classManager = registry.Get<JSchemaClassManager>();
            Expect.IsNotNull(classManager.JSchemaResolver);
            (classManager.JSchemaResolver is FileSystemJSchemaResolver).IsTrue("unexpected resolver type");
            Expect.IsNotNull(classManager.JSchemaNameParser);
        }

        [UnitTest]
        [TestGroup("JSchema")]
        public void CanGetClassManagerWithNameParser()
        {
            JSchemaManagementRegistry registry = new JSchemaManagementRegistry(RootData);
            JSchemaClassManager classManager = registry.Get<JSchemaClassManager>();
            Expect.IsNotNull(classManager.JSchemaNameParser);
        }
        
        [UnitTest]
        [TestGroup("JSchema")]
        public void CanLoadDefinitionsFromCommon()
        {
             JSchemaManagementRegistry registry = new JSchemaManagementRegistry(RootData);
             JSchemaClassManager classManager = registry.Get<JSchemaClassManager>();
             JSchemaClass common = classManager.LoadJSchemaClass(new UnixPath("~/_data/JsonSchema/common_v1.yaml"));
             OutLine(common.ToJson(true));
             IEnumerable<JSchemaClass> definitions = JSchemaClass.FromDefinitions(common.JSchema, classManager);
             OutLine(definitions.ToJson(true), ConsoleColor.Yellow);
        }

        [UnitTest]
        [TestGroup("JSchema")]
        public void CanLoadJSchemaClassWithClassName()
        {
            JSchemaManagementRegistry registry = JSchemaManagementRegistry.ForJSchemasWithClassNamePropertiesOf(RootData, "@type", "class", "className");
            JSchemaClassManager classManager = registry.Get<JSchemaClassManager>();
            JSchemaClass app = classManager.LoadJSchemaClass(new UnixPath("~/_data/JsonSchema/application_v1.yaml"));
            Expect.IsNotNull(app);
            Expect.AreEqual("Application", app.ClassName);
        }
        
        [UnitTest]
        [TestGroup("JSchema")]
        public void CanGetPropertySchema()
        {
            JSchema root = GetOrganizationJSchema(out JSchemaClassManager jSchemaSchemaManager);
            JSchema jSchema = jSchemaSchemaManager.GetPropertySchema(root, "BusinessName");
            Expect.IsNotNull(jSchema, "schema not found");
        }

        [UnitTest]
        [TestGroup("JSchema")]
        public void CanGetRootClassName()
        {
            // check @type
            // check title
            // check javaType
            // check _tableNameProperties
            JSchema jSchema = JSchemaLoader.LoadYamlJSchema(OrganizationDataPath);
            JSchemaClassManager nameProvider = new JSchemaClassManager("@type", "title", "javaType");
            string tableName = nameProvider.GetObjectClassName(jSchema);
            Expect.IsNotNullOrEmpty(tableName);
            Expect.AreEqual("Organization", tableName);
        }

        [UnitTest]
        [TestGroup("JSchema")]
        public void CanGetPropertyNames()
        {
            JSchema jSchema = JSchemaLoader.LoadYamlJSchema(OrganizationDataPath);
            JSchemaClassManager jSchemaClassManager = new JSchemaClassManager("@type", "title", "javaType")
            {
                ParsePropertyName = propertyName => propertyName.PascalCase()
            };
            string[] propertyNames = jSchemaClassManager.GetPropertyNames(jSchema);
            HashSet<string> propertyNameHashSet = new HashSet<string>(propertyNames);
            propertyNameHashSet.Contains("FriendlyId").IsTrue("FriendlyId not found");
            propertyNameHashSet.Contains("WebsiteUrl").IsTrue("WebsiteUrl not found");
            
            propertyNameHashSet.Contains("BusinessNameId").IsTrue("BusinessNameId not found");
            propertyNameHashSet.Contains("TaxIdsId").IsTrue("TaxIdsId");
            propertyNameHashSet.Contains("IndustryCodesId").IsTrue("IndustryCodesId");
        }

        [UnitTest]
        [TestGroup("JSchema")]
        public void SameReferenceFromTwoSchemasAreEqual()
        {
            JSchema orgSchema = GetOrganizationJSchema(out JSchemaClassManager jSchemaManager);
            JSchema companySchema = GetCompanyJSchema(out JSchemaClassManager ignore);
            
            JSchema businessNameFromOrg = jSchemaManager.GetPropertySchema(orgSchema, "BusinessName");
            Expect.IsNotNull(businessNameFromOrg);
            Expect.IsNotNullOrEmpty(businessNameFromOrg.ToString());

            JSchema businessNameFromCompany = jSchemaManager.GetPropertySchema(companySchema, "BusinessName");
            Expect.IsNotNull(businessNameFromCompany);
            Expect.IsNotNullOrEmpty(businessNameFromCompany.ToString());
            
            Expect.AreEqual(businessNameFromOrg.ToString(), businessNameFromCompany.ToString());
            Expect.AreEqual(businessNameFromOrg.ToJson(), businessNameFromCompany.ToJson());
            Expect.AreEqual(businessNameFromOrg.ToJson().Sha256(), businessNameFromCompany.ToJson().Sha256());
        }

        [UnitTest]
        [TestGroup("JSchema")]
        public void CanGetArrayPropertyClassName()
        {
            JSchemaSchemaDefinitionGenerationSettings settings = GetGenerationSettings(nameof(CanGetArrayPropertyClassName), OrganizationDataPath);
            JSchemaClassManager jSchemaClassManager = settings.JSchemaClassManager;
            JSchema schema = settings.JSchemas.First();
            string className = jSchemaClassManager.GetArrayPropertyClassName(schema, "contacts");
            Expect.AreEqual("Contact", className);
        }

        [UnitTest]
        [TestGroup("JSchema")]
        public void AddJSchemaAddsColumns()
        {
            JSchemaSchemaDefinitionGenerationSettings settings = GetGenerationSettings(nameof(GenerateSchemaDefinitionTest), OrganizationDataPath, CompanyDataPath);
            JSchemaSchemaDefinitionGenerator generator = JSchemaSchemaDefinitionGenerator.Create(settings);
            JSchemaSchemaDefinition result = generator.GenerateSchemaDefinition(settings.JSchemas.ToArray());
            SchemaDefinition schemaDefinition = result.SchemaDefinition;
            Table orgTable = schemaDefinition.Tables.FirstOrDefault(t => t.Name.Equals("Organization"));
            Expect.IsNotNull(orgTable, "Organization table not found");
            Expect.AreEqual(5, orgTable.Columns.Length);
            OutLineFormat("Columns: \r\n{0}", orgTable.Columns.ToJson(true));
        }
        
        [UnitTest]
        [TestGroup("JSchema")]
        public void GenerateSchemaDefinitionTest()
        {
            JSchemaSchemaDefinitionGenerationSettings settings = GetGenerationSettings(nameof(GenerateSchemaDefinitionTest), OrganizationDataPath, CompanyDataPath);
            JSchemaSchemaDefinition schemaDefinition =  JSchemaSchemaDefinitionGenerator.GenerateSchemaDefinition(settings);
            
            OutLine(schemaDefinition.ToJson(true), ConsoleColor.Cyan);
        }

        private JSchema GetCompanyJSchema(out JSchemaClassManager jSchemaClassManager)
        {
            return GetJSchema(CompanyDataPath, out jSchemaClassManager);
        }
        
        private JSchema GetOrganizationJSchema(out JSchemaClassManager jSchemaClassManager)
        {
            return GetJSchema(OrganizationDataPath, out jSchemaClassManager);
        }
        
        private JSchema GetJSchema(string dataPath, out JSchemaClassManager jSchemaClassManager)
        {
            JSchema jSchema = JSchemaLoader.LoadYamlJSchema(dataPath);
            jSchemaClassManager = GetJSchemaManager();
            return jSchema;
        }

        private JSchemaSchemaDefinitionGenerationSettings GetGenerationSettings(string schemaName, params string[] dataPaths)
        {
            return new JSchemaSchemaDefinitionGenerationSettings()
            {
                Name = schemaName,
                JSchemas = dataPaths.Select(JSchemaLoader.LoadYamlJSchema).ToList(), 
                SerializationFormat = SerializationFormat.Yaml,
                JSchemaClassManager = GetJSchemaManager(),
                JSchemaResolver = GetJSchemaResolver()
            };
        }

        private JSchemaClassManager GetJSchemaManager<T>() where T : JSchemaClassManager, new()
        {
            T result = new T {JSchemaResolver = GetJSchemaResolver()};
            return result;
        }
        
        private JSchemaResolver GetJSchemaResolver()
        {
            return new FileSystemYamlJSchemaResolver(RootData)
            {
                JSchemaLoader = JSchemaLoader.ForFormat(SerializationFormat.Yaml)
            };
        }
        
        private JSchemaClassManager GetJSchemaManager()
        {
            return new JavaJSchemaClassManager();
        }
    }
}