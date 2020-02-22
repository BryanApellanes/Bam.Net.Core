using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
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
        private UnixPath OrganizationDataPath => new UnixPath(Path.Combine(RootData, "organization_v1.yaml"));
        private UnixPath CompanyDataPath => new UnixPath(Path.Combine(RootData, "company_v1.yaml"));

        [UnitTest]
        [TestGroup("JSchema")]
        public void GeneratedSchemaHasSubTypes()
        {
            ConsoleLogger logger = new ConsoleLogger(){AddDetails = false, UseColors = true};
            logger.StartLoggingThread();
            Log.Default = logger;
            
            JSchemaSchemaDefinitionGenerator generator = new JSchemaSchemaDefinitionGenerator(new JavaJSchemaManager());
            List<JSchema> schemas = generator.LoadJSchemas(ApplicationSchema);
            (schemas.Count > 0).IsTrue();
        }
        
        [UnitTest]
        [TestGroup("JSchema")]
        public void CanGenerateFromDirectory()
        {
            ConsoleLogger logger = new ConsoleLogger(){AddDetails = false, UseColors = true};
            logger.StartLoggingThread();
            Log.Default = logger;
            
            JSchemaSchemaDefinitionGenerator generator = new JSchemaSchemaDefinitionGenerator(new JavaJSchemaManager());
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
        public void CanGetAllTableNames()
        {
            JSchema jSchema = GetOrganizationJSchema(out JSchemaManager jSchemaSchemaManager);
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
        public void CanGetPropertySchema()
        {
            JSchema root = GetOrganizationJSchema(out JSchemaManager jSchemaSchemaManager);
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
            JSchemaManager nameProvider = new JSchemaManager("@type", "title", "javaType");
            string tableName = nameProvider.GetObjectClassName(jSchema);
            Expect.IsNotNullOrEmpty(tableName);
            Expect.AreEqual("Organization", tableName);
        }

        [UnitTest]
        [TestGroup("JSchema")]
        public void CanGetPropertyNames()
        {
            JSchema jSchema = JSchemaLoader.LoadYamlJSchema(OrganizationDataPath);
            JSchemaManager jSchemaManager = new JSchemaManager("@type", "title", "javaType")
            {
                ParsePropertyNameFunction = propertyName => propertyName.PascalCase()
            };
            string[] propertyNames = jSchemaManager.GetPropertyNames(jSchema);
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
            JSchema orgSchema = GetOrganizationJSchema(out JSchemaManager jSchemaManager);
            JSchema companySchema = GetCompanyJSchema(out JSchemaManager ignore);
            
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
            JSchemaManager jSchemaManager = settings.JSchemaManager;
            JSchema schema = settings.JSchemas.First();
            string className = jSchemaManager.GetArrayPropertyClassName(schema, "contacts");
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


        [UnitTest]
        [TestGroup("JSchema")]
        public void CanLoadDirectory()
        {
            JSchemaSchemaDefinitionGenerator generator = new JSchemaSchemaDefinitionGenerator();
            generator.LoadJSchemas(new DirectoryInfo(RootData), out List<JSchemaLoadResult> loadResults);
            
            OutLineFormat("Load result count {0}", ConsoleColor.Cyan, loadResults.Count);
            OutLineFormat("Load success count {0}", ConsoleColor.Green, loadResults.Count(lr => lr.Success));
            OutLineFormat("Load error count {0}", ConsoleColor.Yellow, loadResults.Count(lr => !lr.Success));
            OutLineFormat("Errors: \r\n{0}", ConsoleColor.DarkYellow, loadResults.Where(lr=> !lr.Success).Select(lr=> $"{lr.Path}: {lr.Message}").ToArray().ToDelimited(v=> $"{v}\r\n"));
        }
        
        private JSchema GetCompanyJSchema(out JSchemaManager jSchemaManager)
        {
            return GetJSchema(CompanyDataPath, out jSchemaManager);
        }
        
        private JSchema GetOrganizationJSchema(out JSchemaManager jSchemaManager)
        {
            return GetJSchema(OrganizationDataPath, out jSchemaManager);
        }
        
        private JSchema GetJSchema(string dataPath, out JSchemaManager jSchemaManager)
        {
            JSchema jSchema = JSchemaLoader.LoadYamlJSchema(dataPath);
            jSchemaManager = GetJSchemaManager();
            return jSchema;
        }

        private JSchemaSchemaDefinitionGenerationSettings GetGenerationSettings(string schemaName, params string[] dataPaths)
        {
            return new JSchemaSchemaDefinitionGenerationSettings()
            {
                Name = schemaName,
                JSchemas = dataPaths.Select(JSchemaLoader.LoadYamlJSchema).ToList(), 
                SerializationFormat = SerializationFormat.Yaml,
                JSchemaManager = GetJSchemaManager(),
            };
        }

        private JSchemaManager GetJSchemaManager()
        {
            return new JavaJSchemaManager();
        }
    }
}