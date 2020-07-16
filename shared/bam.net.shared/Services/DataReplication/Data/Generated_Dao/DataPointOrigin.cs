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

namespace Bam.Net.Services.DataReplication.Data.Dao
{
	// schema = DataReplication
	// connection Name = DataReplication
	[Serializable]
	[Bam.Net.Data.Table("DataPointOrigin", "DataReplication")]
	public partial class DataPointOrigin: Bam.Net.Data.Dao
	{
		public DataPointOrigin():base()
		{
			this.SetKeyColumnName();
			this.SetChildren();
		}

		public DataPointOrigin(DataRow data)
			: base(data)
		{
			this.SetKeyColumnName();
			this.SetChildren();
		}

		public DataPointOrigin(Database db)
			: base(db)
		{
			this.SetKeyColumnName();
			this.SetChildren();
		}

		public DataPointOrigin(Database db, DataRow data)
			: base(db, data)
		{
			this.SetKeyColumnName();
			this.SetChildren();
		}

		[Bam.Net.Exclude]
		public static implicit operator DataPointOrigin(DataRow data)
		{
			return new DataPointOrigin(data);
		}

		private void SetChildren()
		{


			if(_database != null)
			{
				this.ChildCollections.Add("DataPoint_DataPointOriginId", new DataPointCollection(Database.GetQuery<DataPointColumns, DataPoint>((c) => c.DataPointOriginId == GetULongValue("Id")), this, "DataPointOriginId"));
			}


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

	// property:MachineName, columnName: MachineName	
	[Bam.Net.Data.Column(Name="MachineName", DbDataType="VarChar", MaxLength="4000", AllowNull=true)]
	public string MachineName
	{
		get
		{
			return GetStringValue("MachineName");
		}
		set
		{
			SetValue("MachineName", value);
		}
	}

	// property:AssemblyPath, columnName: AssemblyPath	
	[Bam.Net.Data.Column(Name="AssemblyPath", DbDataType="VarChar", MaxLength="4000", AllowNull=true)]
	public string AssemblyPath
	{
		get
		{
			return GetStringValue("AssemblyPath");
		}
		set
		{
			SetValue("AssemblyPath", value);
		}
	}

	// property:Key, columnName: Key	
	[Bam.Net.Data.Column(Name="Key", DbDataType="BigInt", MaxLength="19", AllowNull=true)]
	public ulong? Key
	{
		get
		{
			return GetULongValue("Key");
		}
		set
		{
			SetValue("Key", value);
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



	[Bam.Net.Exclude]	
	public DataPointCollection DataPointsByDataPointOriginId
	{
		get
		{
			if (this.IsNew)
			{
				throw new InvalidOperationException("The current instance of type({0}) hasn't been saved and will have no child collections, call Save() or Save(Database) first."._Format(this.GetType().Name));
			}

			if(!this.ChildCollections.ContainsKey("DataPoint_DataPointOriginId"))
			{
				SetChildren();
			}

			var c = (DataPointCollection)this.ChildCollections["DataPoint_DataPointOriginId"];
			if(!c.Loaded)
			{
				c.Load(Database);
			}
			return c;
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
				var colFilter = new DataPointOriginColumns();
				return (colFilter.KeyColumn == IdValue);
			}
		}

		/// <summary>
        /// Return every record in the DataPointOrigin table.
        /// </summary>
		/// <param name="database">
		/// The database to load from or null
		/// </param>
		public static DataPointOriginCollection LoadAll(Database database = null)
		{
			Database db = database ?? Db.For<DataPointOrigin>();
            SqlStringBuilder sql = db.GetSqlStringBuilder();
            sql.Select<DataPointOrigin>();
            var results = new DataPointOriginCollection(db, sql.GetDataTable(db))
            {
                Database = db
            };
            return results;
        }

        /// <summary>
        /// Process all records in batches of the specified size
        /// </summary>
        [Bam.Net.Exclude]
        public static async Task BatchAll(int batchSize, Action<IEnumerable<DataPointOrigin>> batchProcessor, Database database = null)
		{
			await System.Threading.Tasks.Task.Run(async ()=>
			{
				DataPointOriginColumns columns = new DataPointOriginColumns();
				var orderBy = Bam.Net.Data.Order.By<DataPointOriginColumns>(c => c.KeyColumn, Bam.Net.Data.SortOrder.Ascending);
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
		public static async Task BatchQuery(int batchSize, QueryFilter filter, Action<IEnumerable<DataPointOrigin>> batchProcessor, Database database = null)
		{
			await BatchQuery(batchSize, (c) => filter, batchProcessor, database);
		}

		/// <summary>
		/// Process results of a query in batches of the specified size
		/// </summary>
		[Bam.Net.Exclude]
		public static async Task BatchQuery(int batchSize, WhereDelegate<DataPointOriginColumns> where, Action<IEnumerable<DataPointOrigin>> batchProcessor, Database database = null)
		{
			await System.Threading.Tasks.Task.Run(async ()=>
			{
				DataPointOriginColumns columns = new DataPointOriginColumns();
				var orderBy = Bam.Net.Data.Order.By<DataPointOriginColumns>(c => c.KeyColumn, Bam.Net.Data.SortOrder.Ascending);
				var results = Top(batchSize, where, orderBy, database);
				while(results.Count > 0)
				{
					await System.Threading.Tasks.Task.Run(()=>
					{
						batchProcessor(results);
					});
					long topId = results.Select(d => d.Property<long>(columns.KeyColumn.ToString())).ToArray().Largest();
					results = Top(batchSize, (DataPointOriginColumns)where(columns) && columns.KeyColumn > topId, orderBy, database);
				}
			});
		}

		/// <summary>
		/// Process results of a query in batches of the specified size
		/// </summary>
		[Bam.Net.Exclude]
		public static async Task BatchQuery<ColType>(int batchSize, QueryFilter filter, Action<IEnumerable<DataPointOrigin>> batchProcessor, Bam.Net.Data.OrderBy<DataPointOriginColumns> orderBy, Database database = null)
		{
			await BatchQuery<ColType>(batchSize, (c) => filter, batchProcessor, orderBy, database);
		}

		/// <summary>
		/// Process results of a query in batches of the specified size
		/// </summary>
		[Bam.Net.Exclude]
		public static async Task BatchQuery<ColType>(int batchSize, WhereDelegate<DataPointOriginColumns> where, Action<IEnumerable<DataPointOrigin>> batchProcessor, Bam.Net.Data.OrderBy<DataPointOriginColumns> orderBy, Database database = null)
		{
			await System.Threading.Tasks.Task.Run(async ()=>
			{
				DataPointOriginColumns columns = new DataPointOriginColumns();
				var results = Top(batchSize, where, orderBy, database);
				while(results.Count > 0)
				{
					await System.Threading.Tasks.Task.Run(()=>
					{
						batchProcessor(results);
					});
					ColType top = results.Select(d => d.Property<ColType>(orderBy.Column.ToString())).ToArray().Largest();
					results = Top(batchSize, (DataPointOriginColumns)where(columns) && orderBy.Column > top, orderBy, database);
				}
			});
		}

		public static DataPointOrigin GetById(uint? id, Database database = null)
		{
			Args.ThrowIfNull(id, "id");
			Args.ThrowIf(!id.HasValue, "specified DataPointOrigin.Id was null");
			return GetById(id.Value, database);
		}

		public static DataPointOrigin GetById(uint id, Database database = null)
		{
			return GetById((ulong)id, database);
		}

		public static DataPointOrigin GetById(int? id, Database database = null)
		{
			Args.ThrowIfNull(id, "id");
			Args.ThrowIf(!id.HasValue, "specified DataPointOrigin.Id was null");
			return GetById(id.Value, database);
		}                                    
                                    
		public static DataPointOrigin GetById(int id, Database database = null)
		{
			return GetById((long)id, database);
		}

		public static DataPointOrigin GetById(long? id, Database database = null)
		{
			Args.ThrowIfNull(id, "id");
			Args.ThrowIf(!id.HasValue, "specified DataPointOrigin.Id was null");
			return GetById(id.Value, database);
		}
                                    
		public static DataPointOrigin GetById(long id, Database database = null)
		{
			return OneWhere(c => c.KeyColumn == id, database);
		}

		public static DataPointOrigin GetById(ulong? id, Database database = null)
		{
			Args.ThrowIfNull(id, "id");
			Args.ThrowIf(!id.HasValue, "specified DataPointOrigin.Id was null");
			return GetById(id.Value, database);
		}
                                    
		public static DataPointOrigin GetById(ulong id, Database database = null)
		{
			return OneWhere(c => c.KeyColumn == id, database);
		}

		public static DataPointOrigin GetByUuid(string uuid, Database database = null)
		{
			return OneWhere(c => Bam.Net.Data.Query.Where("Uuid") == uuid, database);
		}

		public static DataPointOrigin GetByCuid(string cuid, Database database = null)
		{
			return OneWhere(c => Bam.Net.Data.Query.Where("Cuid") == cuid, database);
		}

		[Bam.Net.Exclude]
		public static DataPointOriginCollection Query(QueryFilter filter, Database database = null)
		{
			return Where(filter, database);
		}

		[Bam.Net.Exclude]
		public static DataPointOriginCollection Where(QueryFilter filter, Database database = null)
		{
			WhereDelegate<DataPointOriginColumns> whereDelegate = (c) => filter;
			return Where(whereDelegate, database);
		}

		/// <summary>
		/// Execute a query and return the results.
		/// </summary>
		/// <param name="where">A Func delegate that recieves a DataPointOriginColumns
		/// and returns a QueryFilter which is the result of any comparisons
		/// between DataPointOriginColumns and other values
		/// </param>
		/// <param name="db"></param>
		[Bam.Net.Exclude]
		public static DataPointOriginCollection Where(Func<DataPointOriginColumns, QueryFilter<DataPointOriginColumns>> where, OrderBy<DataPointOriginColumns> orderBy = null, Database database = null)
		{
			database = database ?? Db.For<DataPointOrigin>();
			return new DataPointOriginCollection(database.GetQuery<DataPointOriginColumns, DataPointOrigin>(where, orderBy), true);
		}

		/// <summary>
		/// Execute a query and return the results.
		/// </summary>
		/// <param name="where">A WhereDelegate that recieves a DataPointOriginColumns
		/// and returns a IQueryFilter which is the result of any comparisons
		/// between DataPointOriginColumns and other values
		/// </param>
		/// <param name="db"></param>
		[Bam.Net.Exclude]
		public static DataPointOriginCollection Where(WhereDelegate<DataPointOriginColumns> where, Database database = null)
		{
			database = database ?? Db.For<DataPointOrigin>();
			var results = new DataPointOriginCollection(database, database.GetQuery<DataPointOriginColumns, DataPointOrigin>(where), true);
			return results;
		}

		/// <summary>
		/// Execute a query and return the results.
		/// </summary>
		/// <param name="where">A WhereDelegate that recieves a DataPointOriginColumns
		/// and returns a IQueryFilter which is the result of any comparisons
		/// between DataPointOriginColumns and other values
		/// </param>
		/// <param name="orderBy">
		/// Specifies what column and direction to order the results by
		/// </param>
		/// <param name="database"></param>
		[Bam.Net.Exclude]
		public static DataPointOriginCollection Where(WhereDelegate<DataPointOriginColumns> where, OrderBy<DataPointOriginColumns> orderBy = null, Database database = null)
		{
			database = database ?? Db.For<DataPointOrigin>();
			var results = new DataPointOriginCollection(database, database.GetQuery<DataPointOriginColumns, DataPointOrigin>(where, orderBy), true);
			return results;
		}

		/// <summary>
		/// This method is intended to respond to client side Qi queries.
		/// Use of this method from .Net should be avoided in favor of
		/// one of the methods that take a delegate of type
		/// WhereDelegate`DataPointOriginColumns`.
		/// </summary>
		/// <param name="where"></param>
		/// <param name="database"></param>
		public static DataPointOriginCollection Where(QiQuery where, Database database = null)
		{
			var results = new DataPointOriginCollection(database, Select<DataPointOriginColumns>.From<DataPointOrigin>().Where(where, database));
			return results;
		}

		/// <summary>
		/// Get one entry matching the specified filter.  If none exists
		/// one will be created; success will depend on the nullability
		/// of the specified columns.
		/// </summary>
		[Bam.Net.Exclude]
		public static DataPointOrigin GetOneWhere(QueryFilter where, Database database = null)
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
		public static DataPointOrigin OneWhere(QueryFilter where, Database database = null)
		{
			WhereDelegate<DataPointOriginColumns> whereDelegate = (c) => where;
			var result = Top(1, whereDelegate, database);
			return OneOrThrow(result);
		}

		/// <summary>
		/// Set one entry matching the specified filter.  If none exists
		/// one will be created; success will depend on the nullability
		/// of the specified columns.
		/// </summary>
		[Bam.Net.Exclude]
		public static void SetOneWhere(WhereDelegate<DataPointOriginColumns> where, Database database = null)
		{
			SetOneWhere(where, out DataPointOrigin ignore, database);
		}

		/// <summary>
		/// Set one entry matching the specified filter.  If none exists
		/// one will be created; success will depend on the nullability
		/// of the specified columns.
		/// </summary>
		[Bam.Net.Exclude]
		public static void SetOneWhere(WhereDelegate<DataPointOriginColumns> where, out DataPointOrigin result, Database database = null)
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
		public static DataPointOrigin GetOneWhere(WhereDelegate<DataPointOriginColumns> where, Database database = null)
		{
			var result = OneWhere(where, database);
			if(result == null)
			{
				DataPointOriginColumns c = new DataPointOriginColumns();
				IQueryFilter filter = where(c);
				result = CreateFromFilter(filter, database);
			}

			return result;
		}

		/// <summary>
		/// Execute a query that should return only one result.  If more
		/// than one result is returned a MultipleEntriesFoundException will
		/// be thrown.  This method is most commonly used to retrieve a
		/// single DataPointOrigin instance by its Id/Key value
		/// </summary>
		/// <param name="where">A WhereDelegate that recieves a DataPointOriginColumns
		/// and returns a IQueryFilter which is the result of any comparisons
		/// between DataPointOriginColumns and other values
		/// </param>
		/// <param name="database"></param>
		[Bam.Net.Exclude]
		public static DataPointOrigin OneWhere(WhereDelegate<DataPointOriginColumns> where, Database database = null)
		{
			var result = Top(1, where, database);
			return OneOrThrow(result);
		}

		/// <summary>
		/// This method is intended to respond to client side Qi queries.
		/// Use of this method from .Net should be avoided in favor of
		/// one of the methods that take a delegate of type
		/// WhereDelegate`DataPointOriginColumns`.
		/// </summary>
		/// <param name="where"></param>
		/// <param name="database"></param>
		public static DataPointOrigin OneWhere(QiQuery where, Database database = null)
		{
			var results = Top(1, where, database);
			return OneOrThrow(results);
		}

		/// <summary>
		/// Execute a query and return the first result.  This method will issue a sql TOP clause so only the
		/// specified number of values will be returned.
		/// </summary>
		/// <param name="where">A WhereDelegate that recieves a DataPointOriginColumns
		/// and returns a IQueryFilter which is the result of any comparisons
		/// between DataPointOriginColumns and other values
		/// </param>
		/// <param name="database"></param>
		[Bam.Net.Exclude]
		public static DataPointOrigin FirstOneWhere(WhereDelegate<DataPointOriginColumns> where, Database database = null)
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
		/// <param name="where">A WhereDelegate that recieves a DataPointOriginColumns
		/// and returns a IQueryFilter which is the result of any comparisons
		/// between DataPointOriginColumns and other values
		/// </param>
		/// <param name="database"></param>
		[Bam.Net.Exclude]
		public static DataPointOrigin FirstOneWhere(WhereDelegate<DataPointOriginColumns> where, OrderBy<DataPointOriginColumns> orderBy, Database database = null)
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
		/// <param name="where">A WhereDelegate that recieves a DataPointOriginColumns
		/// and returns a IQueryFilter which is the result of any comparisons
		/// between DataPointOriginColumns and other values
		/// </param>
		/// <param name="database"></param>
		[Bam.Net.Exclude]
		public static DataPointOrigin FirstOneWhere(QueryFilter where, OrderBy<DataPointOriginColumns> orderBy = null, Database database = null)
		{
			WhereDelegate<DataPointOriginColumns> whereDelegate = (c) => where;
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
		/// <param name="where">A WhereDelegate that recieves a DataPointOriginColumns
		/// and returns a IQueryFilter which is the result of any comparisons
		/// between DataPointOriginColumns and other values
		/// </param>
		/// <param name="database"></param>
		[Bam.Net.Exclude]
		public static DataPointOriginCollection Top(int count, WhereDelegate<DataPointOriginColumns> where, Database database = null)
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
		/// <param name="where">A WhereDelegate that recieves a DataPointOriginColumns
		/// and returns a IQueryFilter which is the result of any comparisons
		/// between DataPointOriginColumns and other values
		/// </param>
		/// <param name="orderBy">
		/// Specifies what column and direction to order the results by
		/// </param>
		/// <param name="database">
		/// Which database to query or null to use the default
		/// </param>
		[Bam.Net.Exclude]
		public static DataPointOriginCollection Top(int count, WhereDelegate<DataPointOriginColumns> where, OrderBy<DataPointOriginColumns> orderBy, Database database = null)
		{
			DataPointOriginColumns c = new DataPointOriginColumns();
			IQueryFilter filter = where(c);

			Database db = database ?? Db.For<DataPointOrigin>();
			QuerySet query = GetQuerySet(db);
			query.Top<DataPointOrigin>(count);
			query.Where(filter);

			if(orderBy != null)
			{
				query.OrderBy<DataPointOriginColumns>(orderBy);
			}

			query.Execute(db);
			var results = query.Results.As<DataPointOriginCollection>(0);
			results.Database = db;
			return results;
		}

		[Bam.Net.Exclude]
		public static DataPointOriginCollection Top(int count, QueryFilter where, Database database)
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
		public static DataPointOriginCollection Top(int count, QueryFilter where, OrderBy<DataPointOriginColumns> orderBy = null, Database database = null)
		{
			Database db = database ?? Db.For<DataPointOrigin>();
			QuerySet query = GetQuerySet(db);
			query.Top<DataPointOrigin>(count);
			query.Where(where);

			if(orderBy != null)
			{
				query.OrderBy<DataPointOriginColumns>(orderBy);
			}

			query.Execute(db);
			var results = query.Results.As<DataPointOriginCollection>(0);
			results.Database = db;
			return results;
		}

		[Bam.Net.Exclude]
		public static DataPointOriginCollection Top(int count, QueryFilter where, string orderBy = null, SortOrder sortOrder = SortOrder.Ascending, Database database = null)
		{
			Database db = database ?? Db.For<DataPointOrigin>();
			QuerySet query = GetQuerySet(db);
			query.Top<DataPointOrigin>(count);
			query.Where(where);

			if(orderBy != null)
			{
				query.OrderBy(orderBy, sortOrder);
			}

			query.Execute(db);
			var results = query.Results.As<DataPointOriginCollection>(0);
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
		public static DataPointOriginCollection Top(int count, QiQuery where, Database database = null)
		{
			Database db = database ?? Db.For<DataPointOrigin>();
			QuerySet query = GetQuerySet(db);
			query.Top<DataPointOrigin>(count);
			query.Where(where);
			query.Execute(db);
			var results = query.Results.As<DataPointOriginCollection>(0);
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
			Database db = database ?? Db.For<DataPointOrigin>();
            QuerySet query = GetQuerySet(db);
            query.Count<DataPointOrigin>();
            query.Execute(db);
            return (long)query.Results[0].DataRow[0];
        }

		/// <summary>
		/// Execute a query and return the number of results
		/// </summary>
		/// <param name="where">A WhereDelegate that recieves a DataPointOriginColumns
		/// and returns a IQueryFilter which is the result of any comparisons
		/// between DataPointOriginColumns and other values
		/// </param>
		/// <param name="database">
		/// Which database to query or null to use the default
		/// </param>
		[Bam.Net.Exclude]
		public static long Count(WhereDelegate<DataPointOriginColumns> where, Database database = null)
		{
			DataPointOriginColumns c = new DataPointOriginColumns();
			IQueryFilter filter = where(c) ;

			Database db = database ?? Db.For<DataPointOrigin>();
			QuerySet query = GetQuerySet(db);
			query.Count<DataPointOrigin>();
			query.Where(filter);
			query.Execute(db);
			return query.Results.As<CountResult>(0).Value;
		}

		public static long Count(QiQuery where, Database database = null)
		{
		    Database db = database ?? Db.For<DataPointOrigin>();
			QuerySet query = GetQuerySet(db);
			query.Count<DataPointOrigin>();
			query.Where(where);
			query.Execute(db);
			return query.Results.As<CountResult>(0).Value;
		}

		private static DataPointOrigin CreateFromFilter(IQueryFilter filter, Database database = null)
		{
			Database db = database ?? Db.For<DataPointOrigin>();
			var dao = new DataPointOrigin();
			filter.Parameters.Each(p=>
			{
				dao.Property(p.ColumnName, p.Value);
			});
			dao.Save(db);
			return dao;
		}

		private static DataPointOrigin OneOrThrow(DataPointOriginCollection c)
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
