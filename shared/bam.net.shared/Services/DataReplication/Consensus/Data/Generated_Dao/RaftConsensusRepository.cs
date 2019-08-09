/*
This file was generated and should not be modified directly
*/
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Threading.Tasks;
using Bam.Net;
using Bam.Net.Data;
using Bam.Net.Data.Repositories;
using Bam.Net.Services.DataReplication.Consensus.Data;

namespace Bam.Net.Services.DataReplication.Consensus.Data.Dao.Repository
{
	[Serializable]
	public class RaftConsensusRepository: DaoRepository
	{
		public RaftConsensusRepository()
		{
			SchemaName = "RaftConsensus";
			BaseNamespace = "Bam.Net.Services.DataReplication.Consensus.Data";			

			
			AddType<Bam.Net.Services.DataReplication.Consensus.Data.RaftFollowerWriteLog>();
			
			
			AddType<Bam.Net.Services.DataReplication.Consensus.Data.RaftLogEntryCommit>();
			
			
			AddType<Bam.Net.Services.DataReplication.Consensus.Data.RaftNodeIdentifier>();
			
			
			AddType<Bam.Net.Services.DataReplication.Consensus.Data.RaftLogEntry>();
			
			
			AddType<Bam.Net.Services.DataReplication.Consensus.Data.RaftLeaderElection>();
			
			
			AddType<Bam.Net.Services.DataReplication.Consensus.Data.RaftVote>();
			

			DaoAssembly = typeof(RaftConsensusRepository).Assembly;
		}

		object _addLock = new object();
        public override void AddType(Type type)
        {
            lock (_addLock)
            {
                base.AddType(type);
                DaoAssembly = typeof(RaftConsensusRepository).Assembly;
            }
        }

		
		/// <summary>
		/// Set one entry matching the specified filter.  If none exists 
		/// one is created; success depends on the nullability
		/// of the specified columns.
		/// </summary>
		public void SetOneRaftFollowerWriteLogWhere(WhereDelegate<RaftFollowerWriteLogColumns> where)
		{
			Bam.Net.Services.DataReplication.Consensus.Data.Dao.RaftFollowerWriteLog.SetOneWhere(where, Database);
		}

		/// <summary>
		/// Set one entry matching the specified filter.  If none exists 
		/// one is created; success depends on the nullability
		/// of the specified columns.
		/// </summary>
		public void SetOneRaftFollowerWriteLogWhere(WhereDelegate<RaftFollowerWriteLogColumns> where, out Bam.Net.Services.DataReplication.Consensus.Data.RaftFollowerWriteLog result)
		{
			Bam.Net.Services.DataReplication.Consensus.Data.Dao.RaftFollowerWriteLog.SetOneWhere(where, out Bam.Net.Services.DataReplication.Consensus.Data.Dao.RaftFollowerWriteLog daoResult, Database);
			result = daoResult.CopyAs<Bam.Net.Services.DataReplication.Consensus.Data.RaftFollowerWriteLog>();
		}

		/// <summary>
		/// Get one entry matching the specified filter.  If none exists 
		/// one is created; success depends on the nullability
		/// of the specified columns.
		/// </summary>
		/// <param name="where"></param>
		public Bam.Net.Services.DataReplication.Consensus.Data.RaftFollowerWriteLog GetOneRaftFollowerWriteLogWhere(WhereDelegate<RaftFollowerWriteLogColumns> where)
		{
			Type wrapperType = GetWrapperType<Bam.Net.Services.DataReplication.Consensus.Data.RaftFollowerWriteLog>();
			return (Bam.Net.Services.DataReplication.Consensus.Data.RaftFollowerWriteLog)Bam.Net.Services.DataReplication.Consensus.Data.Dao.RaftFollowerWriteLog.GetOneWhere(where, Database)?.CopyAs(wrapperType, this);
		}

		/// <summary>
		/// Execute a query that should return only one result.  If no result is found null is returned.  If more
		/// than one result is returned a MultipleEntriesFoundException is thrown.  This method is most commonly used to retrieve a
		/// single RaftFollowerWriteLog instance by its Id/Key value
		/// </summary>
		/// <param name="where">A WhereDelegate that receives a RaftFollowerWriteLogColumns 
		/// and returns a IQueryFilter which is the result of any comparisons
		/// between RaftFollowerWriteLogColumns and other values
		/// </param>
		public Bam.Net.Services.DataReplication.Consensus.Data.RaftFollowerWriteLog OneRaftFollowerWriteLogWhere(WhereDelegate<RaftFollowerWriteLogColumns> where)
        {
            Type wrapperType = GetWrapperType<Bam.Net.Services.DataReplication.Consensus.Data.RaftFollowerWriteLog>();
            return (Bam.Net.Services.DataReplication.Consensus.Data.RaftFollowerWriteLog)Bam.Net.Services.DataReplication.Consensus.Data.Dao.RaftFollowerWriteLog.OneWhere(where, Database)?.CopyAs(wrapperType, this);
        }

		/// <summary>
		/// Execute a query and return the results. 
		/// </summary>
		/// <param name="where">A WhereDelegate that receives a Bam.Net.Services.DataReplication.Consensus.Data.RaftFollowerWriteLogColumns 
		/// and returns a IQueryFilter which is the result of any comparisons
		/// between Bam.Net.Services.DataReplication.Consensus.Data.RaftFollowerWriteLogColumns and other values
		/// </param>
		public IEnumerable<Bam.Net.Services.DataReplication.Consensus.Data.RaftFollowerWriteLog> RaftFollowerWriteLogsWhere(WhereDelegate<RaftFollowerWriteLogColumns> where, OrderBy<RaftFollowerWriteLogColumns> orderBy = null)
        {
            return Wrap<Bam.Net.Services.DataReplication.Consensus.Data.RaftFollowerWriteLog>(Bam.Net.Services.DataReplication.Consensus.Data.Dao.RaftFollowerWriteLog.Where(where, orderBy, Database));
        }
		
