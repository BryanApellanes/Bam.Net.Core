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
using Bam.Net.CoreServices.Auth.Data;

namespace Bam.Net.CoreServices.Auth.Data.Dao.Repository
{
	[Serializable]
	public class AuthRepository: DaoRepository
	{
		public AuthRepository()
		{
			SchemaName = "Auth";
			BaseNamespace = "Bam.Net.CoreServices.Auth.Data";			

			
			AddType<Bam.Net.CoreServices.Auth.Data.AccessToken>();
			
			
			AddType<Bam.Net.CoreServices.Auth.Data.AuthProviderSettings>();
			

			DaoAssembly = typeof(AuthRepository).Assembly;
		}

		object _addLock = new object();
        public override void AddType(Type type)
        {
            lock (_addLock)
            {
                base.AddType(type);
                DaoAssembly = typeof(AuthRepository).Assembly;
            }
        }

		
		/// <summary>
		/// Set one entry matching the specified filter.  If none exists 
		/// one is created; success depends on the nullability
		/// of the specified columns.
		/// </summary>
		public void SetOneAccessTokenWhere(WhereDelegate<AccessTokenColumns> where)
		{
			Bam.Net.CoreServices.Auth.Data.Dao.AccessToken.SetOneWhere(where, Database);
		}

		/// <summary>
		/// Set one entry matching the specified filter.  If none exists 
		/// one is created; success depends on the nullability
		/// of the specified columns.
		/// </summary>
		public void SetOneAccessTokenWhere(WhereDelegate<AccessTokenColumns> where, out Bam.Net.CoreServices.Auth.Data.AccessToken result)
		{
			Bam.Net.CoreServices.Auth.Data.Dao.AccessToken.SetOneWhere(where, out Bam.Net.CoreServices.Auth.Data.Dao.AccessToken daoResult, Database);
			result = daoResult.CopyAs<Bam.Net.CoreServices.Auth.Data.AccessToken>();
		}

		/// <summary>
		/// Get one entry matching the specified filter.  If none exists 
		/// one is created; success depends on the nullability
		/// of the specified columns.
		/// </summary>
		/// <param name="where"></param>
		public Bam.Net.CoreServices.Auth.Data.AccessToken GetOneAccessTokenWhere(WhereDelegate<AccessTokenColumns> where)
		{
			Type wrapperType = GetWrapperType<Bam.Net.CoreServices.Auth.Data.AccessToken>();
			return (Bam.Net.CoreServices.Auth.Data.AccessToken)Bam.Net.CoreServices.Auth.Data.Dao.AccessToken.GetOneWhere(where, Database)?.CopyAs(wrapperType, this);
		}

		/// <summary>
		/// Execute a query that should return only one result.  If no result is found null is returned.  If more
		/// than one result is returned a MultipleEntriesFoundException is thrown.  This method is most commonly used to retrieve a
		/// single AccessToken instance by its Id/Key value
		/// </summary>
		/// <param name="where">A WhereDelegate that receives a AccessTokenColumns 
		/// and returns a IQueryFilter which is the result of any comparisons
		/// between AccessTokenColumns and other values
		/// </param>
		public Bam.Net.CoreServices.Auth.Data.AccessToken OneAccessTokenWhere(WhereDelegate<AccessTokenColumns> where)
        {
            Type wrapperType = GetWrapperType<Bam.Net.CoreServices.Auth.Data.AccessToken>();
            return (Bam.Net.CoreServices.Auth.Data.AccessToken)Bam.Net.CoreServices.Auth.Data.Dao.AccessToken.OneWhere(where, Database)?.CopyAs(wrapperType, this);
        }

