using System;
using System.Collections.Generic;
using System.Text;
using Bam.Net.Data;

namespace Bam.Net.Services.DataReplication.Consensus.Data.Dao
{
    public class RaftLeaderElectionColumns: QueryFilter<RaftLeaderElectionColumns>, IFilterToken
    {
        public RaftLeaderElectionColumns() { }
        public RaftLeaderElectionColumns(string columnName)
            : base(columnName)
        { }
		
		public RaftLeaderElectionColumns KeyColumn
		{
			get
			{
				return new RaftLeaderElectionColumns("Id");
			}
		}	

        public RaftLeaderElectionColumns Id
        {
            get
            {
                return new RaftLeaderElectionColumns("Id");
            }
        }
        public RaftLeaderElectionColumns Uuid
        {
            get
            {
                return new RaftLeaderElectionColumns("Uuid");
            }
        }
        public RaftLeaderElectionColumns Cuid
        {
            get
            {
                return new RaftLeaderElectionColumns("Cuid");
            }
        }
        public RaftLeaderElectionColumns Term
        {
            get
            {
                return new RaftLeaderElectionColumns("Term");
            }
        }
        public RaftLeaderElectionColumns CompositeKeyId
        {
            get
            {
                return new RaftLeaderElectionColumns("CompositeKeyId");
            }
        }
        public RaftLeaderElectionColumns CompositeKey
        {
            get
            {
                return new RaftLeaderElectionColumns("CompositeKey");
            }
        }
        public RaftLeaderElectionColumns CreatedBy
        {
            get
            {
                return new RaftLeaderElectionColumns("CreatedBy");
            }
        }
        public RaftLeaderElectionColumns ModifiedBy
        {
            get
            {
                return new RaftLeaderElectionColumns("ModifiedBy");
            }
        }
        public RaftLeaderElectionColumns Modified
        {
            get
            {
                return new RaftLeaderElectionColumns("Modified");
            }
        }
        public RaftLeaderElectionColumns Deleted
        {
            get
            {
                return new RaftLeaderElectionColumns("Deleted");
            }
        }
        public RaftLeaderElectionColumns Created
        {
            get
            {
                return new RaftLeaderElectionColumns("Created");
            }
        }



		protected internal Type TableType
		{
			get
			{
				return typeof(RaftLeaderElection);
			}
		}

		public string Operator { get; set; }

        public override string ToString()
        {
            return base.ColumnName;
        }
	}
}