		/// <summary>
		/// Execute a query and return the specified number
		/// of values. This method issues a sql TOP clause so only the 
		/// specified number of values will be returned.
		/// </summary>
		/// <param name="count">The number of values to return.
		/// This value is used in the sql query so no more than this 
		/// number of values will be returned by the database.
		/// </param>
		/// <param name="where">A WhereDelegate that receives a RaftFollowerWriteLogColumns 
		/// and returns a IQueryFilter which is the result of any comparisons
		/// between RaftFollowerWriteLogColumns and other values
		/// </param>
		public IEnumerable<Bam.Net.Services.DataReplication.Consensus.Data.RaftFollowerWriteLog> TopRaftFollowerWriteLogsWhere(int count, WhereDelegate<RaftFollowerWriteLogColumns> where)
        {
            return Wrap<Bam.Net.Services.DataReplication.Consensus.Data.RaftFollowerWriteLog>(Bam.Net.Services.DataReplication.Consensus.Data.Dao.RaftFollowerWriteLog.Top(count, where, Database));
        }

        public IEnumerable<Bam.Net.Services.DataReplication.Consensus.Data.RaftFollowerWriteLog> TopRaftFollowerWriteLogsWhere(int count, WhereDelegate<RaftFollowerWriteLogColumns> where, OrderBy<RaftFollowerWriteLogColumns> orderBy)
        {
            return Wrap<Bam.Net.Services.DataReplication.Consensus.Data.RaftFollowerWriteLog>(Bam.Net.Services.DataReplication.Consensus.Data.Dao.RaftFollowerWriteLog.Top(count, where, orderBy, Database));
        }
                                
		/// <summary>
		/// Return the count of RaftFollowerWriteLogs
		/// </summary>
		public long CountRaftFollowerWriteLogs()
        {
            return Bam.Net.Services.DataReplication.Consensus.Data.Dao.RaftFollowerWriteLog.Count(Database);
        }

		/// <summary>
		/// Execute a query and return the number of results
		/// </summary>
		/// <param name="where">A WhereDelegate that receives a RaftFollowerWriteLogColumns 
		/// and returns a IQueryFilter which is the result of any comparisons
		/// between RaftFollowerWriteLogColumns and other values
		/// </param>
        public long CountRaftFollowerWriteLogsWhere(WhereDelegate<RaftFollowerWriteLogColumns> where)
        {
            return Bam.Net.Services.DataReplication.Consensus.Data.Dao.RaftFollowerWriteLog.Count(where, Database);
        }
        
        public async Task BatchQueryRaftFollowerWriteLogs(int batchSize, WhereDelegate<RaftFollowerWriteLogColumns> where, Action<IEnumerable<Bam.Net.Services.DataReplication.Consensus.Data.RaftFollowerWriteLog>> batchProcessor)
        {
            await Bam.Net.Services.DataReplication.Consensus.Data.Dao.RaftFollowerWriteLog.BatchQuery(batchSize, where, (batch) =>
            {
				batchProcessor(Wrap<Bam.Net.Services.DataReplication.Consensus.Data.RaftFollowerWriteLog>(batch));
            }, Database);
        }
		
        public async Task BatchAllRaftFollowerWriteLogs(int batchSize, Action<IEnumerable<Bam.Net.Services.DataReplication.Consensus.Data.RaftFollowerWriteLog>> batchProcessor)
        {
            await Bam.Net.Services.DataReplication.Consensus.Data.Dao.RaftFollowerWriteLog.BatchAll(batchSize, (batch) =>
            {
				batchProcessor(Wrap<Bam.Net.Services.DataReplication.Consensus.Data.RaftFollowerWriteLog>(batch));
            }, Database);
        }

		
		/// <summary>
		/// Set one entry matching the specified filter.  If none exists 
		/// one is created; success depends on the nullability
		/// of the specified columns.
		/// </summary>
		public void SetOneRaftLogEntryCommitWhere(WhereDelegate<RaftLogEntryCommitColumns> where)
		{
			Bam.Net.Services.DataReplication.Consensus.Data.Dao.RaftLogEntryCommit.SetOneWhere(where, Database);
		}

		/// <summary>
		/// Set one entry matching the specified filter.  If none exists 
		/// one is created; success depends on the nullability
		/// of the specified columns.
		/// </summary>
		public void SetOneRaftLogEntryCommitWhere(WhereDelegate<RaftLogEntryCommitColumns> where, out Bam.Net.Services.DataReplication.Consensus.Data.RaftLogEntryCommit result)
		{
			Bam.Net.Services.DataReplication.Consensus.Data.Dao.RaftLogEntryCommit.SetOneWhere(where, out Bam.Net.Services.DataReplication.Consensus.Data.Dao.RaftLogEntryCommit daoResult, Database);
			result = daoResult.CopyAs<Bam.Net.Services.DataReplication.Consensus.Data.RaftLogEntryCommit>();
		}

		/// <summary>
		/// Get one entry matching the specified filter.  If none exists 
		/// one is created; success depends on the nullability
		/// of the specified columns.
		/// </summary>
		/// <param name="where"></param>
		public Bam.Net.Services.DataReplication.Consensus.Data.RaftLogEntryCommit GetOneRaftLogEntryCommitWhere(WhereDelegate<RaftLogEntryCommitColumns> where)
		{
			Type wrapperType = GetWrapperType<Bam.Net.Services.DataReplication.Consensus.Data.RaftLogEntryCommit>();
			return (Bam.Net.Services.DataReplication.Consensus.Data.RaftLogEntryCommit)Bam.Net.Services.DataReplication.Consensus.Data.Dao.RaftLogEntryCommit.GetOneWhere(where, Database)?.CopyAs(wrapperType, this);
		}

