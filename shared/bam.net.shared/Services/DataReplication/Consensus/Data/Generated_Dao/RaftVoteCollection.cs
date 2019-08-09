using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.Common;
using Bam.Net.Data;

namespace Bam.Net.Services.DataReplication.Consensus.Data.Dao
{
    public class RaftVoteCollection: DaoCollection<RaftVoteColumns, RaftVote>
    { 
		public RaftVoteCollection(){}
		public RaftVoteCollection(Database db, DataTable table, Bam.Net.Data.Dao dao = null, string rc = null) : base(db, table, dao, rc) { }
		public RaftVoteCollection(DataTable table, Bam.Net.Data.Dao dao = null, string rc = null) : base(table, dao, rc) { }
		public RaftVoteCollection(Query<RaftVoteColumns, RaftVote> q, Bam.Net.Data.Dao dao = null, string rc = null) : base(q, dao, rc) { }
		public RaftVoteCollection(Database db, Query<RaftVoteColumns, RaftVote> q, bool load) : base(db, q, load) { }
		public RaftVoteCollection(Query<RaftVoteColumns, RaftVote> q, bool load) : base(q, load) { }
    }
}