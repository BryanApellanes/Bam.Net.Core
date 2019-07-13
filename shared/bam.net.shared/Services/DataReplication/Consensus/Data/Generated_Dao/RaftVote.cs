/*
	This file was generated and should not be modified directly (handlebars template)
*/
// Model is Table
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Threading.Tasks;
using Bam.Net;
using Bam.Net.Data;
using Bam.Net.Data.Qi;

namespace Bam.Net.Services.DataReplication.Consensus.Data.Dao
{
	// schema = Consensus
	// connection Name = Consensus
	[Serializable]
	[Bam.Net.Data.Table("RaftVote", "Consensus")]
	public partial class RaftVote: Bam.Net.Data.Dao
	{
		public RaftVote():base()
		{
			this.SetKeyColumnName();
			this.SetChildren();
		}

		public RaftVote(DataRow data)
			: base(data)
		{
			this.SetKeyColumnName();
			this.SetChildren();
		}

		public RaftVote(Database db)
			: base(db)
		{
			this.SetKeyColumnName();
			this.SetChildren();
		}

		public RaftVote(Database db, DataRow data)
			: base(db, data)
		{
			this.SetKeyColumnName();
			this.SetChildren();
		}

		[Bam.Net.Exclude]
		public static implicit operator RaftVote(DataRow data)
		{
			return new RaftVote(data);
		}

		private void SetChildren()
		{




		} // end SetChildren

	// property:Id, columnName: Id	
	[Bam.Net.Data.Column(Name="Id", DbDataType="BigInt", MaxLength="19", AllowNull=false)]
	public ulong? Id
	{
		get
		{
			return GetULongValue("Id");
		}
		set
		{
			SetValue("Id", value);
		}
	}

	// property:Uuid, columnName: Uuid	
	[Bam.Net.Data.Column(Name="Uuid", DbDataType="VarChar", MaxLength="4000", AllowNull=false)]
	public string Uuid
	{
		get
		{
			return GetStringValue("Uuid");
		}
		set
		{
			SetValue("Uuid", value);
		}
	}

	// property:Cuid, columnName: Cuid	
	[Bam.Net.Data.Column(Name="Cuid", DbDataType="VarChar", MaxLength="4000", AllowNull=true)]
	public string Cuid
	{
		get
		{
			return GetStringValue("Cuid");
		}
		set
		{
			SetValue("Cuid", value);
		}
	}

	// property:FromNodeIdentifier, columnName: FromNodeIdentifier	
	[Bam.Net.Data.Column(Name="FromNodeIdentifier", DbDataType="VarChar", MaxLength="4000", AllowNull=true)]
	public string FromNodeIdentifier
	{
		get
		{
			return GetStringValue("FromNodeIdentifier");
		}
		set
		{
			SetValue("FromNodeIdentifier", value);
		}
	}

	// property:ForNodeIdentifier, columnName: ForNodeIdentifier	
	[Bam.Net.Data.Column(Name="ForNodeIdentifier", DbDataType="VarChar", MaxLength="4000", AllowNull=true)]
	public string ForNodeIdentifier
	{
		get
		{
			return GetStringValue("ForNodeIdentifier");
		}
		set
		{
			SetValue("ForNodeIdentifier", value);
		}
	}

	// property:CreatedBy, columnName: CreatedBy	
	[Bam.Net.Data.Column(Name="CreatedBy", DbDataType="VarChar", MaxLength="4000", AllowNull=true)]
	public string CreatedBy
	{
		get
		{
			return GetStringValue("CreatedBy");
		}
		set
		{
			SetValue("CreatedBy", value);
		}
	}

	// property:ModifiedBy, columnName: ModifiedBy	
	[Bam.Net.Data.Column(Name="ModifiedBy", DbDataType="VarChar", MaxLength="4000", AllowNull=true)]
	public string ModifiedBy
	{
		get
		{
			return GetStringValue("ModifiedBy");
		}
		set
		{
			SetValue("ModifiedBy", value);
		}
	}