		/// <summary>
		/// Execute a query and return the results. 
		/// </summary>
		/// <param name="where">A WhereDelegate that receives a Bam.Net.CoreServices.Auth.Data.AccessTokenColumns 
		/// and returns a IQueryFilter which is the result of any comparisons
		/// between Bam.Net.CoreServices.Auth.Data.AccessTokenColumns and other values
		/// </param>
		public IEnumerable<Bam.Net.CoreServices.Auth.Data.AccessToken> AccessTokensWhere(WhereDelegate<AccessTokenColumns> where, OrderBy<AccessTokenColumns> orderBy = null)
        {
            return Wrap<Bam.Net.CoreServices.Auth.Data.AccessToken>(Bam.Net.CoreServices.Auth.Data.Dao.AccessToken.Where(where, orderBy, Database));
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
		/// <param name="where">A WhereDelegate that receives a AccessTokenColumns 
		/// and returns a IQueryFilter which is the result of any comparisons
		/// between AccessTokenColumns and other values
		/// </param>
		public IEnumerable<Bam.Net.CoreServices.Auth.Data.AccessToken> TopAccessTokensWhere(int count, WhereDelegate<AccessTokenColumns> where)
        {
            return Wrap<Bam.Net.CoreServices.Auth.Data.AccessToken>(Bam.Net.CoreServices.Auth.Data.Dao.AccessToken.Top(count, where, Database));
        }

        public IEnumerable<Bam.Net.CoreServices.Auth.Data.AccessToken> TopAccessTokensWhere(int count, WhereDelegate<AccessTokenColumns> where, OrderBy<AccessTokenColumns> orderBy)
        {
            return Wrap<Bam.Net.CoreServices.Auth.Data.AccessToken>(Bam.Net.CoreServices.Auth.Data.Dao.AccessToken.Top(count, where, orderBy, Database));
        }
                                
		/// <summary>
		/// Return the count of AccessTokens
		/// </summary>
		public long CountAccessTokens()
        {
            return Bam.Net.CoreServices.Auth.Data.Dao.AccessToken.Count(Database);
        }

		/// <summary>
		/// Execute a query and return the number of results
		/// </summary>
		/// <param name="where">A WhereDelegate that receives a AccessTokenColumns 
		/// and returns a IQueryFilter which is the result of any comparisons
		/// between AccessTokenColumns and other values
		/// </param>
        public long CountAccessTokensWhere(WhereDelegate<AccessTokenColumns> where)
        {
            return Bam.Net.CoreServices.Auth.Data.Dao.AccessToken.Count(where, Database);
        }
        
        public async Task BatchQueryAccessTokens(int batchSize, WhereDelegate<AccessTokenColumns> where, Action<IEnumerable<Bam.Net.CoreServices.Auth.Data.AccessToken>> batchProcessor)
        {
            await Bam.Net.CoreServices.Auth.Data.Dao.AccessToken.BatchQuery(batchSize, where, (batch) =>
            {
				batchProcessor(Wrap<Bam.Net.CoreServices.Auth.Data.AccessToken>(batch));
            }, Database);
        }
		
        public async Task BatchAllAccessTokens(int batchSize, Action<IEnumerable<Bam.Net.CoreServices.Auth.Data.AccessToken>> batchProcessor)
        {
            await Bam.Net.CoreServices.Auth.Data.Dao.AccessToken.BatchAll(batchSize, (batch) =>
            {
				batchProcessor(Wrap<Bam.Net.CoreServices.Auth.Data.AccessToken>(batch));
            }, Database);
        }

		
		/// <summary>
		/// Set one entry matching the specified filter.  If none exists 
		/// one is created; success depends on the nullability
		/// of the specified columns.
		/// </summary>
		public void SetOneAuthProviderSettingsWhere(WhereDelegate<AuthProviderSettingsColumns> where)
		{
			Bam.Net.CoreServices.Auth.Data.Dao.AuthProviderSettings.SetOneWhere(where, Database);
		}

		/// <summary>
		/// Set one entry matching the specified filter.  If none exists 
		/// one is created; success depends on the nullability
		/// of the specified columns.
		/// </summary>
		public void SetOneAuthProviderSettingsWhere(WhereDelegate<AuthProviderSettingsColumns> where, out Bam.Net.CoreServices.Auth.Data.AuthProviderSettings result)
		{
			Bam.Net.CoreServices.Auth.Data.Dao.AuthProviderSettings.SetOneWhere(where, out Bam.Net.CoreServices.Auth.Data.Dao.AuthProviderSettings daoResult, Database);
			result = daoResult.CopyAs<Bam.Net.CoreServices.Auth.Data.AuthProviderSettings>();
		}

		/// <summary>
		/// Get one entry matching the specified filter.  If none exists 
		/// one is created; success depends on the nullability
		/// of the specified columns.
		/// </summary>
		/// <param name="where"></param>
		public Bam.Net.CoreServices.Auth.Data.AuthProviderSettings GetOneAuthProviderSettingsWhere(WhereDelegate<AuthProviderSettingsColumns> where)
		{
			Type wrapperType = GetWrapperType<Bam.Net.CoreServices.Auth.Data.AuthProviderSettings>();
			return (Bam.Net.CoreServices.Auth.Data.AuthProviderSettings)Bam.Net.CoreServices.Auth.Data.Dao.AuthProviderSettings.GetOneWhere(where, Database)?.CopyAs(wrapperType, this);
		}

		/// <summary>
		/// Execute a query that should return only one result.  If no result is found null is returned.  If more
		/// than one result is returned a MultipleEntriesFoundException is thrown.  This method is most commonly used to retrieve a
		/// single AuthProviderSettings instance by its Id/Key value
		/// </summary>
		/// <param name="where">A WhereDelegate that receives a AuthProviderSettingsColumns 
		/// and returns a IQueryFilter which is the result of any comparisons
		/// between AuthProviderSettingsColumns and other values
		/// </param>
		public Bam.Net.CoreServices.Auth.Data.AuthProviderSettings OneAuthProviderSettingsWhere(WhereDelegate<AuthProviderSettingsColumns> where)
        {
            Type wrapperType = GetWrapperType<Bam.Net.CoreServices.Auth.Data.AuthProviderSettings>();
            return (Bam.Net.CoreServices.Auth.Data.AuthProviderSettings)Bam.Net.CoreServices.Auth.Data.Dao.AuthProviderSettings.OneWhere(where, Database)?.CopyAs(wrapperType, this);
        }

		/// <summary>
		/// Execute a query and return the results. 
		/// </summary>
		/// <param name="where">A WhereDelegate that receives a Bam.Net.CoreServices.Auth.Data.AuthProviderSettingsColumns 
		/// and returns a IQueryFilter which is the result of any comparisons
		/// between Bam.Net.CoreServices.Auth.Data.AuthProviderSettingsColumns and other values
		/// </param>
		public IEnumerable<Bam.Net.CoreServices.Auth.Data.AuthProviderSettings> AuthProviderSettingsesWhere(WhereDelegate<AuthProviderSettingsColumns> where, OrderBy<AuthProviderSettingsColumns> orderBy = null)
        {
            return Wrap<Bam.Net.CoreServices.Auth.Data.AuthProviderSettings>(Bam.Net.CoreServices.Auth.Data.Dao.AuthProviderSettings.Where(where, orderBy, Database));
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
		/// <param name="where">A WhereDelegate that receives a AuthProviderSettingsColumns 
		/// and returns a IQueryFilter which is the result of any comparisons
		/// between AuthProviderSettingsColumns and other values
		/// </param>
		public IEnumerable<Bam.Net.CoreServices.Auth.Data.AuthProviderSettings> TopAuthProviderSettingsesWhere(int count, WhereDelegate<AuthProviderSettingsColumns> where)
        {
            return Wrap<Bam.Net.CoreServices.Auth.Data.AuthProviderSettings>(Bam.Net.CoreServices.Auth.Data.Dao.AuthProviderSettings.Top(count, where, Database));
        }

        public IEnumerable<Bam.Net.CoreServices.Auth.Data.AuthProviderSettings> TopAuthProviderSettingsesWhere(int count, WhereDelegate<AuthProviderSettingsColumns> where, OrderBy<AuthProviderSettingsColumns> orderBy)
        {
            return Wrap<Bam.Net.CoreServices.Auth.Data.AuthProviderSettings>(Bam.Net.CoreServices.Auth.Data.Dao.AuthProviderSettings.Top(count, where, orderBy, Database));
        }
                                
		/// <summary>
		/// Return the count of AuthProviderSettingses
		/// </summary>
		public long CountAuthProviderSettingses()
        {
            return Bam.Net.CoreServices.Auth.Data.Dao.AuthProviderSettings.Count(Database);
        }

		/// <summary>
		/// Execute a query and return the number of results
		/// </summary>
		/// <param name="where">A WhereDelegate that receives a AuthProviderSettingsColumns 
		/// and returns a IQueryFilter which is the result of any comparisons
		/// between AuthProviderSettingsColumns and other values
		/// </param>
        public long CountAuthProviderSettingsesWhere(WhereDelegate<AuthProviderSettingsColumns> where)
        {
            return Bam.Net.CoreServices.Auth.Data.Dao.AuthProviderSettings.Count(where, Database);
        }
        
        public async Task BatchQueryAuthProviderSettingses(int batchSize, WhereDelegate<AuthProviderSettingsColumns> where, Action<IEnumerable<Bam.Net.CoreServices.Auth.Data.AuthProviderSettings>> batchProcessor)
        {
            await Bam.Net.CoreServices.Auth.Data.Dao.AuthProviderSettings.BatchQuery(batchSize, where, (batch) =>
            {
				batchProcessor(Wrap<Bam.Net.CoreServices.Auth.Data.AuthProviderSettings>(batch));
            }, Database);
        }
		
        public async Task BatchAllAuthProviderSettingses(int batchSize, Action<IEnumerable<Bam.Net.CoreServices.Auth.Data.AuthProviderSettings>> batchProcessor)
        {
            await Bam.Net.CoreServices.Auth.Data.Dao.AuthProviderSettings.BatchAll(batchSize, (batch) =>
            {
				batchProcessor(Wrap<Bam.Net.CoreServices.Auth.Data.AuthProviderSettings>(batch));
            }, Database);
        }


	}
}																								
