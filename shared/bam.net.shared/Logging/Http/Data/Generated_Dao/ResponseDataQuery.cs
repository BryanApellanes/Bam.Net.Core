/*
	This file was generated and should not be modified directly
*/
using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.Common;
using Bam.Net.Data;

namespace Bam.Net.Logging.Http.Data.Dao
{
    public class ResponseDataQuery: Query<ResponseDataColumns, ResponseData>
    { 
		public ResponseDataQuery(){}
		public ResponseDataQuery(WhereDelegate<ResponseDataColumns> where, OrderBy<ResponseDataColumns> orderBy = null, Database db = null) : base(where, orderBy, db) { }
		public ResponseDataQuery(Func<ResponseDataColumns, QueryFilter<ResponseDataColumns>> where, OrderBy<ResponseDataColumns> orderBy = null, Database db = null) : base(where, orderBy, db) { }		
		public ResponseDataQuery(Delegate where, Database db = null) : base(where, db) { }
		
        public static ResponseDataQuery Where(WhereDelegate<ResponseDataColumns> where)
        {
            return Where(where, null, null);
        }

        public static ResponseDataQuery Where(WhereDelegate<ResponseDataColumns> where, OrderBy<ResponseDataColumns> orderBy = null, Database db = null)
        {
            return new ResponseDataQuery(where, orderBy, db);
        }

		public ResponseDataCollection Execute()
		{
			return new ResponseDataCollection(this, true);
		}
    }
}