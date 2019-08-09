/*
	This file was generated and should not be modified directly
*/
using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.Common;
using Bam.Net.Data;

namespace Bam.Net.Services.DataReplication.Consensus.Data.Dao
{
    public class RaftLogEntryCommitPagedQuery: PagedQuery<RaftLogEntryCommitColumns, RaftLogEntryCommit>
    { 
		public RaftLogEntryCommitPagedQuery(RaftLogEntryCommitColumns orderByColumn,RaftLogEntryCommitQuery query, Database db = null) : base(orderByColumn, query, db) { }
    }
}