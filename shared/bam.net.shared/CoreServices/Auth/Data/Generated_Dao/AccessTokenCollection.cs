using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.Common;
using Bam.Net.Data;

namespace Bam.Net.CoreServices.Auth.Data.Dao
{
    public class AccessTokenCollection: DaoCollection<AccessTokenColumns, AccessToken>
    { 
		public AccessTokenCollection(){}
		public AccessTokenCollection(Database db, DataTable table, Bam.Net.Data.Dao dao = null, string rc = null) : base(db, table, dao, rc) { }
		public AccessTokenCollection(DataTable table, Bam.Net.Data.Dao dao = null, string rc = null) : base(table, dao, rc) { }
		public AccessTokenCollection(Query<AccessTokenColumns, AccessToken> q, Bam.Net.Data.Dao dao = null, string rc = null) : base(q, dao, rc) { }
		public AccessTokenCollection(Database db, Query<AccessTokenColumns, AccessToken> q, bool load) : base(db, q, load) { }
		public AccessTokenCollection(Query<AccessTokenColumns, AccessToken> q, bool load) : base(q, load) { }
    }
}