	// property:Modified, columnName: Modified	
	[Bam.Net.Data.Column(Name="Modified", DbDataType="DateTime", MaxLength="8", AllowNull=true)]
	public DateTime? Modified
	{
		get
		{
			return GetDateTimeValue("Modified");
		}
		set
		{
			SetValue("Modified", value);
		}
	}

	// property:Deleted, columnName: Deleted	
	[Bam.Net.Data.Column(Name="Deleted", DbDataType="DateTime", MaxLength="8", AllowNull=true)]
	public DateTime? Deleted
	{
		get
		{
			return GetDateTimeValue("Deleted");
		}
		set
		{
			SetValue("Deleted", value);
		}
	}

	// property:Created, columnName: Created	
	[Bam.Net.Data.Column(Name="Created", DbDataType="DateTime", MaxLength="8", AllowNull=true)]
	public DateTime? Created
	{
		get
		{
			return GetDateTimeValue("Created");
		}
		set
		{
			SetValue("Created", value);
		}
	}


	// start RaftLeaderElectionId -> RaftLeaderElectionId
	[Bam.Net.Data.ForeignKey(
        Table="RaftVote",
		Name="RaftLeaderElectionId",
		DbDataType="BigInt",
		MaxLength="",
		AllowNull=true,
		ReferencedKey="Id",
		ReferencedTable="RaftLeaderElection",
		Suffix="1")]
	public ulong? RaftLeaderElectionId
	{
		get
		{
			return GetULongValue("RaftLeaderElectionId");
		}
		set
		{
			SetValue("RaftLeaderElectionId", value);
		}
	}

    RaftLeaderElection _raftLeaderElectionOfRaftLeaderElectionId;
	public RaftLeaderElection RaftLeaderElectionOfRaftLeaderElectionId
	{
		get
		{
			if(_raftLeaderElectionOfRaftLeaderElectionId == null)
			{
				_raftLeaderElectionOfRaftLeaderElectionId = Bam.Net.Services.DataReplication.Consensus.Data.Dao.RaftLeaderElection.OneWhere(c => c.KeyColumn == this.RaftLeaderElectionId, this.Database);
			}
			return _raftLeaderElectionOfRaftLeaderElectionId;
		}
	}






		/// <summary>
        /// Gets a query filter that should uniquely identify
        /// the current instance.  The default implementation
        /// compares the Id/key field to the current instance's.
        /// </summary>
		[Bam.Net.Exclude]
		public override IQueryFilter GetUniqueFilter()
		{
			if(UniqueFilterProvider != null)
			{
				return UniqueFilterProvider(this);
			}
			else
			{
				var colFilter = new RaftVoteColumns();
				return (colFilter.KeyColumn == IdValue);
			}
		}

		/// <summary>
        /// Return every record in the RaftVote table.
        /// </summary>
		/// <param name="database">
		/// The database to load from or null
		/// </param>
		public static RaftVoteCollection LoadAll(Database database = null)
		{
			Database db = database ?? Db.For<RaftVote>();
            SqlStringBuilder sql = db.GetSqlStringBuilder();
            sql.Select<RaftVote>();
            var results = new RaftVoteCollection(db, sql.GetDataTable(db))
            {
                Database = db
            };
            return results;
        }

        /// <summary>
        /// Process all records in batches of the specified size
        /// </summary>
        [Bam.Net.Exclude]
        public static async Task BatchAll(int batchSize, Action<IEnumerable<RaftVote>> batchProcessor, Database database = null)
		{
			await System.Threading.Tasks.Task.Run(async ()=>
			{
				RaftVoteColumns columns = new RaftVoteColumns();
				var orderBy = Bam.Net.Data.Order.By<RaftVoteColumns>(c => c.KeyColumn, Bam.Net.Data.SortOrder.Ascending);
				var results = Top(batchSize, (c) => c.KeyColumn > 0, orderBy, database);
				while(results.Count > 0)
				{
					await System.Threading.Tasks.Task.Run(()=>
					{
						batchProcessor(results);
					});
					long topId = results.Select(d => d.Property<long>(columns.KeyColumn.ToString())).ToArray().Largest();
					results = Top(batchSize, (c) => c.KeyColumn > topId, orderBy, database);
				}
			});
		}

