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
    public class RetrieveOperationResultQuery: Query<RetrieveOperationResultColumns, RetrieveOperationResult>
    { 
		public RetrieveOperationResultQuery(){}
		public RetrieveOperationResultQuery(WhereDelegate<RetrieveOperationResultColumns> where, OrderBy<RetrieveOperationResultColumns> orderBy = null, Database db = null) : base(where, orderBy, db) { }
		public RetrieveOperationResultQuery(Func<RetrieveOperationResultColumns, QueryFilter<RetrieveOperationResultColumns>> where, OrderBy<RetrieveOperationResultColumns> orderBy = null, Database db = null) : base(where, orderBy, db) { }		
		public RetrieveOperationResultQuery(Delegate where, Database db = null) : base(where, db) { }
		
        public static RetrieveOperationResultQuery Where(WhereDelegate<RetrieveOperationResultColumns> where)
        {
            return Where(where, null, null);
        }

        public static RetrieveOperationResultQuery Where(WhereDelegate<RetrieveOperationResultColumns> where, OrderBy<RetrieveOperationResultColumns> orderBy = null, Database db = null)
        {
            return new RetrieveOperationResultQuery(where, orderBy, db);
        }

		public RetrieveOperationResultCollection Execute()
		{
			return new RetrieveOperationResultCollection(this, true);
		}
    }
}