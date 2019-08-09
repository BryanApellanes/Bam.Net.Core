/*
	This file was generated and should not be modified directly
*/
// model is SchemaDefinition
using System;
using System.Data;
using System.Data.Common;
using Bam.Net;
using Bam.Net.Data;
using Bam.Net.Data.Qi;

namespace Bam.Net.Services.DataReplication.Consensus.Data.Dao
{
	// schema = RaftConsensus
    public static class RaftConsensusContext
    {
		public static string ConnectionName
		{
			get
			{
				return "RaftConsensus";
			}
		}

		public static Database Db
		{
			get
			{
				return Bam.Net.Data.Db.For(ConnectionName);
			}
		}


	public class RaftFollowerWriteLogQueryContext
	{
			public RaftFollowerWriteLogCollection Where(WhereDelegate<RaftFollowerWriteLogColumns> where, Database db = null)
			{
				return Bam.Net.Services.DataReplication.Consensus.Data.Dao.RaftFollowerWriteLog.Where(where, db);
			}
		   
			public RaftFollowerWriteLogCollection Where(WhereDelegate<RaftFollowerWriteLogColumns> where, OrderBy<RaftFollowerWriteLogColumns> orderBy = null, Database db = null)
			{
				return Bam.Net.Services.DataReplication.Consensus.Data.Dao.RaftFollowerWriteLog.Where(where, orderBy, db);
			}

			public RaftFollowerWriteLog OneWhere(WhereDelegate<RaftFollowerWriteLogColumns> where, Database db = null)
			{
				return Bam.Net.Services.DataReplication.Consensus.Data.Dao.RaftFollowerWriteLog.OneWhere(where, db);
			}

			public static RaftFollowerWriteLog GetOneWhere(WhereDelegate<RaftFollowerWriteLogColumns> where, Database db = null)
			{
				return Bam.Net.Services.DataReplication.Consensus.Data.Dao.RaftFollowerWriteLog.GetOneWhere(where, db);
			}
		
			public RaftFollowerWriteLog FirstOneWhere(WhereDelegate<RaftFollowerWriteLogColumns> where, Database db = null)
			{
				return Bam.Net.Services.DataReplication.Consensus.Data.Dao.RaftFollowerWriteLog.FirstOneWhere(where, db);
			}

			public RaftFollowerWriteLogCollection Top(int count, WhereDelegate<RaftFollowerWriteLogColumns> where, Database db = null)
			{
				return Bam.Net.Services.DataReplication.Consensus.Data.Dao.RaftFollowerWriteLog.Top(count, where, db);
			}

			public RaftFollowerWriteLogCollection Top(int count, WhereDelegate<RaftFollowerWriteLogColumns> where, OrderBy<RaftFollowerWriteLogColumns> orderBy, Database db = null)
			{
				return Bam.Net.Services.DataReplication.Consensus.Data.Dao.RaftFollowerWriteLog.Top(count, where, orderBy, db);
			}

			public long Count(WhereDelegate<RaftFollowerWriteLogColumns> where, Database db = null)
			{
				return Bam.Net.Services.DataReplication.Consensus.Data.Dao.RaftFollowerWriteLog.Count(where, db);
			}
	}

	static RaftFollowerWriteLogQueryContext _raftFollowerWriteLogs;
	static object _raftFollowerWriteLogsLock = new object();
	public static RaftFollowerWriteLogQueryContext RaftFollowerWriteLogs
	{
		get
		{
			return _raftFollowerWriteLogsLock.DoubleCheckLock<RaftFollowerWriteLogQueryContext>(ref _raftFollowerWriteLogs, () => new RaftFollowerWriteLogQueryContext());
		}
	}
	public class RaftLogEntryCommitQueryContext
	{
			public RaftLogEntryCommitCollection Where(WhereDelegate<RaftLogEntryCommitColumns> where, Database db = null)
			{
				return Bam.Net.Services.DataReplication.Consensus.Data.Dao.RaftLogEntryCommit.Where(where, db);
			}
		   
			public RaftLogEntryCommitCollection Where(WhereDelegate<RaftLogEntryCommitColumns> where, OrderBy<RaftLogEntryCommitColumns> orderBy = null, Database db = null)
			{
				return Bam.Net.Services.DataReplication.Consensus.Data.Dao.RaftLogEntryCommit.Where(where, orderBy, db);
			}

