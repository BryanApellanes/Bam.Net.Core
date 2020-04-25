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

namespace Bam.Net.CoreServices.Auth.Data.Dao
{
	// schema = Auth
    public static class AuthContext
    {
		public static string ConnectionName
		{
			get
			{
				return "Auth";
			}
		}

		public static Database Db
		{
			get
			{
				return Bam.Net.Data.Db.For(ConnectionName);
			}
		}


	public class AccessTokenQueryContext
	{
			public AccessTokenCollection Where(WhereDelegate<AccessTokenColumns> where, Database db = null)
			{
				return Bam.Net.CoreServices.Auth.Data.Dao.AccessToken.Where(where, db);
			}
		   
			public AccessTokenCollection Where(WhereDelegate<AccessTokenColumns> where, OrderBy<AccessTokenColumns> orderBy = null, Database db = null)
			{
				return Bam.Net.CoreServices.Auth.Data.Dao.AccessToken.Where(where, orderBy, db);
			}

			public AccessToken OneWhere(WhereDelegate<AccessTokenColumns> where, Database db = null)
			{
				return Bam.Net.CoreServices.Auth.Data.Dao.AccessToken.OneWhere(where, db);
			}

			public static AccessToken GetOneWhere(WhereDelegate<AccessTokenColumns> where, Database db = null)
			{
				return Bam.Net.CoreServices.Auth.Data.Dao.AccessToken.GetOneWhere(where, db);
			}
		
			public AccessToken FirstOneWhere(WhereDelegate<AccessTokenColumns> where, Database db = null)
			{
				return Bam.Net.CoreServices.Auth.Data.Dao.AccessToken.FirstOneWhere(where, db);
			}

			public AccessTokenCollection Top(int count, WhereDelegate<AccessTokenColumns> where, Database db = null)
			{
				return Bam.Net.CoreServices.Auth.Data.Dao.AccessToken.Top(count, where, db);
			}

			public AccessTokenCollection Top(int count, WhereDelegate<AccessTokenColumns> where, OrderBy<AccessTokenColumns> orderBy, Database db = null)
			{
				return Bam.Net.CoreServices.Auth.Data.Dao.AccessToken.Top(count, where, orderBy, db);
			}

			public long Count(WhereDelegate<AccessTokenColumns> where, Database db = null)
			{
				return Bam.Net.CoreServices.Auth.Data.Dao.AccessToken.Count(where, db);
			}
	}

	static AccessTokenQueryContext _accessTokens;
	static object _accessTokensLock = new object();
	public static AccessTokenQueryContext AccessTokens
	{
		get
		{
			return _accessTokensLock.DoubleCheckLock<AccessTokenQueryContext>(ref _accessTokens, () => new AccessTokenQueryContext());
		}
	}
	public class AuthProviderSettingsQueryContext
	{
			public AuthProviderSettingsCollection Where(WhereDelegate<AuthProviderSettingsColumns> where, Database db = null)
			{
				return Bam.Net.CoreServices.Auth.Data.Dao.AuthProviderSettings.Where(where, db);
			}
		   
			public AuthProviderSettingsCollection Where(WhereDelegate<AuthProviderSettingsColumns> where, OrderBy<AuthProviderSettingsColumns> orderBy = null, Database db = null)
			{
				return Bam.Net.CoreServices.Auth.Data.Dao.AuthProviderSettings.Where(where, orderBy, db);
			}

			public AuthProviderSettings OneWhere(WhereDelegate<AuthProviderSettingsColumns> where, Database db = null)
			{
				return Bam.Net.CoreServices.Auth.Data.Dao.AuthProviderSettings.OneWhere(where, db);
			}

			public static AuthProviderSettings GetOneWhere(WhereDelegate<AuthProviderSettingsColumns> where, Database db = null)
			{
				return Bam.Net.CoreServices.Auth.Data.Dao.AuthProviderSettings.GetOneWhere(where, db);
			}
		
			public AuthProviderSettings FirstOneWhere(WhereDelegate<AuthProviderSettingsColumns> where, Database db = null)
			{
				return Bam.Net.CoreServices.Auth.Data.Dao.AuthProviderSettings.FirstOneWhere(where, db);
			}

			public AuthProviderSettingsCollection Top(int count, WhereDelegate<AuthProviderSettingsColumns> where, Database db = null)
			{
				return Bam.Net.CoreServices.Auth.Data.Dao.AuthProviderSettings.Top(count, where, db);
			}

			public AuthProviderSettingsCollection Top(int count, WhereDelegate<AuthProviderSettingsColumns> where, OrderBy<AuthProviderSettingsColumns> orderBy, Database db = null)
			{
				return Bam.Net.CoreServices.Auth.Data.Dao.AuthProviderSettings.Top(count, where, orderBy, db);
			}

			public long Count(WhereDelegate<AuthProviderSettingsColumns> where, Database db = null)
			{
				return Bam.Net.CoreServices.Auth.Data.Dao.AuthProviderSettings.Count(where, db);
			}
	}

	static AuthProviderSettingsQueryContext _authProviderSettingses;
	static object _authProviderSettingsesLock = new object();
	public static AuthProviderSettingsQueryContext AuthProviderSettingses
	{
		get
		{
			return _authProviderSettingsesLock.DoubleCheckLock<AuthProviderSettingsQueryContext>(ref _authProviderSettingses, () => new AuthProviderSettingsQueryContext());
		}
	}
    }
}																								
