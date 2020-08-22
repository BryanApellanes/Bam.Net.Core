using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using Bam.Net.CommandLine;
using Bam.Net.Data;
using Bam.Net.Data.Schema;
using Bam.Net.Server;
using Bam.Net.Testing;
using Bam.Shell.CodeGen;
using Markdig.Helpers;

namespace Bam.Net.Application
{
    [Serializable]
    public class UtilityConsoleActions : CommandLineTool
    {
        private static SchemaDefinition _schemaDefinitionForAnalysis;
        private static object _schemaDefinitionForAnalysisLock = new object();

        private static SchemaDefinition GetSchemaDefinitionForAnalysis(string arg)
        {
            return _schemaDefinitionForAnalysisLock.DoubleCheckLock(ref _schemaDefinitionForAnalysis, () =>
            {
                string schemaPath = GetArgument(arg, true);
                if (schemaPath.StartsWith("~/"))
                {
                    schemaPath = Path.Combine(BamHome.UserHome, schemaPath.TruncateFront(2));
                }
                SchemaDefinition schemaDefinition = SchemaDefinition.Load(schemaPath);
                
                return schemaDefinition;
            });
        }
        
        [ConsoleAction("analyzeSchema", "[path to schema file]", "Analyze a schema file before generating code to check for silliness.  This is intended to clean a schema extracted from an existing database.")]
        public void AnalyzeSchema()
        {
            string schemaPath = GetArgument("analyzeSchema", true);
            if (string.IsNullOrEmpty(schemaPath))
            {
                Warn("Please specify the path to the schema file: bamdb /analyzeSchema:[path_to_schema_file]");
                Exit(1);
            }

            SchemaDefinition schema = GetSchemaDefinitionForAnalysis("analyzeSchema");
            Dictionary<string, List<Table>> duplicateClassNames = GetTablesWithDuplicateClassNames(schema);
            foreach (string className in duplicateClassNames.Keys)
            {
                StringBuilder stringBuilder = new StringBuilder();
                stringBuilder.AppendFormat("ClassName: {0}\r\n", duplicateClassNames[className].First().ClassName);
                duplicateClassNames[className].Each(table => stringBuilder.AppendFormat("\tTableName: {0}\r\n", table.Name));
                OutLine(stringBuilder.ToString());
            }
            Thread.Sleep(300);
        }

        [ConsoleAction("fixSchemaMap", "When analyzing a schema, fix known issues.")]
        public void FixSchemaMap()
        {
            SchemaDefinition schema = GetSchemaDefinitionForAnalysis("fixSchemaMap");
            SchemaNameMap map = SchemaNameMap.Load($"{schema.File}.map");
            SchemaNameMap fixedMap = new SchemaNameMap();
            MappedSchemaDefinition mappedSchemaDefinition = new MappedSchemaDefinition(schema, map);
            Dictionary<string, List<Table>> duplicateClassNames = GetTablesWithDuplicateClassNames(schema);
            foreach (string className in duplicateClassNames.Keys)
            {
                for (int i = 1; i < duplicateClassNames[className].Count; i++)
                {
                    Table table = duplicateClassNames[className][i];
                    string newClassName = $"{duplicateClassNames[className][i].ClassName}_{i}";

                    TableNameToClassName mapping = map.TableNamesToClassNames.FirstOrDefault(m => m.TableName.Equals(table.Name));
                    mapping.ClassName = newClassName;
                    fixedMap.Set(mapping);
                }
            }

            schema.Tables.Where(t => t.ClassName[0].IsNumber()).Each(table =>
            {
                fixedMap.Set(new TableNameToClassName {TableName = table.Name, ClassName = $"_{table.ClassName}"});
            });            

            fixedMap.ToJsonFile($"{schema.File}.map.fixed");
            Thread.Sleep(300);
        }

        [ConsoleAction("initDaoConfig", "write a dao config to the file system.")]
        public void InitDaoConfig()
        {
            DaoConfig first = new DaoConfig
            {
                TemplatePath = "Optional path to custom templates",
                Name = "A logical name used to refer to this config",
                ConnectionString = "Database connection string",
                PostgresTableSchema = "The table schema",
                DbType = ExtractionTargetDbTypes.SQLite
            };
            DaoConfig second = new DaoConfig
            {
                TemplatePath = "Optional path to custom templates",
                Name = "A logical name used to refer to this config",
                ConnectionString = "Database connection string",
                PostgresTableSchema = "The table schema",
                DbType = ExtractionTargetDbTypes.SQLite
            };

            string outputDirectory = string.IsNullOrEmpty(Arguments["initDaoConfig"]) ? "." : Arguments["initDaoConfig"];
            string outputPath = new DirectoryInfo(Path.Combine(outputDirectory, "DaoConfigs.yaml")).FullName;
            new DaoConfig[]{first, second}.ToYamlFile(outputPath);
            OutLineFormat("Wrote config file {0}", outputPath);
            Thread.Sleep(300);
        }

        [ConsoleAction("daoConfigFromDatabaseConfig", "write a dao config to the file system given a databaseconfig")]
        public void ConvertDatabaseConfigToDaoConfig()
        {
            string configPath = GetPathArgument("config", "Please enter the path to the databaseconfig");
            DatabaseConfig[] databaseConfigs = DatabaseConfig.LoadConfigs(configPath);
            DatabaseConfig databaseConfig = null; 
            if (Arguments.Contains("name"))
            {
                databaseConfig = databaseConfigs.FirstOrDefault(c => c.ConnectionName.Equals(Arguments["name"]));
            }
            else
            {
                databaseConfig = SelectFrom(databaseConfigs, c => c.ConnectionName);
            }

            string outputPath = new DirectoryInfo(Path.Combine(".", "DaoConfigs.yaml")).FullName;
            DaoConfig.FromDatabaseConfig(databaseConfig).ToYamlFile(outputPath);
            OutLineFormat("Wrote file {0}", ConsoleColor.Cyan, outputPath);
            Thread.Sleep(300);
        }
        
        private Dictionary<string, List<Table>> GetTablesWithDuplicateClassNames(SchemaDefinition schemaDefinition)
        {
            Dictionary<string, List<Table>> all = new Dictionary<string, List<Table>>();
            foreach (Table table in schemaDefinition.Tables)
            {
                string lowerCaseClassName = table.ClassName.ToLowerInvariant();
                if (!all.ContainsKey(lowerCaseClassName))
                {
                    all.Add(lowerCaseClassName, new List<Table>());
                }

                all[lowerCaseClassName].Add(table);
            }

            Dictionary<string, List<Table>> results = new Dictionary<string, List<Table>>();
            foreach (string className in all.Keys)
            {
                if (all[className].Count > 1)
                {
                    results.Add(className, all[className]);
                }
            }

            return results;
        }
    }
}