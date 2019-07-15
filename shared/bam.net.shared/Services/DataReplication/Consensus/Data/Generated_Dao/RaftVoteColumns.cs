using System;
using System.Collections.Generic;
using System.Text;
using Bam.Net.Data;

namespace Bam.Net.Services.DataReplication.Consensus.Data.Dao
{
    public class RaftVoteColumns: QueryFilter<RaftVoteColumns>, IFilterToken
    {
        public RaftVoteColumns() { }
        public RaftVoteColumns(string columnName)
            : base(columnName)
        { }
		
		public RaftVoteColumns KeyColumn
		{
			get
			{
				return new RaftVoteColumns("Id");
			}
		}	

        public RaftVoteColumns Id
        {
            get
            {
                return new RaftVoteColumns("Id");
            }
        }
        public RaftVoteColumns Uuid
        {
            get
            {
                return new RaftVoteColumns("Uuid");
            }
        }
        public RaftVoteColumns Cuid
        {
            get
            {
                return new RaftVoteColumns("Cuid");
            }
        }
        public RaftVoteColumns FromNodeIdentifier
        {
            get
            {
                return new RaftVoteColumns("FromNodeIdentifier");
            }
        }
        public RaftVoteColumns ForNodeIdentifier
        {
            get
            {
                return new RaftVoteColumns("ForNodeIdentifier");
            }
        }
        public RaftVoteColumns ElectionKey
        {
            get
            {
                return new RaftVoteColumns("ElectionKey");
            }
        }
        public RaftVoteColumns CompositeKeyId
        {
            get
            {
                return new RaftVoteColumns("CompositeKeyId");
            }
        }
        public RaftVoteColumns CompositeKey
        {
            get
            {
                return new RaftVoteColumns("CompositeKey");
            }
        }
        public RaftVoteColumns CreatedBy
        {
            get
            {
                return new RaftVoteColumns("CreatedBy");
            }
        }
        public RaftVoteColumns ModifiedBy
        {
            get
            {
                return new RaftVoteColumns("ModifiedBy");
            }
        }
        public RaftVoteColumns Modified
        {
            get
            {
                return new RaftVoteColumns("Modified");
            }
        }
        public RaftVoteColumns Deleted
        {
            get
            {
                return new RaftVoteColumns("Deleted");
            }
        }
        public RaftVoteColumns Created
        {
            get
            {
                return new RaftVoteColumns("Created");
            }
        }


        public RaftVoteColumns RaftLeaderElectionId
        {
            get
            {
                return new RaftVoteColumns("RaftLeaderElectionId");
            }
        }

		protected internal Type TableType
		{
			get
			{
				return typeof(RaftVote);
			}
		}

		public string Operator { get; set; }

        public override string ToString()
        {
            return base.ColumnName;
        }
	}
}