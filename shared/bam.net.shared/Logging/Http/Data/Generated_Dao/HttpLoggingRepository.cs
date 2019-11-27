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
using Bam.Net.Logging.Http.Data;

namespace Bam.Net.Logging.Http.Data.Dao.Repository
{
	[Serializable]
	public class HttpLoggingRepository: DaoRepository
	{
		public HttpLoggingRepository()
		{
			SchemaName = "HttpLogging";
			BaseNamespace = "Bam.Net.Logging.Http.Data";			

			
			AddType<Bam.Net.Logging.Http.Data.ContentNotFoundData>();
			
			
			AddType<Bam.Net.Logging.Http.Data.ResponseData>();
			
			
			AddType<Bam.Net.Logging.Http.Data.CookieData>();
			
			
			AddType<Bam.Net.Logging.Http.Data.HeaderData>();
			
			
			AddType<Bam.Net.Logging.Http.Data.QueryStringData>();
			
			
			AddType<Bam.Net.Logging.Http.Data.RequestData>();
			
			
			AddType<Bam.Net.Logging.Http.Data.UriData>();
			
			
			AddType<Bam.Net.Logging.Http.Data.UserData>();
			
			
			AddType<Bam.Net.Logging.Http.Data.UserKeyData>();
			

			DaoAssembly = typeof(HttpLoggingRepository).Assembly;
		}

		object _addLock = new object();
        public override void AddType(Type type)
        {
            lock (_addLock)
            {
                base.AddType(type);
                DaoAssembly = typeof(HttpLoggingRepository).Assembly;
            }
        }

		
		/// <summary>
		/// Set one entry matching the specified filter.  If none exists 
		/// one is created; success depends on the nullability
		/// of the specified columns.
		/// </summary>
		public void SetOneContentNotFoundDataWhere(WhereDelegate<ContentNotFoundDataColumns> where)
		{
			Bam.Net.Logging.Http.Data.Dao.ContentNotFoundData.SetOneWhere(where, Database);
		}

		/// <summary>
		/// Set one entry matching the specified filter.  If none exists 
		/// one is created; success depends on the nullability
		/// of the specified columns.
		/// </summary>
		public void SetOneContentNotFoundDataWhere(WhereDelegate<ContentNotFoundDataColumns> where, out Bam.Net.Logging.Http.Data.ContentNotFoundData result)
		{
			Bam.Net.Logging.Http.Data.Dao.ContentNotFoundData.SetOneWhere(where, out Bam.Net.Logging.Http.Data.Dao.ContentNotFoundData daoResult, Database);
			result = daoResult.CopyAs<Bam.Net.Logging.Http.Data.ContentNotFoundData>();
		}

		/// <summary>
		/// Get one entry matching the specified filter.  If none exists 
		/// one is created; success depends on the nullability
		/// of the specified columns.
		/// </summary>
		/// <param name="where"></param>
		public Bam.Net.Logging.Http.Data.ContentNotFoundData GetOneContentNotFoundDataWhere(WhereDelegate<ContentNotFoundDataColumns> where)
		{
			Type wrapperType = GetWrapperType<Bam.Net.Logging.Http.Data.ContentNotFoundData>();
			return (Bam.Net.Logging.Http.Data.ContentNotFoundData)Bam.Net.Logging.Http.Data.Dao.ContentNotFoundData.GetOneWhere(where, Database)?.CopyAs(wrapperType, this);
		}

		/// <summary>
		/// Execute a query that should return only one result.  If no result is found null is returned.  If more
		/// than one result is returned a MultipleEntriesFoundException is thrown.  This method is most commonly used to retrieve a
		/// single ContentNotFoundData instance by its Id/Key value
		/// </summary>
		/// <param name="where">A WhereDelegate that receives a ContentNotFoundDataColumns 
		/// and returns a IQueryFilter which is the result of any comparisons
		/// between ContentNotFoundDataColumns and other values
		/// </param>
		public Bam.Net.Logging.Http.Data.ContentNotFoundData OneContentNotFoundDataWhere(WhereDelegate<ContentNotFoundDataColumns> where)
        {
            Type wrapperType = GetWrapperType<Bam.Net.Logging.Http.Data.ContentNotFoundData>();
            return (Bam.Net.Logging.Http.Data.ContentNotFoundData)Bam.Net.Logging.Http.Data.Dao.ContentNotFoundData.OneWhere(where, Database)?.CopyAs(wrapperType, this);
        }