		/// <summary>
		/// Process results of a query in batches of the specified size
		/// </summary>
		[Bam.Net.Exclude]
		public static async Task BatchQuery(int batchSize, QueryFilter filter, Action<IEnumerable<RaftVote>> batchProcessor, Database database = null)
		{
			await BatchQuery(batchSize, (c) => filter, batchProcessor, database);
		}

		/// <summary>
		/// Process results of a query in batches of the specified size
		/// </summary>
		[Bam.Net.Exclude]
		public static async Task BatchQuery(int batchSize, WhereDelegate<RaftVoteColumns> where, Action<IEnumerable<RaftVote>> batchProcessor, Database database = null)
		{
			await System.Threading.Tasks.Task.Run(async ()=>
			{
				RaftVoteColumns columns = new RaftVoteColumns();
				var orderBy = Bam.Net.Data.Order.By<RaftVoteColumns>(c => c.KeyColumn, Bam.Net.Data.SortOrder.Ascending);
				var results = Top(batchSize, where, orderBy, database);
				while(results.Count > 0)
				{
					await System.Threading.Tasks.Task.Run(()=>
					{
						batchProcessor(results);
					});
					long topId = results.Select(d => d.Property<long>(columns.KeyColumn.ToString())).ToArray().Largest();
					results = Top(batchSize, (RaftVoteColumns)where(columns) && columns.KeyColumn > topId, orderBy, database);
				}
			});
		}

		/// <summary>
		/// Process results of a query in batches of the specified size
		/// </summary>
		[Bam.Net.Exclude]
		public static async Task BatchQuery<ColType>(int batchSize, QueryFilter filter, Action<IEnumerable<RaftVote>> batchProcessor, Bam.Net.Data.OrderBy<RaftVoteColumns> orderBy, Database database = null)
		{
			await BatchQuery<ColType>(batchSize, (c) => filter, batchProcessor, orderBy, database);
		}

		/// <summary>
		/// Process results of a query in batches of the specified size
		/// </summary>
		[Bam.Net.Exclude]
		public static async Task BatchQuery<ColType>(int batchSize, WhereDelegate<RaftVoteColumns> where, Action<IEnumerable<RaftVote>> batchProcessor, Bam.Net.Data.OrderBy<RaftVoteColumns> orderBy, Database database = null)
		{
			await System.Threading.Tasks.Task.Run(async ()=>
			{
				RaftVoteColumns columns = new RaftVoteColumns();
				var results = Top(batchSize, where, orderBy, database);
				while(results.Count > 0)
				{
					await System.Threading.Tasks.Task.Run(()=>
					{
						batchProcessor(results);
					});
					ColType top = results.Select(d => d.Property<ColType>(orderBy.Column.ToString())).ToArray().Largest();
					results = Top(batchSize, (RaftVoteColumns)where(columns) && orderBy.Column > top, orderBy, database);
				}
			});
		}

		public static RaftVote GetById(uint id, Database database = null)
		{
			return GetById((ulong)id, database);
		}

		public static RaftVote GetById(int id, Database database = null)
		{
			return GetById((long)id, database);
		}

		public static RaftVote GetById(long id, Database database = null)
		{
			return OneWhere(c => c.KeyColumn == id, database);
		}

		public static RaftVote GetById(ulong id, Database database = null)
		{
			return OneWhere(c => c.KeyColumn == id, database);
		}

		public static RaftVote GetByUuid(string uuid, Database database = null)
		{
			return OneWhere(c => Bam.Net.Data.Query.Where("Uuid") == uuid, database);
		}

		public static RaftVote GetByCuid(string cuid, Database database = null)
		{
			return OneWhere(c => Bam.Net.Data.Query.Where("Cuid") == cuid, database);
		}