		/// <summary>
		/// Execute a query that should return only one result.  If no result is found null is returned.  If more
		/// than one result is returned a MultipleEntriesFoundException is thrown.  This method is most commonly used to retrieve a
		/// single RaftLogEntryCommit instance by its Id/Key value
		/// </summary>
		/// <param name="where">A WhereDelegate that receives a RaftLogEntryCommitColumns 
		/// and returns a IQueryFilter which is the result of any comparisons
		/// between RaftLogEntryCommitColumns and other values
		/// </param>
		public Bam.Net.Services.DataReplication.Consensus.Data.RaftLogEntryCommit OneRaftLogEntryCommitWhere(WhereDelegate<RaftLogEntryCommitColumns> where)
        {
            Type wrapperType = GetWrapperType<Bam.Net.Services.DataReplication.Consensus.Data.RaftLogEntryCommit>();
            return (Bam.Net.Services.DataReplication.Consensus.Data.RaftLogEntryCommit)Bam.Net.Services.DataReplication.Consensus.Data.Dao.RaftLogEntryCommit.OneWhere(where, Database)?.CopyAs(wrapperType, this);
        }

		/// <summary>
		/// Execute a query and return the results. 
		/// </summary>
		/// <param name="where">A WhereDelegate that receives a Bam.Net.Services.DataReplication.Consensus.Data.RaftLogEntryCommitColumns 
		/// and returns a IQueryFilter which is the result of any comparisons
		/// between Bam.Net.Services.DataReplication.Consensus.Data.RaftLogEntryCommitColumns and other values
		/// </param>
		public IEnumerable<Bam.Net.Services.DataReplication.Consensus.Data.RaftLogEntryCommit> RaftLogEntryCommitsWhere(WhereDelegate<RaftLogEntryCommitColumns> where, OrderBy<RaftLogEntryCommitColumns> orderBy = null)
        {
            return Wrap<Bam.Net.Services.DataReplication.Consensus.Data.RaftLogEntryCommit>(Bam.Net.Services.DataReplication.Consensus.Data.Dao.RaftLogEntryCommit.Where(where, orderBy, Database));
        }
		
		/// <summary>
		/// Execute a query and return the specified number
		/// of values. This method issues a sql TOP clause so only the 
		/// specified number of values will be returned.
		/// </summary>
		/// <param name="count">The number of values to return.
		/// This value is used in the sql query so no more than this 
		/// number of values will be returned by the database.
		/// </param>
		/// <param name="where">A WhereDelegate that receives a RaftLogEntryCommitColumns 
		/// and returns a IQueryFilter which is the result of any comparisons
		/// between RaftLogEntryCommitColumns and other values
		/// </param>
		public IEnumerable<Bam.Net.Services.DataReplication.Consensus.Data.RaftLogEntryCommit> TopRaftLogEntryCommitsWhere(int count, WhereDelegate<RaftLogEntryCommitColumns> where)
        {
            return Wrap<Bam.Net.Services.DataReplication.Consensus.Data.RaftLogEntryCommit>(Bam.Net.Services.DataReplication.Consensus.Data.Dao.RaftLogEntryCommit.Top(count, where, Database));
        }

        public IEnumerable<Bam.Net.Services.DataReplication.Consensus.Data.RaftLogEntryCommit> TopRaftLogEntryCommitsWhere(int count, WhereDelegate<RaftLogEntryCommitColumns> where, OrderBy<RaftLogEntryCommitColumns> orderBy)
        {
            return Wrap<Bam.Net.Services.DataReplication.Consensus.Data.RaftLogEntryCommit>(Bam.Net.Services.DataReplication.Consensus.Data.Dao.RaftLogEntryCommit.Top(count, where, orderBy, Database));
        }
                                
		/// <summary>
		/// Return the count of RaftLogEntryCommits
		/// </summary>
		public long CountRaftLogEntryCommits()
        {
            return Bam.Net.Services.DataReplication.Consensus.Data.Dao.RaftLogEntryCommit.Count(Database);
        }

		/// <summary>
		/// Execute a query and return the number of results
		/// </summary>
		/// <param name="where">A WhereDelegate that receives a RaftLogEntryCommitColumns 
		/// and returns a IQueryFilter which is the result of any comparisons
		/// between RaftLogEntryCommitColumns and other values
		/// </param>
        public long CountRaftLogEntryCommitsWhere(WhereDelegate<RaftLogEntryCommitColumns> where)
        {
            return Bam.Net.Services.DataReplication.Consensus.Data.Dao.RaftLogEntryCommit.Count(where, Database);
        }
        
        public async Task BatchQueryRaftLogEntryCommits(int batchSize, WhereDelegate<RaftLogEntryCommitColumns> where, Action<IEnumerable<Bam.Net.Services.DataReplication.Consensus.Data.RaftLogEntryCommit>> batchProcessor)
        {
            await Bam.Net.Services.DataReplication.Consensus.Data.Dao.RaftLogEntryCommit.BatchQuery(batchSize, where, (batch) =>
            {
				batchProcessor(Wrap<Bam.Net.Services.DataReplication.Consensus.Data.RaftLogEntryCommit>(batch));
            }, Database);
        }
		
        public async Task BatchAllRaftLogEntryCommits(int batchSize, Action<IEnumerable<Bam.Net.Services.DataReplication.Consensus.Data.RaftLogEntryCommit>> batchProcessor)
        {
            await Bam.Net.Services.DataReplication.Consensus.Data.Dao.RaftLogEntryCommit.BatchAll(batchSize, (batch) =>
            {
				batchProcessor(Wrap<Bam.Net.Services.DataReplication.Consensus.Data.RaftLogEntryCommit>(batch));
            }, Database);
        }

		
		/// <summary>
		/// Set one entry matching the specified filter.  If none exists 
		/// one is created; success depends on the nullability
		/// of the specified columns.
		/// </summary>
		public void SetOneRaftNodeIdentifierWhere(WhereDelegate<RaftNodeIdentifierColumns> where)
		{
			Bam.Net.Services.DataReplication.Consensus.Data.Dao.RaftNodeIdentifier.SetOneWhere(where, Database);
		}

