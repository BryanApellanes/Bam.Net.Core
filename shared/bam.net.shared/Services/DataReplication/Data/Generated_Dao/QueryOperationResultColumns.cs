using System;
using System.Collections.Generic;
using System.Text;
using Bam.Net.Data;

namespace Bam.Net.Services.DataReplication.Data.Dao
{
    public class QueryOperationResultColumns: QueryFilter<QueryOperationResultColumns>, IFilterToken
    {
        public QueryOperationResultColumns() { }
        public QueryOperationResultColumns(string columnName)
            : base(columnName)
        { }
		
		public QueryOperationResultColumns KeyColumn
		{
			get
			{
				return new QueryOperationResultColumns("Id");
			}
		}	

        public QueryOperationResultColumns Id
        {
            get
            {
                return new QueryOperationResultColumns("Id");
            }
        }
        public QueryOperationResultColumns Uuid
        {
            get
            {
                return new QueryOperationResultColumns("Uuid");
            }
        }
        public QueryOperationResultColumns Cuid
        {
            get
            {
                return new QueryOperationResultColumns("Cuid");
            }
        }
        public QueryOperationResultColumns QueryOperationIdentifier
        {
            get
            {
                return new QueryOperationResultColumns("QueryOperationIdentifier");
            }
        }
        public QueryOperationResultColumns TypeName
        {
            get
            {
                return new QueryOperationResultColumns("TypeName");
            }
        }
        public QueryOperationResultColumns Data
        {
            get
            {
                return new QueryOperationResultColumns("Data");
            }
        }
        public QueryOperationResultColumns CreatedBy
        {
            get
            {
                return new QueryOperationResultColumns("CreatedBy");
            }
        }
        public QueryOperationResultColumns ModifiedBy
        {
            get
            {
                return new QueryOperationResultColumns("ModifiedBy");
            }
        }
        public QueryOperationResultColumns Modified
        {
            get
            {
                return new QueryOperationResultColumns("Modified");
            }
        }
        public QueryOperationResultColumns Deleted
        {
            get
            {
                return new QueryOperationResultColumns("Deleted");
            }
        }
        public QueryOperationResultColumns Created
        {
            get
            {
                return new QueryOperationResultColumns("Created");
            }
        }



		protected internal Type TableType
		{
			get
			{
				return typeof(QueryOperationResult);
			}
		}

		public string Operator { get; set; }

        public override string ToString()
        {
            return base.ColumnName;
        }
	}
}