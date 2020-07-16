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
	[Bam.Net.Data.Table("DataPropertyFilter", "DataReplication")]
	public partial class DataPropertyFilter: Bam.Net.Data.Dao
	{
		public DataPropertyFilter():base()
		{
			this.SetKeyColumnName();
			this.SetChildren();
		}

		public DataPropertyFilter(DataRow data)
			: base(data)
		{
			this.SetKeyColumnName();
			this.SetChildren();
		}

		public DataPropertyFilter(Database db)
			: base(db)
		{
			this.SetKeyColumnName();
			this.SetChildren();
		}

		public DataPropertyFilter(Database db, DataRow data)
			: base(db, data)
		{
			this.SetKeyColumnName();
			this.SetChildren();
		}

		[Bam.Net.Exclude]
		public static implicit operator DataPropertyFilter(DataRow data)
		{
			return new DataPropertyFilter(data);
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

	// property:TypeNamespace, columnName: TypeNamespace	
	[Bam.Net.Data.Column(Name="TypeNamespace", DbDataType="VarChar", MaxLength="4000", AllowNull=true)]
	public string TypeNamespace
	{
		get
		{
			return GetStringValue("TypeNamespace");
		}
		set
		{
			SetValue("TypeNamespace", value);
		}
	}

	// property:TypeName, columnName: TypeName	
	[Bam.Net.Data.Column(Name="TypeName", DbDataType="VarChar", MaxLength="4000", AllowNull=true)]
	public string TypeName
	{
		get
		{
			return GetStringValue("TypeName");
		}
		set
		{
			SetValue("TypeName", value);
		}
	}

	// property:InstanceIdentifier, columnName: InstanceIdentifier	
	[Bam.Net.Data.Column(Name="InstanceIdentifier", DbDataType="VarChar", MaxLength="4000", AllowNull=true)]
	public string InstanceIdentifier
	{
		get
		{
			return GetStringValue("InstanceIdentifier");
		}
		set
		{
			SetValue("InstanceIdentifier", value);
		}
	}

	// property:Name, columnName: Name	
	[Bam.Net.Data.Column(Name="Name", DbDataType="VarChar", MaxLength="4000", AllowNull=true)]
	public string Name
	{
		get
		{
			return GetStringValue("Name");
		}
		set
		{
			SetValue("Name", value);
		}
	}

	// property:CreateOperationId, columnName: CreateOperationId	
	[Bam.Net.Data.Column(Name="CreateOperationId", DbDataType="BigInt", MaxLength="19", AllowNull=true)]
	public ulong? CreateOperationId
	{
		get
		{
			return GetULongValue("CreateOperationId");
		}
		set
		{
			SetValue("CreateOperationId", value);
		}
	}

	// property:DeleteEventId, columnName: DeleteEventId	
	[Bam.Net.Data.Column(Name="DeleteEventId", DbDataType="BigInt", MaxLength="19", AllowNull=true)]
	public ulong? DeleteEventId
	{
		get
		{
			return GetULongValue("DeleteEventId");
		}
		set
		{
			SetValue("DeleteEventId", value);
		}
	}

	// property:DeleteOperationId, columnName: DeleteOperationId	
	[Bam.Net.Data.Column(Name="DeleteOperationId", DbDataType="BigInt", MaxLength="19", AllowNull=true)]
	public ulong? DeleteOperationId
	{
		get
		{
			return GetULongValue("DeleteOperationId");
		}
		set
		{
			SetValue("DeleteOperationId", value);
		}
	}

	// property:UpdateOperationId, columnName: UpdateOperationId	
	[Bam.Net.Data.Column(Name="UpdateOperationId", DbDataType="BigInt", MaxLength="19", AllowNull=true)]
	public ulong? UpdateOperationId
	{
		get
		{
			return GetULongValue("UpdateOperationId");
		}
		set
		{
			SetValue("UpdateOperationId", value);
		}
	}

	// property:WriteEventId, columnName: WriteEventId	
	[Bam.Net.Data.Column(Name="WriteEventId", DbDataType="BigInt", MaxLength="19", AllowNull=true)]
	public ulong? WriteEventId
	{
		get
		{
			return GetULongValue("WriteEventId");
		}
		set
		{
			SetValue("WriteEventId", value);
		}
	}

	// property:SaveOperationId, columnName: SaveOperationId	
	[Bam.Net.Data.Column(Name="SaveOperationId", DbDataType="BigInt", MaxLength="19", AllowNull=true)]
	public ulong? SaveOperationId
	{
		get
		{
			return GetULongValue("SaveOperationId");
		}
		set
		{
			SetValue("SaveOperationId", value);
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


	// start QueryOperationId -> QueryOperationId
	[Bam.Net.Data.ForeignKey(
        Table="DataPropertyFilter",
		Name="QueryOperationId",
		DbDataType="BigInt",
		MaxLength="",
		AllowNull=true,
		ReferencedKey="Id",
		ReferencedTable="QueryOperation",
		Suffix="1")]
	public ulong? QueryOperationId
	{
		get
		{
			return GetULongValue("QueryOperationId");
		}
		set
		{
			SetValue("QueryOperationId", value);
		}
	}

    QueryOperation _queryOperationOfQueryOperationId;
	public QueryOperation QueryOperationOfQueryOperationId
	{
		get
		{
			if(_queryOperationOfQueryOperationId == null)
			{
				_queryOperationOfQueryOperationId = Bam.Net.Services.DataReplication.Data.Dao.QueryOperation.OneWhere(c => c.KeyColumn == this.QueryOperationId, this.Database);
			}
			return _queryOperationOfQueryOperationId;
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
				var colFilter = new DataPropertyFilterColumns();
				return (colFilter.KeyColumn == IdValue);
			}
		}

		/// <summary>
        /// Return every record in the DataPropertyFilter table.
        /// </summary>
		/// <param name="database">
		/// The database to load from or null
		/// </param>
		public static DataPropertyFilterCollection LoadAll(Database database = null)
		{
			Database db = database ?? Db.For<DataPropertyFilter>();
            SqlStringBuilder sql = db.GetSqlStringBuilder();
            sql.Select<DataPropertyFilter>();
            var results = new DataPropertyFilterCollection(db, sql.GetDataTable(db))
            {
                Database = db
            };
            return results;
        }

        /// <summary>
        /// Process all records in batches of the specified size
        /// </summary>
        [Bam.Net.Exclude]
        public static async Task BatchAll(int batchSize, Action<IEnumerable<DataPropertyFilter>> batchProcessor, Database database = null)
		{
			await System.Threading.Tasks.Task.Run(async ()=>
			{
				DataPropertyFilterColumns columns = new DataPropertyFilterColumns();
				var orderBy = Bam.Net.Data.Order.By<DataPropertyFilterColumns>(c => c.KeyColumn, Bam.Net.Data.SortOrder.Ascending);
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
		public static async Task BatchQuery(int batchSize, QueryFilter filter, Action<IEnumerable<DataPropertyFilter>> batchProcessor, Database database = null)
		{
			await BatchQuery(batchSize, (c) => filter, batchProcessor, database);
		}

		/// <summary>
		/// Process results of a query in batches of the specified size
		/// </summary>
		[Bam.Net.Exclude]
		public static async Task BatchQuery(int batchSize, WhereDelegate<DataPropertyFilterColumns> where, Action<IEnumerable<DataPropertyFilter>> batchProcessor, Database database = null)
		{
			await System.Threading.Tasks.Task.Run(async ()=>
			{
				DataPropertyFilterColumns columns = new DataPropertyFilterColumns();
				var orderBy = Bam.Net.Data.Order.By<DataPropertyFilterColumns>(c => c.KeyColumn, Bam.Net.Data.SortOrder.Ascending);
				var results = Top(batchSize, where, orderBy, database);
				while(results.Count > 0)
				{
					await System.Threading.Tasks.Task.Run(()=>
					{
						batchProcessor(results);
					});
					long topId = results.Select(d => d.Property<long>(columns.KeyColumn.ToString())).ToArray().Largest();
					results = Top(batchSize, (DataPropertyFilterColumns)where(columns) && columns.KeyColumn > topId, orderBy, database);
				}
			});
		}

		/// <summary>
		/// Process results of a query in batches of the specified size
		/// </summary>
		[Bam.Net.Exclude]
		public static async Task BatchQuery<ColType>(int batchSize, QueryFilter filter, Action<IEnumerable<DataPropertyFilter>> batchProcessor, Bam.Net.Data.OrderBy<DataPropertyFilterColumns> orderBy, Database database = null)
		{
			await BatchQuery<ColType>(batchSize, (c) => filter, batchProcessor, orderBy, database);
		}

		/// <summary>
		/// Process results of a query in batches of the specified size
		/// </summary>
		[Bam.Net.Exclude]
		public static async Task BatchQuery<ColType>(int batchSize, WhereDelegate<DataPropertyFilterColumns> where, Action<IEnumerable<DataPropertyFilter>> batchProcessor, Bam.Net.Data.OrderBy<DataPropertyFilterColumns> orderBy, Database database = null)
		{
			await System.Threading.Tasks.Task.Run(async ()=>
			{
				DataPropertyFilterColumns columns = new DataPropertyFilterColumns();
				var results = Top(batchSize, where, orderBy, database);
				while(results.Count > 0)
				{
					await System.Threading.Tasks.Task.Run(()=>
					{
						batchProcessor(results);
					});
					ColType top = results.Select(d => d.Property<ColType>(orderBy.Column.ToString())).ToArray().Largest();
					results = Top(batchSize, (DataPropertyFilterColumns)where(columns) && orderBy.Column > top, orderBy, database);
				}
			});
		}

		public static DataPropertyFilter GetById(uint? id, Database database = null)
		{
			Args.ThrowIfNull(id, "id");
			Args.ThrowIf(!id.HasValue, "specified DataPropertyFilter.Id was null");
			return GetById(id.Value, database);
		}

		public static DataPropertyFilter GetById(uint id, Database database = null)
		{
			return GetById((ulong)id, database);
		}

		public static DataPropertyFilter GetById(int? id, Database database = null)
		{
			Args.ThrowIfNull(id, "id");
			Args.ThrowIf(!id.HasValue, "specified DataPropertyFilter.Id was null");
			return GetById(id.Value, database);
		}                                    
                                    
		public static DataPropertyFilter GetById(int id, Database database = null)
		{
			return GetById((long)id, database);
		}

		public static DataPropertyFilter GetById(long? id, Database database = null)
		{
			Args.ThrowIfNull(id, "id");
			Args.ThrowIf(!id.HasValue, "specified DataPropertyFilter.Id was null");
			return GetById(id.Value, database);
		}
                                    
		public static DataPropertyFilter GetById(long id, Database database = null)
		{
			return OneWhere(c => c.KeyColumn == id, database);
		}

		public static DataPropertyFilter GetById(ulong? id, Database database = null)
		{
			Args.ThrowIfNull(id, "id");
			Args.ThrowIf(!id.HasValue, "specified DataPropertyFilter.Id was null");
			return GetById(id.Value, database);
		}
                                    
		public static DataPropertyFilter GetById(ulong id, Database database = null)
		{
			return OneWhere(c => c.KeyColumn == id, database);
		}

		public static DataPropertyFilter GetByUuid(string uuid, Database database = null)
		{
			return OneWhere(c => Bam.Net.Data.Query.Where("Uuid") == uuid, database);
		}

		public static DataPropertyFilter GetByCuid(string cuid, Database database = null)
		{
			return OneWhere(c => Bam.Net.Data.Query.Where("Cuid") == cuid, database);
		}

		[Bam.Net.Exclude]
		public static DataPropertyFilterCollection Query(QueryFilter filter, Database database = null)
		{
			return Where(filter, database);
		}

		[Bam.Net.Exclude]
		public static DataPropertyFilterCollection Where(QueryFilter filter, Database database = null)
		{
			WhereDelegate<DataPropertyFilterColumns> whereDelegate = (c) => filter;
			return Where(whereDelegate, database);
		}

		/// <summary>
		/// Execute a query and return the results.
		/// </summary>
		/// <param name="where">A Func delegate that recieves a DataPropertyFilterColumns
		/// and returns a QueryFilter which is the result of any comparisons
		/// between DataPropertyFilterColumns and other values
		/// </param>
		/// <param name="db"></param>
		[Bam.Net.Exclude]
		public static DataPropertyFilterCollection Where(Func<DataPropertyFilterColumns, QueryFilter<DataPropertyFilterColumns>> where, OrderBy<DataPropertyFilterColumns> orderBy = null, Database database = null)
		{
			database = database ?? Db.For<DataPropertyFilter>();
			return new DataPropertyFilterCollection(database.GetQuery<DataPropertyFilterColumns, DataPropertyFilter>(where, orderBy), true);
		}

		/// <summary>
		/// Execute a query and return the results.
		/// </summary>
		/// <param name="where">A WhereDelegate that recieves a DataPropertyFilterColumns
		/// and returns a IQueryFilter which is the result of any comparisons
		/// between DataPropertyFilterColumns and other values
		/// </param>
		/// <param name="db"></param>
		[Bam.Net.Exclude]
		public static DataPropertyFilterCollection Where(WhereDelegate<DataPropertyFilterColumns> where, Database database = null)
		{
			database = database ?? Db.For<DataPropertyFilter>();
			var results = new DataPropertyFilterCollection(database, database.GetQuery<DataPropertyFilterColumns, DataPropertyFilter>(where), true);
			return results;
		}

		/// <summary>
		/// Execute a query and return the results.
		/// </summary>
		/// <param name="where">A WhereDelegate that recieves a DataPropertyFilterColumns
		/// and returns a IQueryFilter which is the result of any comparisons
		/// between DataPropertyFilterColumns and other values
		/// </param>
		/// <param name="orderBy">
		/// Specifies what column and direction to order the results by
		/// </param>
		/// <param name="database"></param>
		[Bam.Net.Exclude]
		public static DataPropertyFilterCollection Where(WhereDelegate<DataPropertyFilterColumns> where, OrderBy<DataPropertyFilterColumns> orderBy = null, Database database = null)
		{
			database = database ?? Db.For<DataPropertyFilter>();
			var results = new DataPropertyFilterCollection(database, database.GetQuery<DataPropertyFilterColumns, DataPropertyFilter>(where, orderBy), true);
			return results;
		}

		/// <summary>
		/// This method is intended to respond to client side Qi queries.
		/// Use of this method from .Net should be avoided in favor of
		/// one of the methods that take a delegate of type
		/// WhereDelegate`DataPropertyFilterColumns`.
		/// </summary>
		/// <param name="where"></param>
		/// <param name="database"></param>
		public static DataPropertyFilterCollection Where(QiQuery where, Database database = null)
		{
			var results = new DataPropertyFilterCollection(database, Select<DataPropertyFilterColumns>.From<DataPropertyFilter>().Where(where, database));
			return results;
		}

		/// <summary>
		/// Get one entry matching the specified filter.  If none exists
		/// one will be created; success will depend on the nullability
		/// of the specified columns.
		/// </summary>
		[Bam.Net.Exclude]
		public static DataPropertyFilter GetOneWhere(QueryFilter where, Database database = null)
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
		public static DataPropertyFilter OneWhere(QueryFilter where, Database database = null)
		{
			WhereDelegate<DataPropertyFilterColumns> whereDelegate = (c) => where;
			var result = Top(1, whereDelegate, database);
			return OneOrThrow(result);
		}

		/// <summary>
		/// Set one entry matching the specified filter.  If none exists
		/// one will be created; success will depend on the nullability
		/// of the specified columns.
		/// </summary>
		[Bam.Net.Exclude]
		public static void SetOneWhere(WhereDelegate<DataPropertyFilterColumns> where, Database database = null)
		{
			SetOneWhere(where, out DataPropertyFilter ignore, database);
		}

		/// <summary>
		/// Set one entry matching the specified filter.  If none exists
		/// one will be created; success will depend on the nullability
		/// of the specified columns.
		/// </summary>
		[Bam.Net.Exclude]
		public static void SetOneWhere(WhereDelegate<DataPropertyFilterColumns> where, out DataPropertyFilter result, Database database = null)
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
		public static DataPropertyFilter GetOneWhere(WhereDelegate<DataPropertyFilterColumns> where, Database database = null)
		{
			var result = OneWhere(where, database);
			if(result == null)
			{
				DataPropertyFilterColumns c = new DataPropertyFilterColumns();
				IQueryFilter filter = where(c);
				result = CreateFromFilter(filter, database);
			}

			return result;
		}

		/// <summary>
		/// Execute a query that should return only one result.  If more
		/// than one result is returned a MultipleEntriesFoundException will
		/// be thrown.  This method is most commonly used to retrieve a
		/// single DataPropertyFilter instance by its Id/Key value
		/// </summary>
		/// <param name="where">A WhereDelegate that recieves a DataPropertyFilterColumns
		/// and returns a IQueryFilter which is the result of any comparisons
		/// between DataPropertyFilterColumns and other values
		/// </param>
		/// <param name="database"></param>
		[Bam.Net.Exclude]
		public static DataPropertyFilter OneWhere(WhereDelegate<DataPropertyFilterColumns> where, Database database = null)
		{
			var result = Top(1, where, database);
			return OneOrThrow(result);
		}

		/// <summary>
		/// This method is intended to respond to client side Qi queries.
		/// Use of this method from .Net should be avoided in favor of
		/// one of the methods that take a delegate of type
		/// WhereDelegate`DataPropertyFilterColumns`.
		/// </summary>
		/// <param name="where"></param>
		/// <param name="database"></param>
		public static DataPropertyFilter OneWhere(QiQuery where, Database database = null)
		{
			var results = Top(1, where, database);
			return OneOrThrow(results);
		}

		/// <summary>
		/// Execute a query and return the first result.  This method will issue a sql TOP clause so only the
		/// specified number of values will be returned.
		/// </summary>
		/// <param name="where">A WhereDelegate that recieves a DataPropertyFilterColumns
		/// and returns a IQueryFilter which is the result of any comparisons
		/// between DataPropertyFilterColumns and other values
		/// </param>
		/// <param name="database"></param>
		[Bam.Net.Exclude]
		public static DataPropertyFilter FirstOneWhere(WhereDelegate<DataPropertyFilterColumns> where, Database database = null)
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
		/// <param name="where">A WhereDelegate that recieves a DataPropertyFilterColumns
		/// and returns a IQueryFilter which is the result of any comparisons
		/// between DataPropertyFilterColumns and other values
		/// </param>
		/// <param name="database"></param>
		[Bam.Net.Exclude]
		public static DataPropertyFilter FirstOneWhere(WhereDelegate<DataPropertyFilterColumns> where, OrderBy<DataPropertyFilterColumns> orderBy, Database database = null)
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
		/// <param name="where">A WhereDelegate that recieves a DataPropertyFilterColumns
		/// and returns a IQueryFilter which is the result of any comparisons
		/// between DataPropertyFilterColumns and other values
		/// </param>
		/// <param name="database"></param>
		[Bam.Net.Exclude]
		public static DataPropertyFilter FirstOneWhere(QueryFilter where, OrderBy<DataPropertyFilterColumns> orderBy = null, Database database = null)
		{
			WhereDelegate<DataPropertyFilterColumns> whereDelegate = (c) => where;
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
		/// <param name="where">A WhereDelegate that recieves a DataPropertyFilterColumns
		/// and returns a IQueryFilter which is the result of any comparisons
		/// between DataPropertyFilterColumns and other values
		/// </param>
		/// <param name="database"></param>
		[Bam.Net.Exclude]
		public static DataPropertyFilterCollection Top(int count, WhereDelegate<DataPropertyFilterColumns> where, Database database = null)
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
		/// <param name="where">A WhereDelegate that recieves a DataPropertyFilterColumns
		/// and returns a IQueryFilter which is the result of any comparisons
		/// between DataPropertyFilterColumns and other values
		/// </param>
		/// <param name="orderBy">
		/// Specifies what column and direction to order the results by
		/// </param>
		/// <param name="database">
		/// Which database to query or null to use the default
		/// </param>
		[Bam.Net.Exclude]
		public static DataPropertyFilterCollection Top(int count, WhereDelegate<DataPropertyFilterColumns> where, OrderBy<DataPropertyFilterColumns> orderBy, Database database = null)
		{
			DataPropertyFilterColumns c = new DataPropertyFilterColumns();
			IQueryFilter filter = where(c);

			Database db = database ?? Db.For<DataPropertyFilter>();
			QuerySet query = GetQuerySet(db);
			query.Top<DataPropertyFilter>(count);
			query.Where(filter);

			if(orderBy != null)
			{
				query.OrderBy<DataPropertyFilterColumns>(orderBy);
			}

			query.Execute(db);
			var results = query.Results.As<DataPropertyFilterCollection>(0);
			results.Database = db;
			return results;
		}

		[Bam.Net.Exclude]
		public static DataPropertyFilterCollection Top(int count, QueryFilter where, Database database)
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
		public static DataPropertyFilterCollection Top(int count, QueryFilter where, OrderBy<DataPropertyFilterColumns> orderBy = null, Database database = null)
		{
			Database db = database ?? Db.For<DataPropertyFilter>();
			QuerySet query = GetQuerySet(db);
			query.Top<DataPropertyFilter>(count);
			query.Where(where);

			if(orderBy != null)
			{
				query.OrderBy<DataPropertyFilterColumns>(orderBy);
			}

			query.Execute(db);
			var results = query.Results.As<DataPropertyFilterCollection>(0);
			results.Database = db;
			return results;
		}

		[Bam.Net.Exclude]
		public static DataPropertyFilterCollection Top(int count, QueryFilter where, string orderBy = null, SortOrder sortOrder = SortOrder.Ascending, Database database = null)
		{
			Database db = database ?? Db.For<DataPropertyFilter>();
			QuerySet query = GetQuerySet(db);
			query.Top<DataPropertyFilter>(count);
			query.Where(where);

			if(orderBy != null)
			{
				query.OrderBy(orderBy, sortOrder);
			}

			query.Execute(db);
			var results = query.Results.As<DataPropertyFilterCollection>(0);
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
		public static DataPropertyFilterCollection Top(int count, QiQuery where, Database database = null)
		{
			Database db = database ?? Db.For<DataPropertyFilter>();
			QuerySet query = GetQuerySet(db);
			query.Top<DataPropertyFilter>(count);
			query.Where(where);
			query.Execute(db);
			var results = query.Results.As<DataPropertyFilterCollection>(0);
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
			Database db = database ?? Db.For<DataPropertyFilter>();
            QuerySet query = GetQuerySet(db);
            query.Count<DataPropertyFilter>();
            query.Execute(db);
            return (long)query.Results[0].DataRow[0];
        }

		/// <summary>
		/// Execute a query and return the number of results
		/// </summary>
		/// <param name="where">A WhereDelegate that recieves a DataPropertyFilterColumns
		/// and returns a IQueryFilter which is the result of any comparisons
		/// between DataPropertyFilterColumns and other values
		/// </param>
		/// <param name="database">
		/// Which database to query or null to use the default
		/// </param>
		[Bam.Net.Exclude]
		public static long Count(WhereDelegate<DataPropertyFilterColumns> where, Database database = null)
		{
			DataPropertyFilterColumns c = new DataPropertyFilterColumns();
			IQueryFilter filter = where(c) ;

			Database db = database ?? Db.For<DataPropertyFilter>();
			QuerySet query = GetQuerySet(db);
			query.Count<DataPropertyFilter>();
			query.Where(filter);
			query.Execute(db);
			return query.Results.As<CountResult>(0).Value;
		}

		public static long Count(QiQuery where, Database database = null)
		{
		    Database db = database ?? Db.For<DataPropertyFilter>();
			QuerySet query = GetQuerySet(db);
			query.Count<DataPropertyFilter>();
			query.Where(where);
			query.Execute(db);
			return query.Results.As<CountResult>(0).Value;
		}

		private static DataPropertyFilter CreateFromFilter(IQueryFilter filter, Database database = null)
		{
			Database db = database ?? Db.For<DataPropertyFilter>();
			var dao = new DataPropertyFilter();
			filter.Parameters.Each(p=>
			{
				dao.Property(p.ColumnName, p.Value);
			});
			dao.Save(db);
			return dao;
		}

		private static DataPropertyFilter OneOrThrow(DataPropertyFilterCollection c)
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
