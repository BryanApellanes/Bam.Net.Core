using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.Common;
using Bam.Net.Data;

namespace Bam.Net.Services.DataReplication.Consensus.Data.Dao
{
    public class RaftNodeIdentifierCollection: DaoCollection<RaftNodeIdentifierColumns, RaftNodeIdentifier>
    { 
		public RaftNodeIdentifierCollection(){}
		public RaftNodeIdentifierCollection(Database db, DataTable table, Bam.Net.Data.Dao dao = null, string rc = null) : base(db, table, dao, rc) { }
		public RaftNodeIdentifierCollection(DataTable table, Bam.Net.Data.Dao dao = null, string rc = null) : base(table, dao, rc) { }
		public RaftNodeIdentifierCollection(Query<RaftNodeIdentifierColumns, RaftNodeIdentifier> q, Bam.Net.Data.Dao dao = null, string rc = null) : base(q, dao, rc) { }
		public RaftNodeIdentifierCollection(Database db, Query<RaftNodeIdentifierColumns, RaftNodeIdentifier> q, bool load) : base(db, q, load) { }
		public RaftNodeIdentifierCollection(Query<RaftNodeIdentifierColumns, RaftNodeIdentifier> q, bool load) : base(q, load) { }
    }
}