		[Bam.Net.Exclude]
		public static RaftVoteCollection Query(QueryFilter filter, Database database = null)
		{
			return Where(filter, database);
		}

		[Bam.Net.Exclude]
		public static RaftVoteCollection Where(QueryFilter filter, Database database = null)
		{
			WhereDelegate<RaftVoteColumns> whereDelegate = (c) => filter;
			return Where(whereDelegate, database);
		}

		/// <summary>
		/// Execute a query and return the results.
		/// </summary>
		/// <param name="where">A Func delegate that recieves a RaftVoteColumns
		/// and returns a QueryFilter which is the result of any comparisons
		/// between RaftVoteColumns and other values
		/// </param>
		/// <param name="db"></param>
		[Bam.Net.Exclude]
		public static RaftVoteCollection Where(Func<RaftVoteColumns, QueryFilter<RaftVoteColumns>> where, OrderBy<RaftVoteColumns> orderBy = null, Database database = null)
		{
			database = database ?? Db.For<RaftVote>();
			return new RaftVoteCollection(database.GetQuery<RaftVoteColumns, RaftVote>(where, orderBy), true);
		}

		/// <summary>
		/// Execute a query and return the results.
		/// </summary>
		/// <param name="where">A WhereDelegate that recieves a RaftVoteColumns
		/// and returns a IQueryFilter which is the result of any comparisons
		/// between RaftVoteColumns and other values
		/// </param>
		/// <param name="db"></param>
		[Bam.Net.Exclude]
		public static RaftVoteCollection Where(WhereDelegate<RaftVoteColumns> where, Database database = null)
		{
			database = database ?? Db.For<RaftVote>();
			var results = new RaftVoteCollection(database, database.GetQuery<RaftVoteColumns, RaftVote>(where), true);
			return results;
		}

		/// <summary>
		/// Execute a query and return the results.
		/// </summary>
		/// <param name="where">A WhereDelegate that recieves a RaftVoteColumns
		/// and returns a IQueryFilter which is the result of any comparisons
		/// between RaftVoteColumns and other values
		/// </param>
		/// <param name="orderBy">
		/// Specifies what column and direction to order the results by
		/// </param>
		/// <param name="database"></param>
		[Bam.Net.Exclude]
		public static RaftVoteCollection Where(WhereDelegate<RaftVoteColumns> where, OrderBy<RaftVoteColumns> orderBy = null, Database database = null)
		{
			database = database ?? Db.For<RaftVote>();
			var results = new RaftVoteCollection(database, database.GetQuery<RaftVoteColumns, RaftVote>(where, orderBy), true);
			return results;
		}

		/// <summary>
		/// This method is intended to respond to client side Qi queries.
		/// Use of this method from .Net should be avoided in favor of
		/// one of the methods that take a delegate of type
		/// WhereDelegate`RaftVoteColumns`.
		/// </summary>
		/// <param name="where"></param>
		/// <param name="database"></param>
		public static RaftVoteCollection Where(QiQuery where, Database database = null)
		{
			var results = new RaftVoteCollection(database, Select<RaftVoteColumns>.From<RaftVote>().Where(where, database));
			return results;
		}

		/// <summary>
		/// Get one entry matching the specified filter.  If none exists
		/// one will be created; success will depend on the nullability
		/// of the specified columns.
		/// </summary>
		[Bam.Net.Exclude]
		public static RaftVote GetOneWhere(QueryFilter where, Database database = null)
		{
			var result = OneWhere(where, database);
			if(result == null)
			{
				result = CreateFromFilter(where, database);
			}

			return result;
		}

		/// <summary>
		/// Execute a query that should return only one result.  If more
		/// than one result is returned a MultipleEntriesFoundException will
		/// be thrown.
		/// </summary>
		/// <param name="where"></param>
		/// <param name="database"></param>
		[Bam.Net.Exclude]
		public static RaftVote OneWhere(QueryFilter where, Database database = null)
		{
			WhereDelegate<RaftVoteColumns> whereDelegate = (c) => where;
			var result = Top(1, whereDelegate, database);
			return OneOrThrow(result);
		}

