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

        public Column Column { get; set; }

        public string TableClassName
        {
            get
            {
                return Column.TableClassName;
            }
        }

        public string PropertyName
        {
            get { return Column.PropertyName; }
        }

        public string Name
        {
            get
            {
                return Column.Name;
            }
        }

        public string DbDataType
        {
            get
            {
                return Column.DbDataType;
            }
        }

        public string MaxLength
        {
            get
            {
                return Column.MaxLength;
            }
        }

        public string AllowNull
        {
            get
            {
                return Column.AllowNull.ToString().ToLowerInvariant();
            }
        }

        public string NativeType
        {
            get
            {
                return Column.NativeType;
            }
        }

        public string DataType
        {
            get
            {
                return Column.DataType.ToString();
            }
        }
    }
}
