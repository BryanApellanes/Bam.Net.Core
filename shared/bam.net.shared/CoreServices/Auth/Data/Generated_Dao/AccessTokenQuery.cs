/*
	This file was generated and should not be modified directly
*/
using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.Common;
using Bam.Net.Data;

namespace Bam.Net.CoreServices.Auth.Data.Dao
{
    public class AccessTokenQuery: Query<AccessTokenColumns, AccessToken>
    { 
		public AccessTokenQuery(){}
		public AccessTokenQuery(WhereDelegate<AccessTokenColumns> where, OrderBy<AccessTokenColumns> orderBy = null, Database db = null) : base(where, orderBy, db) { }
		public AccessTokenQuery(Func<AccessTokenColumns, QueryFilter<AccessTokenColumns>> where, OrderBy<AccessTokenColumns> orderBy = null, Database db = null) : base(where, orderBy, db) { }		
		public AccessTokenQuery(Delegate where, Database db = null) : base(where, db) { }
		
        public static AccessTokenQuery Where(WhereDelegate<AccessTokenColumns> where)
        {
            return Where(where, null, null);
        }

        public static AccessTokenQuery Where(WhereDelegate<AccessTokenColumns> where, OrderBy<AccessTokenColumns> orderBy = null, Database db = null)
        {
            return new AccessTokenQuery(where, orderBy, db);
        }

		public AccessTokenCollection Execute()
		{
			return new AccessTokenCollection(this, true);
		}
    }
}