		/// <summary>
		/// Set one entry matching the specified filter.  If none exists
		/// one will be created; success will depend on the nullability
		/// of the specified columns.
		/// </summary>
		[Bam.Net.Exclude]
		public static void SetOneWhere(WhereDelegate<RaftVoteColumns> where, Database database = null)
		{
			SetOneWhere(where, out RaftVote ignore, database);
		}

		/// <summary>
		/// Set one entry matching the specified filter.  If none exists
		/// one will be created; success will depend on the nullability
		/// of the specified columns.
		/// </summary>
		[Bam.Net.Exclude]
		public static void SetOneWhere(WhereDelegate<RaftVoteColumns> where, out RaftVote result, Database database = null)
		{
			result = GetOneWhere(where, database);
		}

		/// <summary>
		/// Get one entry matching the specified filter.  If none exists
		/// one will be created; success will depend on the nullability
		/// of the specified columns.
		/// </summary>
		/// <param name="where"></param>
		/// <param name="database"></param>
		[Bam.Net.Exclude]
		public static RaftVote GetOneWhere(WhereDelegate<RaftVoteColumns> where, Database database = null)
		{
			var result = OneWhere(where, database);
			if(result == null)
			{
				RaftVoteColumns c = new RaftVoteColumns();
				IQueryFilter filter = where(c);
				result = CreateFromFilter(filter, database);
			}

			return result;
		}

		/// <summary>
		/// Execute a query that should return only one result.  If more
		/// than one result is returned a MultipleEntriesFoundException will
		/// be thrown.  This method is most commonly used to retrieve a
		/// single @Model.ClassName instance by its Id/Key value
		/// </summary>
		/// <param name="where">A WhereDelegate that recieves a RaftVoteColumns
		/// and returns a IQueryFilter which is the result of any comparisons
		/// between RaftVoteColumns and other values
		/// </param>
		/// <param name="database"></param>
		[Bam.Net.Exclude]
		public static RaftVote OneWhere(WhereDelegate<RaftVoteColumns> where, Database database = null)
		{
			var result = Top(1, where, database);
			return OneOrThrow(result);
		}

		/// <summary>
		/// This method is intended to respond to client side Qi queries.
		/// Use of this method from .Net should be avoided in favor of
		/// one of the methods that take a delegate of type
		/// WhereDelegate`RaftVoteColumns`.
		/// </summary>
		/// <param name="where"></param>
		/// <param name="database"></param>
		public static RaftVote OneWhere(QiQuery where, Database database = null)
		{
			var results = Top(1, where, database);
			return OneOrThrow(results);
		}

		/// <summary>
		/// Execute a query and return the first result.  This method will issue a sql TOP clause so only the
		/// specified number of values will be returned.
		/// </summary>
		/// <param name="where">A WhereDelegate that recieves a RaftVoteColumns
		/// and returns a IQueryFilter which is the result of any comparisons
		/// between RaftVoteColumns and other values
		/// </param>
		/// <param name="database"></param>
		[Bam.Net.Exclude]
		public static RaftVote FirstOneWhere(WhereDelegate<RaftVoteColumns> where, Database database = null)
		{
			var results = Top(1, where, database);
			if(results.Count > 0)
			{
				return results[0];
			}
			else
			{
				return null;
			}
		}

		/// <summary>
		/// Execute a query and return the first result.  This method will issue a sql TOP clause so only the
		/// specified number of values will be returned.
		/// </summary>
		/// <param name="where">A WhereDelegate that recieves a RaftVoteColumns
		/// and returns a IQueryFilter which is the result of any comparisons
		/// between RaftVoteColumns and other values
		/// </param>
		/// <param name="database"></param>
		[Bam.Net.Exclude]
		public static RaftVote FirstOneWhere(WhereDelegate<RaftVoteColumns> where, OrderBy<RaftVoteColumns> orderBy, Database database = null)
		{
			var results = Top(1, where, orderBy, database);
			if(results.Count > 0)
			{
				return results[0];
			}
			else
			{
				return null;
			}
		}

