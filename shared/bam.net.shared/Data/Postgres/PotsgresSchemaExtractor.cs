using Bam.Net.Data.Npgsql;

namespace Bam.Net.Data.Postgres
{
    public class PostgresSchemaExtractor: NpgsqlSchemaExtractor
    {
        public PostgresSchemaExtractor(NpgsqlDatabase database) : base(database)
        {
        }
    }
}
