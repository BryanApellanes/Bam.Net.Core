/*
	Copyright © Bryan Apellanes 2015  
*/

using System;
using System.Data;
using System.Data.Common;
using Bam.Net.Incubation;
using FirebirdSql.Data.FirebirdClient;

namespace Bam.Net.Data.FirebirdSql
{
    public class FirebirdSqlDatabase : Database, IHasConnectionStringResolver
    {
        public FirebirdSqlDatabase()
        {
            ConnectionStringResolver = DefaultConnectionStringResolver.Instance;
            Register();
        }
        public FirebirdSqlDatabase(string serverName, string databaseName, FirebirdSqlCredentials credentials = null)
            : this(serverName, databaseName, databaseName, credentials)
        { }

        public FirebirdSqlDatabase(string serverName, string databaseName, string connectionName, FirebirdSqlCredentials credentials = null)
        {
            ColumnNameProvider = (c) => "\"{0}\""._Format(c.Name);
            ConnectionStringResolver = new FirebirdSqlConnectionStringResolver(serverName, databaseName, credentials);
            ConnectionName = connectionName;
            Register();
        }

        public FirebirdSqlDatabase(string connectionString, string connectionName = null)
            : base(connectionString, connectionName)
        {
        }

        private void Register()
        {
            ServiceProvider = new Incubator();
            ServiceProvider.Set<DbProviderFactory>(FirebirdClientFactory.Instance);
            FirebirdSqlRegistrar.Register(this);
            Infos.Add(new DatabaseInfo(this));
        }

        public IConnectionStringResolver ConnectionStringResolver
        {
            get;
            set;
        }

        string _connectionString;
        public override string ConnectionString
        {
            get
            {
                if (string.IsNullOrEmpty(_connectionString))
                {
                    _connectionString = ConnectionStringResolver.Resolve(ConnectionName).ConnectionString;
                }

                return _connectionString;
            }
            set
            {
                _connectionString = value;
            }
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
    }
}
