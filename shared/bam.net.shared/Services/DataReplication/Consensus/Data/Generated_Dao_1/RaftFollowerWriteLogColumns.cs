using System;
using System.Collections.Generic;
using System.Text;
using Bam.Net.Data;

namespace Bam.Net.Services.DataReplication.Consensus.Data.Dao
{
    public class RaftFollowerWriteLogColumns: QueryFilter<RaftFollowerWriteLogColumns>, IFilterToken
    {
        public RaftFollowerWriteLogColumns() { }
        public RaftFollowerWriteLogColumns(string columnName)
            : base(columnName)
        { }
		
		public RaftFollowerWriteLogColumns KeyColumn
		{
			get
			{
				return new RaftFollowerWriteLogColumns("Id");
			}
		}	

        public RaftFollowerWriteLogColumns Id
        {
            get
            {
                return new RaftFollowerWriteLogColumns("Id");
            }
        }
        public RaftFollowerWriteLogColumns Uuid
        {
            get
            {
                return new RaftFollowerWriteLogColumns("Uuid");
            }
        }
        public RaftFollowerWriteLogColumns Cuid
        {
            get
            {
                return new RaftFollowerWriteLogColumns("Cuid");
            }
        }
        public RaftFollowerWriteLogColumns NodeIdentifier
        {
            get
            {
                return new RaftFollowerWriteLogColumns("NodeIdentifier");
            }
        }
        public RaftFollowerWriteLogColumns LogEntryIdentifier
        {
            get
            {
                return new RaftFollowerWriteLogColumns("LogEntryIdentifier");
            }
        }
        public RaftFollowerWriteLogColumns CreatedBy
        {
            get
            {
                return new RaftFollowerWriteLogColumns("CreatedBy");
            }
        }
        public RaftFollowerWriteLogColumns ModifiedBy
        {
            get
            {
                return new RaftFollowerWriteLogColumns("ModifiedBy");
            }
        }
        public RaftFollowerWriteLogColumns Modified
        {
            get
            {
                return new RaftFollowerWriteLogColumns("Modified");
            }
        }
        public RaftFollowerWriteLogColumns Deleted
        {
            get
            {
                return new RaftFollowerWriteLogColumns("Deleted");
            }
        }
        public RaftFollowerWriteLogColumns Created
        {
            get
            {
                return new RaftFollowerWriteLogColumns("Created");
            }
        }



		protected internal Type TableType
		{
			get
			{
				return typeof(RaftFollowerWriteLog);
			}
		}

		public string Operator { get; set; }

        public override string ToString()
        {
            return base.ColumnName;
        }
	}
}