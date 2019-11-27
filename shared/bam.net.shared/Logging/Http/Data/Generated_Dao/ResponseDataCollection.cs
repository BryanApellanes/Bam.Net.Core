using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.Common;
using Bam.Net.Data;

namespace Bam.Net.Logging.Http.Data.Dao
{
    public class ResponseDataCollection: DaoCollection<ResponseDataColumns, ResponseData>
    { 
		public ResponseDataCollection(){}
		public ResponseDataCollection(Database db, DataTable table, Bam.Net.Data.Dao dao = null, string rc = null) : base(db, table, dao, rc) { }
		public ResponseDataCollection(DataTable table, Bam.Net.Data.Dao dao = null, string rc = null) : base(table, dao, rc) { }
		public ResponseDataCollection(Query<ResponseDataColumns, ResponseData> q, Bam.Net.Data.Dao dao = null, string rc = null) : base(q, dao, rc) { }
		public ResponseDataCollection(Database db, Query<ResponseDataColumns, ResponseData> q, bool load) : base(db, q, load) { }
		public ResponseDataCollection(Query<ResponseDataColumns, ResponseData> q, bool load) : base(q, load) { }
    }
}