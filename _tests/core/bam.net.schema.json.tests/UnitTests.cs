using System;
using System.Collections.Generic;
using Bam.Net.Application.Json;
using Bam.Net.Data.Schema;
using Bam.Net.Testing;
using Bam.Net.Testing.Unit;
using Newtonsoft.Json.Schema;

namespace Bam.Net.Schema.Json.Tests
{
    [Serializable]
    public class UnitTests : CommandLineTestInterface
    {
        private UnixPath DataPath => new UnixPath("~/_data/JsonSchema/organization_v1.yaml");
        
        [UnitTest]
        public void CanResolveUnixPath()
        {
            UnixPath path = new UnixPath("~/src");
            path.Resolve().StartsWith("~").IsFalse();
            path.Path.StartsWith("~/").IsTrue();
            path.Resolve().StartsWith(BamPaths.UserHome);
            OutLineFormat("Unix path: {0}", ConsoleColor.Cyan, path.Resolve());
        }
        
        [UnitTest]
        [TestGroup("JSchema")]
        public void CanLoadJSchemaWithoutExceptions()
        {
            JSchema jSchema = JSchemaLoader.LoadJSchema(DataPath, SerializationFormat.Yaml);
            OutLine(jSchema.ToString(), ConsoleColor.Cyan);
        }

        [UnitTest]
        [TestGroup("JSchema")]
        public void CanGenerateSchemaDefinition()
        {
            SchemaDefinition schemaDefinition =  JSchemaSchemaGenerator.GenerateSchemaDefinition(DataPath, SerializationFormat.Yaml);
            OutLine(schemaDefinition.ToJson(true), ConsoleColor.Cyan);
        }

        [UnitTest]
        [TestGroup("JSchema")]
        public void CanGetRootTableName()
        {
            // check @type
            // check title
            // check javaType
            // check _tableNameProperties
            JSchema jSchema = JSchemaLoader.LoadYamlJSchema(DataPath);
            JSchemaNameProvider nameProvider = new JSchemaNameProvider("@type", "title", "javaType");
            string tableName = nameProvider.GetRootTableName(jSchema);
            Expect.IsNotNullOrEmpty(tableName);
            Expect.AreEqual("Organization", tableName);
        }

        [UnitTest]
        [TestGroup("JSchema")]
        public void CanGetRootClassName()
        {
            // check @type
            // check title
            // check javaType
            // check _tableNameProperties
            JSchema jSchema = JSchemaLoader.LoadYamlJSchema(DataPath);
            JSchemaNameProvider nameProvider = new JSchemaNameProvider("@type", "title", "javaType");
            string tableName = nameProvider.GetRootClassName(jSchema);
            Expect.IsNotNullOrEmpty(tableName);
            Expect.AreEqual("Organization", tableName);
        }

        [UnitTest]
        [TestGroup("JSchema")]
        public void CanGetColumnNames()
        {
            JSchema jSchema = JSchemaLoader.LoadYamlJSchema(DataPath);
            JSchemaNameProvider nameProvider = new JSchemaNameProvider("@type", "title", "javaType")
            {
                ParsePropertyNameForColumnNameFunction = propertyName => propertyName.PascalCase()
            };
            string[] columnNames = nameProvider.GetColumnNames(jSchema);
            HashSet<string> columnHashSet = new HashSet<string>(columnNames);
            columnHashSet.Contains("FriendlyId").IsTrue("FriendlyId not found");
            columnHashSet.Contains("WebsiteUrl").IsTrue("WebsiteUrl not found");
            
            columnHashSet.Contains("BusinessNameId").IsTrue("BusinessNameId not found");
            columnHashSet.Contains("TaxIdsId").IsTrue("TaxIdsId");
            columnHashSet.Contains("IndustryCodesId").IsTrue("IndustryCodesId");
        }
    }
}