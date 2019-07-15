using System;
using System.Collections.Generic;
using System.Text;
using Bam.Net.Data;

namespace Bam.Net.Services.DataReplication.Consensus.Data.Dao
{
    public class RaftLogEntryColumns: QueryFilter<RaftLogEntryColumns>, IFilterToken
    {
        public RaftLogEntryColumns() { }
        public RaftLogEntryColumns(string columnName)
            : base(columnName)
        { }
		
		public RaftLogEntryColumns KeyColumn
		{
			get
			{
				return new RaftLogEntryColumns("Id");
			}
		}	

        public RaftLogEntryColumns Id
        {
            get
            {
                return new RaftLogEntryColumns("Id");
            }
        }
        public RaftLogEntryColumns Uuid
        {
            get
            {
                return new RaftLogEntryColumns("Uuid");
            }
        }
        public RaftLogEntryColumns Cuid
        {
            get
            {
                return new RaftLogEntryColumns("Cuid");
            }
        }
        public RaftLogEntryColumns InstanceId
        {
            get
            {
                return new RaftLogEntryColumns("InstanceId");
            }
        }
        public RaftLogEntryColumns TypeId
        {
            get
            {
                return new RaftLogEntryColumns("TypeId");
            }
        }
        public RaftLogEntryColumns PropertyId
        {
            get
            {
                return new RaftLogEntryColumns("PropertyId");
            }
        }
        public RaftLogEntryColumns Value
        {
            get
            {
                return new RaftLogEntryColumns("Value");
            }
        }
        public RaftLogEntryColumns CompositeKeyId
        {
            get
            {
                return new RaftLogEntryColumns("CompositeKeyId");
            }
        }
        public RaftLogEntryColumns CompositeKey
        {
            get
            {
                return new RaftLogEntryColumns("CompositeKey");
            }
        }
        public RaftLogEntryColumns CreatedBy
        {
            get
            {
                return new RaftLogEntryColumns("CreatedBy");
            }
        }
        public RaftLogEntryColumns ModifiedBy
        {
            get
            {
                return new RaftLogEntryColumns("ModifiedBy");
            }
        }
        public RaftLogEntryColumns Modified
        {
            get
            {
                return new RaftLogEntryColumns("Modified");
            }
        }
        public RaftLogEntryColumns Deleted
        {
            get
            {
                return new RaftLogEntryColumns("Deleted");
            }
        }
        public RaftLogEntryColumns Created
        {
            get
            {
                return new RaftLogEntryColumns("Created");
            }
        }



		protected internal Type TableType
		{
			get
			{
				return typeof(RaftLogEntry);
			}
		}

		public string Operator { get; set; }

        public override string ToString()
        {
            return base.ColumnName;
        }
	}
}