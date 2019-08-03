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
    public class RaftLogEntryCommitQuery: Query<RaftLogEntryCommitColumns, RaftLogEntryCommit>
    { 
		public RaftLogEntryCommitQuery(){}
		public RaftLogEntryCommitQuery(WhereDelegate<RaftLogEntryCommitColumns> where, OrderBy<RaftLogEntryCommitColumns> orderBy = null, Database db = null) : base(where, orderBy, db) { }
		public RaftLogEntryCommitQuery(Func<RaftLogEntryCommitColumns, QueryFilter<RaftLogEntryCommitColumns>> where, OrderBy<RaftLogEntryCommitColumns> orderBy = null, Database db = null) : base(where, orderBy, db) { }		
		public RaftLogEntryCommitQuery(Delegate where, Database db = null) : base(where, db) { }
		
        public static RaftLogEntryCommitQuery Where(WhereDelegate<RaftLogEntryCommitColumns> where)
        {
            return Where(where, null, null);
        }

        public static RaftLogEntryCommitQuery Where(WhereDelegate<RaftLogEntryCommitColumns> where, OrderBy<RaftLogEntryCommitColumns> orderBy = null, Database db = null)
        {
            return new RaftLogEntryCommitQuery(where, orderBy, db);
        }

		public RaftLogEntryCommitCollection Execute()
		{
			return new RaftLogEntryCommitCollection(this, true);
		}
    }
}