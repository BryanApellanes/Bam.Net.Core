using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using Bam.Net;
using Bam.Net.CommandLine;
using Bam.Net.Data;
using Bam.Net.Data.MsSql;
using Bam.Net.Data.MySql;
using Bam.Net.Data.Npgsql;
using Bam.Net.Data.Schema;
using Bam.Net.Data.SQLite;
using Bam.Net.Logging;
using GraphQL.Types;
using Org.BouncyCastle.Crypto.Tls;
using KeyValuePair = Bam.Net.KeyValuePair;

namespace Bam.Shell.CodeGen
{
    public class SchemaExtractionProvider : CodeGenProvider
    {
        public SchemaExtractionProvider()
        {
        }

        public const string DefaultDaoConfigFile = "./.bamdb.daoconfigs";
        public const string DefaultOutput = "./_gen";
        
        public override void RegisterArguments(string[] args)
        {
            base.RegisterArguments(args);
            AddValidArgument("output", false, true, "Schema: The directory path to output generated files to.");
            AddValidArgument("config", false, false, "Schema: The file containing serialized DaoConfigs.");
            AddValidArgument("configName", false, false, "Schema | Dao: The name of the entry in the config file to use.");
            AddValidArgument("postgresSchema", false, false, "Schema: The name of the postgres table_schema to work with when extracting a schema from a postgres database.");
            
            AddValidArgument("namespace", false, true, "Dao: The namespace to place generated dao classes into.");
        }

        public override void Generate(Action<string> output = null, Action<string> error = null)
        {
            string writeTo = DefaultOutput;
            if (Arguments.Contains("output"))
            {
                writeTo = Arguments["output"];
                if (writeTo.StartsWith("~"))
                {
                    writeTo = Path.Combine(BamHome.UserHome, writeTo.TruncateFront(2));
                }
            }

            string srcDir = Path.Combine(writeTo, "src");
            
            DirectoryInfo outputDir = new DirectoryInfo(writeTo);

            SchemaExtractorInfo extractorInfo = GetExtractor(output, error);
            ISchemaExtractor extractor = extractorInfo.SchemaExtractor;
            ConsoleLogger logger = new ConsoleLogger {AddDetails = false};
            ((Loggable) extractor).Subscribe(logger);
            output("Beginning schema extraction");
            output(extractor.PropertiesToString());
            SchemaDefinition schema = extractor.Extract();
            schema.DbType = extractorInfo.DbType.ToString();
            string schemaFilePath = new FileInfo(Path.Combine(writeTo, $"{extractorInfo.DbType.ToString()}-{schema.Name}.schema")).FullName;
            string schemaNameMapFilePath = $"{schemaFilePath}.map";
            schema.Save(schemaFilePath);
            extractor.NameMap.ToJsonFile(schemaNameMapFilePath);
            output($"Extraction complete {schemaFilePath}");
        }
        
        protected SchemaExtractorInfo GetExtractor(Action<string> output = null, Action<string> error = null)
        {
            string configFile = GetConfigFile(error);

            DaoConfig config = GetConfig(output, error, configFile);

            ExtractionTargetDbTypes dbType = config.DbType;
            if (dbType == null || dbType == ExtractionTargetDbTypes.Invalid)
            {
                error($"Invalid dbType: {config.DbType}");
                Exit(1);
            }

            Database db = GetDatabase(dbType, config.ConnectionString);
            switch (dbType)
            {
                case ExtractionTargetDbTypes.MsSql:
                    return new SchemaExtractorInfo()
                    {
                        SchemaExtractor = new MsSqlSchemaExtractor((MsSqlDatabase) db),
                        DbType = dbType
                    };
                case ExtractionTargetDbTypes.MySql:
                    return new SchemaExtractorInfo
                    {
                        SchemaExtractor = new MySqlSchemaExtractor((MySqlDatabase) db),
                        DbType = dbType
                    };
                case ExtractionTargetDbTypes.Postgres:
                    string tableSchema = config.PostgresTableSchema;
                    if (string.IsNullOrWhiteSpace(tableSchema))
                    {
                        tableSchema = GetArgument("postgresSchema",
                            "Please enter the name of the postgres table_schema to work with");
                    }
                    return new SchemaExtractorInfo
                    {
                        SchemaExtractor = new NpgsqlSchemaExtractor((NpgsqlDatabase) db)
                        {
                            TableSchema =  tableSchema
                        },
                        DbType = dbType
                    };
                case ExtractionTargetDbTypes.Invalid:
                case ExtractionTargetDbTypes.SQLite:
                default:
                    return new SchemaExtractorInfo
                    {
                        SchemaExtractor = new SQLiteSchemaExtractor((SQLiteDatabase) db),
                        DbType = dbType
                    };
            }
        }

        private static DaoConfig GetConfig(Action<string> output, Action<string> error, string configFile)
        {
            List<DaoConfig> configs = configFile.FromJsonFile<List<DaoConfig>>();
            DaoConfig config = null;
            if (Arguments.Contains("configName"))
            {
                string specifiedConfigName = Arguments["configName"];
                config = configs.FirstOrDefault(c => c.Name.Equals(specifiedConfigName));
            }
            else if (configs.Count > 1)
            {
                config = SelectFrom(configs, conf => $"{conf.Name} {conf.DbType}");
            }
            else
            {
                config = configs.FirstOrDefault();
            }

            if (config == null && configs.Count > 0)
            {
                config = configs.FirstOrDefault();
            }

            if (config == null)
            {
                error("Unable to get DaoConfig");
                Exit(1);
            }

            return config;
        }

        private static string GetConfigFile(Action<string> error)
        {
            string configFile = DefaultDaoConfigFile;
            if (Arguments.Contains("config"))
            {
                configFile = Arguments["config"];
            }

            if(configFile.StartsWith("~/"))
            {
                configFile = Path.Combine(BamHome.UserHome, configFile.TruncateFront(2));
            }
            
            if (!File.Exists(configFile))
            {
                error($"Config file was not found: {configFile}");
                Exit(1);
            }

            return configFile;
        }

        protected Database GetDatabase(ExtractionTargetDbTypes dbType, string connectionString)
        {
            switch (dbType)
            {
                case ExtractionTargetDbTypes.MsSql:
                    return new MsSqlDatabase(connectionString);
                case ExtractionTargetDbTypes.MySql:
                    return new MySqlDatabase(connectionString);
                case ExtractionTargetDbTypes.Postgres:
                    return new NpgsqlDatabase(connectionString);
                case ExtractionTargetDbTypes.Invalid:
                case ExtractionTargetDbTypes.SQLite:
                default:
                    return SQLiteDatabase.FromConnectionString(connectionString);
            }
        }

    }
}