		/// <summary>
		/// Set one entry matching the specified filter.  If none exists 
		/// one is created; success depends on the nullability
		/// of the specified columns.
		/// </summary>
		public void SetOneRaftNodeIdentifierWhere(WhereDelegate<RaftNodeIdentifierColumns> where, out Bam.Net.Services.DataReplication.Consensus.Data.RaftNodeIdentifier result)
		{
			Bam.Net.Services.DataReplication.Consensus.Data.Dao.RaftNodeIdentifier.SetOneWhere(where, out Bam.Net.Services.DataReplication.Consensus.Data.Dao.RaftNodeIdentifier daoResult, Database);
			result = daoResult.CopyAs<Bam.Net.Services.DataReplication.Consensus.Data.RaftNodeIdentifier>();
		}

		/// <summary>
		/// Get one entry matching the specified filter.  If none exists 
		/// one is created; success depends on the nullability
		/// of the specified columns.
		/// </summary>
		/// <param name="where"></param>
		public Bam.Net.Services.DataReplication.Consensus.Data.RaftNodeIdentifier GetOneRaftNodeIdentifierWhere(WhereDelegate<RaftNodeIdentifierColumns> where)
		{
			Type wrapperType = GetWrapperType<Bam.Net.Services.DataReplication.Consensus.Data.RaftNodeIdentifier>();
			return (Bam.Net.Services.DataReplication.Consensus.Data.RaftNodeIdentifier)Bam.Net.Services.DataReplication.Consensus.Data.Dao.RaftNodeIdentifier.GetOneWhere(where, Database)?.CopyAs(wrapperType, this);
		}

		/// <summary>
		/// Execute a query that should return only one result.  If no result is found null is returned.  If more
		/// than one result is returned a MultipleEntriesFoundException is thrown.  This method is most commonly used to retrieve a
		/// single RaftNodeIdentifier instance by its Id/Key value
		/// </summary>
		/// <param name="where">A WhereDelegate that receives a RaftNodeIdentifierColumns 
		/// and returns a IQueryFilter which is the result of any comparisons
		/// between RaftNodeIdentifierColumns and other values
		/// </param>
		public Bam.Net.Services.DataReplication.Consensus.Data.RaftNodeIdentifier OneRaftNodeIdentifierWhere(WhereDelegate<RaftNodeIdentifierColumns> where)
        {
            Type wrapperType = GetWrapperType<Bam.Net.Services.DataReplication.Consensus.Data.RaftNodeIdentifier>();
            return (Bam.Net.Services.DataReplication.Consensus.Data.RaftNodeIdentifier)Bam.Net.Services.DataReplication.Consensus.Data.Dao.RaftNodeIdentifier.OneWhere(where, Database)?.CopyAs(wrapperType, this);
        }

		/// <summary>
		/// Execute a query and return the results. 
		/// </summary>
		/// <param name="where">A WhereDelegate that receives a Bam.Net.Services.DataReplication.Consensus.Data.RaftNodeIdentifierColumns 
		/// and returns a IQueryFilter which is the result of any comparisons
		/// between Bam.Net.Services.DataReplication.Consensus.Data.RaftNodeIdentifierColumns and other values
		/// </param>
		public IEnumerable<Bam.Net.Services.DataReplication.Consensus.Data.RaftNodeIdentifier> RaftNodeIdentifiersWhere(WhereDelegate<RaftNodeIdentifierColumns> where, OrderBy<RaftNodeIdentifierColumns> orderBy = null)
        {
            return Wrap<Bam.Net.Services.DataReplication.Consensus.Data.RaftNodeIdentifier>(Bam.Net.Services.DataReplication.Consensus.Data.Dao.RaftNodeIdentifier.Where(where, orderBy, Database));
        }
		
		/// <summary>
		/// Execute a query and return the specified number
		/// of values. This method issues a sql TOP clause so only the 
		/// specified number of values will be returned.
		/// </summary>
		/// <param name="count">The number of values to return.
		/// This value is used in the sql query so no more than this 
		/// number of values will be returned by the database.
		/// </param>
		/// <param name="where">A WhereDelegate that receives a RaftNodeIdentifierColumns 
		/// and returns a IQueryFilter which is the result of any comparisons
		/// between RaftNodeIdentifierColumns and other values
		/// </param>
		public IEnumerable<Bam.Net.Services.DataReplication.Consensus.Data.RaftNodeIdentifier> TopRaftNodeIdentifiersWhere(int count, WhereDelegate<RaftNodeIdentifierColumns> where)
        {
            return Wrap<Bam.Net.Services.DataReplication.Consensus.Data.RaftNodeIdentifier>(Bam.Net.Services.DataReplication.Consensus.Data.Dao.RaftNodeIdentifier.Top(count, where, Database));
        }

        public IEnumerable<Bam.Net.Services.DataReplication.Consensus.Data.RaftNodeIdentifier> TopRaftNodeIdentifiersWhere(int count, WhereDelegate<RaftNodeIdentifierColumns> where, OrderBy<RaftNodeIdentifierColumns> orderBy)
        {
            return Wrap<Bam.Net.Services.DataReplication.Consensus.Data.RaftNodeIdentifier>(Bam.Net.Services.DataReplication.Consensus.Data.Dao.RaftNodeIdentifier.Top(count, where, orderBy, Database));
        }
                                
		/// <summary>
		/// Return the count of RaftNodeIdentifiers
		/// </summary>
		public long CountRaftNodeIdentifiers()
        {
            return Bam.Net.Services.DataReplication.Consensus.Data.Dao.RaftNodeIdentifier.Count(Database);
        }

