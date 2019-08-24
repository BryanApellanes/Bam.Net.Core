using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.Common;
using Bam.Net.Data;

namespace Bam.Net.Services.DataReplication.Data.Dao
{
    public class QueryOperationResultCollection: DaoCollection<QueryOperationResultColumns, QueryOperationResult>
    { 
		public QueryOperationResultCollection(){}
		public QueryOperationResultCollection(Database db, DataTable table, Bam.Net.Data.Dao dao = null, string rc = null) : base(db, table, dao, rc) { }
		public QueryOperationResultCollection(DataTable table, Bam.Net.Data.Dao dao = null, string rc = null) : base(table, dao, rc) { }
		public QueryOperationResultCollection(Query<QueryOperationResultColumns, QueryOperationResult> q, Bam.Net.Data.Dao dao = null, string rc = null) : base(q, dao, rc) { }
		public QueryOperationResultCollection(Database db, Query<QueryOperationResultColumns, QueryOperationResult> q, bool load) : base(db, q, load) { }
		public QueryOperationResultCollection(Query<QueryOperationResultColumns, QueryOperationResult> q, bool load) : base(q, load) { }
    }
}