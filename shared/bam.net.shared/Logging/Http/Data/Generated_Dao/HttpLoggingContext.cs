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

namespace Bam.Net.Logging.Http.Data.Dao
{
	// schema = HttpLogging
    public static class HttpLoggingContext
    {
		public static string ConnectionName
		{
			get
			{
				return "HttpLogging";
			}
		}

		public static Database Db
		{
			get
			{
				return Bam.Net.Data.Db.For(ConnectionName);
			}
		}


	public class ContentNotFoundDataQueryContext
	{
			public ContentNotFoundDataCollection Where(WhereDelegate<ContentNotFoundDataColumns> where, Database db = null)
			{
				return Bam.Net.Logging.Http.Data.Dao.ContentNotFoundData.Where(where, db);
			}
		   
			public ContentNotFoundDataCollection Where(WhereDelegate<ContentNotFoundDataColumns> where, OrderBy<ContentNotFoundDataColumns> orderBy = null, Database db = null)
			{
				return Bam.Net.Logging.Http.Data.Dao.ContentNotFoundData.Where(where, orderBy, db);
			}

			public ContentNotFoundData OneWhere(WhereDelegate<ContentNotFoundDataColumns> where, Database db = null)
			{
				return Bam.Net.Logging.Http.Data.Dao.ContentNotFoundData.OneWhere(where, db);
			}

			public static ContentNotFoundData GetOneWhere(WhereDelegate<ContentNotFoundDataColumns> where, Database db = null)
			{
				return Bam.Net.Logging.Http.Data.Dao.ContentNotFoundData.GetOneWhere(where, db);
			}
		
			public ContentNotFoundData FirstOneWhere(WhereDelegate<ContentNotFoundDataColumns> where, Database db = null)
			{
				return Bam.Net.Logging.Http.Data.Dao.ContentNotFoundData.FirstOneWhere(where, db);
			}

			public ContentNotFoundDataCollection Top(int count, WhereDelegate<ContentNotFoundDataColumns> where, Database db = null)
			{
				return Bam.Net.Logging.Http.Data.Dao.ContentNotFoundData.Top(count, where, db);
			}

			public ContentNotFoundDataCollection Top(int count, WhereDelegate<ContentNotFoundDataColumns> where, OrderBy<ContentNotFoundDataColumns> orderBy, Database db = null)
			{
				return Bam.Net.Logging.Http.Data.Dao.ContentNotFoundData.Top(count, where, orderBy, db);
			}

			public long Count(WhereDelegate<ContentNotFoundDataColumns> where, Database db = null)
			{
				return Bam.Net.Logging.Http.Data.Dao.ContentNotFoundData.Count(where, db);
			}
	}

	static ContentNotFoundDataQueryContext _contentNotFoundDatas;
	static object _contentNotFoundDatasLock = new object();
	public static ContentNotFoundDataQueryContext ContentNotFoundDatas
	{
		get
		{
			return _contentNotFoundDatasLock.DoubleCheckLock<ContentNotFoundDataQueryContext>(ref _contentNotFoundDatas, () => new ContentNotFoundDataQueryContext());
		}
	}
	public class ResponseDataQueryContext
	{
			public ResponseDataCollection Where(WhereDelegate<ResponseDataColumns> where, Database db = null)
			{
				return Bam.Net.Logging.Http.Data.Dao.ResponseData.Where(where, db);
			}
		   
			public ResponseDataCollection Where(WhereDelegate<ResponseDataColumns> where, OrderBy<ResponseDataColumns> orderBy = null, Database db = null)
			{
				return Bam.Net.Logging.Http.Data.Dao.ResponseData.Where(where, orderBy, db);
			}

			public ResponseData OneWhere(WhereDelegate<ResponseDataColumns> where, Database db = null)
			{
				return Bam.Net.Logging.Http.Data.Dao.ResponseData.OneWhere(where, db);
			}

			public static ResponseData GetOneWhere(WhereDelegate<ResponseDataColumns> where, Database db = null)
			{
				return Bam.Net.Logging.Http.Data.Dao.ResponseData.GetOneWhere(where, db);
			}
		
