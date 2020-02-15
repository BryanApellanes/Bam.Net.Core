using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Bam.Net.Data.MsSql;
using Bam.Net.Data.MySql;
using Bam.Net.Data.Npgsql;
using Bam.Net.Data.SQLite;
using Bam.Net.Server;

namespace Bam.Net.Data
{
    public class DatabaseConfig
    {
        readonly Dictionary<RelationalDatabaseTypes, Func<Database>> _databaseTypes;

        public DatabaseConfig()
        {
            _databaseTypes = new Dictionary<RelationalDatabaseTypes, Func<Database>>()
            {
                {RelationalDatabaseTypes.SQLite, () => new SQLiteDatabase(ConnectionName)},
                {
                    RelationalDatabaseTypes.MsSql,
                    () => new MsSqlDatabase(ServerName, DatabaseName, ConnectionName,
                        Credentials.CopyAs<MsSqlCredentials>())
                },
                {
                    RelationalDatabaseTypes.Npgsql,
                    () => new NpgsqlDatabase(ServerName, DatabaseName, ConnectionName,
                        Credentials.CopyAs<NpgsqlCredentials>())
                },
                {
                    RelationalDatabaseTypes.MySql,
                    () => new MySqlDatabase(ServerName, DatabaseName, ConnectionName,
                        Credentials.CopyAs<MySqlCredentials>())
                }
            };
        }
        
        public string ConnectionName { get; set; }
        public string DatabaseName { get; set; }
        public string ServerName { get; set; }
        
        public DatabaseCredentials Credentials { get; set; }

        public RelationalDatabaseTypes DatabaseType { get; set; }
        
        public Database GetDatabase()
        {
            return _databaseTypes[DatabaseType]();
        }

        public T GetDatabase<T>() where T : Database
        {
            return (T) GetDatabase();
        }

        /// <summary>
        /// Load the database configs from ~/.bam/databaseconfigs.yaml
        /// </summary>
        /// <returns></returns>
        public static DatabaseConfig[] LoadProfileConfigs()
        {
            string databaseconfigsPath = Path.Combine(BamHome.Profile, $"{nameof(DatabaseConfig).Pluralize()}.yaml");
            DatabaseConfig[] result = new DatabaseConfig[]{};
            if (File.Exists(databaseconfigsPath))
            {
                result = LoadConfigs(databaseconfigsPath);
            }
            else
            {
                result.ToJsonFile(databaseconfigsPath);
            }

            return result;
        }

        public static DatabaseConfig[] LoadConfigs(string filePath = null)
        {
            filePath = filePath ?? $"{nameof(DatabaseConfig).Pluralize()}.yaml";
            return new FileInfo(filePath).FromYamlFile<DatabaseConfig[]>();
        }

        public static Database GetFirstDatabase(string filePath = null)
        {
            DatabaseConfig config = LoadConfigs(filePath).FirstOrDefault();
            return config?.GetDatabase();
        }
    }
}