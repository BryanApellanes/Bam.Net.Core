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

namespace Bam.Net.CoreServices.Auth.Data.Dao
{
	// schema = Auth
	// connection Name = Auth
	[Serializable]
	[Bam.Net.Data.Table("AuthProviderSettings", "Auth")]
	public partial class AuthProviderSettings: Bam.Net.Data.Dao
	{
		public AuthProviderSettings():base()
		{
			this.SetKeyColumnName();
			this.SetChildren();
		}

		public AuthProviderSettings(DataRow data)
			: base(data)
		{
			this.SetKeyColumnName();
			this.SetChildren();
		}

		public AuthProviderSettings(Database db)
			: base(db)
		{
			this.SetKeyColumnName();
			this.SetChildren();
		}

		public AuthProviderSettings(Database db, DataRow data)
			: base(db, data)
		{
			this.SetKeyColumnName();
			this.SetChildren();
		}

		[Bam.Net.Exclude]
		public static implicit operator AuthProviderSettings(DataRow data)
		{
			return new AuthProviderSettings(data);
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

	// property:ApplicationName, columnName: ApplicationName	
	[Bam.Net.Data.Column(Name="ApplicationName", DbDataType="VarChar", MaxLength="4000", AllowNull=true)]
	public string ApplicationName
	{
		get
		{
			return GetStringValue("ApplicationName");
		}
		set
		{
			SetValue("ApplicationName", value);
		}
	}

	// property:ApplicationIdentifier, columnName: ApplicationIdentifier	
	[Bam.Net.Data.Column(Name="ApplicationIdentifier", DbDataType="VarChar", MaxLength="4000", AllowNull=true)]
	public string ApplicationIdentifier
	{
		get
		{
			return GetStringValue("ApplicationIdentifier");
		}
		set
		{
			SetValue("ApplicationIdentifier", value);
		}
	}

	// property:ProviderName, columnName: ProviderName	
	[Bam.Net.Data.Column(Name="ProviderName", DbDataType="VarChar", MaxLength="4000", AllowNull=true)]
	public string ProviderName
	{
		get
		{
			return GetStringValue("ProviderName");
		}
		set
		{
			SetValue("ProviderName", value);
		}
	}

	// property:State, columnName: State	
	[Bam.Net.Data.Column(Name="State", DbDataType="VarChar", MaxLength="4000", AllowNull=true)]
	public string State
	{
		get
		{
			return GetStringValue("State");
		}
		set
		{
			SetValue("State", value);
		}
	}

	// property:Code, columnName: Code	
	[Bam.Net.Data.Column(Name="Code", DbDataType="VarChar", MaxLength="4000", AllowNull=true)]
	public string Code
	{
		get
		{
			return GetStringValue("Code");
		}
		set
		{
			SetValue("Code", value);
		}
	}

	// property:ClientId, columnName: ClientId	
	[Bam.Net.Data.Column(Name="ClientId", DbDataType="VarChar", MaxLength="4000", AllowNull=true)]
	public string ClientId
	{
		get
		{
			return GetStringValue("ClientId");
		}
		set
		{
			SetValue("ClientId", value);
		}
	}

	// property:ClientSecret, columnName: ClientSecret	
	[Bam.Net.Data.Column(Name="ClientSecret", DbDataType="VarChar", MaxLength="4000", AllowNull=true)]
	public string ClientSecret
	{
		get
		{
			return GetStringValue("ClientSecret");
		}
		set
		{
			SetValue("ClientSecret", value);
		}
	}

	// property:AuthorizationUrl, columnName: AuthorizationUrl	
	[Bam.Net.Data.Column(Name="AuthorizationUrl", DbDataType="VarChar", MaxLength="4000", AllowNull=true)]
	public string AuthorizationUrl
	{
		get
		{
			return GetStringValue("AuthorizationUrl");
		}
		set
		{
			SetValue("AuthorizationUrl", value);
		}
	}

	// property:AuthorizationCallbackEndpoint, columnName: AuthorizationCallbackEndpoint	
	[Bam.Net.Data.Column(Name="AuthorizationCallbackEndpoint", DbDataType="VarChar", MaxLength="4000", AllowNull=true)]
	public string AuthorizationCallbackEndpoint
	{
		get
		{
			return GetStringValue("AuthorizationCallbackEndpoint");
		}
		set
		{
			SetValue("AuthorizationCallbackEndpoint", value);
		}
	}

	// property:AuthorizationEndpointFormat, columnName: AuthorizationEndpointFormat	
	[Bam.Net.Data.Column(Name="AuthorizationEndpointFormat", DbDataType="VarChar", MaxLength="4000", AllowNull=true)]
	public string AuthorizationEndpointFormat
	{
		get
		{
			return GetStringValue("AuthorizationEndpointFormat");
		}
		set
		{
			SetValue("AuthorizationEndpointFormat", value);
		}
	}

	// property:AuthorizationCallbackEndpointFormat, columnName: AuthorizationCallbackEndpointFormat	
	[Bam.Net.Data.Column(Name="AuthorizationCallbackEndpointFormat", DbDataType="VarChar", MaxLength="4000", AllowNull=true)]
	public string AuthorizationCallbackEndpointFormat
	{
		get
		{
			return GetStringValue("AuthorizationCallbackEndpointFormat");
		}
		set
		{
			SetValue("AuthorizationCallbackEndpointFormat", value);
		}
	}

	// property:Version, columnName: Version	
	[Bam.Net.Data.Column(Name="Version", DbDataType="VarChar", MaxLength="4000", AllowNull=true)]
	public string Version
	{
		get
		{
			return GetStringValue("Version");
		}
		set
		{
			SetValue("Version", value);
		}
	}

	// property:AccessToken, columnName: AccessToken	
	[Bam.Net.Data.Column(Name="AccessToken", DbDataType="VarChar", MaxLength="4000", AllowNull=true)]
	public string AccessToken
	{
		get
		{
			return GetStringValue("AccessToken");
		}
		set
		{
			SetValue("AccessToken", value);
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
				var colFilter = new AuthProviderSettingsColumns();
				return (colFilter.KeyColumn == IdValue);
			}
		}

		/// <summary>
        /// Return every record in the AuthProviderSettings table.
        /// </summary>
		/// <param name="database">
		/// The database to load from or null
		/// </param>
		public static AuthProviderSettingsCollection LoadAll(Database database = null)
		{
			Database db = database ?? Db.For<AuthProviderSettings>();
            SqlStringBuilder sql = db.GetSqlStringBuilder();
            sql.Select<AuthProviderSettings>();
            var results = new AuthProviderSettingsCollection(db, sql.GetDataTable(db))
            {
                Database = db
            };
            return results;
        }

        /// <summary>
        /// Process all records in batches of the specified size
        /// </summary>
        [Bam.Net.Exclude]
        public static async Task BatchAll(int batchSize, Action<IEnumerable<AuthProviderSettings>> batchProcessor, Database database = null)
		{
			await System.Threading.Tasks.Task.Run(async ()=>
			{
				AuthProviderSettingsColumns columns = new AuthProviderSettingsColumns();
				var orderBy = Bam.Net.Data.Order.By<AuthProviderSettingsColumns>(c => c.KeyColumn, Bam.Net.Data.SortOrder.Ascending);
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
		public static async Task BatchQuery(int batchSize, QueryFilter filter, Action<IEnumerable<AuthProviderSettings>> batchProcessor, Database database = null)
		{
			await BatchQuery(batchSize, (c) => filter, batchProcessor, database);
		}

		/// <summary>
		/// Process results of a query in batches of the specified size
		/// </summary>
		[Bam.Net.Exclude]
		public static async Task BatchQuery(int batchSize, WhereDelegate<AuthProviderSettingsColumns> where, Action<IEnumerable<AuthProviderSettings>> batchProcessor, Database database = null)
		{
			await System.Threading.Tasks.Task.Run(async ()=>
			{
				AuthProviderSettingsColumns columns = new AuthProviderSettingsColumns();
				var orderBy = Bam.Net.Data.Order.By<AuthProviderSettingsColumns>(c => c.KeyColumn, Bam.Net.Data.SortOrder.Ascending);
				var results = Top(batchSize, where, orderBy, database);
				while(results.Count > 0)
				{
					await System.Threading.Tasks.Task.Run(()=>
					{
						batchProcessor(results);
					});
					long topId = results.Select(d => d.Property<long>(columns.KeyColumn.ToString())).ToArray().Largest();
					results = Top(batchSize, (AuthProviderSettingsColumns)where(columns) && columns.KeyColumn > topId, orderBy, database);
				}
			});
		}

		/// <summary>
		/// Process results of a query in batches of the specified size
		/// </summary>
		[Bam.Net.Exclude]
		public static async Task BatchQuery<ColType>(int batchSize, QueryFilter filter, Action<IEnumerable<AuthProviderSettings>> batchProcessor, Bam.Net.Data.OrderBy<AuthProviderSettingsColumns> orderBy, Database database = null)
		{
			await BatchQuery<ColType>(batchSize, (c) => filter, batchProcessor, orderBy, database);
		}

		/// <summary>
		/// Process results of a query in batches of the specified size
		/// </summary>
		[Bam.Net.Exclude]
		public static async Task BatchQuery<ColType>(int batchSize, WhereDelegate<AuthProviderSettingsColumns> where, Action<IEnumerable<AuthProviderSettings>> batchProcessor, Bam.Net.Data.OrderBy<AuthProviderSettingsColumns> orderBy, Database database = null)
		{
			await System.Threading.Tasks.Task.Run(async ()=>
			{
				AuthProviderSettingsColumns columns = new AuthProviderSettingsColumns();
				var results = Top(batchSize, where, orderBy, database);
				while(results.Count > 0)
				{
					await System.Threading.Tasks.Task.Run(()=>
					{
						batchProcessor(results);
					});
					ColType top = results.Select(d => d.Property<ColType>(orderBy.Column.ToString())).ToArray().Largest();
					results = Top(batchSize, (AuthProviderSettingsColumns)where(columns) && orderBy.Column > top, orderBy, database);
				}
			});
		}

		public static AuthProviderSettings GetById(uint? id, Database database = null)
		{
			Args.ThrowIfNull(id, "id");
			Args.ThrowIf(!id.HasValue, "specified AuthProviderSettings.Id was null");
			return GetById(id.Value, database);
		}

		public static AuthProviderSettings GetById(uint id, Database database = null)
		{
			return GetById((ulong)id, database);
		}

		public static AuthProviderSettings GetById(int? id, Database database = null)
		{
			Args.ThrowIfNull(id, "id");
			Args.ThrowIf(!id.HasValue, "specified AuthProviderSettings.Id was null");
			return GetById(id.Value, database);
		}                                    
                                    
		public static AuthProviderSettings GetById(int id, Database database = null)
		{
			return GetById((long)id, database);
		}

		public static AuthProviderSettings GetById(long? id, Database database = null)
		{
			Args.ThrowIfNull(id, "id");
			Args.ThrowIf(!id.HasValue, "specified AuthProviderSettings.Id was null");
			return GetById(id.Value, database);
		}
                                    
		public static AuthProviderSettings GetById(long id, Database database = null)
		{
			return OneWhere(c => c.KeyColumn == id, database);
		}

		public static AuthProviderSettings GetById(ulong? id, Database database = null)
		{
			Args.ThrowIfNull(id, "id");
			Args.ThrowIf(!id.HasValue, "specified AuthProviderSettings.Id was null");
			return GetById(id.Value, database);
		}
                                    
		public static AuthProviderSettings GetById(ulong id, Database database = null)
		{
			return OneWhere(c => c.KeyColumn == id, database);
		}

		public static AuthProviderSettings GetByUuid(string uuid, Database database = null)
		{
			return OneWhere(c => Bam.Net.Data.Query.Where("Uuid") == uuid, database);
		}

		public static AuthProviderSettings GetByCuid(string cuid, Database database = null)
		{
			return OneWhere(c => Bam.Net.Data.Query.Where("Cuid") == cuid, database);
		}

		[Bam.Net.Exclude]
		public static AuthProviderSettingsCollection Query(QueryFilter filter, Database database = null)
		{
			return Where(filter, database);
		}

		[Bam.Net.Exclude]
		public static AuthProviderSettingsCollection Where(QueryFilter filter, Database database = null)
		{
			WhereDelegate<AuthProviderSettingsColumns> whereDelegate = (c) => filter;
			return Where(whereDelegate, database);
		}

		/// <summary>
		/// Execute a query and return the results.
		/// </summary>
		/// <param name="where">A Func delegate that recieves a AuthProviderSettingsColumns
		/// and returns a QueryFilter which is the result of any comparisons
		/// between AuthProviderSettingsColumns and other values
		/// </param>
		/// <param name="db"></param>
		[Bam.Net.Exclude]
		public static AuthProviderSettingsCollection Where(Func<AuthProviderSettingsColumns, QueryFilter<AuthProviderSettingsColumns>> where, OrderBy<AuthProviderSettingsColumns> orderBy = null, Database database = null)
		{
			database = database ?? Db.For<AuthProviderSettings>();
			return new AuthProviderSettingsCollection(database.GetQuery<AuthProviderSettingsColumns, AuthProviderSettings>(where, orderBy), true);
		}

		/// <summary>
		/// Execute a query and return the results.
		/// </summary>
		/// <param name="where">A WhereDelegate that recieves a AuthProviderSettingsColumns
		/// and returns a IQueryFilter which is the result of any comparisons
		/// between AuthProviderSettingsColumns and other values
		/// </param>
		/// <param name="db"></param>
		[Bam.Net.Exclude]
		public static AuthProviderSettingsCollection Where(WhereDelegate<AuthProviderSettingsColumns> where, Database database = null)
		{
			database = database ?? Db.For<AuthProviderSettings>();
			var results = new AuthProviderSettingsCollection(database, database.GetQuery<AuthProviderSettingsColumns, AuthProviderSettings>(where), true);
			return results;
		}

		/// <summary>
		/// Execute a query and return the results.
		/// </summary>
		/// <param name="where">A WhereDelegate that recieves a AuthProviderSettingsColumns
		/// and returns a IQueryFilter which is the result of any comparisons
		/// between AuthProviderSettingsColumns and other values
		/// </param>
		/// <param name="orderBy">
		/// Specifies what column and direction to order the results by
		/// </param>
		/// <param name="database"></param>
		[Bam.Net.Exclude]
		public static AuthProviderSettingsCollection Where(WhereDelegate<AuthProviderSettingsColumns> where, OrderBy<AuthProviderSettingsColumns> orderBy = null, Database database = null)
		{
			database = database ?? Db.For<AuthProviderSettings>();
			var results = new AuthProviderSettingsCollection(database, database.GetQuery<AuthProviderSettingsColumns, AuthProviderSettings>(where, orderBy), true);
			return results;
		}

		/// <summary>
		/// This method is intended to respond to client side Qi queries.
		/// Use of this method from .Net should be avoided in favor of
		/// one of the methods that take a delegate of type
		/// WhereDelegate`AuthProviderSettingsColumns`.
		/// </summary>
		/// <param name="where"></param>
		/// <param name="database"></param>
		public static AuthProviderSettingsCollection Where(QiQuery where, Database database = null)
		{
			var results = new AuthProviderSettingsCollection(database, Select<AuthProviderSettingsColumns>.From<AuthProviderSettings>().Where(where, database));
			return results;
		}

		/// <summary>
		/// Get one entry matching the specified filter.  If none exists
		/// one will be created; success will depend on the nullability
		/// of the specified columns.
		/// </summary>
		[Bam.Net.Exclude]
		public static AuthProviderSettings GetOneWhere(QueryFilter where, Database database = null)
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
		public static AuthProviderSettings OneWhere(QueryFilter where, Database database = null)
		{
			WhereDelegate<AuthProviderSettingsColumns> whereDelegate = (c) => where;
			var result = Top(1, whereDelegate, database);
			return OneOrThrow(result);
		}

		/// <summary>
		/// Set one entry matching the specified filter.  If none exists
		/// one will be created; success will depend on the nullability
		/// of the specified columns.
		/// </summary>
		[Bam.Net.Exclude]
		public static void SetOneWhere(WhereDelegate<AuthProviderSettingsColumns> where, Database database = null)
		{
			SetOneWhere(where, out AuthProviderSettings ignore, database);
		}

		/// <summary>
		/// Set one entry matching the specified filter.  If none exists
		/// one will be created; success will depend on the nullability
		/// of the specified columns.
		/// </summary>
		[Bam.Net.Exclude]
		public static void SetOneWhere(WhereDelegate<AuthProviderSettingsColumns> where, out AuthProviderSettings result, Database database = null)
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
		public static AuthProviderSettings GetOneWhere(WhereDelegate<AuthProviderSettingsColumns> where, Database database = null)
		{
			var result = OneWhere(where, database);
			if(result == null)
			{
				AuthProviderSettingsColumns c = new AuthProviderSettingsColumns();
				IQueryFilter filter = where(c);
				result = CreateFromFilter(filter, database);
			}

			return result;
		}

		/// <summary>
		/// Execute a query that should return only one result.  If more
		/// than one result is returned a MultipleEntriesFoundException will
		/// be thrown.  This method is most commonly used to retrieve a
		/// single AuthProviderSettings instance by its Id/Key value
		/// </summary>
		/// <param name="where">A WhereDelegate that recieves a AuthProviderSettingsColumns
		/// and returns a IQueryFilter which is the result of any comparisons
		/// between AuthProviderSettingsColumns and other values
		/// </param>
		/// <param name="database"></param>
		[Bam.Net.Exclude]
		public static AuthProviderSettings OneWhere(WhereDelegate<AuthProviderSettingsColumns> where, Database database = null)
		{
			var result = Top(1, where, database);
			return OneOrThrow(result);
		}

		/// <summary>
		/// This method is intended to respond to client side Qi queries.
		/// Use of this method from .Net should be avoided in favor of
		/// one of the methods that take a delegate of type
		/// WhereDelegate`AuthProviderSettingsColumns`.
		/// </summary>
		/// <param name="where"></param>
		/// <param name="database"></param>
		public static AuthProviderSettings OneWhere(QiQuery where, Database database = null)
		{
			var results = Top(1, where, database);
			return OneOrThrow(results);
		}

		/// <summary>
		/// Execute a query and return the first result.  This method will issue a sql TOP clause so only the
		/// specified number of values will be returned.
		/// </summary>
		/// <param name="where">A WhereDelegate that recieves a AuthProviderSettingsColumns
		/// and returns a IQueryFilter which is the result of any comparisons
		/// between AuthProviderSettingsColumns and other values
		/// </param>
		/// <param name="database"></param>
		[Bam.Net.Exclude]
		public static AuthProviderSettings FirstOneWhere(WhereDelegate<AuthProviderSettingsColumns> where, Database database = null)
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
		/// <param name="where">A WhereDelegate that recieves a AuthProviderSettingsColumns
		/// and returns a IQueryFilter which is the result of any comparisons
		/// between AuthProviderSettingsColumns and other values
		/// </param>
		/// <param name="database"></param>
		[Bam.Net.Exclude]
		public static AuthProviderSettings FirstOneWhere(WhereDelegate<AuthProviderSettingsColumns> where, OrderBy<AuthProviderSettingsColumns> orderBy, Database database = null)
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
		/// <param name="where">A WhereDelegate that recieves a AuthProviderSettingsColumns
		/// and returns a IQueryFilter which is the result of any comparisons
		/// between AuthProviderSettingsColumns and other values
		/// </param>
		/// <param name="database"></param>
		[Bam.Net.Exclude]
		public static AuthProviderSettings FirstOneWhere(QueryFilter where, OrderBy<AuthProviderSettingsColumns> orderBy = null, Database database = null)
		{
			WhereDelegate<AuthProviderSettingsColumns> whereDelegate = (c) => where;
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
		/// <param name="where">A WhereDelegate that recieves a AuthProviderSettingsColumns
		/// and returns a IQueryFilter which is the result of any comparisons
		/// between AuthProviderSettingsColumns and other values
		/// </param>
		/// <param name="database"></param>
		[Bam.Net.Exclude]
		public static AuthProviderSettingsCollection Top(int count, WhereDelegate<AuthProviderSettingsColumns> where, Database database = null)
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
		/// <param name="where">A WhereDelegate that recieves a AuthProviderSettingsColumns
		/// and returns a IQueryFilter which is the result of any comparisons
		/// between AuthProviderSettingsColumns and other values
		/// </param>
		/// <param name="orderBy">
		/// Specifies what column and direction to order the results by
		/// </param>
		/// <param name="database">
		/// Which database to query or null to use the default
		/// </param>
		[Bam.Net.Exclude]
		public static AuthProviderSettingsCollection Top(int count, WhereDelegate<AuthProviderSettingsColumns> where, OrderBy<AuthProviderSettingsColumns> orderBy, Database database = null)
		{
			AuthProviderSettingsColumns c = new AuthProviderSettingsColumns();
			IQueryFilter filter = where(c);

			Database db = database ?? Db.For<AuthProviderSettings>();
			QuerySet query = GetQuerySet(db);
			query.Top<AuthProviderSettings>(count);
			query.Where(filter);

			if(orderBy != null)
			{
				query.OrderBy<AuthProviderSettingsColumns>(orderBy);
			}

			query.Execute(db);
			var results = query.Results.As<AuthProviderSettingsCollection>(0);
			results.Database = db;
			return results;
		}

		[Bam.Net.Exclude]
		public static AuthProviderSettingsCollection Top(int count, QueryFilter where, Database database)
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
		public static AuthProviderSettingsCollection Top(int count, QueryFilter where, OrderBy<AuthProviderSettingsColumns> orderBy = null, Database database = null)
		{
			Database db = database ?? Db.For<AuthProviderSettings>();
			QuerySet query = GetQuerySet(db);
			query.Top<AuthProviderSettings>(count);
			query.Where(where);

			if(orderBy != null)
			{
				query.OrderBy<AuthProviderSettingsColumns>(orderBy);
			}

			query.Execute(db);
			var results = query.Results.As<AuthProviderSettingsCollection>(0);
			results.Database = db;
			return results;
		}

		[Bam.Net.Exclude]
		public static AuthProviderSettingsCollection Top(int count, QueryFilter where, string orderBy = null, SortOrder sortOrder = SortOrder.Ascending, Database database = null)
		{
			Database db = database ?? Db.For<AuthProviderSettings>();
			QuerySet query = GetQuerySet(db);
			query.Top<AuthProviderSettings>(count);
			query.Where(where);

			if(orderBy != null)
			{
				query.OrderBy(orderBy, sortOrder);
			}

			query.Execute(db);
			var results = query.Results.As<AuthProviderSettingsCollection>(0);
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
		public static AuthProviderSettingsCollection Top(int count, QiQuery where, Database database = null)
		{
			Database db = database ?? Db.For<AuthProviderSettings>();
			QuerySet query = GetQuerySet(db);
			query.Top<AuthProviderSettings>(count);
			query.Where(where);
			query.Execute(db);
			var results = query.Results.As<AuthProviderSettingsCollection>(0);
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
			Database db = database ?? Db.For<AuthProviderSettings>();
            QuerySet query = GetQuerySet(db);
            query.Count<AuthProviderSettings>();
            query.Execute(db);
            return (long)query.Results[0].DataRow[0];
        }

		/// <summary>
		/// Execute a query and return the number of results
		/// </summary>
		/// <param name="where">A WhereDelegate that recieves a AuthProviderSettingsColumns
		/// and returns a IQueryFilter which is the result of any comparisons
		/// between AuthProviderSettingsColumns and other values
		/// </param>
		/// <param name="database">
		/// Which database to query or null to use the default
		/// </param>
		[Bam.Net.Exclude]
		public static long Count(WhereDelegate<AuthProviderSettingsColumns> where, Database database = null)
		{
			AuthProviderSettingsColumns c = new AuthProviderSettingsColumns();
			IQueryFilter filter = where(c) ;

			Database db = database ?? Db.For<AuthProviderSettings>();
			QuerySet query = GetQuerySet(db);
			query.Count<AuthProviderSettings>();
			query.Where(filter);
			query.Execute(db);
			return query.Results.As<CountResult>(0).Value;
		}

		public static long Count(QiQuery where, Database database = null)
		{
		    Database db = database ?? Db.For<AuthProviderSettings>();
			QuerySet query = GetQuerySet(db);
			query.Count<AuthProviderSettings>();
			query.Where(where);
			query.Execute(db);
			return query.Results.As<CountResult>(0).Value;
		}

		private static AuthProviderSettings CreateFromFilter(IQueryFilter filter, Database database = null)
		{
			Database db = database ?? Db.For<AuthProviderSettings>();
			var dao = new AuthProviderSettings();
			filter.Parameters.Each(p=>
			{
				dao.Property(p.ColumnName, p.Value);
			});
			dao.Save(db);
			return dao;
		}

		private static AuthProviderSettings OneOrThrow(AuthProviderSettingsCollection c)
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