			public ResponseData FirstOneWhere(WhereDelegate<ResponseDataColumns> where, Database db = null)
			{
				return Bam.Net.Logging.Http.Data.Dao.ResponseData.FirstOneWhere(where, db);
			}

			public ResponseDataCollection Top(int count, WhereDelegate<ResponseDataColumns> where, Database db = null)
			{
				return Bam.Net.Logging.Http.Data.Dao.ResponseData.Top(count, where, db);
			}

			public ResponseDataCollection Top(int count, WhereDelegate<ResponseDataColumns> where, OrderBy<ResponseDataColumns> orderBy, Database db = null)
			{
				return Bam.Net.Logging.Http.Data.Dao.ResponseData.Top(count, where, orderBy, db);
			}

			public long Count(WhereDelegate<ResponseDataColumns> where, Database db = null)
			{
				return Bam.Net.Logging.Http.Data.Dao.ResponseData.Count(where, db);
			}
	}

	static ResponseDataQueryContext _responseDatas;
	static object _responseDatasLock = new object();
	public static ResponseDataQueryContext ResponseDatas
	{
		get
		{
			return _responseDatasLock.DoubleCheckLock<ResponseDataQueryContext>(ref _responseDatas, () => new ResponseDataQueryContext());
		}
	}
	public class CookieDataQueryContext
	{
			public CookieDataCollection Where(WhereDelegate<CookieDataColumns> where, Database db = null)
			{
				return Bam.Net.Logging.Http.Data.Dao.CookieData.Where(where, db);
			}
		   
			public CookieDataCollection Where(WhereDelegate<CookieDataColumns> where, OrderBy<CookieDataColumns> orderBy = null, Database db = null)
			{
				return Bam.Net.Logging.Http.Data.Dao.CookieData.Where(where, orderBy, db);
			}

			public CookieData OneWhere(WhereDelegate<CookieDataColumns> where, Database db = null)
			{
				return Bam.Net.Logging.Http.Data.Dao.CookieData.OneWhere(where, db);
			}

			public static CookieData GetOneWhere(WhereDelegate<CookieDataColumns> where, Database db = null)
			{
				return Bam.Net.Logging.Http.Data.Dao.CookieData.GetOneWhere(where, db);
			}
		
			public CookieData FirstOneWhere(WhereDelegate<CookieDataColumns> where, Database db = null)
			{
				return Bam.Net.Logging.Http.Data.Dao.CookieData.FirstOneWhere(where, db);
			}

			public CookieDataCollection Top(int count, WhereDelegate<CookieDataColumns> where, Database db = null)
			{
				return Bam.Net.Logging.Http.Data.Dao.CookieData.Top(count, where, db);
			}

			public CookieDataCollection Top(int count, WhereDelegate<CookieDataColumns> where, OrderBy<CookieDataColumns> orderBy, Database db = null)
			{
				return Bam.Net.Logging.Http.Data.Dao.CookieData.Top(count, where, orderBy, db);
			}

			public long Count(WhereDelegate<CookieDataColumns> where, Database db = null)
			{
				return Bam.Net.Logging.Http.Data.Dao.CookieData.Count(where, db);
			}
	}

	static CookieDataQueryContext _cookieDatas;
	static object _cookieDatasLock = new object();
	public static CookieDataQueryContext CookieDatas
	{
		get
		{
			return _cookieDatasLock.DoubleCheckLock<CookieDataQueryContext>(ref _cookieDatas, () => new CookieDataQueryContext());
		}
	}
	public class HeaderDataQueryContext
	{
			public HeaderDataCollection Where(WhereDelegate<HeaderDataColumns> where, Database db = null)
			{
				return Bam.Net.Logging.Http.Data.Dao.HeaderData.Where(where, db);
			}
		   
			public HeaderDataCollection Where(WhereDelegate<HeaderDataColumns> where, OrderBy<HeaderDataColumns> orderBy = null, Database db = null)
			{
				return Bam.Net.Logging.Http.Data.Dao.HeaderData.Where(where, orderBy, db);
			}

			public HeaderData OneWhere(WhereDelegate<HeaderDataColumns> where, Database db = null)
			{
				return Bam.Net.Logging.Http.Data.Dao.HeaderData.OneWhere(where, db);
			}

