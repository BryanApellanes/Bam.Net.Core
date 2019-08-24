using System;
using System.Collections.Generic;
using System.Text;
using Bam.Net.Data;

namespace Bam.Net.Services.DataReplication.Data.Dao
{
    public class DataPointOriginColumns: QueryFilter<DataPointOriginColumns>, IFilterToken
    {
        public DataPointOriginColumns() { }
        public DataPointOriginColumns(string columnName)
            : base(columnName)
        { }
		
		public DataPointOriginColumns KeyColumn
		{
			get
			{
				return new DataPointOriginColumns("Id");
			}
		}	

        public DataPointOriginColumns Id
        {
            get
            {
                return new DataPointOriginColumns("Id");
            }
        }
        public DataPointOriginColumns Uuid
        {
            get
            {
                return new DataPointOriginColumns("Uuid");
            }
        }
        public DataPointOriginColumns Cuid
        {
            get
            {
                return new DataPointOriginColumns("Cuid");
            }
        }
        public DataPointOriginColumns MachineName
        {
            get
            {
                return new DataPointOriginColumns("MachineName");
            }
        }
        public DataPointOriginColumns AssemblyPath
        {
            get
            {
                return new DataPointOriginColumns("AssemblyPath");
            }
        }
        public DataPointOriginColumns Key
        {
            get
            {
                return new DataPointOriginColumns("Key");
            }
        }
        public DataPointOriginColumns CompositeKeyId
        {
            get
            {
                return new DataPointOriginColumns("CompositeKeyId");
            }
        }
        public DataPointOriginColumns CompositeKey
        {
            get
            {
                return new DataPointOriginColumns("CompositeKey");
            }
        }
        public DataPointOriginColumns CreatedBy
        {
            get
            {
                return new DataPointOriginColumns("CreatedBy");
            }
        }
        public DataPointOriginColumns ModifiedBy
        {
            get
            {
                return new DataPointOriginColumns("ModifiedBy");
            }
        }
        public DataPointOriginColumns Modified
        {
            get
            {
                return new DataPointOriginColumns("Modified");
            }
        }
        public DataPointOriginColumns Deleted
        {
            get
            {
                return new DataPointOriginColumns("Deleted");
            }
        }
        public DataPointOriginColumns Created
        {
            get
            {
                return new DataPointOriginColumns("Created");
            }
        }



		protected internal Type TableType
		{
			get
			{
				return typeof(DataPointOrigin);
			}
		}

		public string Operator { get; set; }

        public override string ToString()
        {
            return base.ColumnName;
        }
	}
}