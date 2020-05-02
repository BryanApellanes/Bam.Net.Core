/*
	Copyright © Bryan Apellanes 2015  
*/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Bam.Net.Incubation;
using System.Data.Common;
using System.Data.SqlClient;
using Bam.Net.Data;
using Bam.Net.Data.Npgsql;
using Bam.Net.Data.Postgres;
using Npgsql;

namespace Bam.Net.Data.Posgres
{
    public class PostgresDatabase : NpgsqlDatabase
    {
        public PostgresDatabase() : base()
        {
        }

        public PostgresDatabase(string serverName, string databaseName, NpgsqlCredentials credentials = null)
            : base(serverName, databaseName, databaseName, credentials)
        { }

        public PostgresDatabase(string serverName, string databaseName, string connectionName, NpgsqlCredentials credentials = null)
            : base(serverName, databaseName, connectionName, credentials)
        {
        }

        public PostgresDatabase(string connectionString, string connectionName = null)
            : base(connectionString, connectionName)
        {
        }

        public static bool TryCreate(string serverName, string databaseName, PostgresCredentials credentials, int port = 5432)
        {
            try
            {
                Create(serverName, databaseName, credentials, port);
                return true;
            }
            catch (Exception ex)
            {
                Logging.Log.Error("Error creating Postgres database ({0}) on server ({1}): {2}", databaseName, serverName, ex.Message);
                return false;
            }
        }
        
        public static void Create(string serverName, string databaseName, PostgresCredentials credentials, int port = 5432)
        {
            NpgsqlDatabase.Create(serverName, databaseName, credentials, port);
        }
    }
}