			public static HeaderData GetOneWhere(WhereDelegate<HeaderDataColumns> where, Database db = null)
			{
				return Bam.Net.Logging.Http.Data.Dao.HeaderData.GetOneWhere(where, db);
			}
		
			public HeaderData FirstOneWhere(WhereDelegate<HeaderDataColumns> where, Database db = null)
			{
				return Bam.Net.Logging.Http.Data.Dao.HeaderData.FirstOneWhere(where, db);
			}

			public HeaderDataCollection Top(int count, WhereDelegate<HeaderDataColumns> where, Database db = null)
			{
				return Bam.Net.Logging.Http.Data.Dao.HeaderData.Top(count, where, db);
			}

			public HeaderDataCollection Top(int count, WhereDelegate<HeaderDataColumns> where, OrderBy<HeaderDataColumns> orderBy, Database db = null)
			{
				return Bam.Net.Logging.Http.Data.Dao.HeaderData.Top(count, where, orderBy, db);
			}

			public long Count(WhereDelegate<HeaderDataColumns> where, Database db = null)
			{
				return Bam.Net.Logging.Http.Data.Dao.HeaderData.Count(where, db);
			}
	}

	static HeaderDataQueryContext _headerDatas;
	static object _headerDatasLock = new object();
	public static HeaderDataQueryContext HeaderDatas
	{
		get
		{
			return _headerDatasLock.DoubleCheckLock<HeaderDataQueryContext>(ref _headerDatas, () => new HeaderDataQueryContext());
		}
	}
	public class QueryStringDataQueryContext
	{
			public QueryStringDataCollection Where(WhereDelegate<QueryStringDataColumns> where, Database db = null)
			{
				return Bam.Net.Logging.Http.Data.Dao.QueryStringData.Where(where, db);
			}
		   
			public QueryStringDataCollection Where(WhereDelegate<QueryStringDataColumns> where, OrderBy<QueryStringDataColumns> orderBy = null, Database db = null)
			{
				return Bam.Net.Logging.Http.Data.Dao.QueryStringData.Where(where, orderBy, db);
			}

			public QueryStringData OneWhere(WhereDelegate<QueryStringDataColumns> where, Database db = null)
			{
				return Bam.Net.Logging.Http.Data.Dao.QueryStringData.OneWhere(where, db);
			}

			public static QueryStringData GetOneWhere(WhereDelegate<QueryStringDataColumns> where, Database db = null)
			{
				return Bam.Net.Logging.Http.Data.Dao.QueryStringData.GetOneWhere(where, db);
			}
		
			public QueryStringData FirstOneWhere(WhereDelegate<QueryStringDataColumns> where, Database db = null)
			{
				return Bam.Net.Logging.Http.Data.Dao.QueryStringData.FirstOneWhere(where, db);
			}

			public QueryStringDataCollection Top(int count, WhereDelegate<QueryStringDataColumns> where, Database db = null)
			{
				return Bam.Net.Logging.Http.Data.Dao.QueryStringData.Top(count, where, db);
			}

			public QueryStringDataCollection Top(int count, WhereDelegate<QueryStringDataColumns> where, OrderBy<QueryStringDataColumns> orderBy, Database db = null)
			{
				return Bam.Net.Logging.Http.Data.Dao.QueryStringData.Top(count, where, orderBy, db);
			}

			public long Count(WhereDelegate<QueryStringDataColumns> where, Database db = null)
			{
				return Bam.Net.Logging.Http.Data.Dao.QueryStringData.Count(where, db);
			}
	}

	static QueryStringDataQueryContext _queryStringDatas;
	static object _queryStringDatasLock = new object();
	public static QueryStringDataQueryContext QueryStringDatas
	{
		get
		{
			return _queryStringDatasLock.DoubleCheckLock<QueryStringDataQueryContext>(ref _queryStringDatas, () => new QueryStringDataQueryContext());
		}
	}
	public class RequestDataQueryContext
	{
			public RequestDataCollection Where(WhereDelegate<RequestDataColumns> where, Database db = null)
			{
				return Bam.Net.Logging.Http.Data.Dao.RequestData.Where(where, db);
			}
		   
			public RequestDataCollection Where(WhereDelegate<RequestDataColumns> where, OrderBy<RequestDataColumns> orderBy = null, Database db = null)
			{
				return Bam.Net.Logging.Http.Data.Dao.RequestData.Where(where, orderBy, db);
			}

