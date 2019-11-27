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
    public class UserKeyDataQuery: Query<UserKeyDataColumns, UserKeyData>
    { 
		public UserKeyDataQuery(){}
		public UserKeyDataQuery(WhereDelegate<UserKeyDataColumns> where, OrderBy<UserKeyDataColumns> orderBy = null, Database db = null) : base(where, orderBy, db) { }
		public UserKeyDataQuery(Func<UserKeyDataColumns, QueryFilter<UserKeyDataColumns>> where, OrderBy<UserKeyDataColumns> orderBy = null, Database db = null) : base(where, orderBy, db) { }		
		public UserKeyDataQuery(Delegate where, Database db = null) : base(where, db) { }
		
        public static UserKeyDataQuery Where(WhereDelegate<UserKeyDataColumns> where)
        {
            return Where(where, null, null);
        }

        public static UserKeyDataQuery Where(WhereDelegate<UserKeyDataColumns> where, OrderBy<UserKeyDataColumns> orderBy = null, Database db = null)
        {
            return new UserKeyDataQuery(where, orderBy, db);
        }

		public UserKeyDataCollection Execute()
		{
			return new UserKeyDataCollection(this, true);
		}
    }
}