		/// <summary>
		/// Execute a query and return the results. 
		/// </summary>
		/// <param name="where">A WhereDelegate that receives a Bam.Net.Logging.Http.Data.ContentNotFoundDataColumns 
		/// and returns a IQueryFilter which is the result of any comparisons
		/// between Bam.Net.Logging.Http.Data.ContentNotFoundDataColumns and other values
		/// </param>
		public IEnumerable<Bam.Net.Logging.Http.Data.ContentNotFoundData> ContentNotFoundDatasWhere(WhereDelegate<ContentNotFoundDataColumns> where, OrderBy<ContentNotFoundDataColumns> orderBy = null)
        {
            return Wrap<Bam.Net.Logging.Http.Data.ContentNotFoundData>(Bam.Net.Logging.Http.Data.Dao.ContentNotFoundData.Where(where, orderBy, Database));
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
		/// <param name="where">A WhereDelegate that receives a ContentNotFoundDataColumns 
		/// and returns a IQueryFilter which is the result of any comparisons
		/// between ContentNotFoundDataColumns and other values
		/// </param>
		public IEnumerable<Bam.Net.Logging.Http.Data.ContentNotFoundData> TopContentNotFoundDatasWhere(int count, WhereDelegate<ContentNotFoundDataColumns> where)
        {
            return Wrap<Bam.Net.Logging.Http.Data.ContentNotFoundData>(Bam.Net.Logging.Http.Data.Dao.ContentNotFoundData.Top(count, where, Database));
        }

        public IEnumerable<Bam.Net.Logging.Http.Data.ContentNotFoundData> TopContentNotFoundDatasWhere(int count, WhereDelegate<ContentNotFoundDataColumns> where, OrderBy<ContentNotFoundDataColumns> orderBy)
        {
            return Wrap<Bam.Net.Logging.Http.Data.ContentNotFoundData>(Bam.Net.Logging.Http.Data.Dao.ContentNotFoundData.Top(count, where, orderBy, Database));
        }
                                
		/// <summary>
		/// Return the count of ContentNotFoundDatas
		/// </summary>
		public long CountContentNotFoundDatas()
        {
            return Bam.Net.Logging.Http.Data.Dao.ContentNotFoundData.Count(Database);
        }

		/// <summary>
		/// Execute a query and return the number of results
		/// </summary>
		/// <param name="where">A WhereDelegate that receives a ContentNotFoundDataColumns 
		/// and returns a IQueryFilter which is the result of any comparisons
		/// between ContentNotFoundDataColumns and other values
		/// </param>
        public long CountContentNotFoundDatasWhere(WhereDelegate<ContentNotFoundDataColumns> where)
        {
            return Bam.Net.Logging.Http.Data.Dao.ContentNotFoundData.Count(where, Database);
        }
        
        public async Task BatchQueryContentNotFoundDatas(int batchSize, WhereDelegate<ContentNotFoundDataColumns> where, Action<IEnumerable<Bam.Net.Logging.Http.Data.ContentNotFoundData>> batchProcessor)
        {
            await Bam.Net.Logging.Http.Data.Dao.ContentNotFoundData.BatchQuery(batchSize, where, (batch) =>
            {
				batchProcessor(Wrap<Bam.Net.Logging.Http.Data.ContentNotFoundData>(batch));
            }, Database);
        }
		
        public async Task BatchAllContentNotFoundDatas(int batchSize, Action<IEnumerable<Bam.Net.Logging.Http.Data.ContentNotFoundData>> batchProcessor)
        {
            await Bam.Net.Logging.Http.Data.Dao.ContentNotFoundData.BatchAll(batchSize, (batch) =>
            {
				batchProcessor(Wrap<Bam.Net.Logging.Http.Data.ContentNotFoundData>(batch));
            }, Database);
        }

		
		/// <summary>
		/// Set one entry matching the specified filter.  If none exists 
		/// one is created; success depends on the nullability
		/// of the specified columns.
		/// </summary>
		public void SetOneResponseDataWhere(WhereDelegate<ResponseDataColumns> where)
		{
			Bam.Net.Logging.Http.Data.Dao.ResponseData.SetOneWhere(where, Database);
		}

		/// <summary>
		/// Set one entry matching the specified filter.  If none exists 
		/// one is created; success depends on the nullability
		/// of the specified columns.
		/// </summary>
		public void SetOneResponseDataWhere(WhereDelegate<ResponseDataColumns> where, out Bam.Net.Logging.Http.Data.ResponseData result)
		{
			Bam.Net.Logging.Http.Data.Dao.ResponseData.SetOneWhere(where, out Bam.Net.Logging.Http.Data.Dao.ResponseData daoResult, Database);
			result = daoResult.CopyAs<Bam.Net.Logging.Http.Data.ResponseData>();
		}

		/// <summary>
		/// Get one entry matching the specified filter.  If none exists 
		/// one is created; success depends on the nullability
		/// of the specified columns.
		/// </summary>
		/// <param name="where"></param>
		public Bam.Net.Logging.Http.Data.ResponseData GetOneResponseDataWhere(WhereDelegate<ResponseDataColumns> where)
		{
			Type wrapperType = GetWrapperType<Bam.Net.Logging.Http.Data.ResponseData>();
			return (Bam.Net.Logging.Http.Data.ResponseData)Bam.Net.Logging.Http.Data.Dao.ResponseData.GetOneWhere(where, Database)?.CopyAs(wrapperType, this);
		}

		/// <summary>
		/// Execute a query that should return only one result.  If no result is found null is returned.  If more
		/// than one result is returned a MultipleEntriesFoundException is thrown.  This method is most commonly used to retrieve a
		/// single ResponseData instance by its Id/Key value
		/// </summary>
		/// <param name="where">A WhereDelegate that receives a ResponseDataColumns 
		/// and returns a IQueryFilter which is the result of any comparisons
		/// between ResponseDataColumns and other values
		/// </param>
		public Bam.Net.Logging.Http.Data.ResponseData OneResponseDataWhere(WhereDelegate<ResponseDataColumns> where)
        {
            Type wrapperType = GetWrapperType<Bam.Net.Logging.Http.Data.ResponseData>();
            return (Bam.Net.Logging.Http.Data.ResponseData)Bam.Net.Logging.Http.Data.Dao.ResponseData.OneWhere(where, Database)?.CopyAs(wrapperType, this);
        }

		/// <summary>
		/// Execute a query and return the results. 
		/// </summary>
		/// <param name="where">A WhereDelegate that receives a Bam.Net.Logging.Http.Data.ResponseDataColumns 
		/// and returns a IQueryFilter which is the result of any comparisons
		/// between Bam.Net.Logging.Http.Data.ResponseDataColumns and other values
		/// </param>
		public IEnumerable<Bam.Net.Logging.Http.Data.ResponseData> ResponseDatasWhere(WhereDelegate<ResponseDataColumns> where, OrderBy<ResponseDataColumns> orderBy = null)
        {
            return Wrap<Bam.Net.Logging.Http.Data.ResponseData>(Bam.Net.Logging.Http.Data.Dao.ResponseData.Where(where, orderBy, Database));
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
		/// <param name="where">A WhereDelegate that receives a ResponseDataColumns 
		/// and returns a IQueryFilter which is the result of any comparisons
		/// between ResponseDataColumns and other values
		/// </param>
		public IEnumerable<Bam.Net.Logging.Http.Data.ResponseData> TopResponseDatasWhere(int count, WhereDelegate<ResponseDataColumns> where)
        {
            return Wrap<Bam.Net.Logging.Http.Data.ResponseData>(Bam.Net.Logging.Http.Data.Dao.ResponseData.Top(count, where, Database));
        }

        public IEnumerable<Bam.Net.Logging.Http.Data.ResponseData> TopResponseDatasWhere(int count, WhereDelegate<ResponseDataColumns> where, OrderBy<ResponseDataColumns> orderBy)
        {
            return Wrap<Bam.Net.Logging.Http.Data.ResponseData>(Bam.Net.Logging.Http.Data.Dao.ResponseData.Top(count, where, orderBy, Database));
        }
                                
		/// <summary>
		/// Return the count of ResponseDatas
		/// </summary>
		public long CountResponseDatas()
        {
            return Bam.Net.Logging.Http.Data.Dao.ResponseData.Count(Database);
        }

		/// <summary>
		/// Execute a query and return the number of results
		/// </summary>
		/// <param name="where">A WhereDelegate that receives a ResponseDataColumns 
		/// and returns a IQueryFilter which is the result of any comparisons
		/// between ResponseDataColumns and other values
		/// </param>
        public long CountResponseDatasWhere(WhereDelegate<ResponseDataColumns> where)
        {
            return Bam.Net.Logging.Http.Data.Dao.ResponseData.Count(where, Database);
        }
        
        public async Task BatchQueryResponseDatas(int batchSize, WhereDelegate<ResponseDataColumns> where, Action<IEnumerable<Bam.Net.Logging.Http.Data.ResponseData>> batchProcessor)
        {
            await Bam.Net.Logging.Http.Data.Dao.ResponseData.BatchQuery(batchSize, where, (batch) =>
            {
				batchProcessor(Wrap<Bam.Net.Logging.Http.Data.ResponseData>(batch));
            }, Database);
        }
		
        public async Task BatchAllResponseDatas(int batchSize, Action<IEnumerable<Bam.Net.Logging.Http.Data.ResponseData>> batchProcessor)
        {
            await Bam.Net.Logging.Http.Data.Dao.ResponseData.BatchAll(batchSize, (batch) =>
            {
				batchProcessor(Wrap<Bam.Net.Logging.Http.Data.ResponseData>(batch));
            }, Database);
        }

		
		/// <summary>
		/// Set one entry matching the specified filter.  If none exists 
		/// one is created; success depends on the nullability
		/// of the specified columns.
		/// </summary>
		public void SetOneCookieDataWhere(WhereDelegate<CookieDataColumns> where)
		{
			Bam.Net.Logging.Http.Data.Dao.CookieData.SetOneWhere(where, Database);
		}

		/// <summary>
		/// Set one entry matching the specified filter.  If none exists 
		/// one is created; success depends on the nullability
		/// of the specified columns.
		/// </summary>
		public void SetOneCookieDataWhere(WhereDelegate<CookieDataColumns> where, out Bam.Net.Logging.Http.Data.CookieData result)
		{
			Bam.Net.Logging.Http.Data.Dao.CookieData.SetOneWhere(where, out Bam.Net.Logging.Http.Data.Dao.CookieData daoResult, Database);
			result = daoResult.CopyAs<Bam.Net.Logging.Http.Data.CookieData>();
		}

		/// <summary>
		/// Get one entry matching the specified filter.  If none exists 
		/// one is created; success depends on the nullability
		/// of the specified columns.
		/// </summary>
		/// <param name="where"></param>
		public Bam.Net.Logging.Http.Data.CookieData GetOneCookieDataWhere(WhereDelegate<CookieDataColumns> where)
		{
			Type wrapperType = GetWrapperType<Bam.Net.Logging.Http.Data.CookieData>();
			return (Bam.Net.Logging.Http.Data.CookieData)Bam.Net.Logging.Http.Data.Dao.CookieData.GetOneWhere(where, Database)?.CopyAs(wrapperType, this);
		}

		/// <summary>
		/// Execute a query that should return only one result.  If no result is found null is returned.  If more
		/// than one result is returned a MultipleEntriesFoundException is thrown.  This method is most commonly used to retrieve a
		/// single CookieData instance by its Id/Key value
		/// </summary>
		/// <param name="where">A WhereDelegate that receives a CookieDataColumns 
		/// and returns a IQueryFilter which is the result of any comparisons
		/// between CookieDataColumns and other values
		/// </param>
		public Bam.Net.Logging.Http.Data.CookieData OneCookieDataWhere(WhereDelegate<CookieDataColumns> where)
        {
            Type wrapperType = GetWrapperType<Bam.Net.Logging.Http.Data.CookieData>();
            return (Bam.Net.Logging.Http.Data.CookieData)Bam.Net.Logging.Http.Data.Dao.CookieData.OneWhere(where, Database)?.CopyAs(wrapperType, this);
        }

		/// <summary>
		/// Execute a query and return the results. 
		/// </summary>
		/// <param name="where">A WhereDelegate that receives a Bam.Net.Logging.Http.Data.CookieDataColumns 
		/// and returns a IQueryFilter which is the result of any comparisons
		/// between Bam.Net.Logging.Http.Data.CookieDataColumns and other values
		/// </param>
		public IEnumerable<Bam.Net.Logging.Http.Data.CookieData> CookieDatasWhere(WhereDelegate<CookieDataColumns> where, OrderBy<CookieDataColumns> orderBy = null)
        {
            return Wrap<Bam.Net.Logging.Http.Data.CookieData>(Bam.Net.Logging.Http.Data.Dao.CookieData.Where(where, orderBy, Database));
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
		/// <param name="where">A WhereDelegate that receives a CookieDataColumns 
		/// and returns a IQueryFilter which is the result of any comparisons
		/// between CookieDataColumns and other values
		/// </param>
		public IEnumerable<Bam.Net.Logging.Http.Data.CookieData> TopCookieDatasWhere(int count, WhereDelegate<CookieDataColumns> where)
        {
            return Wrap<Bam.Net.Logging.Http.Data.CookieData>(Bam.Net.Logging.Http.Data.Dao.CookieData.Top(count, where, Database));
        }

        public IEnumerable<Bam.Net.Logging.Http.Data.CookieData> TopCookieDatasWhere(int count, WhereDelegate<CookieDataColumns> where, OrderBy<CookieDataColumns> orderBy)
        {
            return Wrap<Bam.Net.Logging.Http.Data.CookieData>(Bam.Net.Logging.Http.Data.Dao.CookieData.Top(count, where, orderBy, Database));
        }
                                
		/// <summary>
		/// Return the count of CookieDatas
		/// </summary>
		public long CountCookieDatas()
        {
            return Bam.Net.Logging.Http.Data.Dao.CookieData.Count(Database);
        }

		/// <summary>
		/// Execute a query and return the number of results
		/// </summary>
		/// <param name="where">A WhereDelegate that receives a CookieDataColumns 
		/// and returns a IQueryFilter which is the result of any comparisons
		/// between CookieDataColumns and other values
		/// </param>
        public long CountCookieDatasWhere(WhereDelegate<CookieDataColumns> where)
        {
            return Bam.Net.Logging.Http.Data.Dao.CookieData.Count(where, Database);
        }
        
        public async Task BatchQueryCookieDatas(int batchSize, WhereDelegate<CookieDataColumns> where, Action<IEnumerable<Bam.Net.Logging.Http.Data.CookieData>> batchProcessor)
        {
            await Bam.Net.Logging.Http.Data.Dao.CookieData.BatchQuery(batchSize, where, (batch) =>
            {
				batchProcessor(Wrap<Bam.Net.Logging.Http.Data.CookieData>(batch));
            }, Database);
        }
		
        public async Task BatchAllCookieDatas(int batchSize, Action<IEnumerable<Bam.Net.Logging.Http.Data.CookieData>> batchProcessor)
        {
            await Bam.Net.Logging.Http.Data.Dao.CookieData.BatchAll(batchSize, (batch) =>
            {
				batchProcessor(Wrap<Bam.Net.Logging.Http.Data.CookieData>(batch));
            }, Database);
        }

		
		/// <summary>
		/// Set one entry matching the specified filter.  If none exists 
		/// one is created; success depends on the nullability
		/// of the specified columns.
		/// </summary>
		public void SetOneHeaderDataWhere(WhereDelegate<HeaderDataColumns> where)
		{
			Bam.Net.Logging.Http.Data.Dao.HeaderData.SetOneWhere(where, Database);
		}

		/// <summary>
		/// Set one entry matching the specified filter.  If none exists 
		/// one is created; success depends on the nullability
		/// of the specified columns.
		/// </summary>
		public void SetOneHeaderDataWhere(WhereDelegate<HeaderDataColumns> where, out Bam.Net.Logging.Http.Data.HeaderData result)
		{
			Bam.Net.Logging.Http.Data.Dao.HeaderData.SetOneWhere(where, out Bam.Net.Logging.Http.Data.Dao.HeaderData daoResult, Database);
			result = daoResult.CopyAs<Bam.Net.Logging.Http.Data.HeaderData>();
		}

		/// <summary>
		/// Get one entry matching the specified filter.  If none exists 
		/// one is created; success depends on the nullability
		/// of the specified columns.
		/// </summary>
		/// <param name="where"></param>
		public Bam.Net.Logging.Http.Data.HeaderData GetOneHeaderDataWhere(WhereDelegate<HeaderDataColumns> where)
		{
			Type wrapperType = GetWrapperType<Bam.Net.Logging.Http.Data.HeaderData>();
			return (Bam.Net.Logging.Http.Data.HeaderData)Bam.Net.Logging.Http.Data.Dao.HeaderData.GetOneWhere(where, Database)?.CopyAs(wrapperType, this);
		}

		/// <summary>
		/// Execute a query that should return only one result.  If no result is found null is returned.  If more
		/// than one result is returned a MultipleEntriesFoundException is thrown.  This method is most commonly used to retrieve a
		/// single HeaderData instance by its Id/Key value
		/// </summary>
		/// <param name="where">A WhereDelegate that receives a HeaderDataColumns 
		/// and returns a IQueryFilter which is the result of any comparisons
		/// between HeaderDataColumns and other values
		/// </param>
		public Bam.Net.Logging.Http.Data.HeaderData OneHeaderDataWhere(WhereDelegate<HeaderDataColumns> where)
        {
            Type wrapperType = GetWrapperType<Bam.Net.Logging.Http.Data.HeaderData>();
            return (Bam.Net.Logging.Http.Data.HeaderData)Bam.Net.Logging.Http.Data.Dao.HeaderData.OneWhere(where, Database)?.CopyAs(wrapperType, this);
        }

		/// <summary>
		/// Execute a query and return the results. 
		/// </summary>
		/// <param name="where">A WhereDelegate that receives a Bam.Net.Logging.Http.Data.HeaderDataColumns 
		/// and returns a IQueryFilter which is the result of any comparisons
		/// between Bam.Net.Logging.Http.Data.HeaderDataColumns and other values
		/// </param>
		public IEnumerable<Bam.Net.Logging.Http.Data.HeaderData> HeaderDatasWhere(WhereDelegate<HeaderDataColumns> where, OrderBy<HeaderDataColumns> orderBy = null)
        {
            return Wrap<Bam.Net.Logging.Http.Data.HeaderData>(Bam.Net.Logging.Http.Data.Dao.HeaderData.Where(where, orderBy, Database));
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
		/// <param name="where">A WhereDelegate that receives a HeaderDataColumns 
		/// and returns a IQueryFilter which is the result of any comparisons
		/// between HeaderDataColumns and other values
		/// </param>
		public IEnumerable<Bam.Net.Logging.Http.Data.HeaderData> TopHeaderDatasWhere(int count, WhereDelegate<HeaderDataColumns> where)
        {
            return Wrap<Bam.Net.Logging.Http.Data.HeaderData>(Bam.Net.Logging.Http.Data.Dao.HeaderData.Top(count, where, Database));
        }

        public IEnumerable<Bam.Net.Logging.Http.Data.HeaderData> TopHeaderDatasWhere(int count, WhereDelegate<HeaderDataColumns> where, OrderBy<HeaderDataColumns> orderBy)
        {
            return Wrap<Bam.Net.Logging.Http.Data.HeaderData>(Bam.Net.Logging.Http.Data.Dao.HeaderData.Top(count, where, orderBy, Database));
        }
                                
		/// <summary>
		/// Return the count of HeaderDatas
		/// </summary>
		public long CountHeaderDatas()
        {
            return Bam.Net.Logging.Http.Data.Dao.HeaderData.Count(Database);
        }

		/// <summary>
		/// Execute a query and return the number of results
		/// </summary>
		/// <param name="where">A WhereDelegate that receives a HeaderDataColumns 
		/// and returns a IQueryFilter which is the result of any comparisons
		/// between HeaderDataColumns and other values
		/// </param>
        public long CountHeaderDatasWhere(WhereDelegate<HeaderDataColumns> where)
        {
            return Bam.Net.Logging.Http.Data.Dao.HeaderData.Count(where, Database);
        }
        
        public async Task BatchQueryHeaderDatas(int batchSize, WhereDelegate<HeaderDataColumns> where, Action<IEnumerable<Bam.Net.Logging.Http.Data.HeaderData>> batchProcessor)
        {
            await Bam.Net.Logging.Http.Data.Dao.HeaderData.BatchQuery(batchSize, where, (batch) =>
            {
				batchProcessor(Wrap<Bam.Net.Logging.Http.Data.HeaderData>(batch));
            }, Database);
        }
		
        public async Task BatchAllHeaderDatas(int batchSize, Action<IEnumerable<Bam.Net.Logging.Http.Data.HeaderData>> batchProcessor)
        {
            await Bam.Net.Logging.Http.Data.Dao.HeaderData.BatchAll(batchSize, (batch) =>
            {
				batchProcessor(Wrap<Bam.Net.Logging.Http.Data.HeaderData>(batch));
            }, Database);
        }

		
		/// <summary>
		/// Set one entry matching the specified filter.  If none exists 
		/// one is created; success depends on the nullability
		/// of the specified columns.
		/// </summary>
		public void SetOneQueryStringDataWhere(WhereDelegate<QueryStringDataColumns> where)
		{
			Bam.Net.Logging.Http.Data.Dao.QueryStringData.SetOneWhere(where, Database);
		}

		/// <summary>
		/// Set one entry matching the specified filter.  If none exists 
		/// one is created; success depends on the nullability
		/// of the specified columns.
		/// </summary>
		public void SetOneQueryStringDataWhere(WhereDelegate<QueryStringDataColumns> where, out Bam.Net.Logging.Http.Data.QueryStringData result)
		{
			Bam.Net.Logging.Http.Data.Dao.QueryStringData.SetOneWhere(where, out Bam.Net.Logging.Http.Data.Dao.QueryStringData daoResult, Database);
			result = daoResult.CopyAs<Bam.Net.Logging.Http.Data.QueryStringData>();
		}

		/// <summary>
		/// Get one entry matching the specified filter.  If none exists 
		/// one is created; success depends on the nullability
		/// of the specified columns.
		/// </summary>
		/// <param name="where"></param>
		public Bam.Net.Logging.Http.Data.QueryStringData GetOneQueryStringDataWhere(WhereDelegate<QueryStringDataColumns> where)
		{
			Type wrapperType = GetWrapperType<Bam.Net.Logging.Http.Data.QueryStringData>();
			return (Bam.Net.Logging.Http.Data.QueryStringData)Bam.Net.Logging.Http.Data.Dao.QueryStringData.GetOneWhere(where, Database)?.CopyAs(wrapperType, this);
		}

		/// <summary>
		/// Execute a query that should return only one result.  If no result is found null is returned.  If more
		/// than one result is returned a MultipleEntriesFoundException is thrown.  This method is most commonly used to retrieve a
		/// single QueryStringData instance by its Id/Key value
		/// </summary>
		/// <param name="where">A WhereDelegate that receives a QueryStringDataColumns 
		/// and returns a IQueryFilter which is the result of any comparisons
		/// between QueryStringDataColumns and other values
		/// </param>
		public Bam.Net.Logging.Http.Data.QueryStringData OneQueryStringDataWhere(WhereDelegate<QueryStringDataColumns> where)
        {
            Type wrapperType = GetWrapperType<Bam.Net.Logging.Http.Data.QueryStringData>();
            return (Bam.Net.Logging.Http.Data.QueryStringData)Bam.Net.Logging.Http.Data.Dao.QueryStringData.OneWhere(where, Database)?.CopyAs(wrapperType, this);
        }

		/// <summary>
		/// Execute a query and return the results. 
		/// </summary>
		/// <param name="where">A WhereDelegate that receives a Bam.Net.Logging.Http.Data.QueryStringDataColumns 
		/// and returns a IQueryFilter which is the result of any comparisons
		/// between Bam.Net.Logging.Http.Data.QueryStringDataColumns and other values
		/// </param>
		public IEnumerable<Bam.Net.Logging.Http.Data.QueryStringData> QueryStringDatasWhere(WhereDelegate<QueryStringDataColumns> where, OrderBy<QueryStringDataColumns> orderBy = null)
        {
            return Wrap<Bam.Net.Logging.Http.Data.QueryStringData>(Bam.Net.Logging.Http.Data.Dao.QueryStringData.Where(where, orderBy, Database));
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
		/// <param name="where">A WhereDelegate that receives a QueryStringDataColumns 
		/// and returns a IQueryFilter which is the result of any comparisons
		/// between QueryStringDataColumns and other values
		/// </param>
		public IEnumerable<Bam.Net.Logging.Http.Data.QueryStringData> TopQueryStringDatasWhere(int count, WhereDelegate<QueryStringDataColumns> where)
        {
            return Wrap<Bam.Net.Logging.Http.Data.QueryStringData>(Bam.Net.Logging.Http.Data.Dao.QueryStringData.Top(count, where, Database));
        }

        public IEnumerable<Bam.Net.Logging.Http.Data.QueryStringData> TopQueryStringDatasWhere(int count, WhereDelegate<QueryStringDataColumns> where, OrderBy<QueryStringDataColumns> orderBy)
        {
            return Wrap<Bam.Net.Logging.Http.Data.QueryStringData>(Bam.Net.Logging.Http.Data.Dao.QueryStringData.Top(count, where, orderBy, Database));
        }
                                
		/// <summary>
		/// Return the count of QueryStringDatas
		/// </summary>
		public long CountQueryStringDatas()
        {
            return Bam.Net.Logging.Http.Data.Dao.QueryStringData.Count(Database);
        }

		/// <summary>
		/// Execute a query and return the number of results
		/// </summary>
		/// <param name="where">A WhereDelegate that receives a QueryStringDataColumns 
		/// and returns a IQueryFilter which is the result of any comparisons
		/// between QueryStringDataColumns and other values
		/// </param>
        public long CountQueryStringDatasWhere(WhereDelegate<QueryStringDataColumns> where)
        {
            return Bam.Net.Logging.Http.Data.Dao.QueryStringData.Count(where, Database);
        }
        
        public async Task BatchQueryQueryStringDatas(int batchSize, WhereDelegate<QueryStringDataColumns> where, Action<IEnumerable<Bam.Net.Logging.Http.Data.QueryStringData>> batchProcessor)
        {
            await Bam.Net.Logging.Http.Data.Dao.QueryStringData.BatchQuery(batchSize, where, (batch) =>
            {
				batchProcessor(Wrap<Bam.Net.Logging.Http.Data.QueryStringData>(batch));
            }, Database);
        }
		
        public async Task BatchAllQueryStringDatas(int batchSize, Action<IEnumerable<Bam.Net.Logging.Http.Data.QueryStringData>> batchProcessor)
        {
            await Bam.Net.Logging.Http.Data.Dao.QueryStringData.BatchAll(batchSize, (batch) =>
            {
				batchProcessor(Wrap<Bam.Net.Logging.Http.Data.QueryStringData>(batch));
            }, Database);
        }

		
		/// <summary>
		/// Set one entry matching the specified filter.  If none exists 
		/// one is created; success depends on the nullability
		/// of the specified columns.
		/// </summary>
		public void SetOneRequestDataWhere(WhereDelegate<RequestDataColumns> where)
		{
			Bam.Net.Logging.Http.Data.Dao.RequestData.SetOneWhere(where, Database);
		}

		/// <summary>
		/// Set one entry matching the specified filter.  If none exists 
		/// one is created; success depends on the nullability
		/// of the specified columns.
		/// </summary>
		public void SetOneRequestDataWhere(WhereDelegate<RequestDataColumns> where, out Bam.Net.Logging.Http.Data.RequestData result)
		{
			Bam.Net.Logging.Http.Data.Dao.RequestData.SetOneWhere(where, out Bam.Net.Logging.Http.Data.Dao.RequestData daoResult, Database);
			result = daoResult.CopyAs<Bam.Net.Logging.Http.Data.RequestData>();
		}

		/// <summary>
		/// Get one entry matching the specified filter.  If none exists 
		/// one is created; success depends on the nullability
		/// of the specified columns.
		/// </summary>
		/// <param name="where"></param>
		public Bam.Net.Logging.Http.Data.RequestData GetOneRequestDataWhere(WhereDelegate<RequestDataColumns> where)
		{
			Type wrapperType = GetWrapperType<Bam.Net.Logging.Http.Data.RequestData>();
			return (Bam.Net.Logging.Http.Data.RequestData)Bam.Net.Logging.Http.Data.Dao.RequestData.GetOneWhere(where, Database)?.CopyAs(wrapperType, this);
		}

		/// <summary>
		/// Execute a query that should return only one result.  If no result is found null is returned.  If more
		/// than one result is returned a MultipleEntriesFoundException is thrown.  This method is most commonly used to retrieve a
		/// single RequestData instance by its Id/Key value
		/// </summary>
		/// <param name="where">A WhereDelegate that receives a RequestDataColumns 
		/// and returns a IQueryFilter which is the result of any comparisons
		/// between RequestDataColumns and other values
		/// </param>
		public Bam.Net.Logging.Http.Data.RequestData OneRequestDataWhere(WhereDelegate<RequestDataColumns> where)
        {
            Type wrapperType = GetWrapperType<Bam.Net.Logging.Http.Data.RequestData>();
            return (Bam.Net.Logging.Http.Data.RequestData)Bam.Net.Logging.Http.Data.Dao.RequestData.OneWhere(where, Database)?.CopyAs(wrapperType, this);
        }

		/// <summary>
		/// Execute a query and return the results. 
		/// </summary>
		/// <param name="where">A WhereDelegate that receives a Bam.Net.Logging.Http.Data.RequestDataColumns 
		/// and returns a IQueryFilter which is the result of any comparisons
		/// between Bam.Net.Logging.Http.Data.RequestDataColumns and other values
		/// </param>
		public IEnumerable<Bam.Net.Logging.Http.Data.RequestData> RequestDatasWhere(WhereDelegate<RequestDataColumns> where, OrderBy<RequestDataColumns> orderBy = null)
        {
            return Wrap<Bam.Net.Logging.Http.Data.RequestData>(Bam.Net.Logging.Http.Data.Dao.RequestData.Where(where, orderBy, Database));
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
		/// <param name="where">A WhereDelegate that receives a RequestDataColumns 
		/// and returns a IQueryFilter which is the result of any comparisons
		/// between RequestDataColumns and other values
		/// </param>
		public IEnumerable<Bam.Net.Logging.Http.Data.RequestData> TopRequestDatasWhere(int count, WhereDelegate<RequestDataColumns> where)
        {
            return Wrap<Bam.Net.Logging.Http.Data.RequestData>(Bam.Net.Logging.Http.Data.Dao.RequestData.Top(count, where, Database));
        }

        public IEnumerable<Bam.Net.Logging.Http.Data.RequestData> TopRequestDatasWhere(int count, WhereDelegate<RequestDataColumns> where, OrderBy<RequestDataColumns> orderBy)
        {
            return Wrap<Bam.Net.Logging.Http.Data.RequestData>(Bam.Net.Logging.Http.Data.Dao.RequestData.Top(count, where, orderBy, Database));
        }
                                
		/// <summary>
		/// Return the count of RequestDatas
		/// </summary>
		public long CountRequestDatas()
        {
            return Bam.Net.Logging.Http.Data.Dao.RequestData.Count(Database);
        }

		/// <summary>
		/// Execute a query and return the number of results
		/// </summary>
		/// <param name="where">A WhereDelegate that receives a RequestDataColumns 
		/// and returns a IQueryFilter which is the result of any comparisons
		/// between RequestDataColumns and other values
		/// </param>
        public long CountRequestDatasWhere(WhereDelegate<RequestDataColumns> where)
        {
            return Bam.Net.Logging.Http.Data.Dao.RequestData.Count(where, Database);
        }
        
        public async Task BatchQueryRequestDatas(int batchSize, WhereDelegate<RequestDataColumns> where, Action<IEnumerable<Bam.Net.Logging.Http.Data.RequestData>> batchProcessor)
        {
            await Bam.Net.Logging.Http.Data.Dao.RequestData.BatchQuery(batchSize, where, (batch) =>
            {
				batchProcessor(Wrap<Bam.Net.Logging.Http.Data.RequestData>(batch));
            }, Database);
        }
		
        public async Task BatchAllRequestDatas(int batchSize, Action<IEnumerable<Bam.Net.Logging.Http.Data.RequestData>> batchProcessor)
        {
            await Bam.Net.Logging.Http.Data.Dao.RequestData.BatchAll(batchSize, (batch) =>
            {
				batchProcessor(Wrap<Bam.Net.Logging.Http.Data.RequestData>(batch));
            }, Database);
        }

		
		/// <summary>
		/// Set one entry matching the specified filter.  If none exists 
		/// one is created; success depends on the nullability
		/// of the specified columns.
		/// </summary>
		public void SetOneUriDataWhere(WhereDelegate<UriDataColumns> where)
		{
			Bam.Net.Logging.Http.Data.Dao.UriData.SetOneWhere(where, Database);
		}

		/// <summary>
		/// Set one entry matching the specified filter.  If none exists 
		/// one is created; success depends on the nullability
		/// of the specified columns.
		/// </summary>
		public void SetOneUriDataWhere(WhereDelegate<UriDataColumns> where, out Bam.Net.Logging.Http.Data.UriData result)
		{
			Bam.Net.Logging.Http.Data.Dao.UriData.SetOneWhere(where, out Bam.Net.Logging.Http.Data.Dao.UriData daoResult, Database);
			result = daoResult.CopyAs<Bam.Net.Logging.Http.Data.UriData>();
		}

		/// <summary>
		/// Get one entry matching the specified filter.  If none exists 
		/// one is created; success depends on the nullability
		/// of the specified columns.
		/// </summary>
		/// <param name="where"></param>
		public Bam.Net.Logging.Http.Data.UriData GetOneUriDataWhere(WhereDelegate<UriDataColumns> where)
		{
			Type wrapperType = GetWrapperType<Bam.Net.Logging.Http.Data.UriData>();
			return (Bam.Net.Logging.Http.Data.UriData)Bam.Net.Logging.Http.Data.Dao.UriData.GetOneWhere(where, Database)?.CopyAs(wrapperType, this);
		}

		/// <summary>
		/// Execute a query that should return only one result.  If no result is found null is returned.  If more
		/// than one result is returned a MultipleEntriesFoundException is thrown.  This method is most commonly used to retrieve a
		/// single UriData instance by its Id/Key value
		/// </summary>
		/// <param name="where">A WhereDelegate that receives a UriDataColumns 
		/// and returns a IQueryFilter which is the result of any comparisons
		/// between UriDataColumns and other values
		/// </param>
		public Bam.Net.Logging.Http.Data.UriData OneUriDataWhere(WhereDelegate<UriDataColumns> where)
        {
            Type wrapperType = GetWrapperType<Bam.Net.Logging.Http.Data.UriData>();
            return (Bam.Net.Logging.Http.Data.UriData)Bam.Net.Logging.Http.Data.Dao.UriData.OneWhere(where, Database)?.CopyAs(wrapperType, this);
        }

		/// <summary>
		/// Execute a query and return the results. 
		/// </summary>
		/// <param name="where">A WhereDelegate that receives a Bam.Net.Logging.Http.Data.UriDataColumns 
		/// and returns a IQueryFilter which is the result of any comparisons
		/// between Bam.Net.Logging.Http.Data.UriDataColumns and other values
		/// </param>
		public IEnumerable<Bam.Net.Logging.Http.Data.UriData> UriDatasWhere(WhereDelegate<UriDataColumns> where, OrderBy<UriDataColumns> orderBy = null)
        {
            return Wrap<Bam.Net.Logging.Http.Data.UriData>(Bam.Net.Logging.Http.Data.Dao.UriData.Where(where, orderBy, Database));
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
		/// <param name="where">A WhereDelegate that receives a UriDataColumns 
		/// and returns a IQueryFilter which is the result of any comparisons
		/// between UriDataColumns and other values
		/// </param>
		public IEnumerable<Bam.Net.Logging.Http.Data.UriData> TopUriDatasWhere(int count, WhereDelegate<UriDataColumns> where)
        {
            return Wrap<Bam.Net.Logging.Http.Data.UriData>(Bam.Net.Logging.Http.Data.Dao.UriData.Top(count, where, Database));
        }

        public IEnumerable<Bam.Net.Logging.Http.Data.UriData> TopUriDatasWhere(int count, WhereDelegate<UriDataColumns> where, OrderBy<UriDataColumns> orderBy)
        {
            return Wrap<Bam.Net.Logging.Http.Data.UriData>(Bam.Net.Logging.Http.Data.Dao.UriData.Top(count, where, orderBy, Database));
        }
                                
		/// <summary>
		/// Return the count of UriDatas
		/// </summary>
		public long CountUriDatas()
        {
            return Bam.Net.Logging.Http.Data.Dao.UriData.Count(Database);
        }

		/// <summary>
		/// Execute a query and return the number of results
		/// </summary>
		/// <param name="where">A WhereDelegate that receives a UriDataColumns 
		/// and returns a IQueryFilter which is the result of any comparisons
		/// between UriDataColumns and other values
		/// </param>
        public long CountUriDatasWhere(WhereDelegate<UriDataColumns> where)
        {
            return Bam.Net.Logging.Http.Data.Dao.UriData.Count(where, Database);
        }
        
        public async Task BatchQueryUriDatas(int batchSize, WhereDelegate<UriDataColumns> where, Action<IEnumerable<Bam.Net.Logging.Http.Data.UriData>> batchProcessor)
        {
            await Bam.Net.Logging.Http.Data.Dao.UriData.BatchQuery(batchSize, where, (batch) =>
            {
				batchProcessor(Wrap<Bam.Net.Logging.Http.Data.UriData>(batch));
            }, Database);
        }
		
        public async Task BatchAllUriDatas(int batchSize, Action<IEnumerable<Bam.Net.Logging.Http.Data.UriData>> batchProcessor)
        {
            await Bam.Net.Logging.Http.Data.Dao.UriData.BatchAll(batchSize, (batch) =>
            {
				batchProcessor(Wrap<Bam.Net.Logging.Http.Data.UriData>(batch));
            }, Database);
        }

		
		/// <summary>
		/// Set one entry matching the specified filter.  If none exists 
		/// one is created; success depends on the nullability
		/// of the specified columns.
		/// </summary>
		public void SetOneUserDataWhere(WhereDelegate<UserDataColumns> where)
		{
			Bam.Net.Logging.Http.Data.Dao.UserData.SetOneWhere(where, Database);
		}

		/// <summary>
		/// Set one entry matching the specified filter.  If none exists 
		/// one is created; success depends on the nullability
		/// of the specified columns.
		/// </summary>
		public void SetOneUserDataWhere(WhereDelegate<UserDataColumns> where, out Bam.Net.Logging.Http.Data.UserData result)
		{
			Bam.Net.Logging.Http.Data.Dao.UserData.SetOneWhere(where, out Bam.Net.Logging.Http.Data.Dao.UserData daoResult, Database);
			result = daoResult.CopyAs<Bam.Net.Logging.Http.Data.UserData>();
		}

		/// <summary>
		/// Get one entry matching the specified filter.  If none exists 
		/// one is created; success depends on the nullability
		/// of the specified columns.
		/// </summary>
		/// <param name="where"></param>
		public Bam.Net.Logging.Http.Data.UserData GetOneUserDataWhere(WhereDelegate<UserDataColumns> where)
		{
			Type wrapperType = GetWrapperType<Bam.Net.Logging.Http.Data.UserData>();
			return (Bam.Net.Logging.Http.Data.UserData)Bam.Net.Logging.Http.Data.Dao.UserData.GetOneWhere(where, Database)?.CopyAs(wrapperType, this);
		}

		/// <summary>
		/// Execute a query that should return only one result.  If no result is found null is returned.  If more
		/// than one result is returned a MultipleEntriesFoundException is thrown.  This method is most commonly used to retrieve a
		/// single UserData instance by its Id/Key value
		/// </summary>
		/// <param name="where">A WhereDelegate that receives a UserDataColumns 
		/// and returns a IQueryFilter which is the result of any comparisons
		/// between UserDataColumns and other values
		/// </param>
		public Bam.Net.Logging.Http.Data.UserData OneUserDataWhere(WhereDelegate<UserDataColumns> where)
        {
            Type wrapperType = GetWrapperType<Bam.Net.Logging.Http.Data.UserData>();
            return (Bam.Net.Logging.Http.Data.UserData)Bam.Net.Logging.Http.Data.Dao.UserData.OneWhere(where, Database)?.CopyAs(wrapperType, this);
        }

		/// <summary>
		/// Execute a query and return the results. 
		/// </summary>
		/// <param name="where">A WhereDelegate that receives a Bam.Net.Logging.Http.Data.UserDataColumns 
		/// and returns a IQueryFilter which is the result of any comparisons
		/// between Bam.Net.Logging.Http.Data.UserDataColumns and other values
		/// </param>
		public IEnumerable<Bam.Net.Logging.Http.Data.UserData> UserDatasWhere(WhereDelegate<UserDataColumns> where, OrderBy<UserDataColumns> orderBy = null)
        {
            return Wrap<Bam.Net.Logging.Http.Data.UserData>(Bam.Net.Logging.Http.Data.Dao.UserData.Where(where, orderBy, Database));
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
		/// <param name="where">A WhereDelegate that receives a UserDataColumns 
		/// and returns a IQueryFilter which is the result of any comparisons
		/// between UserDataColumns and other values
		/// </param>
		public IEnumerable<Bam.Net.Logging.Http.Data.UserData> TopUserDatasWhere(int count, WhereDelegate<UserDataColumns> where)
        {
            return Wrap<Bam.Net.Logging.Http.Data.UserData>(Bam.Net.Logging.Http.Data.Dao.UserData.Top(count, where, Database));
        }

        public IEnumerable<Bam.Net.Logging.Http.Data.UserData> TopUserDatasWhere(int count, WhereDelegate<UserDataColumns> where, OrderBy<UserDataColumns> orderBy)
        {
            return Wrap<Bam.Net.Logging.Http.Data.UserData>(Bam.Net.Logging.Http.Data.Dao.UserData.Top(count, where, orderBy, Database));
        }
                                
		/// <summary>
		/// Return the count of UserDatas
		/// </summary>
		public long CountUserDatas()
        {
            return Bam.Net.Logging.Http.Data.Dao.UserData.Count(Database);
        }

		/// <summary>
		/// Execute a query and return the number of results
		/// </summary>
		/// <param name="where">A WhereDelegate that receives a UserDataColumns 
		/// and returns a IQueryFilter which is the result of any comparisons
		/// between UserDataColumns and other values
		/// </param>
        public long CountUserDatasWhere(WhereDelegate<UserDataColumns> where)
        {
            return Bam.Net.Logging.Http.Data.Dao.UserData.Count(where, Database);
        }
        
        public async Task BatchQueryUserDatas(int batchSize, WhereDelegate<UserDataColumns> where, Action<IEnumerable<Bam.Net.Logging.Http.Data.UserData>> batchProcessor)
        {
            await Bam.Net.Logging.Http.Data.Dao.UserData.BatchQuery(batchSize, where, (batch) =>
            {
				batchProcessor(Wrap<Bam.Net.Logging.Http.Data.UserData>(batch));
            }, Database);
        }
		
        public async Task BatchAllUserDatas(int batchSize, Action<IEnumerable<Bam.Net.Logging.Http.Data.UserData>> batchProcessor)
        {
            await Bam.Net.Logging.Http.Data.Dao.UserData.BatchAll(batchSize, (batch) =>
            {
				batchProcessor(Wrap<Bam.Net.Logging.Http.Data.UserData>(batch));
            }, Database);
        }

		
		/// <summary>
		/// Set one entry matching the specified filter.  If none exists 
		/// one is created; success depends on the nullability
		/// of the specified columns.
		/// </summary>
		public void SetOneUserKeyDataWhere(WhereDelegate<UserKeyDataColumns> where)
		{
			Bam.Net.Logging.Http.Data.Dao.UserKeyData.SetOneWhere(where, Database);
		}

		/// <summary>
		/// Set one entry matching the specified filter.  If none exists 
		/// one is created; success depends on the nullability
		/// of the specified columns.
		/// </summary>
		public void SetOneUserKeyDataWhere(WhereDelegate<UserKeyDataColumns> where, out Bam.Net.Logging.Http.Data.UserKeyData result)
		{
			Bam.Net.Logging.Http.Data.Dao.UserKeyData.SetOneWhere(where, out Bam.Net.Logging.Http.Data.Dao.UserKeyData daoResult, Database);
			result = daoResult.CopyAs<Bam.Net.Logging.Http.Data.UserKeyData>();
		}

		/// <summary>
		/// Get one entry matching the specified filter.  If none exists 
		/// one is created; success depends on the nullability
		/// of the specified columns.
		/// </summary>
		/// <param name="where"></param>
		public Bam.Net.Logging.Http.Data.UserKeyData GetOneUserKeyDataWhere(WhereDelegate<UserKeyDataColumns> where)
		{
			Type wrapperType = GetWrapperType<Bam.Net.Logging.Http.Data.UserKeyData>();
			return (Bam.Net.Logging.Http.Data.UserKeyData)Bam.Net.Logging.Http.Data.Dao.UserKeyData.GetOneWhere(where, Database)?.CopyAs(wrapperType, this);
		}

		/// <summary>
		/// Execute a query that should return only one result.  If no result is found null is returned.  If more
		/// than one result is returned a MultipleEntriesFoundException is thrown.  This method is most commonly used to retrieve a
		/// single UserKeyData instance by its Id/Key value
		/// </summary>
		/// <param name="where">A WhereDelegate that receives a UserKeyDataColumns 
		/// and returns a IQueryFilter which is the result of any comparisons
		/// between UserKeyDataColumns and other values
		/// </param>
		public Bam.Net.Logging.Http.Data.UserKeyData OneUserKeyDataWhere(WhereDelegate<UserKeyDataColumns> where)
        {
            Type wrapperType = GetWrapperType<Bam.Net.Logging.Http.Data.UserKeyData>();
            return (Bam.Net.Logging.Http.Data.UserKeyData)Bam.Net.Logging.Http.Data.Dao.UserKeyData.OneWhere(where, Database)?.CopyAs(wrapperType, this);
        }

		/// <summary>
		/// Execute a query and return the results. 
		/// </summary>
		/// <param name="where">A WhereDelegate that receives a Bam.Net.Logging.Http.Data.UserKeyDataColumns 
		/// and returns a IQueryFilter which is the result of any comparisons
		/// between Bam.Net.Logging.Http.Data.UserKeyDataColumns and other values
		/// </param>
		public IEnumerable<Bam.Net.Logging.Http.Data.UserKeyData> UserKeyDatasWhere(WhereDelegate<UserKeyDataColumns> where, OrderBy<UserKeyDataColumns> orderBy = null)
        {
            return Wrap<Bam.Net.Logging.Http.Data.UserKeyData>(Bam.Net.Logging.Http.Data.Dao.UserKeyData.Where(where, orderBy, Database));
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
		/// <param name="where">A WhereDelegate that receives a UserKeyDataColumns 
		/// and returns a IQueryFilter which is the result of any comparisons
		/// between UserKeyDataColumns and other values
		/// </param>
		public IEnumerable<Bam.Net.Logging.Http.Data.UserKeyData> TopUserKeyDatasWhere(int count, WhereDelegate<UserKeyDataColumns> where)
        {
            return Wrap<Bam.Net.Logging.Http.Data.UserKeyData>(Bam.Net.Logging.Http.Data.Dao.UserKeyData.Top(count, where, Database));
        }

        public IEnumerable<Bam.Net.Logging.Http.Data.UserKeyData> TopUserKeyDatasWhere(int count, WhereDelegate<UserKeyDataColumns> where, OrderBy<UserKeyDataColumns> orderBy)
        {
            return Wrap<Bam.Net.Logging.Http.Data.UserKeyData>(Bam.Net.Logging.Http.Data.Dao.UserKeyData.Top(count, where, orderBy, Database));
        }
                                
		/// <summary>
		/// Return the count of UserKeyDatas
		/// </summary>
		public long CountUserKeyDatas()
        {
            return Bam.Net.Logging.Http.Data.Dao.UserKeyData.Count(Database);
        }

		/// <summary>
		/// Execute a query and return the number of results
		/// </summary>
		/// <param name="where">A WhereDelegate that receives a UserKeyDataColumns 
		/// and returns a IQueryFilter which is the result of any comparisons
		/// between UserKeyDataColumns and other values
		/// </param>
        public long CountUserKeyDatasWhere(WhereDelegate<UserKeyDataColumns> where)
        {
            return Bam.Net.Logging.Http.Data.Dao.UserKeyData.Count(where, Database);
        }
        
        public async Task BatchQueryUserKeyDatas(int batchSize, WhereDelegate<UserKeyDataColumns> where, Action<IEnumerable<Bam.Net.Logging.Http.Data.UserKeyData>> batchProcessor)
        {
            await Bam.Net.Logging.Http.Data.Dao.UserKeyData.BatchQuery(batchSize, where, (batch) =>
            {
				batchProcessor(Wrap<Bam.Net.Logging.Http.Data.UserKeyData>(batch));
            }, Database);
        }
		
        public async Task BatchAllUserKeyDatas(int batchSize, Action<IEnumerable<Bam.Net.Logging.Http.Data.UserKeyData>> batchProcessor)
        {
            await Bam.Net.Logging.Http.Data.Dao.UserKeyData.BatchAll(batchSize, (batch) =>
            {
				batchProcessor(Wrap<Bam.Net.Logging.Http.Data.UserKeyData>(batch));
            }, Database);
        }


	}
}																								
