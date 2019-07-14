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
    public class RaftLeaderElectionQuery: Query<RaftLeaderElectionColumns, RaftLeaderElection>
    { 
		public RaftLeaderElectionQuery(){}
		public RaftLeaderElectionQuery(WhereDelegate<RaftLeaderElectionColumns> where, OrderBy<RaftLeaderElectionColumns> orderBy = null, Database db = null) : base(where, orderBy, db) { }
		public RaftLeaderElectionQuery(Func<RaftLeaderElectionColumns, QueryFilter<RaftLeaderElectionColumns>> where, OrderBy<RaftLeaderElectionColumns> orderBy = null, Database db = null) : base(where, orderBy, db) { }		
		public RaftLeaderElectionQuery(Delegate where, Database db = null) : base(where, db) { }
		
        public static RaftLeaderElectionQuery Where(WhereDelegate<RaftLeaderElectionColumns> where)
        {
            return Where(where, null, null);
        }

        public static RaftLeaderElectionQuery Where(WhereDelegate<RaftLeaderElectionColumns> where, OrderBy<RaftLeaderElectionColumns> orderBy = null, Database db = null)
        {
            return new RaftLeaderElectionQuery(where, orderBy, db);
        }

		public RaftLeaderElectionCollection Execute()
		{
			return new RaftLeaderElectionCollection(this, true);
		}
    }
}