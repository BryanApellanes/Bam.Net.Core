using System;
using System.Collections.Generic;
using Bam.Net.Data.MsSql;
using Bam.Net.Data.MySql;
using Bam.Net.Data.Npgsql;
using Bam.Net.Data.SQLite;

namespace Bam.Net.Data
{
    public class DatabaseConfig
    {
        Dictionary<RelationalDatabaseTypes, Func<Database>> _databaseTypes;

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
        
        public IDatabaseCredentials Credentials { get; set; }
        
        public RelationalDatabaseTypes DatabaseType { get; set; }
        
        public Database GetDatabase()
        {
            return _databaseTypes[DatabaseType]();
        }

        public T GetDatabase<T>() where T : Database
        {
            return (T) GetDatabase();
        }
    }
}