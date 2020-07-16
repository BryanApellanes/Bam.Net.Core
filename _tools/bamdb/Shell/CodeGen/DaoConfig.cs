using System;
using System.IO;
using Bam.Net;
using Bam.Net.Data;

namespace Bam.Shell.CodeGen
{
    [Serializable]
    public class DaoConfig
    {
        public DaoConfig()
        {
            TemplatePath = Path.Combine(AppPaths.Data, "Templates");
        }
        
        public string TemplatePath { get; set; }
        public string Name { get; set; }
        public string ConnectionString { get; set; }
        public string PostgresTableSchema { get; set; }
        
        public ExtractionTargetDbTypes DbType { get; set; }

        public static DaoConfig FromDatabaseConfig(DatabaseConfig databaseConfig, string postgresTableSchema = "")
        {
            return new DaoConfig
            {
                TemplatePath = string.Empty,
                Name = databaseConfig.ConnectionName,
                ConnectionString = databaseConfig.GetDatabase().ConnectionString,
                PostgresTableSchema = postgresTableSchema
            };
        }
    }
}