		/// <summary>
		/// Execute a query and return the number of results
		/// </summary>
		/// <param name="where">A WhereDelegate that receives a RaftNodeIdentifierColumns 
		/// and returns a IQueryFilter which is the result of any comparisons
		/// between RaftNodeIdentifierColumns and other values
		/// </param>
        public long CountRaftNodeIdentifiersWhere(WhereDelegate<RaftNodeIdentifierColumns> where)
        {
            return Bam.Net.Services.DataReplication.Consensus.Data.Dao.RaftNodeIdentifier.Count(where, Database);
        }
        
        public async Task BatchQueryRaftNodeIdentifiers(int batchSize, WhereDelegate<RaftNodeIdentifierColumns> where, Action<IEnumerable<Bam.Net.Services.DataReplication.Consensus.Data.RaftNodeIdentifier>> batchProcessor)
        {
            await Bam.Net.Services.DataReplication.Consensus.Data.Dao.RaftNodeIdentifier.BatchQuery(batchSize, where, (batch) =>
            {
				batchProcessor(Wrap<Bam.Net.Services.DataReplication.Consensus.Data.RaftNodeIdentifier>(batch));
            }, Database);
        }
		
        public async Task BatchAllRaftNodeIdentifiers(int batchSize, Action<IEnumerable<Bam.Net.Services.DataReplication.Consensus.Data.RaftNodeIdentifier>> batchProcessor)
        {
            await Bam.Net.Services.DataReplication.Consensus.Data.Dao.RaftNodeIdentifier.BatchAll(batchSize, (batch) =>
            {
				batchProcessor(Wrap<Bam.Net.Services.DataReplication.Consensus.Data.RaftNodeIdentifier>(batch));
            }, Database);
        }

		
		/// <summary>
		/// Set one entry matching the specified filter.  If none exists 
		/// one is created; success depends on the nullability
		/// of the specified columns.
		/// </summary>
		public void SetOneRaftLogEntryWhere(WhereDelegate<RaftLogEntryColumns> where)
		{
			Bam.Net.Services.DataReplication.Consensus.Data.Dao.RaftLogEntry.SetOneWhere(where, Database);
		}

		/// <summary>
		/// Set one entry matching the specified filter.  If none exists 
		/// one is created; success depends on the nullability
		/// of the specified columns.
		/// </summary>
		public void SetOneRaftLogEntryWhere(WhereDelegate<RaftLogEntryColumns> where, out Bam.Net.Services.DataReplication.Consensus.Data.RaftLogEntry result)
		{
			Bam.Net.Services.DataReplication.Consensus.Data.Dao.RaftLogEntry.SetOneWhere(where, out Bam.Net.Services.DataReplication.Consensus.Data.Dao.RaftLogEntry daoResult, Database);
			result = daoResult.CopyAs<Bam.Net.Services.DataReplication.Consensus.Data.RaftLogEntry>();
		}

		/// <summary>
		/// Get one entry matching the specified filter.  If none exists 
		/// one is created; success depends on the nullability
		/// of the specified columns.
		/// </summary>
		/// <param name="where"></param>
		public Bam.Net.Services.DataReplication.Consensus.Data.RaftLogEntry GetOneRaftLogEntryWhere(WhereDelegate<RaftLogEntryColumns> where)
		{
			Type wrapperType = GetWrapperType<Bam.Net.Services.DataReplication.Consensus.Data.RaftLogEntry>();
			return (Bam.Net.Services.DataReplication.Consensus.Data.RaftLogEntry)Bam.Net.Services.DataReplication.Consensus.Data.Dao.RaftLogEntry.GetOneWhere(where, Database)?.CopyAs(wrapperType, this);
		}

		/// <summary>
		/// Execute a query that should return only one result.  If no result is found null is returned.  If more
		/// than one result is returned a MultipleEntriesFoundException is thrown.  This method is most commonly used to retrieve a
		/// single RaftLogEntry instance by its Id/Key value
		/// </summary>
		/// <param name="where">A WhereDelegate that receives a RaftLogEntryColumns 
		/// and returns a IQueryFilter which is the result of any comparisons
		/// between RaftLogEntryColumns and other values
		/// </param>
		public Bam.Net.Services.DataReplication.Consensus.Data.RaftLogEntry OneRaftLogEntryWhere(WhereDelegate<RaftLogEntryColumns> where)
        {
            Type wrapperType = GetWrapperType<Bam.Net.Services.DataReplication.Consensus.Data.RaftLogEntry>();
            return (Bam.Net.Services.DataReplication.Consensus.Data.RaftLogEntry)Bam.Net.Services.DataReplication.Consensus.Data.Dao.RaftLogEntry.OneWhere(where, Database)?.CopyAs(wrapperType, this);
        }

		/// <summary>
		/// Execute a query and return the results. 
		/// </summary>
		/// <param name="where">A WhereDelegate that receives a Bam.Net.Services.DataReplication.Consensus.Data.RaftLogEntryColumns 
		/// and returns a IQueryFilter which is the result of any comparisons
		/// between Bam.Net.Services.DataReplication.Consensus.Data.RaftLogEntryColumns and other values
		/// </param>
		public IEnumerable<Bam.Net.Services.DataReplication.Consensus.Data.RaftLogEntry> RaftLogEntriesWhere(WhereDelegate<RaftLogEntryColumns> where, OrderBy<RaftLogEntryColumns> orderBy = null)
        {
            return Wrap<Bam.Net.Services.DataReplication.Consensus.Data.RaftLogEntry>(Bam.Net.Services.DataReplication.Consensus.Data.Dao.RaftLogEntry.Where(where, orderBy, Database));
        }
		