			public RaftLogEntryCommit OneWhere(WhereDelegate<RaftLogEntryCommitColumns> where, Database db = null)
			{
				return Bam.Net.Services.DataReplication.Consensus.Data.Dao.RaftLogEntryCommit.OneWhere(where, db);
			}

			public static RaftLogEntryCommit GetOneWhere(WhereDelegate<RaftLogEntryCommitColumns> where, Database db = null)
			{
				return Bam.Net.Services.DataReplication.Consensus.Data.Dao.RaftLogEntryCommit.GetOneWhere(where, db);
			}
		
			public RaftLogEntryCommit FirstOneWhere(WhereDelegate<RaftLogEntryCommitColumns> where, Database db = null)
			{
				return Bam.Net.Services.DataReplication.Consensus.Data.Dao.RaftLogEntryCommit.FirstOneWhere(where, db);
			}

			public RaftLogEntryCommitCollection Top(int count, WhereDelegate<RaftLogEntryCommitColumns> where, Database db = null)
			{
				return Bam.Net.Services.DataReplication.Consensus.Data.Dao.RaftLogEntryCommit.Top(count, where, db);
			}

			public RaftLogEntryCommitCollection Top(int count, WhereDelegate<RaftLogEntryCommitColumns> where, OrderBy<RaftLogEntryCommitColumns> orderBy, Database db = null)
			{
				return Bam.Net.Services.DataReplication.Consensus.Data.Dao.RaftLogEntryCommit.Top(count, where, orderBy, db);
			}

			public long Count(WhereDelegate<RaftLogEntryCommitColumns> where, Database db = null)
			{
				return Bam.Net.Services.DataReplication.Consensus.Data.Dao.RaftLogEntryCommit.Count(where, db);
			}
	}

	static RaftLogEntryCommitQueryContext _raftLogEntryCommits;
	static object _raftLogEntryCommitsLock = new object();
	public static RaftLogEntryCommitQueryContext RaftLogEntryCommits
	{
		get
		{
			return _raftLogEntryCommitsLock.DoubleCheckLock<RaftLogEntryCommitQueryContext>(ref _raftLogEntryCommits, () => new RaftLogEntryCommitQueryContext());
		}
	}
	public class RaftNodeIdentifierQueryContext
	{
			public RaftNodeIdentifierCollection Where(WhereDelegate<RaftNodeIdentifierColumns> where, Database db = null)
			{
				return Bam.Net.Services.DataReplication.Consensus.Data.Dao.RaftNodeIdentifier.Where(where, db);
			}
		   
			public RaftNodeIdentifierCollection Where(WhereDelegate<RaftNodeIdentifierColumns> where, OrderBy<RaftNodeIdentifierColumns> orderBy = null, Database db = null)
			{
				return Bam.Net.Services.DataReplication.Consensus.Data.Dao.RaftNodeIdentifier.Where(where, orderBy, db);
			}

			public RaftNodeIdentifier OneWhere(WhereDelegate<RaftNodeIdentifierColumns> where, Database db = null)
			{
				return Bam.Net.Services.DataReplication.Consensus.Data.Dao.RaftNodeIdentifier.OneWhere(where, db);
			}

			public static RaftNodeIdentifier GetOneWhere(WhereDelegate<RaftNodeIdentifierColumns> where, Database db = null)
			{
				return Bam.Net.Services.DataReplication.Consensus.Data.Dao.RaftNodeIdentifier.GetOneWhere(where, db);
			}
		
			public RaftNodeIdentifier FirstOneWhere(WhereDelegate<RaftNodeIdentifierColumns> where, Database db = null)
			{
				return Bam.Net.Services.DataReplication.Consensus.Data.Dao.RaftNodeIdentifier.FirstOneWhere(where, db);
			}

			public RaftNodeIdentifierCollection Top(int count, WhereDelegate<RaftNodeIdentifierColumns> where, Database db = null)
			{
				return Bam.Net.Services.DataReplication.Consensus.Data.Dao.RaftNodeIdentifier.Top(count, where, db);
			}

			public RaftNodeIdentifierCollection Top(int count, WhereDelegate<RaftNodeIdentifierColumns> where, OrderBy<RaftNodeIdentifierColumns> orderBy, Database db = null)
			{
				return Bam.Net.Services.DataReplication.Consensus.Data.Dao.RaftNodeIdentifier.Top(count, where, orderBy, db);
			}