			public RequestData OneWhere(WhereDelegate<RequestDataColumns> where, Database db = null)
			{
				return Bam.Net.Logging.Http.Data.Dao.RequestData.OneWhere(where, db);
			}

			public static RequestData GetOneWhere(WhereDelegate<RequestDataColumns> where, Database db = null)
			{
				return Bam.Net.Logging.Http.Data.Dao.RequestData.GetOneWhere(where, db);
			}
		
			public RequestData FirstOneWhere(WhereDelegate<RequestDataColumns> where, Database db = null)
			{
				return Bam.Net.Logging.Http.Data.Dao.RequestData.FirstOneWhere(where, db);
			}

			public RequestDataCollection Top(int count, WhereDelegate<RequestDataColumns> where, Database db = null)
			{
				return Bam.Net.Logging.Http.Data.Dao.RequestData.Top(count, where, db);
			}

			public RequestDataCollection Top(int count, WhereDelegate<RequestDataColumns> where, OrderBy<RequestDataColumns> orderBy, Database db = null)
			{
				return Bam.Net.Logging.Http.Data.Dao.RequestData.Top(count, where, orderBy, db);
			}

			public long Count(WhereDelegate<RequestDataColumns> where, Database db = null)
			{
				return Bam.Net.Logging.Http.Data.Dao.RequestData.Count(where, db);
			}
	}

	static RequestDataQueryContext _requestDatas;
	static object _requestDatasLock = new object();
	public static RequestDataQueryContext RequestDatas
	{
		get
		{
			return _requestDatasLock.DoubleCheckLock<RequestDataQueryContext>(ref _requestDatas, () => new RequestDataQueryContext());
		}
	}
	public class UriDataQueryContext
	{
			public UriDataCollection Where(WhereDelegate<UriDataColumns> where, Database db = null)
			{
				return Bam.Net.Logging.Http.Data.Dao.UriData.Where(where, db);
			}
		   
			public UriDataCollection Where(WhereDelegate<UriDataColumns> where, OrderBy<UriDataColumns> orderBy = null, Database db = null)
			{
				return Bam.Net.Logging.Http.Data.Dao.UriData.Where(where, orderBy, db);
			}

			public UriData OneWhere(WhereDelegate<UriDataColumns> where, Database db = null)
			{
				return Bam.Net.Logging.Http.Data.Dao.UriData.OneWhere(where, db);
			}

			public static UriData GetOneWhere(WhereDelegate<UriDataColumns> where, Database db = null)
			{
				return Bam.Net.Logging.Http.Data.Dao.UriData.GetOneWhere(where, db);
			}
		
			public UriData FirstOneWhere(WhereDelegate<UriDataColumns> where, Database db = null)
			{
				return Bam.Net.Logging.Http.Data.Dao.UriData.FirstOneWhere(where, db);
			}

			public UriDataCollection Top(int count, WhereDelegate<UriDataColumns> where, Database db = null)
			{
				return Bam.Net.Logging.Http.Data.Dao.UriData.Top(count, where, db);
			}

			public UriDataCollection Top(int count, WhereDelegate<UriDataColumns> where, OrderBy<UriDataColumns> orderBy, Database db = null)
			{
				return Bam.Net.Logging.Http.Data.Dao.UriData.Top(count, where, orderBy, db);
			}

			public long Count(WhereDelegate<UriDataColumns> where, Database db = null)
			{
				return Bam.Net.Logging.Http.Data.Dao.UriData.Count(where, db);
			}
	}

	static UriDataQueryContext _uriDatas;
	static object _uriDatasLock = new object();
	public static UriDataQueryContext UriDatas
	{
		get
		{
			return _uriDatasLock.DoubleCheckLock<UriDataQueryContext>(ref _uriDatas, () => new UriDataQueryContext());
		}
	}
	public class UserDataQueryContext
	{
			public UserDataCollection Where(WhereDelegate<UserDataColumns> where, Database db = null)
			{
				return Bam.Net.Logging.Http.Data.Dao.UserData.Where(where, db);
			}
		   
