using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.Common;
using Bam.Net.Data;

namespace Bam.Net.Services.DataReplication.Consensus.Data.Dao
{
    public class RaftLogEntryCollection: DaoCollection<RaftLogEntryColumns, RaftLogEntry>
    { 
		public RaftLogEntryCollection(){}
		public RaftLogEntryCollection(Database db, DataTable table, Bam.Net.Data.Dao dao = null, string rc = null) : base(db, table, dao, rc) { }
		public RaftLogEntryCollection(DataTable table, Bam.Net.Data.Dao dao = null, string rc = null) : base(table, dao, rc) { }
		public RaftLogEntryCollection(Query<RaftLogEntryColumns, RaftLogEntry> q, Bam.Net.Data.Dao dao = null, string rc = null) : base(q, dao, rc) { }
		public RaftLogEntryCollection(Database db, Query<RaftLogEntryColumns, RaftLogEntry> q, bool load) : base(db, q, load) { }
		public RaftLogEntryCollection(Query<RaftLogEntryColumns, RaftLogEntry> q, bool load) : base(q, load) { }
    }
}