		/// <summary>
		/// Execute a query and return the specified number
		/// of values. This method issues a sql TOP clause so only the 
		/// specified number of values will be returned.
		/// </summary>
		/// <param name="count">The number of values to return.
		/// This value is used in the sql query so no more than this 
		/// number of values will be returned by the database.
		/// </param>
		/// <param name="where">A WhereDelegate that receives a RaftLogEntryColumns 
		/// and returns a IQueryFilter which is the result of any comparisons
		/// between RaftLogEntryColumns and other values
		/// </param>
		public IEnumerable<Bam.Net.Services.DataReplication.Consensus.Data.RaftLogEntry> TopRaftLogEntriesWhere(int count, WhereDelegate<RaftLogEntryColumns> where)
        {
            return Wrap<Bam.Net.Services.DataReplication.Consensus.Data.RaftLogEntry>(Bam.Net.Services.DataReplication.Consensus.Data.Dao.RaftLogEntry.Top(count, where, Database));
        }

        public IEnumerable<Bam.Net.Services.DataReplication.Consensus.Data.RaftLogEntry> TopRaftLogEntriesWhere(int count, WhereDelegate<RaftLogEntryColumns> where, OrderBy<RaftLogEntryColumns> orderBy)
        {
            return Wrap<Bam.Net.Services.DataReplication.Consensus.Data.RaftLogEntry>(Bam.Net.Services.DataReplication.Consensus.Data.Dao.RaftLogEntry.Top(count, where, orderBy, Database));
        }
                                
		/// <summary>
		/// Return the count of RaftLogEntries
		/// </summary>
		public long CountRaftLogEntries()
        {
            return Bam.Net.Services.DataReplication.Consensus.Data.Dao.RaftLogEntry.Count(Database);
        }

		/// <summary>
		/// Execute a query and return the number of results
		/// </summary>
		/// <param name="where">A WhereDelegate that receives a RaftLogEntryColumns 
		/// and returns a IQueryFilter which is the result of any comparisons
		/// between RaftLogEntryColumns and other values
		/// </param>
        public long CountRaftLogEntriesWhere(WhereDelegate<RaftLogEntryColumns> where)
        {
            return Bam.Net.Services.DataReplication.Consensus.Data.Dao.RaftLogEntry.Count(where, Database);
        }
        
        public async Task BatchQueryRaftLogEntries(int batchSize, WhereDelegate<RaftLogEntryColumns> where, Action<IEnumerable<Bam.Net.Services.DataReplication.Consensus.Data.RaftLogEntry>> batchProcessor)
        {
            await Bam.Net.Services.DataReplication.Consensus.Data.Dao.RaftLogEntry.BatchQuery(batchSize, where, (batch) =>
            {
				batchProcessor(Wrap<Bam.Net.Services.DataReplication.Consensus.Data.RaftLogEntry>(batch));
            }, Database);
        }
		
        public async Task BatchAllRaftLogEntries(int batchSize, Action<IEnumerable<Bam.Net.Services.DataReplication.Consensus.Data.RaftLogEntry>> batchProcessor)
        {
            await Bam.Net.Services.DataReplication.Consensus.Data.Dao.RaftLogEntry.BatchAll(batchSize, (batch) =>
            {
				batchProcessor(Wrap<Bam.Net.Services.DataReplication.Consensus.Data.RaftLogEntry>(batch));
            }, Database);
        }

		
		/// <summary>
		/// Set one entry matching the specified filter.  If none exists 
		/// one is created; success depends on the nullability
		/// of the specified columns.
		/// </summary>
		public void SetOneRaftLeaderElectionWhere(WhereDelegate<RaftLeaderElectionColumns> where)
		{
			Bam.Net.Services.DataReplication.Consensus.Data.Dao.RaftLeaderElection.SetOneWhere(where, Database);
		}

		/// <summary>
		/// Set one entry matching the specified filter.  If none exists 
		/// one is created; success depends on the nullability
		/// of the specified columns.
		/// </summary>
		public void SetOneRaftLeaderElectionWhere(WhereDelegate<RaftLeaderElectionColumns> where, out Bam.Net.Services.DataReplication.Consensus.Data.RaftLeaderElection result)
		{
			Bam.Net.Services.DataReplication.Consensus.Data.Dao.RaftLeaderElection.SetOneWhere(where, out Bam.Net.Services.DataReplication.Consensus.Data.Dao.RaftLeaderElection daoResult, Database);
			result = daoResult.CopyAs<Bam.Net.Services.DataReplication.Consensus.Data.RaftLeaderElection>();
		}

		/// <summary>
		/// Get one entry matching the specified filter.  If none exists 
		/// one is created; success depends on the nullability
		/// of the specified columns.
		/// </summary>
		/// <param name="where"></param>
		public Bam.Net.Services.DataReplication.Consensus.Data.RaftLeaderElection GetOneRaftLeaderElectionWhere(WhereDelegate<RaftLeaderElectionColumns> where)
		{
			Type wrapperType = GetWrapperType<Bam.Net.Services.DataReplication.Consensus.Data.RaftLeaderElection>();
			return (Bam.Net.Services.DataReplication.Consensus.Data.RaftLeaderElection)Bam.Net.Services.DataReplication.Consensus.Data.Dao.RaftLeaderElection.GetOneWhere(where, Database)?.CopyAs(wrapperType, this);
		}

		/// <summary>
		/// Execute a query that should return only one result.  If no result is found null is returned.  If more
		/// than one result is returned a MultipleEntriesFoundException is thrown.  This method is most commonly used to retrieve a
		/// single RaftLeaderElection instance by its Id/Key value
		/// </summary>
		/// <param name="where">A WhereDelegate that receives a RaftLeaderElectionColumns 
		/// and returns a IQueryFilter which is the result of any comparisons
		/// between RaftLeaderElectionColumns and other values
		/// </param>
		public Bam.Net.Services.DataReplication.Consensus.Data.RaftLeaderElection OneRaftLeaderElectionWhere(WhereDelegate<RaftLeaderElectionColumns> where)
        {
            Type wrapperType = GetWrapperType<Bam.Net.Services.DataReplication.Consensus.Data.RaftLeaderElection>();
            return (Bam.Net.Services.DataReplication.Consensus.Data.RaftLeaderElection)Bam.Net.Services.DataReplication.Consensus.Data.Dao.RaftLeaderElection.OneWhere(where, Database)?.CopyAs(wrapperType, this);
        }

