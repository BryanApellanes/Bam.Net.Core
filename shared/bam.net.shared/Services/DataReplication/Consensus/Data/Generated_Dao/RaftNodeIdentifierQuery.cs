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
    public class RaftNodeIdentifierQuery: Query<RaftNodeIdentifierColumns, RaftNodeIdentifier>
    { 
		public RaftNodeIdentifierQuery(){}
		public RaftNodeIdentifierQuery(WhereDelegate<RaftNodeIdentifierColumns> where, OrderBy<RaftNodeIdentifierColumns> orderBy = null, Database db = null) : base(where, orderBy, db) { }
		public RaftNodeIdentifierQuery(Func<RaftNodeIdentifierColumns, QueryFilter<RaftNodeIdentifierColumns>> where, OrderBy<RaftNodeIdentifierColumns> orderBy = null, Database db = null) : base(where, orderBy, db) { }		
		public RaftNodeIdentifierQuery(Delegate where, Database db = null) : base(where, db) { }
		
        public static RaftNodeIdentifierQuery Where(WhereDelegate<RaftNodeIdentifierColumns> where)
        {
            return Where(where, null, null);
        }

        public static RaftNodeIdentifierQuery Where(WhereDelegate<RaftNodeIdentifierColumns> where, OrderBy<RaftNodeIdentifierColumns> orderBy = null, Database db = null)
        {
            return new RaftNodeIdentifierQuery(where, orderBy, db);
        }

		public RaftNodeIdentifierCollection Execute()
		{
			return new RaftNodeIdentifierCollection(this, true);
		}
    }
}