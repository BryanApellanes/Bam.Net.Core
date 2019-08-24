/*
	This file was generated and should not be modified directly
*/
using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.Common;
using Bam.Net.Data;

namespace Bam.Net.Services.DataReplication.Data.Dao
{
    public class QueryOperationResultQuery: Query<QueryOperationResultColumns, QueryOperationResult>
    { 
		public QueryOperationResultQuery(){}
		public QueryOperationResultQuery(WhereDelegate<QueryOperationResultColumns> where, OrderBy<QueryOperationResultColumns> orderBy = null, Database db = null) : base(where, orderBy, db) { }
		public QueryOperationResultQuery(Func<QueryOperationResultColumns, QueryFilter<QueryOperationResultColumns>> where, OrderBy<QueryOperationResultColumns> orderBy = null, Database db = null) : base(where, orderBy, db) { }		
		public QueryOperationResultQuery(Delegate where, Database db = null) : base(where, db) { }
		
        public static QueryOperationResultQuery Where(WhereDelegate<QueryOperationResultColumns> where)
        {
            return Where(where, null, null);
        }

        public static QueryOperationResultQuery Where(WhereDelegate<QueryOperationResultColumns> where, OrderBy<QueryOperationResultColumns> orderBy = null, Database db = null)
        {
            return new QueryOperationResultQuery(where, orderBy, db);
        }

		public QueryOperationResultCollection Execute()
		{
			return new QueryOperationResultCollection(this, true);
		}
    }
}