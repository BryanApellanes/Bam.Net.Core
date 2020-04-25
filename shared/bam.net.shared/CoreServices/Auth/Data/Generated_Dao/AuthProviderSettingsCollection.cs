using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.Common;
using Bam.Net.Data;

namespace Bam.Net.CoreServices.Auth.Data.Dao
{
    public class AuthProviderSettingsCollection: DaoCollection<AuthProviderSettingsColumns, AuthProviderSettings>
    { 
		public AuthProviderSettingsCollection(){}
		public AuthProviderSettingsCollection(Database db, DataTable table, Bam.Net.Data.Dao dao = null, string rc = null) : base(db, table, dao, rc) { }
		public AuthProviderSettingsCollection(DataTable table, Bam.Net.Data.Dao dao = null, string rc = null) : base(table, dao, rc) { }
		public AuthProviderSettingsCollection(Query<AuthProviderSettingsColumns, AuthProviderSettings> q, Bam.Net.Data.Dao dao = null, string rc = null) : base(q, dao, rc) { }
		public AuthProviderSettingsCollection(Database db, Query<AuthProviderSettingsColumns, AuthProviderSettings> q, bool load) : base(db, q, load) { }
		public AuthProviderSettingsCollection(Query<AuthProviderSettingsColumns, AuthProviderSettings> q, bool load) : base(q, load) { }
    }
}