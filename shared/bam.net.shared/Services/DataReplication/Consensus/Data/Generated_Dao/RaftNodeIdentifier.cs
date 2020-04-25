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
	// schema = RaftConsensus
	// connection Name = RaftConsensus
	[Serializable]
	[Bam.Net.Data.Table("RaftNodeIdentifier", "RaftConsensus")]
	public partial class RaftNodeIdentifier: Bam.Net.Data.Dao
	{
		public RaftNodeIdentifier():base()
		{
			this.SetKeyColumnName();
			this.SetChildren();
		}

		public RaftNodeIdentifier(DataRow data)
			: base(data)
		{
			this.SetKeyColumnName();
			this.SetChildren();
		}

		public RaftNodeIdentifier(Database db)
			: base(db)
		{
			this.SetKeyColumnName();
			this.SetChildren();
		}

		public RaftNodeIdentifier(Database db, DataRow data)
			: base(db, data)
		{
			this.SetKeyColumnName();
			this.SetChildren();
		}

		[Bam.Net.Exclude]
		public static implicit operator RaftNodeIdentifier(DataRow data)
		{
			return new RaftNodeIdentifier(data);
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

	// property:HostName, columnName: HostName	
	[Bam.Net.Data.Column(Name="HostName", DbDataType="VarChar", MaxLength="4000", AllowNull=true)]
	public string HostName
	{
		get
		{
			return GetStringValue("HostName");
		}
		set
		{
			SetValue("HostName", value);
		}
	}

	// property:Port, columnName: Port	
	[Bam.Net.Data.Column(Name="Port", DbDataType="Int", MaxLength="10", AllowNull=true)]
	public int? Port
	{
		get
		{
			return GetIntValue("Port");
		}
		set
		{
			SetValue("Port", value);
		}
	}

	// property:CompositeKeyId, columnName: CompositeKeyId	
	[Bam.Net.Data.Column(Name="CompositeKeyId", DbDataType="BigInt", MaxLength="19", AllowNull=true)]
	public ulong? CompositeKeyId
	{
		get
		{
			return GetULongValue("CompositeKeyId");
		}
		set
		{
			SetValue("CompositeKeyId", value);
		}
	}

	// property:CompositeKey, columnName: CompositeKey	
	[Bam.Net.Data.Column(Name="CompositeKey", DbDataType="VarChar", MaxLength="4000", AllowNull=true)]
	public string CompositeKey
	{
		get
		{
			return GetStringValue("CompositeKey");
		}
		set
		{
			SetValue("CompositeKey", value);
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
				var colFilter = new RaftNodeIdentifierColumns();
				return (colFilter.KeyColumn == IdValue);
			}
		}

		/// <summary>
        /// Return every record in the RaftNodeIdentifier table.
        /// </summary>
		/// <param name="database">
		/// The database to load from or null
		/// </param>
		public static RaftNodeIdentifierCollection LoadAll(Database database = null)
		{
			Database db = database ?? Db.For<RaftNodeIdentifier>();
            SqlStringBuilder sql = db.GetSqlStringBuilder();
            sql.Select<RaftNodeIdentifier>();
            var results = new RaftNodeIdentifierCollection(db, sql.GetDataTable(db))
            {
                Database = db
            };
            return results;
        }

        /// <summary>
        /// Process all records in batches of the specified size
        /// </summary>
        [Bam.Net.Exclude]
        public static async Task BatchAll(int batchSize, Action<IEnumerable<RaftNodeIdentifier>> batchProcessor, Database database = null)
		{
			await System.Threading.Tasks.Task.Run(async ()=>
			{
				RaftNodeIdentifierColumns columns = new RaftNodeIdentifierColumns();
				var orderBy = Bam.Net.Data.Order.By<RaftNodeIdentifierColumns>(c => c.KeyColumn, Bam.Net.Data.SortOrder.Ascending);
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
		public static async Task BatchQuery(int batchSize, QueryFilter filter, Action<IEnumerable<RaftNodeIdentifier>> batchProcessor, Database database = null)
		{
			await BatchQuery(batchSize, (c) => filter, batchProcessor, database);
		}

		/// <summary>
		/// Process results of a query in batches of the specified size
		/// </summary>
		[Bam.Net.Exclude]
		public static async Task BatchQuery(int batchSize, WhereDelegate<RaftNodeIdentifierColumns> where, Action<IEnumerable<RaftNodeIdentifier>> batchProcessor, Database database = null)
		{
			await System.Threading.Tasks.Task.Run(async ()=>
			{
				RaftNodeIdentifierColumns columns = new RaftNodeIdentifierColumns();
				var orderBy = Bam.Net.Data.Order.By<RaftNodeIdentifierColumns>(c => c.KeyColumn, Bam.Net.Data.SortOrder.Ascending);
				var results = Top(batchSize, where, orderBy, database);
				while(results.Count > 0)
				{
					await System.Threading.Tasks.Task.Run(()=>
					{
						batchProcessor(results);
					});
					long topId = results.Select(d => d.Property<long>(columns.KeyColumn.ToString())).ToArray().Largest();
					results = Top(batchSize, (RaftNodeIdentifierColumns)where(columns) && columns.KeyColumn > topId, orderBy, database);
				}
			});
		}

		/// <summary>
		/// Process results of a query in batches of the specified size
		/// </summary>
		[Bam.Net.Exclude]
		public static async Task BatchQuery<ColType>(int batchSize, QueryFilter filter, Action<IEnumerable<RaftNodeIdentifier>> batchProcessor, Bam.Net.Data.OrderBy<RaftNodeIdentifierColumns> orderBy, Database database = null)
		{
			await BatchQuery<ColType>(batchSize, (c) => filter, batchProcessor, orderBy, database);
		}

		/// <summary>
		/// Process results of a query in batches of the specified size
		/// </summary>
		[Bam.Net.Exclude]
		public static async Task BatchQuery<ColType>(int batchSize, WhereDelegate<RaftNodeIdentifierColumns> where, Action<IEnumerable<RaftNodeIdentifier>> batchProcessor, Bam.Net.Data.OrderBy<RaftNodeIdentifierColumns> orderBy, Database database = null)
		{
			await System.Threading.Tasks.Task.Run(async ()=>
			{
				RaftNodeIdentifierColumns columns = new RaftNodeIdentifierColumns();
				var results = Top(batchSize, where, orderBy, database);
				while(results.Count > 0)
				{
					await System.Threading.Tasks.Task.Run(()=>
					{
						batchProcessor(results);
					});
					ColType top = results.Select(d => d.Property<ColType>(orderBy.Column.ToString())).ToArray().Largest();
					results = Top(batchSize, (RaftNodeIdentifierColumns)where(columns) && orderBy.Column > top, orderBy, database);
				}
			});
		}

		public static RaftNodeIdentifier GetById(uint? id, Database database = null)
		{
			Args.ThrowIfNull(id, "id");
			Args.ThrowIf(!id.HasValue, "specified RaftNodeIdentifier.Id was null");
			return GetById(id.Value, database);
		}

		public static RaftNodeIdentifier GetById(uint id, Database database = null)
		{
			return GetById((ulong)id, database);
		}

		public static RaftNodeIdentifier GetById(int? id, Database database = null)
		{
			Args.ThrowIfNull(id, "id");
			Args.ThrowIf(!id.HasValue, "specified RaftNodeIdentifier.Id was null");
			return GetById(id.Value, database);
		}                                    
                                    
		public static RaftNodeIdentifier GetById(int id, Database database = null)
		{
			return GetById((long)id, database);
		}

		public static RaftNodeIdentifier GetById(long? id, Database database = null)
		{
			Args.ThrowIfNull(id, "id");
			Args.ThrowIf(!id.HasValue, "specified RaftNodeIdentifier.Id was null");
			return GetById(id.Value, database);
		}
                                    
		public static RaftNodeIdentifier GetById(long id, Database database = null)
		{
			return OneWhere(c => c.KeyColumn == id, database);
		}

		public static RaftNodeIdentifier GetById(ulong? id, Database database = null)
		{
			Args.ThrowIfNull(id, "id");
			Args.ThrowIf(!id.HasValue, "specified RaftNodeIdentifier.Id was null");
			return GetById(id.Value, database);
		}
                                    
		public static RaftNodeIdentifier GetById(ulong id, Database database = null)
		{
			return OneWhere(c => c.KeyColumn == id, database);
		}

		public static RaftNodeIdentifier GetByUuid(string uuid, Database database = null)
		{
			return OneWhere(c => Bam.Net.Data.Query.Where("Uuid") == uuid, database);
		}

		public static RaftNodeIdentifier GetByCuid(string cuid, Database database = null)
		{
			return OneWhere(c => Bam.Net.Data.Query.Where("Cuid") == cuid, database);
		}

		[Bam.Net.Exclude]
		public static RaftNodeIdentifierCollection Query(QueryFilter filter, Database database = null)
		{
			return Where(filter, database);
		}

		[Bam.Net.Exclude]
		public static RaftNodeIdentifierCollection Where(QueryFilter filter, Database database = null)
		{
			WhereDelegate<RaftNodeIdentifierColumns> whereDelegate = (c) => filter;
			return Where(whereDelegate, database);
		}

		/// <summary>
		/// Execute a query and return the results.
		/// </summary>
		/// <param name="where">A Func delegate that recieves a RaftNodeIdentifierColumns
		/// and returns a QueryFilter which is the result of any comparisons
		/// between RaftNodeIdentifierColumns and other values
		/// </param>
		/// <param name="db"></param>
		[Bam.Net.Exclude]
		public static RaftNodeIdentifierCollection Where(Func<RaftNodeIdentifierColumns, QueryFilter<RaftNodeIdentifierColumns>> where, OrderBy<RaftNodeIdentifierColumns> orderBy = null, Database database = null)
		{
			database = database ?? Db.For<RaftNodeIdentifier>();
			return new RaftNodeIdentifierCollection(database.GetQuery<RaftNodeIdentifierColumns, RaftNodeIdentifier>(where, orderBy), true);
		}

		/// <summary>
		/// Execute a query and return the results.
		/// </summary>
		/// <param name="where">A WhereDelegate that recieves a RaftNodeIdentifierColumns
		/// and returns a IQueryFilter which is the result of any comparisons
		/// between RaftNodeIdentifierColumns and other values
		/// </param>
		/// <param name="db"></param>
		[Bam.Net.Exclude]
		public static RaftNodeIdentifierCollection Where(WhereDelegate<RaftNodeIdentifierColumns> where, Database database = null)
		{
			database = database ?? Db.For<RaftNodeIdentifier>();
			var results = new RaftNodeIdentifierCollection(database, database.GetQuery<RaftNodeIdentifierColumns, RaftNodeIdentifier>(where), true);
			return results;
		}

		/// <summary>
		/// Execute a query and return the results.
		/// </summary>
		/// <param name="where">A WhereDelegate that recieves a RaftNodeIdentifierColumns
		/// and returns a IQueryFilter which is the result of any comparisons
		/// between RaftNodeIdentifierColumns and other values
		/// </param>
		/// <param name="orderBy">
		/// Specifies what column and direction to order the results by
		/// </param>
		/// <param name="database"></param>
		[Bam.Net.Exclude]
		public static RaftNodeIdentifierCollection Where(WhereDelegate<RaftNodeIdentifierColumns> where, OrderBy<RaftNodeIdentifierColumns> orderBy = null, Database database = null)
		{
			database = database ?? Db.For<RaftNodeIdentifier>();
			var results = new RaftNodeIdentifierCollection(database, database.GetQuery<RaftNodeIdentifierColumns, RaftNodeIdentifier>(where, orderBy), true);
			return results;
		}

		/// <summary>
		/// This method is intended to respond to client side Qi queries.
		/// Use of this method from .Net should be avoided in favor of
		/// one of the methods that take a delegate of type
		/// WhereDelegate`RaftNodeIdentifierColumns`.
		/// </summary>
		/// <param name="where"></param>
		/// <param name="database"></param>
		public static RaftNodeIdentifierCollection Where(QiQuery where, Database database = null)
		{
			var results = new RaftNodeIdentifierCollection(database, Select<RaftNodeIdentifierColumns>.From<RaftNodeIdentifier>().Where(where, database));
			return results;
		}

		/// <summary>
		/// Get one entry matching the specified filter.  If none exists
		/// one will be created; success will depend on the nullability
		/// of the specified columns.
		/// </summary>
		[Bam.Net.Exclude]
		public static RaftNodeIdentifier GetOneWhere(QueryFilter where, Database database = null)
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
		public static RaftNodeIdentifier OneWhere(QueryFilter where, Database database = null)
		{
			WhereDelegate<RaftNodeIdentifierColumns> whereDelegate = (c) => where;
			var result = Top(1, whereDelegate, database);
			return OneOrThrow(result);
		}

		/// <summary>
		/// Set one entry matching the specified filter.  If none exists
		/// one will be created; success will depend on the nullability
		/// of the specified columns.
		/// </summary>
		[Bam.Net.Exclude]
		public static void SetOneWhere(WhereDelegate<RaftNodeIdentifierColumns> where, Database database = null)
		{
			SetOneWhere(where, out RaftNodeIdentifier ignore, database);
		}

		/// <summary>
		/// Set one entry matching the specified filter.  If none exists
		/// one will be created; success will depend on the nullability
		/// of the specified columns.
		/// </summary>
		[Bam.Net.Exclude]
		public static void SetOneWhere(WhereDelegate<RaftNodeIdentifierColumns> where, out RaftNodeIdentifier result, Database database = null)
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
		public static RaftNodeIdentifier GetOneWhere(WhereDelegate<RaftNodeIdentifierColumns> where, Database database = null)
		{
			var result = OneWhere(where, database);
			if(result == null)
			{
				RaftNodeIdentifierColumns c = new RaftNodeIdentifierColumns();
				IQueryFilter filter = where(c);
				result = CreateFromFilter(filter, database);
			}

			return result;
		}

		/// <summary>
		/// Execute a query that should return only one result.  If more
		/// than one result is returned a MultipleEntriesFoundException will
		/// be thrown.  This method is most commonly used to retrieve a
		/// single RaftNodeIdentifier instance by its Id/Key value
		/// </summary>
		/// <param name="where">A WhereDelegate that recieves a RaftNodeIdentifierColumns
		/// and returns a IQueryFilter which is the result of any comparisons
		/// between RaftNodeIdentifierColumns and other values
		/// </param>
		/// <param name="database"></param>
		[Bam.Net.Exclude]
		public static RaftNodeIdentifier OneWhere(WhereDelegate<RaftNodeIdentifierColumns> where, Database database = null)
		{
			var result = Top(1, where, database);
			return OneOrThrow(result);
		}

		/// <summary>
		/// This method is intended to respond to client side Qi queries.
		/// Use of this method from .Net should be avoided in favor of
		/// one of the methods that take a delegate of type
		/// WhereDelegate`RaftNodeIdentifierColumns`.
		/// </summary>
		/// <param name="where"></param>
		/// <param name="database"></param>
		public static RaftNodeIdentifier OneWhere(QiQuery where, Database database = null)
		{
			var results = Top(1, where, database);
			return OneOrThrow(results);
		}

		/// <summary>
		/// Execute a query and return the first result.  This method will issue a sql TOP clause so only the
		/// specified number of values will be returned.
		/// </summary>
		/// <param name="where">A WhereDelegate that recieves a RaftNodeIdentifierColumns
		/// and returns a IQueryFilter which is the result of any comparisons
		/// between RaftNodeIdentifierColumns and other values
		/// </param>
		/// <param name="database"></param>
		[Bam.Net.Exclude]
		public static RaftNodeIdentifier FirstOneWhere(WhereDelegate<RaftNodeIdentifierColumns> where, Database database = null)
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
		/// <param name="where">A WhereDelegate that recieves a RaftNodeIdentifierColumns
		/// and returns a IQueryFilter which is the result of any comparisons
		/// between RaftNodeIdentifierColumns and other values
		/// </param>
		/// <param name="database"></param>
		[Bam.Net.Exclude]
		public static RaftNodeIdentifier FirstOneWhere(WhereDelegate<RaftNodeIdentifierColumns> where, OrderBy<RaftNodeIdentifierColumns> orderBy, Database database = null)
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
		/// <param name="where">A WhereDelegate that recieves a RaftNodeIdentifierColumns
		/// and returns a IQueryFilter which is the result of any comparisons
		/// between RaftNodeIdentifierColumns and other values
		/// </param>
		/// <param name="database"></param>
		[Bam.Net.Exclude]
		public static RaftNodeIdentifier FirstOneWhere(QueryFilter where, OrderBy<RaftNodeIdentifierColumns> orderBy = null, Database database = null)
		{
			WhereDelegate<RaftNodeIdentifierColumns> whereDelegate = (c) => where;
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
		/// <param name="where">A WhereDelegate that recieves a RaftNodeIdentifierColumns
		/// and returns a IQueryFilter which is the result of any comparisons
		/// between RaftNodeIdentifierColumns and other values
		/// </param>
		/// <param name="database"></param>
		[Bam.Net.Exclude]
		public static RaftNodeIdentifierCollection Top(int count, WhereDelegate<RaftNodeIdentifierColumns> where, Database database = null)
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
		/// <param name="where">A WhereDelegate that recieves a RaftNodeIdentifierColumns
		/// and returns a IQueryFilter which is the result of any comparisons
		/// between RaftNodeIdentifierColumns and other values
		/// </param>
		/// <param name="orderBy">
		/// Specifies what column and direction to order the results by
		/// </param>
		/// <param name="database">
		/// Which database to query or null to use the default
		/// </param>
		[Bam.Net.Exclude]
		public static RaftNodeIdentifierCollection Top(int count, WhereDelegate<RaftNodeIdentifierColumns> where, OrderBy<RaftNodeIdentifierColumns> orderBy, Database database = null)
		{
			RaftNodeIdentifierColumns c = new RaftNodeIdentifierColumns();
			IQueryFilter filter = where(c);

			Database db = database ?? Db.For<RaftNodeIdentifier>();
			QuerySet query = GetQuerySet(db);
			query.Top<RaftNodeIdentifier>(count);
			query.Where(filter);

			if(orderBy != null)
			{
				query.OrderBy<RaftNodeIdentifierColumns>(orderBy);
			}

			query.Execute(db);
			var results = query.Results.As<RaftNodeIdentifierCollection>(0);
			results.Database = db;
			return results;
		}

		[Bam.Net.Exclude]
		public static RaftNodeIdentifierCollection Top(int count, QueryFilter where, Database database)
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
		public static RaftNodeIdentifierCollection Top(int count, QueryFilter where, OrderBy<RaftNodeIdentifierColumns> orderBy = null, Database database = null)
		{
			Database db = database ?? Db.For<RaftNodeIdentifier>();
			QuerySet query = GetQuerySet(db);
			query.Top<RaftNodeIdentifier>(count);
			query.Where(where);

			if(orderBy != null)
			{
				query.OrderBy<RaftNodeIdentifierColumns>(orderBy);
			}

			query.Execute(db);
			var results = query.Results.As<RaftNodeIdentifierCollection>(0);
			results.Database = db;
			return results;
		}

		[Bam.Net.Exclude]
		public static RaftNodeIdentifierCollection Top(int count, QueryFilter where, string orderBy = null, SortOrder sortOrder = SortOrder.Ascending, Database database = null)
		{
			Database db = database ?? Db.For<RaftNodeIdentifier>();
			QuerySet query = GetQuerySet(db);
			query.Top<RaftNodeIdentifier>(count);
			query.Where(where);

			if(orderBy != null)
			{
				query.OrderBy(orderBy, sortOrder);
			}

			query.Execute(db);
			var results = query.Results.As<RaftNodeIdentifierCollection>(0);
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
		public static RaftNodeIdentifierCollection Top(int count, QiQuery where, Database database = null)
		{
			Database db = database ?? Db.For<RaftNodeIdentifier>();
			QuerySet query = GetQuerySet(db);
			query.Top<RaftNodeIdentifier>(count);
			query.Where(where);
			query.Execute(db);
			var results = query.Results.As<RaftNodeIdentifierCollection>(0);
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
			Database db = database ?? Db.For<RaftNodeIdentifier>();
            QuerySet query = GetQuerySet(db);
            query.Count<RaftNodeIdentifier>();
            query.Execute(db);
            return (long)query.Results[0].DataRow[0];
        }

		/// <summary>
		/// Execute a query and return the number of results
		/// </summary>
		/// <param name="where">A WhereDelegate that recieves a RaftNodeIdentifierColumns
		/// and returns a IQueryFilter which is the result of any comparisons
		/// between RaftNodeIdentifierColumns and other values
		/// </param>
		/// <param name="database">
		/// Which database to query or null to use the default
		/// </param>
		[Bam.Net.Exclude]
		public static long Count(WhereDelegate<RaftNodeIdentifierColumns> where, Database database = null)
		{
			RaftNodeIdentifierColumns c = new RaftNodeIdentifierColumns();
			IQueryFilter filter = where(c) ;

			Database db = database ?? Db.For<RaftNodeIdentifier>();
			QuerySet query = GetQuerySet(db);
			query.Count<RaftNodeIdentifier>();
			query.Where(filter);
			query.Execute(db);
			return query.Results.As<CountResult>(0).Value;
		}

		public static long Count(QiQuery where, Database database = null)
		{
		    Database db = database ?? Db.For<RaftNodeIdentifier>();
			QuerySet query = GetQuerySet(db);
			query.Count<RaftNodeIdentifier>();
			query.Where(where);
			query.Execute(db);
			return query.Results.As<CountResult>(0).Value;
		}

		private static RaftNodeIdentifier CreateFromFilter(IQueryFilter filter, Database database = null)
		{
			Database db = database ?? Db.For<RaftNodeIdentifier>();
			var dao = new RaftNodeIdentifier();
			filter.Parameters.Each(p=>
			{
				dao.Property(p.ColumnName, p.Value);
			});
			dao.Save(db);
			return dao;
		}

		private static RaftNodeIdentifier OneOrThrow(RaftNodeIdentifierCollection c)
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
