using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.Common;
using Bam.Net.Data;

namespace Bam.Net.Logging.Http.Data.Dao
{
    public class UserKeyDataCollection: DaoCollection<UserKeyDataColumns, UserKeyData>
    { 
		public UserKeyDataCollection(){}
		public UserKeyDataCollection(Database db, DataTable table, Bam.Net.Data.Dao dao = null, string rc = null) : base(db, table, dao, rc) { }
		public UserKeyDataCollection(DataTable table, Bam.Net.Data.Dao dao = null, string rc = null) : base(table, dao, rc) { }
		public UserKeyDataCollection(Query<UserKeyDataColumns, UserKeyData> q, Bam.Net.Data.Dao dao = null, string rc = null) : base(q, dao, rc) { }
		public UserKeyDataCollection(Database db, Query<UserKeyDataColumns, UserKeyData> q, bool load) : base(db, q, load) { }
		public UserKeyDataCollection(Query<UserKeyDataColumns, UserKeyData> q, bool load) : base(q, load) { }
    }
}