		/// <summary>
		/// Execute a query and return the results. 
		/// </summary>
		/// <param name="where">A WhereDelegate that receives a Bam.Net.Services.DataReplication.Consensus.Data.RaftLeaderElectionColumns 
		/// and returns a IQueryFilter which is the result of any comparisons
		/// between Bam.Net.Services.DataReplication.Consensus.Data.RaftLeaderElectionColumns and other values
		/// </param>
		public IEnumerable<Bam.Net.Services.DataReplication.Consensus.Data.RaftLeaderElection> RaftLeaderElectionsWhere(WhereDelegate<RaftLeaderElectionColumns> where, OrderBy<RaftLeaderElectionColumns> orderBy = null)
        {
            return Wrap<Bam.Net.Services.DataReplication.Consensus.Data.RaftLeaderElection>(Bam.Net.Services.DataReplication.Consensus.Data.Dao.RaftLeaderElection.Where(where, orderBy, Database));
        }
		
		/// <summary>
		/// Execute a query and return the specified number
		/// of values. This method issues a sql TOP clause so only the 
		/// specified number of values will be returned.
		/// </summary>
		/// <param name="count">The number of values to return.
		/// This value is used in the sql query so no more than this 
		/// number of values will be returned by the database.
		/// </param>
		/// <param name="where">A WhereDelegate that receives a RaftLeaderElectionColumns 
		/// and returns a IQueryFilter which is the result of any comparisons
		/// between RaftLeaderElectionColumns and other values
		/// </param>
		public IEnumerable<Bam.Net.Services.DataReplication.Consensus.Data.RaftLeaderElection> TopRaftLeaderElectionsWhere(int count, WhereDelegate<RaftLeaderElectionColumns> where)
        {
            return Wrap<Bam.Net.Services.DataReplication.Consensus.Data.RaftLeaderElection>(Bam.Net.Services.DataReplication.Consensus.Data.Dao.RaftLeaderElection.Top(count, where, Database));
        }

        public IEnumerable<Bam.Net.Services.DataReplication.Consensus.Data.RaftLeaderElection> TopRaftLeaderElectionsWhere(int count, WhereDelegate<RaftLeaderElectionColumns> where, OrderBy<RaftLeaderElectionColumns> orderBy)
        {
            return Wrap<Bam.Net.Services.DataReplication.Consensus.Data.RaftLeaderElection>(Bam.Net.Services.DataReplication.Consensus.Data.Dao.RaftLeaderElection.Top(count, where, orderBy, Database));
        }
                                
		/// <summary>
		/// Return the count of RaftLeaderElections
		/// </summary>
		public long CountRaftLeaderElections()
        {
            return Bam.Net.Services.DataReplication.Consensus.Data.Dao.RaftLeaderElection.Count(Database);
        }

		/// <summary>
		/// Execute a query and return the number of results
		/// </summary>
		/// <param name="where">A WhereDelegate that receives a RaftLeaderElectionColumns 
		/// and returns a IQueryFilter which is the result of any comparisons
		/// between RaftLeaderElectionColumns and other values
		/// </param>
        public long CountRaftLeaderElectionsWhere(WhereDelegate<RaftLeaderElectionColumns> where)
        {
            return Bam.Net.Services.DataReplication.Consensus.Data.Dao.RaftLeaderElection.Count(where, Database);
        }
        
        public async Task BatchQueryRaftLeaderElections(int batchSize, WhereDelegate<RaftLeaderElectionColumns> where, Action<IEnumerable<Bam.Net.Services.DataReplication.Consensus.Data.RaftLeaderElection>> batchProcessor)
        {
            await Bam.Net.Services.DataReplication.Consensus.Data.Dao.RaftLeaderElection.BatchQuery(batchSize, where, (batch) =>
            {
				batchProcessor(Wrap<Bam.Net.Services.DataReplication.Consensus.Data.RaftLeaderElection>(batch));
            }, Database);
        }
		
        public async Task BatchAllRaftLeaderElections(int batchSize, Action<IEnumerable<Bam.Net.Services.DataReplication.Consensus.Data.RaftLeaderElection>> batchProcessor)
        {
            await Bam.Net.Services.DataReplication.Consensus.Data.Dao.RaftLeaderElection.BatchAll(batchSize, (batch) =>
            {
				batchProcessor(Wrap<Bam.Net.Services.DataReplication.Consensus.Data.RaftLeaderElection>(batch));
            }, Database);
        }

		
		/// <summary>
		/// Set one entry matching the specified filter.  If none exists 
		/// one is created; success depends on the nullability
		/// of the specified columns.
		/// </summary>
		public void SetOneRaftVoteWhere(WhereDelegate<RaftVoteColumns> where)
		{
			Bam.Net.Services.DataReplication.Consensus.Data.Dao.RaftVote.SetOneWhere(where, Database);
		}

		/// <summary>
		/// Set one entry matching the specified filter.  If none exists 
		/// one is created; success depends on the nullability
		/// of the specified columns.
		/// </summary>
		public void SetOneRaftVoteWhere(WhereDelegate<RaftVoteColumns> where, out Bam.Net.Services.DataReplication.Consensus.Data.RaftVote result)
		{
			Bam.Net.Services.DataReplication.Consensus.Data.Dao.RaftVote.SetOneWhere(where, out Bam.Net.Services.DataReplication.Consensus.Data.Dao.RaftVote daoResult, Database);
			result = daoResult.CopyAs<Bam.Net.Services.DataReplication.Consensus.Data.RaftVote>();
		}

