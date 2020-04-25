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

namespace Bam.Net.Logging.Http.Data.Dao
{
	// schema = HttpLogging
	// connection Name = HttpLogging
	[Serializable]
	[Bam.Net.Data.Table("ResponseData", "HttpLogging")]
	public partial class ResponseData: Bam.Net.Data.Dao
	{
		public ResponseData():base()
		{
			this.SetKeyColumnName();
			this.SetChildren();
		}

		public ResponseData(DataRow data)
			: base(data)
		{
			this.SetKeyColumnName();
			this.SetChildren();
		}

		public ResponseData(Database db)
			: base(db)
		{
			this.SetKeyColumnName();
			this.SetChildren();
		}

		public ResponseData(Database db, DataRow data)
			: base(db, data)
		{
			this.SetKeyColumnName();
			this.SetChildren();
		}

		[Bam.Net.Exclude]
		public static implicit operator ResponseData(DataRow data)
		{
			return new ResponseData(data);
		}

		private void SetChildren()
		{


			if(_database != null)
			{
				this.ChildCollections.Add("CookieData_ResponseDataId", new CookieDataCollection(Database.GetQuery<CookieDataColumns, CookieData>((c) => c.ResponseDataId == GetULongValue("Id")), this, "ResponseDataId"));
			}
			if(_database != null)
			{
				this.ChildCollections.Add("HeaderData_ResponseDataId", new HeaderDataCollection(Database.GetQuery<HeaderDataColumns, HeaderData>((c) => c.ResponseDataId == GetULongValue("Id")), this, "ResponseDataId"));
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

	// property:RequestId, columnName: RequestId	
	[Bam.Net.Data.Column(Name="RequestId", DbDataType="VarChar", MaxLength="4000", AllowNull=true)]
	public string RequestId
	{
		get
		{
			return GetStringValue("RequestId");
		}
		set
		{
			SetValue("RequestId", value);
		}
	}

	// property:CheckedPaths, columnName: CheckedPaths	
	[Bam.Net.Data.Column(Name="CheckedPaths", DbDataType="VarChar", MaxLength="4000", AllowNull=true)]
	public string CheckedPaths
	{
		get
		{
			return GetStringValue("CheckedPaths");
		}
		set
		{
			SetValue("CheckedPaths", value);
		}
	}

	// property:ContentLength, columnName: ContentLength	
	[Bam.Net.Data.Column(Name="ContentLength", DbDataType="BigInt", MaxLength="19", AllowNull=true)]
	public long? ContentLength
	{
		get
		{
			return GetLongValue("ContentLength");
		}
		set
		{
			SetValue("ContentLength", value);
		}
	}

	// property:ContentType, columnName: ContentType	
	[Bam.Net.Data.Column(Name="ContentType", DbDataType="VarChar", MaxLength="4000", AllowNull=true)]
	public string ContentType
	{
		get
		{
			return GetStringValue("ContentType");
		}
		set
		{
			SetValue("ContentType", value);
		}
	}

	// property:RedirectLocation, columnName: RedirectLocation	
	[Bam.Net.Data.Column(Name="RedirectLocation", DbDataType="VarChar", MaxLength="4000", AllowNull=true)]
	public string RedirectLocation
	{
		get
		{
			return GetStringValue("RedirectLocation");
		}
		set
		{
			SetValue("RedirectLocation", value);
		}
	}

	// property:StatusCode, columnName: StatusCode	
	[Bam.Net.Data.Column(Name="StatusCode", DbDataType="Int", MaxLength="10", AllowNull=true)]
	public int? StatusCode
	{
		get
		{
			return GetIntValue("StatusCode");
		}
		set
		{
			SetValue("StatusCode", value);
		}
	}

	// property:StatusDescription, columnName: StatusDescription	
	[Bam.Net.Data.Column(Name="StatusDescription", DbDataType="VarChar", MaxLength="4000", AllowNull=true)]
	public string StatusDescription
	{
		get
		{
			return GetStringValue("StatusDescription");
		}
		set
		{
			SetValue("StatusDescription", value);
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
	public CookieDataCollection CookieDatasByResponseDataId
	{
		get
		{
			if (this.IsNew)
			{
				throw new InvalidOperationException("The current instance of type({0}) hasn't been saved and will have no child collections, call Save() or Save(Database) first."._Format(this.GetType().Name));
			}

			if(!this.ChildCollections.ContainsKey("CookieData_ResponseDataId"))
			{
				SetChildren();
			}

			var c = (CookieDataCollection)this.ChildCollections["CookieData_ResponseDataId"];
			if(!c.Loaded)
			{
				c.Load(Database);
			}
			return c;
		}
	}
		[Bam.Net.Exclude]	
	public HeaderDataCollection HeaderDatasByResponseDataId
	{
		get
		{
			if (this.IsNew)
			{
				throw new InvalidOperationException("The current instance of type({0}) hasn't been saved and will have no child collections, call Save() or Save(Database) first."._Format(this.GetType().Name));
			}

			if(!this.ChildCollections.ContainsKey("HeaderData_ResponseDataId"))
			{
				SetChildren();
			}

			var c = (HeaderDataCollection)this.ChildCollections["HeaderData_ResponseDataId"];
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
				var colFilter = new ResponseDataColumns();
				return (colFilter.KeyColumn == IdValue);
			}
		}

		/// <summary>
        /// Return every record in the ResponseData table.
        /// </summary>
		/// <param name="database">
		/// The database to load from or null
		/// </param>
		public static ResponseDataCollection LoadAll(Database database = null)
		{
			Database db = database ?? Db.For<ResponseData>();
            SqlStringBuilder sql = db.GetSqlStringBuilder();
            sql.Select<ResponseData>();
            var results = new ResponseDataCollection(db, sql.GetDataTable(db))
            {
                Database = db
            };
            return results;
        }

        /// <summary>
        /// Process all records in batches of the specified size
        /// </summary>
        [Bam.Net.Exclude]
        public static async Task BatchAll(int batchSize, Action<IEnumerable<ResponseData>> batchProcessor, Database database = null)
		{
			await System.Threading.Tasks.Task.Run(async ()=>
			{
				ResponseDataColumns columns = new ResponseDataColumns();
				var orderBy = Bam.Net.Data.Order.By<ResponseDataColumns>(c => c.KeyColumn, Bam.Net.Data.SortOrder.Ascending);
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
		public static async Task BatchQuery(int batchSize, QueryFilter filter, Action<IEnumerable<ResponseData>> batchProcessor, Database database = null)
		{
			await BatchQuery(batchSize, (c) => filter, batchProcessor, database);
		}

		/// <summary>
		/// Process results of a query in batches of the specified size
		/// </summary>
		[Bam.Net.Exclude]
		public static async Task BatchQuery(int batchSize, WhereDelegate<ResponseDataColumns> where, Action<IEnumerable<ResponseData>> batchProcessor, Database database = null)
		{
			await System.Threading.Tasks.Task.Run(async ()=>
			{
				ResponseDataColumns columns = new ResponseDataColumns();
				var orderBy = Bam.Net.Data.Order.By<ResponseDataColumns>(c => c.KeyColumn, Bam.Net.Data.SortOrder.Ascending);
				var results = Top(batchSize, where, orderBy, database);
				while(results.Count > 0)
				{
					await System.Threading.Tasks.Task.Run(()=>
					{
						batchProcessor(results);
					});
					long topId = results.Select(d => d.Property<long>(columns.KeyColumn.ToString())).ToArray().Largest();
					results = Top(batchSize, (ResponseDataColumns)where(columns) && columns.KeyColumn > topId, orderBy, database);
				}
			});
		}

		/// <summary>
		/// Process results of a query in batches of the specified size
		/// </summary>
		[Bam.Net.Exclude]
		public static async Task BatchQuery<ColType>(int batchSize, QueryFilter filter, Action<IEnumerable<ResponseData>> batchProcessor, Bam.Net.Data.OrderBy<ResponseDataColumns> orderBy, Database database = null)
		{
			await BatchQuery<ColType>(batchSize, (c) => filter, batchProcessor, orderBy, database);
		}

		/// <summary>
		/// Process results of a query in batches of the specified size
		/// </summary>
		[Bam.Net.Exclude]
		public static async Task BatchQuery<ColType>(int batchSize, WhereDelegate<ResponseDataColumns> where, Action<IEnumerable<ResponseData>> batchProcessor, Bam.Net.Data.OrderBy<ResponseDataColumns> orderBy, Database database = null)
		{
			await System.Threading.Tasks.Task.Run(async ()=>
			{
				ResponseDataColumns columns = new ResponseDataColumns();
				var results = Top(batchSize, where, orderBy, database);
				while(results.Count > 0)
				{
					await System.Threading.Tasks.Task.Run(()=>
					{
						batchProcessor(results);
					});
					ColType top = results.Select(d => d.Property<ColType>(orderBy.Column.ToString())).ToArray().Largest();
					results = Top(batchSize, (ResponseDataColumns)where(columns) && orderBy.Column > top, orderBy, database);
				}
			});
		}

		public static ResponseData GetById(uint? id, Database database = null)
		{
			Args.ThrowIfNull(id, "id");
			Args.ThrowIf(!id.HasValue, "specified ResponseData.Id was null");
			return GetById(id.Value, database);
		}

		public static ResponseData GetById(uint id, Database database = null)
		{
			return GetById((ulong)id, database);
		}

		public static ResponseData GetById(int? id, Database database = null)
		{
			Args.ThrowIfNull(id, "id");
			Args.ThrowIf(!id.HasValue, "specified ResponseData.Id was null");
			return GetById(id.Value, database);
		}                                    
                                    
		public static ResponseData GetById(int id, Database database = null)
		{
			return GetById((long)id, database);
		}

		public static ResponseData GetById(long? id, Database database = null)
		{
			Args.ThrowIfNull(id, "id");
			Args.ThrowIf(!id.HasValue, "specified ResponseData.Id was null");
			return GetById(id.Value, database);
		}
                                    
		public static ResponseData GetById(long id, Database database = null)
		{
			return OneWhere(c => c.KeyColumn == id, database);
		}

		public static ResponseData GetById(ulong? id, Database database = null)
		{
			Args.ThrowIfNull(id, "id");
			Args.ThrowIf(!id.HasValue, "specified ResponseData.Id was null");
			return GetById(id.Value, database);
		}
                                    
		public static ResponseData GetById(ulong id, Database database = null)
		{
			return OneWhere(c => c.KeyColumn == id, database);
		}

		public static ResponseData GetByUuid(string uuid, Database database = null)
		{
			return OneWhere(c => Bam.Net.Data.Query.Where("Uuid") == uuid, database);
		}

		public static ResponseData GetByCuid(string cuid, Database database = null)
		{
			return OneWhere(c => Bam.Net.Data.Query.Where("Cuid") == cuid, database);
		}

		[Bam.Net.Exclude]
		public static ResponseDataCollection Query(QueryFilter filter, Database database = null)
		{
			return Where(filter, database);
		}

		[Bam.Net.Exclude]
		public static ResponseDataCollection Where(QueryFilter filter, Database database = null)
		{
			WhereDelegate<ResponseDataColumns> whereDelegate = (c) => filter;
			return Where(whereDelegate, database);
		}

		/// <summary>
		/// Execute a query and return the results.
		/// </summary>
		/// <param name="where">A Func delegate that recieves a ResponseDataColumns
		/// and returns a QueryFilter which is the result of any comparisons
		/// between ResponseDataColumns and other values
		/// </param>
		/// <param name="db"></param>
		[Bam.Net.Exclude]
		public static ResponseDataCollection Where(Func<ResponseDataColumns, QueryFilter<ResponseDataColumns>> where, OrderBy<ResponseDataColumns> orderBy = null, Database database = null)
		{
			database = database ?? Db.For<ResponseData>();
			return new ResponseDataCollection(database.GetQuery<ResponseDataColumns, ResponseData>(where, orderBy), true);
		}

		/// <summary>
		/// Execute a query and return the results.
		/// </summary>
		/// <param name="where">A WhereDelegate that recieves a ResponseDataColumns
		/// and returns a IQueryFilter which is the result of any comparisons
		/// between ResponseDataColumns and other values
		/// </param>
		/// <param name="db"></param>
		[Bam.Net.Exclude]
		public static ResponseDataCollection Where(WhereDelegate<ResponseDataColumns> where, Database database = null)
		{
			database = database ?? Db.For<ResponseData>();
			var results = new ResponseDataCollection(database, database.GetQuery<ResponseDataColumns, ResponseData>(where), true);
			return results;
		}

		/// <summary>
		/// Execute a query and return the results.
		/// </summary>
		/// <param name="where">A WhereDelegate that recieves a ResponseDataColumns
		/// and returns a IQueryFilter which is the result of any comparisons
		/// between ResponseDataColumns and other values
		/// </param>
		/// <param name="orderBy">
		/// Specifies what column and direction to order the results by
		/// </param>
		/// <param name="database"></param>
		[Bam.Net.Exclude]
		public static ResponseDataCollection Where(WhereDelegate<ResponseDataColumns> where, OrderBy<ResponseDataColumns> orderBy = null, Database database = null)
		{
			database = database ?? Db.For<ResponseData>();
			var results = new ResponseDataCollection(database, database.GetQuery<ResponseDataColumns, ResponseData>(where, orderBy), true);
			return results;
		}

		/// <summary>
		/// This method is intended to respond to client side Qi queries.
		/// Use of this method from .Net should be avoided in favor of
		/// one of the methods that take a delegate of type
		/// WhereDelegate`ResponseDataColumns`.
		/// </summary>
		/// <param name="where"></param>
		/// <param name="database"></param>
		public static ResponseDataCollection Where(QiQuery where, Database database = null)
		{
			var results = new ResponseDataCollection(database, Select<ResponseDataColumns>.From<ResponseData>().Where(where, database));
			return results;
		}

		/// <summary>
		/// Get one entry matching the specified filter.  If none exists
		/// one will be created; success will depend on the nullability
		/// of the specified columns.
		/// </summary>
		[Bam.Net.Exclude]
		public static ResponseData GetOneWhere(QueryFilter where, Database database = null)
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
		public static ResponseData OneWhere(QueryFilter where, Database database = null)
		{
			WhereDelegate<ResponseDataColumns> whereDelegate = (c) => where;
			var result = Top(1, whereDelegate, database);
			return OneOrThrow(result);
		}

		/// <summary>
		/// Set one entry matching the specified filter.  If none exists
		/// one will be created; success will depend on the nullability
		/// of the specified columns.
		/// </summary>
		[Bam.Net.Exclude]
		public static void SetOneWhere(WhereDelegate<ResponseDataColumns> where, Database database = null)
		{
			SetOneWhere(where, out ResponseData ignore, database);
		}

		/// <summary>
		/// Set one entry matching the specified filter.  If none exists
		/// one will be created; success will depend on the nullability
		/// of the specified columns.
		/// </summary>
		[Bam.Net.Exclude]
		public static void SetOneWhere(WhereDelegate<ResponseDataColumns> where, out ResponseData result, Database database = null)
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
		public static ResponseData GetOneWhere(WhereDelegate<ResponseDataColumns> where, Database database = null)
		{
			var result = OneWhere(where, database);
			if(result == null)
			{
				ResponseDataColumns c = new ResponseDataColumns();
				IQueryFilter filter = where(c);
				result = CreateFromFilter(filter, database);
			}

			return result;
		}

		/// <summary>
		/// Execute a query that should return only one result.  If more
		/// than one result is returned a MultipleEntriesFoundException will
		/// be thrown.  This method is most commonly used to retrieve a
		/// single ResponseData instance by its Id/Key value
		/// </summary>
		/// <param name="where">A WhereDelegate that recieves a ResponseDataColumns
		/// and returns a IQueryFilter which is the result of any comparisons
		/// between ResponseDataColumns and other values
		/// </param>
		/// <param name="database"></param>
		[Bam.Net.Exclude]
		public static ResponseData OneWhere(WhereDelegate<ResponseDataColumns> where, Database database = null)
		{
			var result = Top(1, where, database);
			return OneOrThrow(result);
		}

		/// <summary>
		/// This method is intended to respond to client side Qi queries.
		/// Use of this method from .Net should be avoided in favor of
		/// one of the methods that take a delegate of type
		/// WhereDelegate`ResponseDataColumns`.
		/// </summary>
		/// <param name="where"></param>
		/// <param name="database"></param>
		public static ResponseData OneWhere(QiQuery where, Database database = null)
		{
			var results = Top(1, where, database);
			return OneOrThrow(results);
		}

		/// <summary>
		/// Execute a query and return the first result.  This method will issue a sql TOP clause so only the
		/// specified number of values will be returned.
		/// </summary>
		/// <param name="where">A WhereDelegate that recieves a ResponseDataColumns
		/// and returns a IQueryFilter which is the result of any comparisons
		/// between ResponseDataColumns and other values
		/// </param>
		/// <param name="database"></param>
		[Bam.Net.Exclude]
		public static ResponseData FirstOneWhere(WhereDelegate<ResponseDataColumns> where, Database database = null)
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
		/// <param name="where">A WhereDelegate that recieves a ResponseDataColumns
		/// and returns a IQueryFilter which is the result of any comparisons
		/// between ResponseDataColumns and other values
		/// </param>
		/// <param name="database"></param>
		[Bam.Net.Exclude]
		public static ResponseData FirstOneWhere(WhereDelegate<ResponseDataColumns> where, OrderBy<ResponseDataColumns> orderBy, Database database = null)
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
		/// <param name="where">A WhereDelegate that recieves a ResponseDataColumns
		/// and returns a IQueryFilter which is the result of any comparisons
		/// between ResponseDataColumns and other values
		/// </param>
		/// <param name="database"></param>
		[Bam.Net.Exclude]
		public static ResponseData FirstOneWhere(QueryFilter where, OrderBy<ResponseDataColumns> orderBy = null, Database database = null)
		{
			WhereDelegate<ResponseDataColumns> whereDelegate = (c) => where;
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
		/// <param name="where">A WhereDelegate that recieves a ResponseDataColumns
		/// and returns a IQueryFilter which is the result of any comparisons
		/// between ResponseDataColumns and other values
		/// </param>
		/// <param name="database"></param>
		[Bam.Net.Exclude]
		public static ResponseDataCollection Top(int count, WhereDelegate<ResponseDataColumns> where, Database database = null)
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
		/// <param name="where">A WhereDelegate that recieves a ResponseDataColumns
		/// and returns a IQueryFilter which is the result of any comparisons
		/// between ResponseDataColumns and other values
		/// </param>
		/// <param name="orderBy">
		/// Specifies what column and direction to order the results by
		/// </param>
		/// <param name="database">
		/// Which database to query or null to use the default
		/// </param>
		[Bam.Net.Exclude]
		public static ResponseDataCollection Top(int count, WhereDelegate<ResponseDataColumns> where, OrderBy<ResponseDataColumns> orderBy, Database database = null)
		{
			ResponseDataColumns c = new ResponseDataColumns();
			IQueryFilter filter = where(c);

			Database db = database ?? Db.For<ResponseData>();
			QuerySet query = GetQuerySet(db);
			query.Top<ResponseData>(count);
			query.Where(filter);

			if(orderBy != null)
			{
				query.OrderBy<ResponseDataColumns>(orderBy);
			}

			query.Execute(db);
			var results = query.Results.As<ResponseDataCollection>(0);
			results.Database = db;
			return results;
		}

		[Bam.Net.Exclude]
		public static ResponseDataCollection Top(int count, QueryFilter where, Database database)
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
		public static ResponseDataCollection Top(int count, QueryFilter where, OrderBy<ResponseDataColumns> orderBy = null, Database database = null)
		{
			Database db = database ?? Db.For<ResponseData>();
			QuerySet query = GetQuerySet(db);
			query.Top<ResponseData>(count);
			query.Where(where);

			if(orderBy != null)
			{
				query.OrderBy<ResponseDataColumns>(orderBy);
			}

			query.Execute(db);
			var results = query.Results.As<ResponseDataCollection>(0);
			results.Database = db;
			return results;
		}

		[Bam.Net.Exclude]
		public static ResponseDataCollection Top(int count, QueryFilter where, string orderBy = null, SortOrder sortOrder = SortOrder.Ascending, Database database = null)
		{
			Database db = database ?? Db.For<ResponseData>();
			QuerySet query = GetQuerySet(db);
			query.Top<ResponseData>(count);
			query.Where(where);

			if(orderBy != null)
			{
				query.OrderBy(orderBy, sortOrder);
			}

			query.Execute(db);
			var results = query.Results.As<ResponseDataCollection>(0);
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
		public static ResponseDataCollection Top(int count, QiQuery where, Database database = null)
		{
			Database db = database ?? Db.For<ResponseData>();
			QuerySet query = GetQuerySet(db);
			query.Top<ResponseData>(count);
			query.Where(where);
			query.Execute(db);
			var results = query.Results.As<ResponseDataCollection>(0);
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
			Database db = database ?? Db.For<ResponseData>();
            QuerySet query = GetQuerySet(db);
            query.Count<ResponseData>();
            query.Execute(db);
            return (long)query.Results[0].DataRow[0];
        }

		/// <summary>
		/// Execute a query and return the number of results
		/// </summary>
		/// <param name="where">A WhereDelegate that recieves a ResponseDataColumns
		/// and returns a IQueryFilter which is the result of any comparisons
		/// between ResponseDataColumns and other values
		/// </param>
		/// <param name="database">
		/// Which database to query or null to use the default
		/// </param>
		[Bam.Net.Exclude]
		public static long Count(WhereDelegate<ResponseDataColumns> where, Database database = null)
		{
			ResponseDataColumns c = new ResponseDataColumns();
			IQueryFilter filter = where(c) ;

			Database db = database ?? Db.For<ResponseData>();
			QuerySet query = GetQuerySet(db);
			query.Count<ResponseData>();
			query.Where(filter);
			query.Execute(db);
			return query.Results.As<CountResult>(0).Value;
		}

		public static long Count(QiQuery where, Database database = null)
		{
		    Database db = database ?? Db.For<ResponseData>();
			QuerySet query = GetQuerySet(db);
			query.Count<ResponseData>();
			query.Where(where);
			query.Execute(db);
			return query.Results.As<CountResult>(0).Value;
		}

		private static ResponseData CreateFromFilter(IQueryFilter filter, Database database = null)
		{
			Database db = database ?? Db.For<ResponseData>();
			var dao = new ResponseData();
			filter.Parameters.Each(p=>
			{
				dao.Property(p.ColumnName, p.Value);
			});
			dao.Save(db);
			return dao;
		}

		private static ResponseData OneOrThrow(ResponseDataCollection c)
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
