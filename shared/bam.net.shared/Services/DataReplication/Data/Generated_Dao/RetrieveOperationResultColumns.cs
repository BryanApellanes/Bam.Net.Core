using System;
using System.Collections.Generic;
using System.Text;
using Bam.Net.Data;

namespace Bam.Net.Services.DataReplication.Data.Dao
{
    public class RetrieveOperationResultColumns: QueryFilter<RetrieveOperationResultColumns>, IFilterToken
    {
        public RetrieveOperationResultColumns() { }
        public RetrieveOperationResultColumns(string columnName)
            : base(columnName)
        { }
		
		public RetrieveOperationResultColumns KeyColumn
		{
			get
			{
				return new RetrieveOperationResultColumns("Id");
			}
		}	

        public RetrieveOperationResultColumns Id
        {
            get
            {
                return new RetrieveOperationResultColumns("Id");
            }
        }
        public RetrieveOperationResultColumns Uuid
        {
            get
            {
                return new RetrieveOperationResultColumns("Uuid");
            }
        }
        public RetrieveOperationResultColumns Cuid
        {
            get
            {
                return new RetrieveOperationResultColumns("Cuid");
            }
        }
        public RetrieveOperationResultColumns RetrieveOperationIdentifier
        {
            get
            {
                return new RetrieveOperationResultColumns("RetrieveOperationIdentifier");
            }
        }
        public RetrieveOperationResultColumns TypeName
        {
            get
            {
                return new RetrieveOperationResultColumns("TypeName");
            }
        }
        public RetrieveOperationResultColumns Data
        {
            get
            {
                return new RetrieveOperationResultColumns("Data");
            }
        }
        public RetrieveOperationResultColumns CreatedBy
        {
            get
            {
                return new RetrieveOperationResultColumns("CreatedBy");
            }
        }
        public RetrieveOperationResultColumns ModifiedBy
        {
            get
            {
                return new RetrieveOperationResultColumns("ModifiedBy");
            }
        }
        public RetrieveOperationResultColumns Modified
        {
            get
            {
                return new RetrieveOperationResultColumns("Modified");
            }
        }
        public RetrieveOperationResultColumns Deleted
        {
            get
            {
                return new RetrieveOperationResultColumns("Deleted");
            }
        }
        public RetrieveOperationResultColumns Created
        {
            get
            {
                return new RetrieveOperationResultColumns("Created");
            }
        }



		protected internal Type TableType
		{
			get
			{
				return typeof(RetrieveOperationResult);
			}
		}

		public string Operator { get; set; }

        public override string ToString()
        {
            return base.ColumnName;
        }
	}
}