			public long Count(WhereDelegate<RaftNodeIdentifierColumns> where, Database db = null)
			{
				return Bam.Net.Services.DataReplication.Consensus.Data.Dao.RaftNodeIdentifier.Count(where, db);
			}
	}

	static RaftNodeIdentifierQueryContext _raftNodeIdentifiers;
	static object _raftNodeIdentifiersLock = new object();
	public static RaftNodeIdentifierQueryContext RaftNodeIdentifiers
	{
		get
		{
			return _raftNodeIdentifiersLock.DoubleCheckLock<RaftNodeIdentifierQueryContext>(ref _raftNodeIdentifiers, () => new RaftNodeIdentifierQueryContext());
		}
	}
	public class RaftLogEntryQueryContext
	{
			public RaftLogEntryCollection Where(WhereDelegate<RaftLogEntryColumns> where, Database db = null)
			{
				return Bam.Net.Services.DataReplication.Consensus.Data.Dao.RaftLogEntry.Where(where, db);
			}
		   
			public RaftLogEntryCollection Where(WhereDelegate<RaftLogEntryColumns> where, OrderBy<RaftLogEntryColumns> orderBy = null, Database db = null)
			{
				return Bam.Net.Services.DataReplication.Consensus.Data.Dao.RaftLogEntry.Where(where, orderBy, db);
			}

			public RaftLogEntry OneWhere(WhereDelegate<RaftLogEntryColumns> where, Database db = null)
			{
				return Bam.Net.Services.DataReplication.Consensus.Data.Dao.RaftLogEntry.OneWhere(where, db);
			}

			public static RaftLogEntry GetOneWhere(WhereDelegate<RaftLogEntryColumns> where, Database db = null)
			{
				return Bam.Net.Services.DataReplication.Consensus.Data.Dao.RaftLogEntry.GetOneWhere(where, db);
			}
		
			public RaftLogEntry FirstOneWhere(WhereDelegate<RaftLogEntryColumns> where, Database db = null)
			{
				return Bam.Net.Services.DataReplication.Consensus.Data.Dao.RaftLogEntry.FirstOneWhere(where, db);
			}

			public RaftLogEntryCollection Top(int count, WhereDelegate<RaftLogEntryColumns> where, Database db = null)
			{
				return Bam.Net.Services.DataReplication.Consensus.Data.Dao.RaftLogEntry.Top(count, where, db);
			}

			public RaftLogEntryCollection Top(int count, WhereDelegate<RaftLogEntryColumns> where, OrderBy<RaftLogEntryColumns> orderBy, Database db = null)
			{
				return Bam.Net.Services.DataReplication.Consensus.Data.Dao.RaftLogEntry.Top(count, where, orderBy, db);
			}

			public long Count(WhereDelegate<RaftLogEntryColumns> where, Database db = null)
			{
				return Bam.Net.Services.DataReplication.Consensus.Data.Dao.RaftLogEntry.Count(where, db);
			}
	}

	static RaftLogEntryQueryContext _raftLogEntries;
	static object _raftLogEntriesLock = new object();
	public static RaftLogEntryQueryContext RaftLogEntries
	{
		get
		{
			return _raftLogEntriesLock.DoubleCheckLock<RaftLogEntryQueryContext>(ref _raftLogEntries, () => new RaftLogEntryQueryContext());
		}
	}
	public class RaftLeaderElectionQueryContext
	{
			public RaftLeaderElectionCollection Where(WhereDelegate<RaftLeaderElectionColumns> where, Database db = null)
			{
				return Bam.Net.Services.DataReplication.Consensus.Data.Dao.RaftLeaderElection.Where(where, db);
			}
		   
			public RaftLeaderElectionCollection Where(WhereDelegate<RaftLeaderElectionColumns> where, OrderBy<RaftLeaderElectionColumns> orderBy = null, Database db = null)
			{
				return Bam.Net.Services.DataReplication.Consensus.Data.Dao.RaftLeaderElection.Where(where, orderBy, db);
			}

			public RaftLeaderElection OneWhere(WhereDelegate<RaftLeaderElectionColumns> where, Database db = null)
			{
				return Bam.Net.Services.DataReplication.Consensus.Data.Dao.RaftLeaderElection.OneWhere(where, db);
			}

