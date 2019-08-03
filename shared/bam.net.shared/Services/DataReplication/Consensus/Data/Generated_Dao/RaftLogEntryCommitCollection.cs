using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.Common;
using Bam.Net.Data;

namespace Bam.Net.Services.DataReplication.Consensus.Data.Dao
{
    public class RaftLogEntryCommitCollection: DaoCollection<RaftLogEntryCommitColumns, RaftLogEntryCommit>
    { 
		public RaftLogEntryCommitCollection(){}
		public RaftLogEntryCommitCollection(Database db, DataTable table, Bam.Net.Data.Dao dao = null, string rc = null) : base(db, table, dao, rc) { }
		public RaftLogEntryCommitCollection(DataTable table, Bam.Net.Data.Dao dao = null, string rc = null) : base(table, dao, rc) { }
		public RaftLogEntryCommitCollection(Query<RaftLogEntryCommitColumns, RaftLogEntryCommit> q, Bam.Net.Data.Dao dao = null, string rc = null) : base(q, dao, rc) { }
		public RaftLogEntryCommitCollection(Database db, Query<RaftLogEntryCommitColumns, RaftLogEntryCommit> q, bool load) : base(db, q, load) { }
		public RaftLogEntryCommitCollection(Query<RaftLogEntryCommitColumns, RaftLogEntryCommit> q, bool load) : base(q, load) { }
    }
}