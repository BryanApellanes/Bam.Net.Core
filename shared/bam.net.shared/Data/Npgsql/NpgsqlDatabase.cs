/*
	Copyright © Bryan Apellanes 2015  
*/

using System;
using System.Data;
using System.Data.Common;
using Bam.Net.Incubation;
using Npgsql;

namespace Bam.Net.Data.Npgsql
{
    public class NpgsqlDatabase : Database, IHasConnectionStringResolver
    {
        public NpgsqlDatabase()
        {
            ConnectionStringResolver = DefaultConnectionStringResolver.Instance;
            Register();
        }
        public NpgsqlDatabase(string serverName, string databaseName, NpgsqlCredentials credentials = null)
            : this(serverName, databaseName, databaseName, credentials)
        { }

        public NpgsqlDatabase(string serverName, string databaseName, string connectionName, NpgsqlCredentials credentials = null)
        {
            ColumnNameProvider = (c) => $"{c.Name}";
            ConnectionStringResolver = new NpgsqlConnectionStringResolver(serverName, databaseName.ToLowerInvariant(), credentials);
            ConnectionName = connectionName;
            Register();
        }

        public NpgsqlDatabase(string connectionString, string connectionName = null)
            : base(connectionString, connectionName)
        {
            Register();
        }

        public IConnectionStringResolver ConnectionStringResolver
        {
            get;
            set;
        }
        
        public override void ExecuteSql(string sqlStatement, System.Data.CommandType commandType, params DbParameter[] dbParameters)
        {
            string[] commands = sqlStatement.DelimitSplit("\r", "\n");
            foreach (string command in commands)
            {
                base.ExecuteSql(command, commandType, dbParameters);
            }
        }
        
        public static void Create(string serverName, string databaseName, NpgsqlCredentials credentials, int port = 5432)
        {
            NpgsqlDatabase db = new NpgsqlDatabase($"User Id={credentials.UserId}; Password={credentials.Password}; Host={serverName}; Port={port};");
            db.CreateDatabase(databaseName);
        }
        
        public void CreateDatabase(string databaseName)
        {
            DbConnection connection = GetDbConnection();
            ExecuteSql($"CREATE DATABASE {databaseName}");
        }
        
        public string PostgresSchema { get; set; }
        
        string _connectionString;
        public override string ConnectionString
        {
            get
            {
                if (string.IsNullOrEmpty(_connectionString))
                {
                    _connectionString = ConnectionStringResolver?.Resolve(ConnectionName)?.ConnectionString;
                }

                return _connectionString;
            }
            set => _connectionString = value;
        }

        public override long? GetLongValue(string columnName, DataRow row)
        {
            object value = row[columnName];
            if (value is long || value is long?)
            {
                return (long?)value;
            }
            else if (value is int || value is int?)
            {
                int d = (int)value;
                return Convert.ToInt64(d);
            }
            return null;
        }
        
        private void Register()
        {
            ServiceProvider = new Incubator();
            ServiceProvider.Set<DbProviderFactory>(NpgsqlFactory.Instance);
            NpgsqlRegistrar.Register(this);
            Infos.Add(new DatabaseInfo(this));
        }
    }
}
