using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.Common;
using Bam.Net.Data;

namespace Bam.Net.Services.DataReplication.Data.Dao
{
    public class DataPointOriginCollection: DaoCollection<DataPointOriginColumns, DataPointOrigin>
    { 
		public DataPointOriginCollection(){}
		public DataPointOriginCollection(Database db, DataTable table, Bam.Net.Data.Dao dao = null, string rc = null) : base(db, table, dao, rc) { }
		public DataPointOriginCollection(DataTable table, Bam.Net.Data.Dao dao = null, string rc = null) : base(table, dao, rc) { }
		public DataPointOriginCollection(Query<DataPointOriginColumns, DataPointOrigin> q, Bam.Net.Data.Dao dao = null, string rc = null) : base(q, dao, rc) { }
		public DataPointOriginCollection(Database db, Query<DataPointOriginColumns, DataPointOrigin> q, bool load) : base(db, q, load) { }
		public DataPointOriginCollection(Query<DataPointOriginColumns, DataPointOrigin> q, bool load) : base(q, load) { }
    }
}