		/// <summary>
		/// Get one entry matching the specified filter.  If none exists 
		/// one is created; success depends on the nullability
		/// of the specified columns.
		/// </summary>
		/// <param name="where"></param>
		public Bam.Net.Services.DataReplication.Consensus.Data.RaftVote GetOneRaftVoteWhere(WhereDelegate<RaftVoteColumns> where)
		{
			Type wrapperType = GetWrapperType<Bam.Net.Services.DataReplication.Consensus.Data.RaftVote>();
			return (Bam.Net.Services.DataReplication.Consensus.Data.RaftVote)Bam.Net.Services.DataReplication.Consensus.Data.Dao.RaftVote.GetOneWhere(where, Database)?.CopyAs(wrapperType, this);
		}

		/// <summary>
		/// Execute a query that should return only one result.  If no result is found null is returned.  If more
		/// than one result is returned a MultipleEntriesFoundException is thrown.  This method is most commonly used to retrieve a
		/// single RaftVote instance by its Id/Key value
		/// </summary>
		/// <param name="where">A WhereDelegate that receives a RaftVoteColumns 
		/// and returns a IQueryFilter which is the result of any comparisons
		/// between RaftVoteColumns and other values
		/// </param>
		public Bam.Net.Services.DataReplication.Consensus.Data.RaftVote OneRaftVoteWhere(WhereDelegate<RaftVoteColumns> where)
        {
            Type wrapperType = GetWrapperType<Bam.Net.Services.DataReplication.Consensus.Data.RaftVote>();
            return (Bam.Net.Services.DataReplication.Consensus.Data.RaftVote)Bam.Net.Services.DataReplication.Consensus.Data.Dao.RaftVote.OneWhere(where, Database)?.CopyAs(wrapperType, this);
        }

		/// <summary>
		/// Execute a query and return the results. 
		/// </summary>
		/// <param name="where">A WhereDelegate that receives a Bam.Net.Services.DataReplication.Consensus.Data.RaftVoteColumns 
		/// and returns a IQueryFilter which is the result of any comparisons
		/// between Bam.Net.Services.DataReplication.Consensus.Data.RaftVoteColumns and other values
		/// </param>
		public IEnumerable<Bam.Net.Services.DataReplication.Consensus.Data.RaftVote> RaftVotesWhere(WhereDelegate<RaftVoteColumns> where, OrderBy<RaftVoteColumns> orderBy = null)
        {
            return Wrap<Bam.Net.Services.DataReplication.Consensus.Data.RaftVote>(Bam.Net.Services.DataReplication.Consensus.Data.Dao.RaftVote.Where(where, orderBy, Database));
        }
		
		/// <summary>
		/// Execute a query and return the specified number
		/// of values. This method issues a sql TOP clause so only the 
		/// specified number of values will be returned.
		/// </summary>
		/// <param name="count">The number of values to return.
		/// This value is used in the sql query so no more than this 
		/// number of values will be returned by the database.
		/// </param>
		/// <param name="where">A WhereDelegate that receives a RaftVoteColumns 
		/// and returns a IQueryFilter which is the result of any comparisons
		/// between RaftVoteColumns and other values
		/// </param>
		public IEnumerable<Bam.Net.Services.DataReplication.Consensus.Data.RaftVote> TopRaftVotesWhere(int count, WhereDelegate<RaftVoteColumns> where)
        {
            return Wrap<Bam.Net.Services.DataReplication.Consensus.Data.RaftVote>(Bam.Net.Services.DataReplication.Consensus.Data.Dao.RaftVote.Top(count, where, Database));
        }

        public IEnumerable<Bam.Net.Services.DataReplication.Consensus.Data.RaftVote> TopRaftVotesWhere(int count, WhereDelegate<RaftVoteColumns> where, OrderBy<RaftVoteColumns> orderBy)
        {
            return Wrap<Bam.Net.Services.DataReplication.Consensus.Data.RaftVote>(Bam.Net.Services.DataReplication.Consensus.Data.Dao.RaftVote.Top(count, where, orderBy, Database));
        }
                                
		/// <summary>
		/// Return the count of RaftVotes
		/// </summary>
		public long CountRaftVotes()
        {
            return Bam.Net.Services.DataReplication.Consensus.Data.Dao.RaftVote.Count(Database);
        }

		/// <summary>
		/// Execute a query and return the number of results
		/// </summary>
		/// <param name="where">A WhereDelegate that receives a RaftVoteColumns 
		/// and returns a IQueryFilter which is the result of any comparisons
		/// between RaftVoteColumns and other values
		/// </param>
        public long CountRaftVotesWhere(WhereDelegate<RaftVoteColumns> where)
        {
            return Bam.Net.Services.DataReplication.Consensus.Data.Dao.RaftVote.Count(where, Database);
        }
        
        public async Task BatchQueryRaftVotes(int batchSize, WhereDelegate<RaftVoteColumns> where, Action<IEnumerable<Bam.Net.Services.DataReplication.Consensus.Data.RaftVote>> batchProcessor)
        {
            await Bam.Net.Services.DataReplication.Consensus.Data.Dao.RaftVote.BatchQuery(batchSize, where, (batch) =>
            {
				batchProcessor(Wrap<Bam.Net.Services.DataReplication.Consensus.Data.RaftVote>(batch));
            }, Database);
        }
		
        public async Task BatchAllRaftVotes(int batchSize, Action<IEnumerable<Bam.Net.Services.DataReplication.Consensus.Data.RaftVote>> batchProcessor)
        {
            await Bam.Net.Services.DataReplication.Consensus.Data.Dao.RaftVote.BatchAll(batchSize, (batch) =>
            {
				batchProcessor(Wrap<Bam.Net.Services.DataReplication.Consensus.Data.RaftVote>(batch));
            }, Database);
        }


	}
}																								