		/// <summary>
		/// Shortcut for Top(1, where, orderBy, database)
		/// </summary>
		/// <param name="where">A WhereDelegate that recieves a RaftVoteColumns
		/// and returns a IQueryFilter which is the result of any comparisons
		/// between RaftVoteColumns and other values
		/// </param>
		/// <param name="database"></param>
		[Bam.Net.Exclude]
		public static RaftVote FirstOneWhere(QueryFilter where, OrderBy<RaftVoteColumns> orderBy = null, Database database = null)
		{
			WhereDelegate<RaftVoteColumns> whereDelegate = (c) => where;
			var results = Top(1, whereDelegate, orderBy, database);
			if(results.Count > 0)
			{
				return results[0];
			}
			else
			{
				return null;
			}
		}

		/// <summary>
		/// Execute a query and return the specified number
		/// of values. This method will issue a sql TOP clause so only the
		/// specified number of values will be returned.
		/// </summary>
		/// <param name="count">The number of values to return.
		/// This value is used in the sql query so no more than this
		/// number of values will be returned by the database.
		/// </param>
		/// <param name="where">A WhereDelegate that recieves a RaftVoteColumns
		/// and returns a IQueryFilter which is the result of any comparisons
		/// between RaftVoteColumns and other values
		/// </param>
		/// <param name="database"></param>
		[Bam.Net.Exclude]
		public static RaftVoteCollection Top(int count, WhereDelegate<RaftVoteColumns> where, Database database = null)
		{
			return Top(count, where, null, database);
		}

		/// <summary>
		/// Execute a query and return the specified number of values.  This method
		/// will issue a sql TOP clause so only the specified number of values
		/// will be returned.
		/// </summary>
		/// <param name="count">The number of values to return.
		/// This value is used in the sql query so no more than this
		/// number of values will be returned by the database.
		/// </param>
		/// <param name="where">A WhereDelegate that recieves a RaftVoteColumns
		/// and returns a IQueryFilter which is the result of any comparisons
		/// between RaftVoteColumns and other values
		/// </param>
		/// <param name="orderBy">
		/// Specifies what column and direction to order the results by
		/// </param>
		/// <param name="database">
		/// Which database to query or null to use the default
		/// </param>
		[Bam.Net.Exclude]
		public static RaftVoteCollection Top(int count, WhereDelegate<RaftVoteColumns> where, OrderBy<RaftVoteColumns> orderBy, Database database = null)
		{
			RaftVoteColumns c = new RaftVoteColumns();
			IQueryFilter filter = where(c);

			Database db = database ?? Db.For<RaftVote>();
			QuerySet query = GetQuerySet(db);
			query.Top<RaftVote>(count);
			query.Where(filter);

			if(orderBy != null)
			{
				query.OrderBy<RaftVoteColumns>(orderBy);
			}

			query.Execute(db);
			var results = query.Results.As<RaftVoteCollection>(0);
			results.Database = db;
			return results;
		}

		[Bam.Net.Exclude]
		public static RaftVoteCollection Top(int count, QueryFilter where, Database database)
		{
			return Top(count, where, null, database);
		}
		/// <summary>
		/// Execute a query and return the specified number of values.  This method
		/// will issue a sql TOP clause so only the specified number of values
		/// will be returned.
		/// of values
		/// </summary>
		/// <param name="count">The number of values to return.
		/// This value is used in the sql query so no more than this
		/// number of values will be returned by the database.
		/// </param>
		/// <param name="where">A QueryFilter used to filter the
		/// results
		/// </param>
		/// <param name="orderBy">
		/// Specifies what column and direction to order the results by
		/// </param>
		/// <param name="database">
		/// Which database to query or null to use the default
		/// </param>
		[Bam.Net.Exclude]
		public static RaftVoteCollection Top(int count, QueryFilter where, OrderBy<RaftVoteColumns> orderBy = null, Database database = null)
		{
			Database db = database ?? Db.For<RaftVote>();
			QuerySet query = GetQuerySet(db);
			query.Top<RaftVote>(count);
			query.Where(where);

			if(orderBy != null)
			{
				query.OrderBy<RaftVoteColumns>(orderBy);
			}

			query.Execute(db);
			var results = query.Results.As<RaftVoteCollection>(0);
			results.Database = db;
			return results;
		}

