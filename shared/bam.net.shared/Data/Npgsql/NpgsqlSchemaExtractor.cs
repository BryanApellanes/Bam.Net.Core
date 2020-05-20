using System.Collections.Generic;
using System.Data;
using System.Linq;
using Bam.Net.Data.Npqsql;
using Bam.Net.Data.Schema;
using Npgsql;

namespace Bam.Net.Data.Npgsql
{
    public class NpgsqlSchemaExtractor : SchemaExtractor
    {
        public NpgsqlSchemaExtractor(NpgsqlDatabase database)
        {
            Database = database;
            TableCatalog = Database.Name;
            ConnectionString = database.ConnectionString;
            _keyColumns = new Dictionary<string, string>();
            _columnDefinitions = new Dictionary<string, Dictionary<string, DataRow>>();
            _foreignKeyDefinitions = new Dictionary<string, NpgsqlForeignKeyDescriptor[]>();
        }

        readonly Dictionary<string, Dictionary<string, DataRow>> _columnDefinitions;
        readonly Dictionary<string, NpgsqlForeignKeyDescriptor[]> _foreignKeyDefinitions;

        public string TableCatalog { get; set; }
        public string TableSchema { get; set; }
        
        public override DataTypes GetColumnDataType(string tableName, string columnName)
        {
            return TranslateDataType(GetColumnDbDataType(tableName, columnName));
        }

        public override string GetColumnDbDataType(string tableName, string columnName)
        {
            return GetTableColumnInfo(tableName, columnName, "data_type");
        }

        public override string GetColumnMaxLength(string tableName, string columnName)
        {
            return GetTableColumnInfo(tableName, columnName, "character_maximum_length");
        }

        public override string[] GetColumnNames(string tableName)
        {
            if (!_columnDefinitions.ContainsKey(tableName))
            {
                SetTableColumnInfo(tableName);
            }

            return _columnDefinitions[tableName].Keys.ToArray();
        }

        public override bool GetColumnNullable(string tableName, string columnName)
        {
            string nullable = GetTableColumnInfo(tableName, columnName, "is_nullable");
            return nullable.IsAffirmative();
        }

        public override ForeignKeyColumn[] GetForeignKeyColumns()
        {
            List<ForeignKeyColumn> results = new List<ForeignKeyColumn>();
            foreach (string tableName in GetTableNames())
            {
                foreach (NpgsqlForeignKeyDescriptor fk in GetForeignKeyInfo(tableName))
                {
                    results.Add(new ForeignKeyColumn
                    {
                        Name = fk.ColumnName, ReferencedKey = fk.ReferencedColumn, ReferencedTable = fk.ReferencedTable
                    });
                }
            }

            return results.ToArray();
        }

        Dictionary<string, string> _keyColumns;
        public override string GetKeyColumnName(string tableName)
        {
            if (!_keyColumns.ContainsKey(tableName))
            {
                string keyColumnQuery = $@"SELECT a.attname as ColumnName
FROM   pg_index i
JOIN   pg_attribute a ON a.attrelid = i.indrelid
                     AND a.attnum = ANY(i.indkey)
WHERE  i.indrelid = '{TableSchema}.{tableName}'::regclass
AND    i.indisprimary;";

                _keyColumns.AddMissing(tableName, Database.QuerySingleColumn<string>(keyColumnQuery).FirstOrDefault());
            }

            return _keyColumns[tableName];
        }

        public override string GetSchemaName()
        {
            return TableSchema;
        }

        string[] _tableNames;
        public override string[] GetTableNames()
        {
            if (_tableNames == null)
            {
                string tableQuery = $"select table_name from information_schema.tables where table_schema = '{TableSchema}' order by table_name asc";

                _tableNames = Database.QuerySingleColumn<string>(tableQuery).ToArray();
            }

            return _tableNames;
        }

        protected override void SetConnectionName(string connectionString)
        {
            NpgsqlConnectionStringBuilder conn = new NpgsqlConnectionStringBuilder(connectionString);
            Database.ConnectionName = conn["Database"].ToString();
        }
        
        protected internal DataTypes TranslateDataType(string sqlDataType)
        {
            string dataType = sqlDataType.ToLowerInvariant();
            switch (dataType)
            {
                case "integer":
                case "int":
                case "smallint":
                case "mediumint":
                    return DataTypes.Int;
                case "text":
                    return DataTypes.String;
                case "blob":
                    return DataTypes.ByteArray;
                case "real":
                    return DataTypes.Decimal;
                case "bigint":
                    return DataTypes.ULong;
                case "datetime":
                    return DataTypes.DateTime;
                case "varchar":
                    return DataTypes.String;
                case "boolean":
                    return DataTypes.Boolean;
                default:
                    return DataTypes.String;
            }
        }
        
        private void SetTableColumnInfo(string tableName)
        {
//            string columnTypeQuery = @"SELECT *
//FROM information_schema.columns
//WHERE table_schema = '{0}'
//  AND table_name   = '{1}'";

            SqlStringBuilder sql = Database.GetSqlStringBuilder();
            sql.Select("information_schema.columns", "*")
                .Where("table_schema", TableSchema)
                .And("table_name", tableName);

            DataTable data = Database.GetDataTable(sql);
            foreach (DataRow row in data.Rows)
            {
                if (!_columnDefinitions.ContainsKey(tableName))
                {
                    _columnDefinitions.AddMissing(tableName, new Dictionary<string, DataRow>());
                }

                string colName = row["column_name"]?.ToString();
                if (!_columnDefinitions[tableName].ContainsKey(colName))
                {
                    _columnDefinitions[tableName].Add(colName, row);
                }
            }
        }
        
        private string GetTableColumnInfo(string tableName, string columnName, string metaColumnName)
        {
            if (!_columnDefinitions.ContainsKey(tableName))
            {
                SetTableColumnInfo(tableName);
            }

            return _columnDefinitions[tableName][columnName][metaColumnName].ToString();
        }

        private NpgsqlForeignKeyDescriptor[] GetForeignKeyInfo(string tableName)
        {
            if (!_foreignKeyDefinitions.ContainsKey(tableName))
            {
                SetForeignKeyInfo(tableName);
            }

            return _foreignKeyDefinitions[tableName];
        }
        
        private void SetForeignKeyInfo(string tableName)
        {
            string fkQuery = $@"SELECT
    tc.table_name as TableName, 
    kcu.column_name as ColumnName, 
    ccu.table_name AS ReferencedTable,
    ccu.column_name AS ReferencedColumn 
FROM 
    information_schema.table_constraints AS tc 
    JOIN information_schema.key_column_usage AS kcu
      ON tc.constraint_name = kcu.constraint_name
      AND tc.table_schema = kcu.table_schema
    JOIN information_schema.constraint_column_usage AS ccu
      ON ccu.constraint_name = tc.constraint_name
      AND ccu.table_schema = tc.table_schema
WHERE
    tc.table_schema = '{TableSchema}' AND 
    tc.constraint_type = 'FOREIGN KEY' AND tc.table_name='{tableName}';";

            _foreignKeyDefinitions.AddMissing(tableName, Database.ExecuteReader<NpgsqlForeignKeyDescriptor>(fkQuery).ToArray());
        }
    }
}
