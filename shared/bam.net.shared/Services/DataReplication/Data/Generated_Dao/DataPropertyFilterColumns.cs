using System;
using System.Collections.Generic;
using System.Text;
using Bam.Net.Data;

namespace Bam.Net.Services.DataReplication.Data.Dao
{
    public class DataPropertyFilterColumns: QueryFilter<DataPropertyFilterColumns>, IFilterToken
    {
        public DataPropertyFilterColumns() { }
        public DataPropertyFilterColumns(string columnName)
            : base(columnName)
        { }
		
		public DataPropertyFilterColumns KeyColumn
		{
			get
			{
				return new DataPropertyFilterColumns("Id");
			}
		}	

        public DataPropertyFilterColumns Id
        {
            get
            {
                return new DataPropertyFilterColumns("Id");
            }
        }
        public DataPropertyFilterColumns Uuid
        {
            get
            {
                return new DataPropertyFilterColumns("Uuid");
            }
        }
        public DataPropertyFilterColumns Cuid
        {
            get
            {
                return new DataPropertyFilterColumns("Cuid");
            }
        }
        public DataPropertyFilterColumns TypeNamespace
        {
            get
            {
                return new DataPropertyFilterColumns("TypeNamespace");
            }
        }
        public DataPropertyFilterColumns TypeName
        {
            get
            {
                return new DataPropertyFilterColumns("TypeName");
            }
        }
        public DataPropertyFilterColumns InstanceIdentifier
        {
            get
            {
                return new DataPropertyFilterColumns("InstanceIdentifier");
            }
        }
        public DataPropertyFilterColumns Name
        {
            get
            {
                return new DataPropertyFilterColumns("Name");
            }
        }
        public DataPropertyFilterColumns CreateOperationId
        {
            get
            {
                return new DataPropertyFilterColumns("CreateOperationId");
            }
        }
        public DataPropertyFilterColumns DeleteEventId
        {
            get
            {
                return new DataPropertyFilterColumns("DeleteEventId");
            }
        }
        public DataPropertyFilterColumns DeleteOperationId
        {
            get
            {
                return new DataPropertyFilterColumns("DeleteOperationId");
            }
        }
        public DataPropertyFilterColumns UpdateOperationId
        {
            get
            {
                return new DataPropertyFilterColumns("UpdateOperationId");
            }
        }
        public DataPropertyFilterColumns WriteEventId
        {
            get
            {
                return new DataPropertyFilterColumns("WriteEventId");
            }
        }
        public DataPropertyFilterColumns SaveOperationId
        {
            get
            {
                return new DataPropertyFilterColumns("SaveOperationId");
            }
        }
        public DataPropertyFilterColumns Key
        {
            get
            {
                return new DataPropertyFilterColumns("Key");
            }
        }
        public DataPropertyFilterColumns CompositeKeyId
        {
            get
            {
                return new DataPropertyFilterColumns("CompositeKeyId");
            }
        }
        public DataPropertyFilterColumns CompositeKey
        {
            get
            {
                return new DataPropertyFilterColumns("CompositeKey");
            }
        }
        public DataPropertyFilterColumns CreatedBy
        {
            get
            {
                return new DataPropertyFilterColumns("CreatedBy");
            }
        }
        public DataPropertyFilterColumns ModifiedBy
        {
            get
            {
                return new DataPropertyFilterColumns("ModifiedBy");
            }
        }
        public DataPropertyFilterColumns Modified
        {
            get
            {
                return new DataPropertyFilterColumns("Modified");
            }
        }
        public DataPropertyFilterColumns Deleted
        {
            get
            {
                return new DataPropertyFilterColumns("Deleted");
            }
        }
        public DataPropertyFilterColumns Created
        {
            get
            {
                return new DataPropertyFilterColumns("Created");
            }
        }


        public DataPropertyFilterColumns QueryOperationId
        {
            get
            {
                return new DataPropertyFilterColumns("QueryOperationId");
            }
        }

		protected internal Type TableType
		{
			get
			{
				return typeof(DataPropertyFilter);
			}
		}

		public string Operator { get; set; }

        public override string ToString()
        {
            return base.ColumnName;
        }
	}
}