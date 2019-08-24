using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.Common;
using Bam.Net.Data;

namespace Bam.Net.Services.DataReplication.Data.Dao
{
    public class RetrieveOperationResultCollection: DaoCollection<RetrieveOperationResultColumns, RetrieveOperationResult>
    { 
		public RetrieveOperationResultCollection(){}
		public RetrieveOperationResultCollection(Database db, DataTable table, Bam.Net.Data.Dao dao = null, string rc = null) : base(db, table, dao, rc) { }
		public RetrieveOperationResultCollection(DataTable table, Bam.Net.Data.Dao dao = null, string rc = null) : base(table, dao, rc) { }
		public RetrieveOperationResultCollection(Query<RetrieveOperationResultColumns, RetrieveOperationResult> q, Bam.Net.Data.Dao dao = null, string rc = null) : base(q, dao, rc) { }
		public RetrieveOperationResultCollection(Database db, Query<RetrieveOperationResultColumns, RetrieveOperationResult> q, bool load) : base(db, q, load) { }
		public RetrieveOperationResultCollection(Query<RetrieveOperationResultColumns, RetrieveOperationResult> q, bool load) : base(q, load) { }
    }
}