using System;
using System.Collections.Generic;
using System.Text;

namespace Bam.Net.Data.Schema
{
    public class ForeignKeyColumnModel
    {
        public ForeignKeyColumnModel(ForeignKeyColumn fk, string nameSpace)
        {
            ForeignKeyColumn = fk;
            Namespace = nameSpace;
        }

        public ForeignKeyColumn ForeignKeyColumn { get; set; }

        public string Namespace { get; set; }

        public string Name { get { return ForeignKeyColumn.Name; } }
        public string TableName { get { return ForeignKeyColumn.TableName; } }
        public string DbDataType { get { return ForeignKeyColumn.DbDataType; } }
        public string MaxLength { get { return ForeignKeyColumn.MaxLength; } }
        public string AllowNull { get { return ForeignKeyColumn.AllowNull.ToString().ToLowerInvariant(); } }

        public string ReferencedKey { get { return ForeignKeyColumn.ReferencedKey; } }
        public string ReferencedTable { get { return ForeignKeyColumn.ReferencedTable; } }
        public string ReferenceNameSuffix { get { return ForeignKeyColumn.ReferenceNameSuffix; } }
        public string NativeType { get { return ForeignKeyColumn.NativeType; } }
        public string PropertyName { get { return ForeignKeyColumn.PropertyName; } }
        public string DataType { get { return ForeignKeyColumn.DataType.ToString(); } }
        public string ReferencedClass { get { return ForeignKeyColumn.ReferencedClass; } }
        public string CamelCaseReferencedClass { get { return ForeignKeyColumn.ReferencedClass.CamelCase(); } }
    }
}
