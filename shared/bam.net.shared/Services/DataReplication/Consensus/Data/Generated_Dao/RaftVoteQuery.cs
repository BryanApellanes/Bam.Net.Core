/*
	This file was generated and should not be modified directly
*/
using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.Common;
using Bam.Net.Data;

namespace Bam.Net.Services.DataReplication.Consensus.Data.Dao
{
    public class RaftVoteQuery: Query<RaftVoteColumns, RaftVote>
    { 
		public RaftVoteQuery(){}
		public RaftVoteQuery(WhereDelegate<RaftVoteColumns> where, OrderBy<RaftVoteColumns> orderBy = null, Database db = null) : base(where, orderBy, db) { }
		public RaftVoteQuery(Func<RaftVoteColumns, QueryFilter<RaftVoteColumns>> where, OrderBy<RaftVoteColumns> orderBy = null, Database db = null) : base(where, orderBy, db) { }		
		public RaftVoteQuery(Delegate where, Database db = null) : base(where, db) { }
		
        public static RaftVoteQuery Where(WhereDelegate<RaftVoteColumns> where)
        {
            return Where(where, null, null);
        }

        public static RaftVoteQuery Where(WhereDelegate<RaftVoteColumns> where, OrderBy<RaftVoteColumns> orderBy = null, Database db = null)
        {
            return new RaftVoteQuery(where, orderBy, db);
        }

		public RaftVoteCollection Execute()
		{
			return new RaftVoteCollection(this, true);
		}
    }
}