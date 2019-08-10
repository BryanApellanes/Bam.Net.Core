using System;
using System.Collections.Generic;
using System.Text;
using Bam.Net.Data;

namespace Bam.Net.Services.DataReplication.Consensus.Data.Dao
{
    public class RaftLogEntryCommitColumns: QueryFilter<RaftLogEntryCommitColumns>, IFilterToken
    {
        public RaftLogEntryCommitColumns() { }
        public RaftLogEntryCommitColumns(string columnName)
            : base(columnName)
        { }
		
		public RaftLogEntryCommitColumns KeyColumn
		{
			get
			{
				return new RaftLogEntryCommitColumns("Id");
			}
		}	

        public RaftLogEntryCommitColumns Id
        {
            get
            {
                return new RaftLogEntryCommitColumns("Id");
            }
        }
        public RaftLogEntryCommitColumns Uuid
        {
            get
            {
                return new RaftLogEntryCommitColumns("Uuid");
            }
        }
        public RaftLogEntryCommitColumns Cuid
        {
            get
            {
                return new RaftLogEntryCommitColumns("Cuid");
            }
        }
        public RaftLogEntryCommitColumns RaftLogEntryId
        {
            get
            {
                return new RaftLogEntryCommitColumns("RaftLogEntryId");
            }
        }
        public RaftLogEntryCommitColumns Seq
        {
            get
            {
                return new RaftLogEntryCommitColumns("Seq");
            }
        }
        public RaftLogEntryCommitColumns Created
        {
            get
            {
                return new RaftLogEntryCommitColumns("Created");
            }
        }



		protected internal Type TableType
		{
			get
			{
				return typeof(RaftLogEntryCommit);
			}
		}

		public string Operator { get; set; }

        public override string ToString()
        {
            return base.ColumnName;
        }
	}
}