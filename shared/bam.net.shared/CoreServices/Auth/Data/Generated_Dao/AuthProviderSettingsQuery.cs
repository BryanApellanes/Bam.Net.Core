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
    public class AuthProviderSettingsQuery: Query<AuthProviderSettingsColumns, AuthProviderSettings>
    { 
		public AuthProviderSettingsQuery(){}
		public AuthProviderSettingsQuery(WhereDelegate<AuthProviderSettingsColumns> where, OrderBy<AuthProviderSettingsColumns> orderBy = null, Database db = null) : base(where, orderBy, db) { }
		public AuthProviderSettingsQuery(Func<AuthProviderSettingsColumns, QueryFilter<AuthProviderSettingsColumns>> where, OrderBy<AuthProviderSettingsColumns> orderBy = null, Database db = null) : base(where, orderBy, db) { }		
		public AuthProviderSettingsQuery(Delegate where, Database db = null) : base(where, db) { }
		
        public static AuthProviderSettingsQuery Where(WhereDelegate<AuthProviderSettingsColumns> where)
        {
            return Where(where, null, null);
        }

        public static AuthProviderSettingsQuery Where(WhereDelegate<AuthProviderSettingsColumns> where, OrderBy<AuthProviderSettingsColumns> orderBy = null, Database db = null)
        {
            return new AuthProviderSettingsQuery(where, orderBy, db);
        }

		public AuthProviderSettingsCollection Execute()
		{
			return new AuthProviderSettingsCollection(this, true);
		}
    }
}