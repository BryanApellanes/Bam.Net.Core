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
    public class RaftLogEntryQuery: Query<RaftLogEntryColumns, RaftLogEntry>
    { 
		public RaftLogEntryQuery(){}
		public RaftLogEntryQuery(WhereDelegate<RaftLogEntryColumns> where, OrderBy<RaftLogEntryColumns> orderBy = null, Database db = null) : base(where, orderBy, db) { }
		public RaftLogEntryQuery(Func<RaftLogEntryColumns, QueryFilter<RaftLogEntryColumns>> where, OrderBy<RaftLogEntryColumns> orderBy = null, Database db = null) : base(where, orderBy, db) { }		
		public RaftLogEntryQuery(Delegate where, Database db = null) : base(where, db) { }
		
        public static RaftLogEntryQuery Where(WhereDelegate<RaftLogEntryColumns> where)
        {
            return Where(where, null, null);
        }

        public static RaftLogEntryQuery Where(WhereDelegate<RaftLogEntryColumns> where, OrderBy<RaftLogEntryColumns> orderBy = null, Database db = null)
        {
            return new RaftLogEntryQuery(where, orderBy, db);
        }

		public RaftLogEntryCollection Execute()
		{
			return new RaftLogEntryCollection(this, true);
		}
    }
}