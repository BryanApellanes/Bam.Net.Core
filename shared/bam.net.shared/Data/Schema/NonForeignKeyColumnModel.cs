using System;
using System.Collections.Generic;
using System.Text;

namespace Bam.Net.Data.Schema
{
    public class NonForeignKeyColumnModel
    {
        public NonForeignKeyColumnModel(Column column)
        {
            Column = column;
        }

        public bool Key => Column is KeyColumn;

        public Column Column { get; set; }

        public string TableClassName => Column.TableClassName;

        public string PropertyName => Column.PropertyName;

        public string Name => Column.Name;

        public string DbDataType => Column.DbDataType;

        public string MaxLength => Column.MaxLength;

        public string AllowNull => Column.AllowNull.ToString().ToLowerInvariant();

        public string NativeType => Column.NativeType;

        public string DataType => Column.DataType.ToString();
    }
}
