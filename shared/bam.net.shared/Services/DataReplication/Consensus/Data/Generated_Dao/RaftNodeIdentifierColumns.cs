using System;
using System.Collections.Generic;
using System.Text;
using Bam.Net.Data;

namespace Bam.Net.Services.DataReplication.Consensus.Data.Dao
{
    public class RaftNodeIdentifierColumns: QueryFilter<RaftNodeIdentifierColumns>, IFilterToken
    {
        public RaftNodeIdentifierColumns() { }
        public RaftNodeIdentifierColumns(string columnName)
            : base(columnName)
        { }
		
		public RaftNodeIdentifierColumns KeyColumn
		{
			get
			{
				return new RaftNodeIdentifierColumns("Id");
			}
		}	

        public RaftNodeIdentifierColumns Id
        {
            get
            {
                return new RaftNodeIdentifierColumns("Id");
            }
        }
        public RaftNodeIdentifierColumns Uuid
        {
            get
            {
                return new RaftNodeIdentifierColumns("Uuid");
            }
        }
        public RaftNodeIdentifierColumns Cuid
        {
            get
            {
                return new RaftNodeIdentifierColumns("Cuid");
            }
        }
        public RaftNodeIdentifierColumns HostName
        {
            get
            {
                return new RaftNodeIdentifierColumns("HostName");
            }
        }
        public RaftNodeIdentifierColumns Port
        {
            get
            {
                return new RaftNodeIdentifierColumns("Port");
            }
        }
        public RaftNodeIdentifierColumns CompositeKeyId
        {
            get
            {
                return new RaftNodeIdentifierColumns("CompositeKeyId");
            }
        }
        public RaftNodeIdentifierColumns CompositeKey
        {
            get
            {
                return new RaftNodeIdentifierColumns("CompositeKey");
            }
        }
        public RaftNodeIdentifierColumns CreatedBy
        {
            get
            {
                return new RaftNodeIdentifierColumns("CreatedBy");
            }
        }
        public RaftNodeIdentifierColumns ModifiedBy
        {
            get
            {
                return new RaftNodeIdentifierColumns("ModifiedBy");
            }
        }
        public RaftNodeIdentifierColumns Modified
        {
            get
            {
                return new RaftNodeIdentifierColumns("Modified");
            }
        }
        public RaftNodeIdentifierColumns Deleted
        {
            get
            {
                return new RaftNodeIdentifierColumns("Deleted");
            }
        }
        public RaftNodeIdentifierColumns Created
        {
            get
            {
                return new RaftNodeIdentifierColumns("Created");
            }
        }



		protected internal Type TableType
		{
			get
			{
				return typeof(RaftNodeIdentifier);
			}
		}

		public string Operator { get; set; }

        public override string ToString()
        {
            return base.ColumnName;
        }
	}
}