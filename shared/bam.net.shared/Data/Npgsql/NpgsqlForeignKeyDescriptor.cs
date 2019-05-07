namespace Bam.Net.Data.Npqsql
{
    public class NpgSqlForeignKeyDescriptor
    {
        public string TableName { get; set; }
        public string ColumnName { get; set; }
        public string ReferencedTable { get; set; }
        public string ReferencedColumn { get; set; }
    }
}