			public UserDataCollection Where(WhereDelegate<UserDataColumns> where, OrderBy<UserDataColumns> orderBy = null, Database db = null)
			{
				return Bam.Net.Logging.Http.Data.Dao.UserData.Where(where, orderBy, db);
			}

			public UserData OneWhere(WhereDelegate<UserDataColumns> where, Database db = null)
			{
				return Bam.Net.Logging.Http.Data.Dao.UserData.OneWhere(where, db);
			}

			public static UserData GetOneWhere(WhereDelegate<UserDataColumns> where, Database db = null)
			{
				return Bam.Net.Logging.Http.Data.Dao.UserData.GetOneWhere(where, db);
			}
		
			public UserData FirstOneWhere(WhereDelegate<UserDataColumns> where, Database db = null)
			{
				return Bam.Net.Logging.Http.Data.Dao.UserData.FirstOneWhere(where, db);
			}

			public UserDataCollection Top(int count, WhereDelegate<UserDataColumns> where, Database db = null)
			{
				return Bam.Net.Logging.Http.Data.Dao.UserData.Top(count, where, db);
			}

			public UserDataCollection Top(int count, WhereDelegate<UserDataColumns> where, OrderBy<UserDataColumns> orderBy, Database db = null)
			{
				return Bam.Net.Logging.Http.Data.Dao.UserData.Top(count, where, orderBy, db);
			}

			public long Count(WhereDelegate<UserDataColumns> where, Database db = null)
			{
				return Bam.Net.Logging.Http.Data.Dao.UserData.Count(where, db);
			}
	}

	static UserDataQueryContext _userDatas;
	static object _userDatasLock = new object();
	public static UserDataQueryContext UserDatas
	{
		get
		{
			return _userDatasLock.DoubleCheckLock<UserDataQueryContext>(ref _userDatas, () => new UserDataQueryContext());
		}
	}
	public class UserKeyDataQueryContext
	{
			public UserKeyDataCollection Where(WhereDelegate<UserKeyDataColumns> where, Database db = null)
			{
				return Bam.Net.Logging.Http.Data.Dao.UserKeyData.Where(where, db);
			}
		   
			public UserKeyDataCollection Where(WhereDelegate<UserKeyDataColumns> where, OrderBy<UserKeyDataColumns> orderBy = null, Database db = null)
			{
				return Bam.Net.Logging.Http.Data.Dao.UserKeyData.Where(where, orderBy, db);
			}

			public UserKeyData OneWhere(WhereDelegate<UserKeyDataColumns> where, Database db = null)
			{
				return Bam.Net.Logging.Http.Data.Dao.UserKeyData.OneWhere(where, db);
			}

			public static UserKeyData GetOneWhere(WhereDelegate<UserKeyDataColumns> where, Database db = null)
			{
				return Bam.Net.Logging.Http.Data.Dao.UserKeyData.GetOneWhere(where, db);
			}
		
			public UserKeyData FirstOneWhere(WhereDelegate<UserKeyDataColumns> where, Database db = null)
			{
				return Bam.Net.Logging.Http.Data.Dao.UserKeyData.FirstOneWhere(where, db);
			}

			public UserKeyDataCollection Top(int count, WhereDelegate<UserKeyDataColumns> where, Database db = null)
			{
				return Bam.Net.Logging.Http.Data.Dao.UserKeyData.Top(count, where, db);
			}

			public UserKeyDataCollection Top(int count, WhereDelegate<UserKeyDataColumns> where, OrderBy<UserKeyDataColumns> orderBy, Database db = null)
			{
				return Bam.Net.Logging.Http.Data.Dao.UserKeyData.Top(count, where, orderBy, db);
			}

			public long Count(WhereDelegate<UserKeyDataColumns> where, Database db = null)
			{
				return Bam.Net.Logging.Http.Data.Dao.UserKeyData.Count(where, db);
			}
	}

	static UserKeyDataQueryContext _userKeyDatas;
	static object _userKeyDatasLock = new object();
	public static UserKeyDataQueryContext UserKeyDatas
	{
		get
		{
			return _userKeyDatasLock.DoubleCheckLock<UserKeyDataQueryContext>(ref _userKeyDatas, () => new UserKeyDataQueryContext());
		}
	}
    }
}																								
