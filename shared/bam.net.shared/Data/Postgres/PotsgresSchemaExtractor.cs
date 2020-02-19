using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;
using Bam.Net.Data.Npgsql;
using Bam.Net.Data.Npqsql;
using Bam.Net.Data.Schema;
using GraphQL.Types;
using Npgsql;

namespace Bam.Net.Data.Postgres
{
    public class PostgresSchemaExtractor: NpgsqlSchemaExtractor
    {
        public PostgresSchemaExtractor(NpgsqlDatabase database) : base(database)
        {
        }
    }
}
