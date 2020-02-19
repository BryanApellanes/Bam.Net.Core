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
    }
}
