using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.Common;
using Bam.Net.Data;

namespace Bam.Net.Services.DataReplication.Consensus.Data.Dao
{
    public class RaftLeaderElectionCollection: DaoCollection<RaftLeaderElectionColumns, RaftLeaderElection>
    { 
		public RaftLeaderElectionCollection(){}
		public RaftLeaderElectionCollection(Database db, DataTable table, Bam.Net.Data.Dao dao = null, string rc = null) : base(db, table, dao, rc) { }
		public RaftLeaderElectionCollection(DataTable table, Bam.Net.Data.Dao dao = null, string rc = null) : base(table, dao, rc) { }
		public RaftLeaderElectionCollection(Query<RaftLeaderElectionColumns, RaftLeaderElection> q, Bam.Net.Data.Dao dao = null, string rc = null) : base(q, dao, rc) { }
		public RaftLeaderElectionCollection(Database db, Query<RaftLeaderElectionColumns, RaftLeaderElection> q, bool load) : base(db, q, load) { }
		public RaftLeaderElectionCollection(Query<RaftLeaderElectionColumns, RaftLeaderElection> q, bool load) : base(q, load) { }
    }
}