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
    public class RaftFollowerWriteLogQuery: Query<RaftFollowerWriteLogColumns, RaftFollowerWriteLog>
    { 
		public RaftFollowerWriteLogQuery(){}
		public RaftFollowerWriteLogQuery(WhereDelegate<RaftFollowerWriteLogColumns> where, OrderBy<RaftFollowerWriteLogColumns> orderBy = null, Database db = null) : base(where, orderBy, db) { }
		public RaftFollowerWriteLogQuery(Func<RaftFollowerWriteLogColumns, QueryFilter<RaftFollowerWriteLogColumns>> where, OrderBy<RaftFollowerWriteLogColumns> orderBy = null, Database db = null) : base(where, orderBy, db) { }		
		public RaftFollowerWriteLogQuery(Delegate where, Database db = null) : base(where, db) { }
		
        public static RaftFollowerWriteLogQuery Where(WhereDelegate<RaftFollowerWriteLogColumns> where)
        {
            return Where(where, null, null);
        }

        public static RaftFollowerWriteLogQuery Where(WhereDelegate<RaftFollowerWriteLogColumns> where, OrderBy<RaftFollowerWriteLogColumns> orderBy = null, Database db = null)
        {
            return new RaftFollowerWriteLogQuery(where, orderBy, db);
        }

		public RaftFollowerWriteLogCollection Execute()
		{
			return new RaftFollowerWriteLogCollection(this, true);
		}
    }
}