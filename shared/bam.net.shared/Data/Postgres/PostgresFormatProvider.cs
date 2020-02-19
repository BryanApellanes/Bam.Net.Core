/*
	Copyright © Bryan Apellanes 2015  
*/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Bam.Net.Data.Npgsql;

namespace Bam.Net.Data.Postgres
{
    /// <summary>
    /// Provides Npgsql specific expression formatting.
    /// It may make sense to put this class into the database.ServiceProvider
    /// container, especially when moving on to implement 
    /// support for other databases.  
    /// </summary>
    internal class PostgresFormatProvider: NpgsqlFormatProvider
    {
    }
}
