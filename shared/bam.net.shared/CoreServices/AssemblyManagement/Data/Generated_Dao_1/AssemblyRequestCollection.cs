using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.Common;
using Bam.Net.Data;

namespace Bam.Net.CoreServices.AssemblyManagement.Data.Dao
{
    public class AssemblyRequestCollection: DaoCollection<AssemblyRequestColumns, AssemblyRequest>
    { 
		public AssemblyRequestCollection(){}
		public AssemblyRequestCollection(Database db, DataTable table, Bam.Net.Data.Dao dao = null, string rc = null) : base(db, table, dao, rc) { }
		public AssemblyRequestCollection(DataTable table, Bam.Net.Data.Dao dao = null, string rc = null) : base(table, dao, rc) { }
		public AssemblyRequestCollection(Query<AssemblyRequestColumns, AssemblyRequest> q, Bam.Net.Data.Dao dao = null, string rc = null) : base(q, dao, rc) { }
		public AssemblyRequestCollection(Database db, Query<AssemblyRequestColumns, AssemblyRequest> q, bool load) : base(db, q, load) { }
		public AssemblyRequestCollection(Query<AssemblyRequestColumns, AssemblyRequest> q, bool load) : base(q, load) { }
    }
}