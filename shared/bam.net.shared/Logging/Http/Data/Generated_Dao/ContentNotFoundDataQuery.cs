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
    public class ContentNotFoundDataQuery: Query<ContentNotFoundDataColumns, ContentNotFoundData>
    { 
		public ContentNotFoundDataQuery(){}
		public ContentNotFoundDataQuery(WhereDelegate<ContentNotFoundDataColumns> where, OrderBy<ContentNotFoundDataColumns> orderBy = null, Database db = null) : base(where, orderBy, db) { }
		public ContentNotFoundDataQuery(Func<ContentNotFoundDataColumns, QueryFilter<ContentNotFoundDataColumns>> where, OrderBy<ContentNotFoundDataColumns> orderBy = null, Database db = null) : base(where, orderBy, db) { }		
		public ContentNotFoundDataQuery(Delegate where, Database db = null) : base(where, db) { }
		
        public static ContentNotFoundDataQuery Where(WhereDelegate<ContentNotFoundDataColumns> where)
        {
            return Where(where, null, null);
        }

        public static ContentNotFoundDataQuery Where(WhereDelegate<ContentNotFoundDataColumns> where, OrderBy<ContentNotFoundDataColumns> orderBy = null, Database db = null)
        {
            return new ContentNotFoundDataQuery(where, orderBy, db);
        }

		public ContentNotFoundDataCollection Execute()
		{
			return new ContentNotFoundDataCollection(this, true);
		}
    }
}