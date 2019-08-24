using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.Common;
using Bam.Net.Data;

namespace Bam.Net.Services.DataReplication.Data.Dao
{
    public class DataPropertyFilterCollection: DaoCollection<DataPropertyFilterColumns, DataPropertyFilter>
    { 
		public DataPropertyFilterCollection(){}
		public DataPropertyFilterCollection(Database db, DataTable table, Bam.Net.Data.Dao dao = null, string rc = null) : base(db, table, dao, rc) { }
		public DataPropertyFilterCollection(DataTable table, Bam.Net.Data.Dao dao = null, string rc = null) : base(table, dao, rc) { }
		public DataPropertyFilterCollection(Query<DataPropertyFilterColumns, DataPropertyFilter> q, Bam.Net.Data.Dao dao = null, string rc = null) : base(q, dao, rc) { }
		public DataPropertyFilterCollection(Database db, Query<DataPropertyFilterColumns, DataPropertyFilter> q, bool load) : base(db, q, load) { }
		public DataPropertyFilterCollection(Query<DataPropertyFilterColumns, DataPropertyFilter> q, bool load) : base(q, load) { }
    }
}