/*
	Copyright © Bryan Apellanes 2015  
*/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using System.Data.SqlClient;
using System.Data.Common;
using Bam.Net.Data;
using Bam.Net.Data.Npgsql;
using Npgsql;

namespace Bam.Net.Data.Postgres
{
	public class PostgresConnectionStringResolver : NpgsqlConnectionStringResolver
	{
		public PostgresConnectionStringResolver(string serverName, string databaseName, NpgsqlCredentials credentials = null) : base(serverName, databaseName, credentials)
		{
		}
	}
}
