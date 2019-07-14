using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.Common;
using Bam.Net.Data;

namespace Bam.Net.Services.DataReplication.Consensus.Data.Dao
{
    public class RaftFollowerWriteLogCollection: DaoCollection<RaftFollowerWriteLogColumns, RaftFollowerWriteLog>
    { 
		public RaftFollowerWriteLogCollection(){}
		public RaftFollowerWriteLogCollection(Database db, DataTable table, Bam.Net.Data.Dao dao = null, string rc = null) : base(db, table, dao, rc) { }
		public RaftFollowerWriteLogCollection(DataTable table, Bam.Net.Data.Dao dao = null, string rc = null) : base(table, dao, rc) { }
		public RaftFollowerWriteLogCollection(Query<RaftFollowerWriteLogColumns, RaftFollowerWriteLog> q, Bam.Net.Data.Dao dao = null, string rc = null) : base(q, dao, rc) { }
		public RaftFollowerWriteLogCollection(Database db, Query<RaftFollowerWriteLogColumns, RaftFollowerWriteLog> q, bool load) : base(db, q, load) { }
		public RaftFollowerWriteLogCollection(Query<RaftFollowerWriteLogColumns, RaftFollowerWriteLog> q, bool load) : base(q, load) { }
    }
}