			public static RaftLeaderElection GetOneWhere(WhereDelegate<RaftLeaderElectionColumns> where, Database db = null)
			{
				return Bam.Net.Services.DataReplication.Consensus.Data.Dao.RaftLeaderElection.GetOneWhere(where, db);
			}
		
			public RaftLeaderElection FirstOneWhere(WhereDelegate<RaftLeaderElectionColumns> where, Database db = null)
			{
				return Bam.Net.Services.DataReplication.Consensus.Data.Dao.RaftLeaderElection.FirstOneWhere(where, db);
			}

			public RaftLeaderElectionCollection Top(int count, WhereDelegate<RaftLeaderElectionColumns> where, Database db = null)
			{
				return Bam.Net.Services.DataReplication.Consensus.Data.Dao.RaftLeaderElection.Top(count, where, db);
			}

			public RaftLeaderElectionCollection Top(int count, WhereDelegate<RaftLeaderElectionColumns> where, OrderBy<RaftLeaderElectionColumns> orderBy, Database db = null)
			{
				return Bam.Net.Services.DataReplication.Consensus.Data.Dao.RaftLeaderElection.Top(count, where, orderBy, db);
			}

			public long Count(WhereDelegate<RaftLeaderElectionColumns> where, Database db = null)
			{
				return Bam.Net.Services.DataReplication.Consensus.Data.Dao.RaftLeaderElection.Count(where, db);
			}
	}

	static RaftLeaderElectionQueryContext _raftLeaderElections;
	static object _raftLeaderElectionsLock = new object();
	public static RaftLeaderElectionQueryContext RaftLeaderElections
	{
		get
		{
			return _raftLeaderElectionsLock.DoubleCheckLock<RaftLeaderElectionQueryContext>(ref _raftLeaderElections, () => new RaftLeaderElectionQueryContext());
		}
	}
	public class RaftVoteQueryContext
	{
			public RaftVoteCollection Where(WhereDelegate<RaftVoteColumns> where, Database db = null)
			{
				return Bam.Net.Services.DataReplication.Consensus.Data.Dao.RaftVote.Where(where, db);
			}
		   
			public RaftVoteCollection Where(WhereDelegate<RaftVoteColumns> where, OrderBy<RaftVoteColumns> orderBy = null, Database db = null)
			{
				return Bam.Net.Services.DataReplication.Consensus.Data.Dao.RaftVote.Where(where, orderBy, db);
			}

			public RaftVote OneWhere(WhereDelegate<RaftVoteColumns> where, Database db = null)
			{
				return Bam.Net.Services.DataReplication.Consensus.Data.Dao.RaftVote.OneWhere(where, db);
			}

			public static RaftVote GetOneWhere(WhereDelegate<RaftVoteColumns> where, Database db = null)
			{
				return Bam.Net.Services.DataReplication.Consensus.Data.Dao.RaftVote.GetOneWhere(where, db);
			}
		
			public RaftVote FirstOneWhere(WhereDelegate<RaftVoteColumns> where, Database db = null)
			{
				return Bam.Net.Services.DataReplication.Consensus.Data.Dao.RaftVote.FirstOneWhere(where, db);
			}

			public RaftVoteCollection Top(int count, WhereDelegate<RaftVoteColumns> where, Database db = null)
			{
				return Bam.Net.Services.DataReplication.Consensus.Data.Dao.RaftVote.Top(count, where, db);
			}

			public RaftVoteCollection Top(int count, WhereDelegate<RaftVoteColumns> where, OrderBy<RaftVoteColumns> orderBy, Database db = null)
			{
				return Bam.Net.Services.DataReplication.Consensus.Data.Dao.RaftVote.Top(count, where, orderBy, db);
			}

			public long Count(WhereDelegate<RaftVoteColumns> where, Database db = null)
			{
				return Bam.Net.Services.DataReplication.Consensus.Data.Dao.RaftVote.Count(where, db);
			}
	}

	static RaftVoteQueryContext _raftVotes;
	static object _raftVotesLock = new object();
	public static RaftVoteQueryContext RaftVotes
	{
		get
		{
			return _raftVotesLock.DoubleCheckLock<RaftVoteQueryContext>(ref _raftVotes, () => new RaftVoteQueryContext());
		}
	}
    }
}																								
