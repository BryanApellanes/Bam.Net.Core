/*
	Copyright © Bryan Apellanes 2015  
*/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Bam.Net;
using Bam.Net.Data;

namespace Bam.Net.Data.Schema
{
    /// <summary>
    /// A column that represents a foreign key
    /// </summary>
    public partial class ForeignKeyColumn: Column
    {
        /// <summary>
        /// Empty constructor provided for deserialization
        /// </summary>
        public ForeignKeyColumn()
        {
            this.ReferencedTable = string.Empty;
            this.DbDataType = string.Empty;
        }
        
        /// <summary>
        /// Instantiate a new ForeignKeyColumn based on the specified column
        /// referencing the specified referencedTable
        /// </summary>
        /// <param name="column"></param>
        /// <param name="referencedTable"></param>
        public ForeignKeyColumn(Column column, string referencedTable)
            : base(column.TableName)
        {
            this.AllowNull = column.AllowNull;
            this.Key = column.Key;
            this.Name = column.Name;
            this.DataType = column.DataType;
            this.ReferencedTable = referencedTable;
            this.DbDataType = column.DbDataType;
        }

        /// <summary>
        /// Instantiate a new ForeignKeyColumn with the specified name
        /// for the specified tableName referencing the specified referencedTable
        /// </summary>
        /// <param name="name"></param>
        /// <param name="tableName"></param>
        /// <param name="referencedTable"></param>
        public ForeignKeyColumn(string name, string tableName, string referencedTable)
            : this(new Column(name, tableName), referencedTable)
        {
        }

        public override DataTypes DataType
        {
            get => DataTypes.ULong;
            set
            {
                // always ULong
            }
        }

        string referenceName;
        public string ReferenceName
        {
            get
            {
                if (string.IsNullOrWhiteSpace(referenceName))
                {
                    return $"FK_{this.TableName}_{ReferencedTable}";
                }
                else
                {
                    return referenceName;
                }
            }
            set => referenceName = value;
        }

        public string ReferenceNameSuffix
        {
            get;
            set;
        }

        public string ReferencedKey { get; set; }
        
        public string ReferencedTable { get; set; }

        string _referencedClass;
        public string ReferencedClass
        {
            get => string.IsNullOrEmpty(_referencedClass) ? ReferencedTable.PascalCase(true, " ", "_").TrimNonLetters() : _referencedClass;
            set => _referencedClass = value;
        }

        string _referencingClass;
        public string ReferencingClass
        {
            get => string.IsNullOrEmpty(_referencingClass) ? TableName.PascalCase(true, " ", "_").TrimNonLetters() : _referencingClass;
            set => _referencingClass = value;
        }

        public override string ToString()
        {
            return this.ReferenceName;
        }

    }
}