		[Bam.Net.Exclude]
		public static RaftVoteCollection Top(int count, QueryFilter where, string orderBy = null, SortOrder sortOrder = SortOrder.Ascending, Database database = null)
		{
			Database db = database ?? Db.For<RaftVote>();
			QuerySet query = GetQuerySet(db);
			query.Top<RaftVote>(count);
			query.Where(where);

			if(orderBy != null)
			{
				query.OrderBy(orderBy, sortOrder);
			}

			query.Execute(db);
			var results = query.Results.As<RaftVoteCollection>(0);
			results.Database = db;
			return results;
		}

		/// <summary>
		/// Execute a query and return the specified number of values.  This method
		/// will issue a sql TOP clause so only the specified number of values
		/// will be returned.
		/// of values
		/// </summary>
		/// <param name="count">The number of values to return.
		/// This value is used in the sql query so no more than this
		/// number of values will be returned by the database.
		/// </param>
		/// <param name="where">A QueryFilter used to filter the
		/// results
		/// </param>
		/// <param name="database">
		/// Which database to query or null to use the default
		/// </param>
		public static RaftVoteCollection Top(int count, QiQuery where, Database database = null)
		{
			Database db = database ?? Db.For<RaftVote>();
			QuerySet query = GetQuerySet(db);
			query.Top<RaftVote>(count);
			query.Where(where);
			query.Execute(db);
			var results = query.Results.As<RaftVoteCollection>(0);
			results.Database = db;
			return results;
		}

		/// <summary>
		/// Return the count of @(Model.ClassName.Pluralize())
		/// </summary>
		/// <param name="database">
		/// Which database to query or null to use the default
		/// </param>
		public static long Count(Database database = null)
        {
			Database db = database ?? Db.For<RaftVote>();
            QuerySet query = GetQuerySet(db);
            query.Count<RaftVote>();
            query.Execute(db);
            return (long)query.Results[0].DataRow[0];
        }

		/// <summary>
		/// Execute a query and return the number of results
		/// </summary>
		/// <param name="where">A WhereDelegate that recieves a RaftVoteColumns
		/// and returns a IQueryFilter which is the result of any comparisons
		/// between RaftVoteColumns and other values
		/// </param>
		/// <param name="database">
		/// Which database to query or null to use the default
		/// </param>
		[Bam.Net.Exclude]
		public static long Count(WhereDelegate<RaftVoteColumns> where, Database database = null)
		{
			RaftVoteColumns c = new RaftVoteColumns();
			IQueryFilter filter = where(c) ;

			Database db = database ?? Db.For<RaftVote>();
			QuerySet query = GetQuerySet(db);
			query.Count<RaftVote>();
			query.Where(filter);
			query.Execute(db);
			return query.Results.As<CountResult>(0).Value;
		}

		public static long Count(QiQuery where, Database database = null)
		{
		    Database db = database ?? Db.For<RaftVote>();
			QuerySet query = GetQuerySet(db);
			query.Count<RaftVote>();
			query.Where(where);
			query.Execute(db);
			return query.Results.As<CountResult>(0).Value;
		}

		private static RaftVote CreateFromFilter(IQueryFilter filter, Database database = null)
		{
			Database db = database ?? Db.For<RaftVote>();
			var dao = new RaftVote();
			filter.Parameters.Each(p=>
			{
				dao.Property(p.ColumnName, p.Value);
			});
			dao.Save(db);
			return dao;
		}

		private static RaftVote OneOrThrow(RaftVoteCollection c)
		{
			if(c.Count == 1)
			{
				return c[0];
			}
			else if(c.Count > 1)
			{
				throw new MultipleEntriesFoundException();
			}

			return null;
		}

	}
}
