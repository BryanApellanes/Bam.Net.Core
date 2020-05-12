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
using Bam.Net.CoreServices.AssemblyManagement.Data;

namespace Bam.Net.CoreServices.AssemblyManagement.Data.Dao.Repository
{
	[Serializable]
	public class AssemblyManagementRepository: DaoInheritanceRepository
	{
		public AssemblyManagementRepository()
		{
			SchemaName = "AssemblyManagement";
			BaseNamespace = "Bam.Net.CoreServices.AssemblyManagement.Data";			

			
			AddType<Bam.Net.CoreServices.AssemblyManagement.Data.AssemblyQualifiedTypeDescriptor>();
			
			
			AddType<Bam.Net.CoreServices.AssemblyManagement.Data.AssemblyRequest>();
			
			
			AddType<Bam.Net.CoreServices.AssemblyManagement.Data.PropertyDescriptor>();
			
			
			AddType<Bam.Net.CoreServices.AssemblyManagement.Data.TypeDescriptor>();
			
			
			AddType<Bam.Net.CoreServices.AssemblyManagement.Data.AssemblyDescriptor>();
			
			
			AddType<Bam.Net.CoreServices.AssemblyManagement.Data.AssemblyReferenceDescriptor>();
			
			
			AddType<Bam.Net.CoreServices.AssemblyManagement.Data.AssemblyRevision>();
			
			
			AddType<Bam.Net.CoreServices.AssemblyManagement.Data.ProcessRuntimeDescriptor>();
			

			DaoAssembly = typeof(AssemblyManagementRepository).Assembly;
		}

		object _addLock = new object();
        public override void AddType(Type type)
        {
            lock (_addLock)
            {
                base.AddType(type);
                DaoAssembly = typeof(AssemblyManagementRepository).Assembly;
            }
        }

		
		/// <summary>
		/// Set one entry matching the specified filter.  If none exists 
		/// one is created; success depends on the nullability
		/// of the specified columns.
		/// </summary>
		public void SetOneAssemblyQualifiedTypeDescriptorWhere(WhereDelegate<AssemblyQualifiedTypeDescriptorColumns> where)
		{
			Bam.Net.CoreServices.AssemblyManagement.Data.Dao.AssemblyQualifiedTypeDescriptor.SetOneWhere(where, Database);
		}

		/// <summary>
		/// Set one entry matching the specified filter.  If none exists 
		/// one is created; success depends on the nullability
		/// of the specified columns.
		/// </summary>
		public void SetOneAssemblyQualifiedTypeDescriptorWhere(WhereDelegate<AssemblyQualifiedTypeDescriptorColumns> where, out Bam.Net.CoreServices.AssemblyManagement.Data.AssemblyQualifiedTypeDescriptor result)
		{
			Bam.Net.CoreServices.AssemblyManagement.Data.Dao.AssemblyQualifiedTypeDescriptor.SetOneWhere(where, out Bam.Net.CoreServices.AssemblyManagement.Data.Dao.AssemblyQualifiedTypeDescriptor daoResult, Database);
			result = daoResult.CopyAs<Bam.Net.CoreServices.AssemblyManagement.Data.AssemblyQualifiedTypeDescriptor>();
		}

		/// <summary>
		/// Get one entry matching the specified filter.  If none exists 
		/// one is created; success depends on the nullability
		/// of the specified columns.
		/// </summary>
		/// <param name="where"></param>
		public Bam.Net.CoreServices.AssemblyManagement.Data.AssemblyQualifiedTypeDescriptor GetOneAssemblyQualifiedTypeDescriptorWhere(WhereDelegate<AssemblyQualifiedTypeDescriptorColumns> where)
		{
			Type wrapperType = GetWrapperType<Bam.Net.CoreServices.AssemblyManagement.Data.AssemblyQualifiedTypeDescriptor>();
			return (Bam.Net.CoreServices.AssemblyManagement.Data.AssemblyQualifiedTypeDescriptor)Bam.Net.CoreServices.AssemblyManagement.Data.Dao.AssemblyQualifiedTypeDescriptor.GetOneWhere(where, Database)?.CopyAs(wrapperType, this);
		}

		/// <summary>
		/// Execute a query that should return only one result.  If no result is found null is returned.  If more
		/// than one result is returned a MultipleEntriesFoundException is thrown.  This method is most commonly used to retrieve a
		/// single AssemblyQualifiedTypeDescriptor instance by its Id/Key value
		/// </summary>
		/// <param name="where">A WhereDelegate that receives a AssemblyQualifiedTypeDescriptorColumns 
		/// and returns a IQueryFilter which is the result of any comparisons
		/// between AssemblyQualifiedTypeDescriptorColumns and other values
		/// </param>
		public Bam.Net.CoreServices.AssemblyManagement.Data.AssemblyQualifiedTypeDescriptor OneAssemblyQualifiedTypeDescriptorWhere(WhereDelegate<AssemblyQualifiedTypeDescriptorColumns> where)
        {
            Type wrapperType = GetWrapperType<Bam.Net.CoreServices.AssemblyManagement.Data.AssemblyQualifiedTypeDescriptor>();
            return (Bam.Net.CoreServices.AssemblyManagement.Data.AssemblyQualifiedTypeDescriptor)Bam.Net.CoreServices.AssemblyManagement.Data.Dao.AssemblyQualifiedTypeDescriptor.OneWhere(where, Database)?.CopyAs(wrapperType, this);
        }

		/// <summary>
		/// Execute a query and return the results. 
		/// </summary>
		/// <param name="where">A WhereDelegate that receives a Bam.Net.CoreServices.AssemblyManagement.Data.AssemblyQualifiedTypeDescriptorColumns 
		/// and returns a IQueryFilter which is the result of any comparisons
		/// between Bam.Net.CoreServices.AssemblyManagement.Data.AssemblyQualifiedTypeDescriptorColumns and other values
		/// </param>
		public IEnumerable<Bam.Net.CoreServices.AssemblyManagement.Data.AssemblyQualifiedTypeDescriptor> AssemblyQualifiedTypeDescriptorsWhere(WhereDelegate<AssemblyQualifiedTypeDescriptorColumns> where, OrderBy<AssemblyQualifiedTypeDescriptorColumns> orderBy = null)
        {
            return Wrap<Bam.Net.CoreServices.AssemblyManagement.Data.AssemblyQualifiedTypeDescriptor>(Bam.Net.CoreServices.AssemblyManagement.Data.Dao.AssemblyQualifiedTypeDescriptor.Where(where, orderBy, Database));
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
		/// <param name="where">A WhereDelegate that receives a AssemblyQualifiedTypeDescriptorColumns 
		/// and returns a IQueryFilter which is the result of any comparisons
		/// between AssemblyQualifiedTypeDescriptorColumns and other values
		/// </param>
		public IEnumerable<Bam.Net.CoreServices.AssemblyManagement.Data.AssemblyQualifiedTypeDescriptor> TopAssemblyQualifiedTypeDescriptorsWhere(int count, WhereDelegate<AssemblyQualifiedTypeDescriptorColumns> where)
        {
            return Wrap<Bam.Net.CoreServices.AssemblyManagement.Data.AssemblyQualifiedTypeDescriptor>(Bam.Net.CoreServices.AssemblyManagement.Data.Dao.AssemblyQualifiedTypeDescriptor.Top(count, where, Database));
        }

        public IEnumerable<Bam.Net.CoreServices.AssemblyManagement.Data.AssemblyQualifiedTypeDescriptor> TopAssemblyQualifiedTypeDescriptorsWhere(int count, WhereDelegate<AssemblyQualifiedTypeDescriptorColumns> where, OrderBy<AssemblyQualifiedTypeDescriptorColumns> orderBy)
        {
            return Wrap<Bam.Net.CoreServices.AssemblyManagement.Data.AssemblyQualifiedTypeDescriptor>(Bam.Net.CoreServices.AssemblyManagement.Data.Dao.AssemblyQualifiedTypeDescriptor.Top(count, where, orderBy, Database));
        }
                                
		/// <summary>
		/// Return the count of AssemblyQualifiedTypeDescriptors
		/// </summary>
		public long CountAssemblyQualifiedTypeDescriptors()
        {
            return Bam.Net.CoreServices.AssemblyManagement.Data.Dao.AssemblyQualifiedTypeDescriptor.Count(Database);
        }

		/// <summary>
		/// Execute a query and return the number of results
		/// </summary>
		/// <param name="where">A WhereDelegate that receives a AssemblyQualifiedTypeDescriptorColumns 
		/// and returns a IQueryFilter which is the result of any comparisons
		/// between AssemblyQualifiedTypeDescriptorColumns and other values
		/// </param>
        public long CountAssemblyQualifiedTypeDescriptorsWhere(WhereDelegate<AssemblyQualifiedTypeDescriptorColumns> where)
        {
            return Bam.Net.CoreServices.AssemblyManagement.Data.Dao.AssemblyQualifiedTypeDescriptor.Count(where, Database);
        }
        
        public async Task BatchQueryAssemblyQualifiedTypeDescriptors(int batchSize, WhereDelegate<AssemblyQualifiedTypeDescriptorColumns> where, Action<IEnumerable<Bam.Net.CoreServices.AssemblyManagement.Data.AssemblyQualifiedTypeDescriptor>> batchProcessor)
        {
            await Bam.Net.CoreServices.AssemblyManagement.Data.Dao.AssemblyQualifiedTypeDescriptor.BatchQuery(batchSize, where, (batch) =>
            {
				batchProcessor(Wrap<Bam.Net.CoreServices.AssemblyManagement.Data.AssemblyQualifiedTypeDescriptor>(batch));
            }, Database);
        }
		
        public async Task BatchAllAssemblyQualifiedTypeDescriptors(int batchSize, Action<IEnumerable<Bam.Net.CoreServices.AssemblyManagement.Data.AssemblyQualifiedTypeDescriptor>> batchProcessor)
        {
            await Bam.Net.CoreServices.AssemblyManagement.Data.Dao.AssemblyQualifiedTypeDescriptor.BatchAll(batchSize, (batch) =>
            {
				batchProcessor(Wrap<Bam.Net.CoreServices.AssemblyManagement.Data.AssemblyQualifiedTypeDescriptor>(batch));
            }, Database);
        }

		
		/// <summary>
		/// Set one entry matching the specified filter.  If none exists 
		/// one is created; success depends on the nullability
		/// of the specified columns.
		/// </summary>
		public void SetOneAssemblyRequestWhere(WhereDelegate<AssemblyRequestColumns> where)
		{
			Bam.Net.CoreServices.AssemblyManagement.Data.Dao.AssemblyRequest.SetOneWhere(where, Database);
		}

		/// <summary>
		/// Set one entry matching the specified filter.  If none exists 
		/// one is created; success depends on the nullability
		/// of the specified columns.
		/// </summary>
		public void SetOneAssemblyRequestWhere(WhereDelegate<AssemblyRequestColumns> where, out Bam.Net.CoreServices.AssemblyManagement.Data.AssemblyRequest result)
		{
			Bam.Net.CoreServices.AssemblyManagement.Data.Dao.AssemblyRequest.SetOneWhere(where, out Bam.Net.CoreServices.AssemblyManagement.Data.Dao.AssemblyRequest daoResult, Database);
			result = daoResult.CopyAs<Bam.Net.CoreServices.AssemblyManagement.Data.AssemblyRequest>();
		}

		/// <summary>
		/// Get one entry matching the specified filter.  If none exists 
		/// one is created; success depends on the nullability
		/// of the specified columns.
		/// </summary>
		/// <param name="where"></param>
		public Bam.Net.CoreServices.AssemblyManagement.Data.AssemblyRequest GetOneAssemblyRequestWhere(WhereDelegate<AssemblyRequestColumns> where)
		{
			Type wrapperType = GetWrapperType<Bam.Net.CoreServices.AssemblyManagement.Data.AssemblyRequest>();
			return (Bam.Net.CoreServices.AssemblyManagement.Data.AssemblyRequest)Bam.Net.CoreServices.AssemblyManagement.Data.Dao.AssemblyRequest.GetOneWhere(where, Database)?.CopyAs(wrapperType, this);
		}

		/// <summary>
		/// Execute a query that should return only one result.  If no result is found null is returned.  If more
		/// than one result is returned a MultipleEntriesFoundException is thrown.  This method is most commonly used to retrieve a
		/// single AssemblyRequest instance by its Id/Key value
		/// </summary>
		/// <param name="where">A WhereDelegate that receives a AssemblyRequestColumns 
		/// and returns a IQueryFilter which is the result of any comparisons
		/// between AssemblyRequestColumns and other values
		/// </param>
		public Bam.Net.CoreServices.AssemblyManagement.Data.AssemblyRequest OneAssemblyRequestWhere(WhereDelegate<AssemblyRequestColumns> where)
        {
            Type wrapperType = GetWrapperType<Bam.Net.CoreServices.AssemblyManagement.Data.AssemblyRequest>();
            return (Bam.Net.CoreServices.AssemblyManagement.Data.AssemblyRequest)Bam.Net.CoreServices.AssemblyManagement.Data.Dao.AssemblyRequest.OneWhere(where, Database)?.CopyAs(wrapperType, this);
        }

		/// <summary>
		/// Execute a query and return the results. 
		/// </summary>
		/// <param name="where">A WhereDelegate that receives a Bam.Net.CoreServices.AssemblyManagement.Data.AssemblyRequestColumns 
		/// and returns a IQueryFilter which is the result of any comparisons
		/// between Bam.Net.CoreServices.AssemblyManagement.Data.AssemblyRequestColumns and other values
		/// </param>
		public IEnumerable<Bam.Net.CoreServices.AssemblyManagement.Data.AssemblyRequest> AssemblyRequestsWhere(WhereDelegate<AssemblyRequestColumns> where, OrderBy<AssemblyRequestColumns> orderBy = null)
        {
            return Wrap<Bam.Net.CoreServices.AssemblyManagement.Data.AssemblyRequest>(Bam.Net.CoreServices.AssemblyManagement.Data.Dao.AssemblyRequest.Where(where, orderBy, Database));
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
		/// <param name="where">A WhereDelegate that receives a AssemblyRequestColumns 
		/// and returns a IQueryFilter which is the result of any comparisons
		/// between AssemblyRequestColumns and other values
		/// </param>
		public IEnumerable<Bam.Net.CoreServices.AssemblyManagement.Data.AssemblyRequest> TopAssemblyRequestsWhere(int count, WhereDelegate<AssemblyRequestColumns> where)
        {
            return Wrap<Bam.Net.CoreServices.AssemblyManagement.Data.AssemblyRequest>(Bam.Net.CoreServices.AssemblyManagement.Data.Dao.AssemblyRequest.Top(count, where, Database));
        }

        public IEnumerable<Bam.Net.CoreServices.AssemblyManagement.Data.AssemblyRequest> TopAssemblyRequestsWhere(int count, WhereDelegate<AssemblyRequestColumns> where, OrderBy<AssemblyRequestColumns> orderBy)
        {
            return Wrap<Bam.Net.CoreServices.AssemblyManagement.Data.AssemblyRequest>(Bam.Net.CoreServices.AssemblyManagement.Data.Dao.AssemblyRequest.Top(count, where, orderBy, Database));
        }
                                
		/// <summary>
		/// Return the count of AssemblyRequests
		/// </summary>
		public long CountAssemblyRequests()
        {
            return Bam.Net.CoreServices.AssemblyManagement.Data.Dao.AssemblyRequest.Count(Database);
        }

		/// <summary>
		/// Execute a query and return the number of results
		/// </summary>
		/// <param name="where">A WhereDelegate that receives a AssemblyRequestColumns 
		/// and returns a IQueryFilter which is the result of any comparisons
		/// between AssemblyRequestColumns and other values
		/// </param>
        public long CountAssemblyRequestsWhere(WhereDelegate<AssemblyRequestColumns> where)
        {
            return Bam.Net.CoreServices.AssemblyManagement.Data.Dao.AssemblyRequest.Count(where, Database);
        }
        
        public async Task BatchQueryAssemblyRequests(int batchSize, WhereDelegate<AssemblyRequestColumns> where, Action<IEnumerable<Bam.Net.CoreServices.AssemblyManagement.Data.AssemblyRequest>> batchProcessor)
        {
            await Bam.Net.CoreServices.AssemblyManagement.Data.Dao.AssemblyRequest.BatchQuery(batchSize, where, (batch) =>
            {
				batchProcessor(Wrap<Bam.Net.CoreServices.AssemblyManagement.Data.AssemblyRequest>(batch));
            }, Database);
        }
		
        public async Task BatchAllAssemblyRequests(int batchSize, Action<IEnumerable<Bam.Net.CoreServices.AssemblyManagement.Data.AssemblyRequest>> batchProcessor)
        {
            await Bam.Net.CoreServices.AssemblyManagement.Data.Dao.AssemblyRequest.BatchAll(batchSize, (batch) =>
            {
				batchProcessor(Wrap<Bam.Net.CoreServices.AssemblyManagement.Data.AssemblyRequest>(batch));
            }, Database);
        }

		
		/// <summary>
		/// Set one entry matching the specified filter.  If none exists 
		/// one is created; success depends on the nullability
		/// of the specified columns.
		/// </summary>
		public void SetOnePropertyDescriptorWhere(WhereDelegate<PropertyDescriptorColumns> where)
		{
			Bam.Net.CoreServices.AssemblyManagement.Data.Dao.PropertyDescriptor.SetOneWhere(where, Database);
		}

		/// <summary>
		/// Set one entry matching the specified filter.  If none exists 
		/// one is created; success depends on the nullability
		/// of the specified columns.
		/// </summary>
		public void SetOnePropertyDescriptorWhere(WhereDelegate<PropertyDescriptorColumns> where, out Bam.Net.CoreServices.AssemblyManagement.Data.PropertyDescriptor result)
		{
			Bam.Net.CoreServices.AssemblyManagement.Data.Dao.PropertyDescriptor.SetOneWhere(where, out Bam.Net.CoreServices.AssemblyManagement.Data.Dao.PropertyDescriptor daoResult, Database);
			result = daoResult.CopyAs<Bam.Net.CoreServices.AssemblyManagement.Data.PropertyDescriptor>();
		}

		/// <summary>
		/// Get one entry matching the specified filter.  If none exists 
		/// one is created; success depends on the nullability
		/// of the specified columns.
		/// </summary>
		/// <param name="where"></param>
		public Bam.Net.CoreServices.AssemblyManagement.Data.PropertyDescriptor GetOnePropertyDescriptorWhere(WhereDelegate<PropertyDescriptorColumns> where)
		{
			Type wrapperType = GetWrapperType<Bam.Net.CoreServices.AssemblyManagement.Data.PropertyDescriptor>();
			return (Bam.Net.CoreServices.AssemblyManagement.Data.PropertyDescriptor)Bam.Net.CoreServices.AssemblyManagement.Data.Dao.PropertyDescriptor.GetOneWhere(where, Database)?.CopyAs(wrapperType, this);
		}

		/// <summary>
		/// Execute a query that should return only one result.  If no result is found null is returned.  If more
		/// than one result is returned a MultipleEntriesFoundException is thrown.  This method is most commonly used to retrieve a
		/// single PropertyDescriptor instance by its Id/Key value
		/// </summary>
		/// <param name="where">A WhereDelegate that receives a PropertyDescriptorColumns 
		/// and returns a IQueryFilter which is the result of any comparisons
		/// between PropertyDescriptorColumns and other values
		/// </param>
		public Bam.Net.CoreServices.AssemblyManagement.Data.PropertyDescriptor OnePropertyDescriptorWhere(WhereDelegate<PropertyDescriptorColumns> where)
        {
            Type wrapperType = GetWrapperType<Bam.Net.CoreServices.AssemblyManagement.Data.PropertyDescriptor>();
            return (Bam.Net.CoreServices.AssemblyManagement.Data.PropertyDescriptor)Bam.Net.CoreServices.AssemblyManagement.Data.Dao.PropertyDescriptor.OneWhere(where, Database)?.CopyAs(wrapperType, this);
        }

		/// <summary>
		/// Execute a query and return the results. 
		/// </summary>
		/// <param name="where">A WhereDelegate that receives a Bam.Net.CoreServices.AssemblyManagement.Data.PropertyDescriptorColumns 
		/// and returns a IQueryFilter which is the result of any comparisons
		/// between Bam.Net.CoreServices.AssemblyManagement.Data.PropertyDescriptorColumns and other values
		/// </param>
		public IEnumerable<Bam.Net.CoreServices.AssemblyManagement.Data.PropertyDescriptor> PropertyDescriptorsWhere(WhereDelegate<PropertyDescriptorColumns> where, OrderBy<PropertyDescriptorColumns> orderBy = null)
        {
            return Wrap<Bam.Net.CoreServices.AssemblyManagement.Data.PropertyDescriptor>(Bam.Net.CoreServices.AssemblyManagement.Data.Dao.PropertyDescriptor.Where(where, orderBy, Database));
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
		/// <param name="where">A WhereDelegate that receives a PropertyDescriptorColumns 
		/// and returns a IQueryFilter which is the result of any comparisons
		/// between PropertyDescriptorColumns and other values
		/// </param>
		public IEnumerable<Bam.Net.CoreServices.AssemblyManagement.Data.PropertyDescriptor> TopPropertyDescriptorsWhere(int count, WhereDelegate<PropertyDescriptorColumns> where)
        {
            return Wrap<Bam.Net.CoreServices.AssemblyManagement.Data.PropertyDescriptor>(Bam.Net.CoreServices.AssemblyManagement.Data.Dao.PropertyDescriptor.Top(count, where, Database));
        }

        public IEnumerable<Bam.Net.CoreServices.AssemblyManagement.Data.PropertyDescriptor> TopPropertyDescriptorsWhere(int count, WhereDelegate<PropertyDescriptorColumns> where, OrderBy<PropertyDescriptorColumns> orderBy)
        {
            return Wrap<Bam.Net.CoreServices.AssemblyManagement.Data.PropertyDescriptor>(Bam.Net.CoreServices.AssemblyManagement.Data.Dao.PropertyDescriptor.Top(count, where, orderBy, Database));
        }
                                
		/// <summary>
		/// Return the count of PropertyDescriptors
		/// </summary>
		public long CountPropertyDescriptors()
        {
            return Bam.Net.CoreServices.AssemblyManagement.Data.Dao.PropertyDescriptor.Count(Database);
        }

		/// <summary>
		/// Execute a query and return the number of results
		/// </summary>
		/// <param name="where">A WhereDelegate that receives a PropertyDescriptorColumns 
		/// and returns a IQueryFilter which is the result of any comparisons
		/// between PropertyDescriptorColumns and other values
		/// </param>
        public long CountPropertyDescriptorsWhere(WhereDelegate<PropertyDescriptorColumns> where)
        {
            return Bam.Net.CoreServices.AssemblyManagement.Data.Dao.PropertyDescriptor.Count(where, Database);
        }
        
        public async Task BatchQueryPropertyDescriptors(int batchSize, WhereDelegate<PropertyDescriptorColumns> where, Action<IEnumerable<Bam.Net.CoreServices.AssemblyManagement.Data.PropertyDescriptor>> batchProcessor)
        {
            await Bam.Net.CoreServices.AssemblyManagement.Data.Dao.PropertyDescriptor.BatchQuery(batchSize, where, (batch) =>
            {
				batchProcessor(Wrap<Bam.Net.CoreServices.AssemblyManagement.Data.PropertyDescriptor>(batch));
            }, Database);
        }
		
        public async Task BatchAllPropertyDescriptors(int batchSize, Action<IEnumerable<Bam.Net.CoreServices.AssemblyManagement.Data.PropertyDescriptor>> batchProcessor)
        {
            await Bam.Net.CoreServices.AssemblyManagement.Data.Dao.PropertyDescriptor.BatchAll(batchSize, (batch) =>
            {
				batchProcessor(Wrap<Bam.Net.CoreServices.AssemblyManagement.Data.PropertyDescriptor>(batch));
            }, Database);
        }

		
		/// <summary>
		/// Set one entry matching the specified filter.  If none exists 
		/// one is created; success depends on the nullability
		/// of the specified columns.
		/// </summary>
		public void SetOneTypeDescriptorWhere(WhereDelegate<TypeDescriptorColumns> where)
		{
			Bam.Net.CoreServices.AssemblyManagement.Data.Dao.TypeDescriptor.SetOneWhere(where, Database);
		}

		/// <summary>
		/// Set one entry matching the specified filter.  If none exists 
		/// one is created; success depends on the nullability
		/// of the specified columns.
		/// </summary>
		public void SetOneTypeDescriptorWhere(WhereDelegate<TypeDescriptorColumns> where, out Bam.Net.CoreServices.AssemblyManagement.Data.TypeDescriptor result)
		{
			Bam.Net.CoreServices.AssemblyManagement.Data.Dao.TypeDescriptor.SetOneWhere(where, out Bam.Net.CoreServices.AssemblyManagement.Data.Dao.TypeDescriptor daoResult, Database);
			result = daoResult.CopyAs<Bam.Net.CoreServices.AssemblyManagement.Data.TypeDescriptor>();
		}

		/// <summary>
		/// Get one entry matching the specified filter.  If none exists 
		/// one is created; success depends on the nullability
		/// of the specified columns.
		/// </summary>
		/// <param name="where"></param>
		public Bam.Net.CoreServices.AssemblyManagement.Data.TypeDescriptor GetOneTypeDescriptorWhere(WhereDelegate<TypeDescriptorColumns> where)
		{
			Type wrapperType = GetWrapperType<Bam.Net.CoreServices.AssemblyManagement.Data.TypeDescriptor>();
			return (Bam.Net.CoreServices.AssemblyManagement.Data.TypeDescriptor)Bam.Net.CoreServices.AssemblyManagement.Data.Dao.TypeDescriptor.GetOneWhere(where, Database)?.CopyAs(wrapperType, this);
		}

		/// <summary>
		/// Execute a query that should return only one result.  If no result is found null is returned.  If more
		/// than one result is returned a MultipleEntriesFoundException is thrown.  This method is most commonly used to retrieve a
		/// single TypeDescriptor instance by its Id/Key value
		/// </summary>
		/// <param name="where">A WhereDelegate that receives a TypeDescriptorColumns 
		/// and returns a IQueryFilter which is the result of any comparisons
		/// between TypeDescriptorColumns and other values
		/// </param>
		public Bam.Net.CoreServices.AssemblyManagement.Data.TypeDescriptor OneTypeDescriptorWhere(WhereDelegate<TypeDescriptorColumns> where)
        {
            Type wrapperType = GetWrapperType<Bam.Net.CoreServices.AssemblyManagement.Data.TypeDescriptor>();
            return (Bam.Net.CoreServices.AssemblyManagement.Data.TypeDescriptor)Bam.Net.CoreServices.AssemblyManagement.Data.Dao.TypeDescriptor.OneWhere(where, Database)?.CopyAs(wrapperType, this);
        }

		/// <summary>
		/// Execute a query and return the results. 
		/// </summary>
		/// <param name="where">A WhereDelegate that receives a Bam.Net.CoreServices.AssemblyManagement.Data.TypeDescriptorColumns 
		/// and returns a IQueryFilter which is the result of any comparisons
		/// between Bam.Net.CoreServices.AssemblyManagement.Data.TypeDescriptorColumns and other values
		/// </param>
		public IEnumerable<Bam.Net.CoreServices.AssemblyManagement.Data.TypeDescriptor> TypeDescriptorsWhere(WhereDelegate<TypeDescriptorColumns> where, OrderBy<TypeDescriptorColumns> orderBy = null)
        {
            return Wrap<Bam.Net.CoreServices.AssemblyManagement.Data.TypeDescriptor>(Bam.Net.CoreServices.AssemblyManagement.Data.Dao.TypeDescriptor.Where(where, orderBy, Database));
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
		/// <param name="where">A WhereDelegate that receives a TypeDescriptorColumns 
		/// and returns a IQueryFilter which is the result of any comparisons
		/// between TypeDescriptorColumns and other values
		/// </param>
		public IEnumerable<Bam.Net.CoreServices.AssemblyManagement.Data.TypeDescriptor> TopTypeDescriptorsWhere(int count, WhereDelegate<TypeDescriptorColumns> where)
        {
            return Wrap<Bam.Net.CoreServices.AssemblyManagement.Data.TypeDescriptor>(Bam.Net.CoreServices.AssemblyManagement.Data.Dao.TypeDescriptor.Top(count, where, Database));
        }

        public IEnumerable<Bam.Net.CoreServices.AssemblyManagement.Data.TypeDescriptor> TopTypeDescriptorsWhere(int count, WhereDelegate<TypeDescriptorColumns> where, OrderBy<TypeDescriptorColumns> orderBy)
        {
            return Wrap<Bam.Net.CoreServices.AssemblyManagement.Data.TypeDescriptor>(Bam.Net.CoreServices.AssemblyManagement.Data.Dao.TypeDescriptor.Top(count, where, orderBy, Database));
        }
                                
		/// <summary>
		/// Return the count of TypeDescriptors
		/// </summary>
		public long CountTypeDescriptors()
        {
            return Bam.Net.CoreServices.AssemblyManagement.Data.Dao.TypeDescriptor.Count(Database);
        }

		/// <summary>
		/// Execute a query and return the number of results
		/// </summary>
		/// <param name="where">A WhereDelegate that receives a TypeDescriptorColumns 
		/// and returns a IQueryFilter which is the result of any comparisons
		/// between TypeDescriptorColumns and other values
		/// </param>
        public long CountTypeDescriptorsWhere(WhereDelegate<TypeDescriptorColumns> where)
        {
            return Bam.Net.CoreServices.AssemblyManagement.Data.Dao.TypeDescriptor.Count(where, Database);
        }
        
        public async Task BatchQueryTypeDescriptors(int batchSize, WhereDelegate<TypeDescriptorColumns> where, Action<IEnumerable<Bam.Net.CoreServices.AssemblyManagement.Data.TypeDescriptor>> batchProcessor)
        {
            await Bam.Net.CoreServices.AssemblyManagement.Data.Dao.TypeDescriptor.BatchQuery(batchSize, where, (batch) =>
            {
				batchProcessor(Wrap<Bam.Net.CoreServices.AssemblyManagement.Data.TypeDescriptor>(batch));
            }, Database);
        }
		
        public async Task BatchAllTypeDescriptors(int batchSize, Action<IEnumerable<Bam.Net.CoreServices.AssemblyManagement.Data.TypeDescriptor>> batchProcessor)
        {
            await Bam.Net.CoreServices.AssemblyManagement.Data.Dao.TypeDescriptor.BatchAll(batchSize, (batch) =>
            {
				batchProcessor(Wrap<Bam.Net.CoreServices.AssemblyManagement.Data.TypeDescriptor>(batch));
            }, Database);
        }

		
		/// <summary>
		/// Set one entry matching the specified filter.  If none exists 
		/// one is created; success depends on the nullability
		/// of the specified columns.
		/// </summary>
		public void SetOneAssemblyDescriptorWhere(WhereDelegate<AssemblyDescriptorColumns> where)
		{
			Bam.Net.CoreServices.AssemblyManagement.Data.Dao.AssemblyDescriptor.SetOneWhere(where, Database);
		}

		/// <summary>
		/// Set one entry matching the specified filter.  If none exists 
		/// one is created; success depends on the nullability
		/// of the specified columns.
		/// </summary>
		public void SetOneAssemblyDescriptorWhere(WhereDelegate<AssemblyDescriptorColumns> where, out Bam.Net.CoreServices.AssemblyManagement.Data.AssemblyDescriptor result)
		{
			Bam.Net.CoreServices.AssemblyManagement.Data.Dao.AssemblyDescriptor.SetOneWhere(where, out Bam.Net.CoreServices.AssemblyManagement.Data.Dao.AssemblyDescriptor daoResult, Database);
			result = daoResult.CopyAs<Bam.Net.CoreServices.AssemblyManagement.Data.AssemblyDescriptor>();
		}

		/// <summary>
		/// Get one entry matching the specified filter.  If none exists 
		/// one is created; success depends on the nullability
		/// of the specified columns.
		/// </summary>
		/// <param name="where"></param>
		public Bam.Net.CoreServices.AssemblyManagement.Data.AssemblyDescriptor GetOneAssemblyDescriptorWhere(WhereDelegate<AssemblyDescriptorColumns> where)
		{
			Type wrapperType = GetWrapperType<Bam.Net.CoreServices.AssemblyManagement.Data.AssemblyDescriptor>();
			return (Bam.Net.CoreServices.AssemblyManagement.Data.AssemblyDescriptor)Bam.Net.CoreServices.AssemblyManagement.Data.Dao.AssemblyDescriptor.GetOneWhere(where, Database)?.CopyAs(wrapperType, this);
		}

		/// <summary>
		/// Execute a query that should return only one result.  If no result is found null is returned.  If more
		/// than one result is returned a MultipleEntriesFoundException is thrown.  This method is most commonly used to retrieve a
		/// single AssemblyDescriptor instance by its Id/Key value
		/// </summary>
		/// <param name="where">A WhereDelegate that receives a AssemblyDescriptorColumns 
		/// and returns a IQueryFilter which is the result of any comparisons
		/// between AssemblyDescriptorColumns and other values
		/// </param>
		public Bam.Net.CoreServices.AssemblyManagement.Data.AssemblyDescriptor OneAssemblyDescriptorWhere(WhereDelegate<AssemblyDescriptorColumns> where)
        {
            Type wrapperType = GetWrapperType<Bam.Net.CoreServices.AssemblyManagement.Data.AssemblyDescriptor>();
            return (Bam.Net.CoreServices.AssemblyManagement.Data.AssemblyDescriptor)Bam.Net.CoreServices.AssemblyManagement.Data.Dao.AssemblyDescriptor.OneWhere(where, Database)?.CopyAs(wrapperType, this);
        }

		/// <summary>
		/// Execute a query and return the results. 
		/// </summary>
		/// <param name="where">A WhereDelegate that receives a Bam.Net.CoreServices.AssemblyManagement.Data.AssemblyDescriptorColumns 
		/// and returns a IQueryFilter which is the result of any comparisons
		/// between Bam.Net.CoreServices.AssemblyManagement.Data.AssemblyDescriptorColumns and other values
		/// </param>
		public IEnumerable<Bam.Net.CoreServices.AssemblyManagement.Data.AssemblyDescriptor> AssemblyDescriptorsWhere(WhereDelegate<AssemblyDescriptorColumns> where, OrderBy<AssemblyDescriptorColumns> orderBy = null)
        {
            return Wrap<Bam.Net.CoreServices.AssemblyManagement.Data.AssemblyDescriptor>(Bam.Net.CoreServices.AssemblyManagement.Data.Dao.AssemblyDescriptor.Where(where, orderBy, Database));
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
		/// <param name="where">A WhereDelegate that receives a AssemblyDescriptorColumns 
		/// and returns a IQueryFilter which is the result of any comparisons
		/// between AssemblyDescriptorColumns and other values
		/// </param>
		public IEnumerable<Bam.Net.CoreServices.AssemblyManagement.Data.AssemblyDescriptor> TopAssemblyDescriptorsWhere(int count, WhereDelegate<AssemblyDescriptorColumns> where)
        {
            return Wrap<Bam.Net.CoreServices.AssemblyManagement.Data.AssemblyDescriptor>(Bam.Net.CoreServices.AssemblyManagement.Data.Dao.AssemblyDescriptor.Top(count, where, Database));
        }

        public IEnumerable<Bam.Net.CoreServices.AssemblyManagement.Data.AssemblyDescriptor> TopAssemblyDescriptorsWhere(int count, WhereDelegate<AssemblyDescriptorColumns> where, OrderBy<AssemblyDescriptorColumns> orderBy)
        {
            return Wrap<Bam.Net.CoreServices.AssemblyManagement.Data.AssemblyDescriptor>(Bam.Net.CoreServices.AssemblyManagement.Data.Dao.AssemblyDescriptor.Top(count, where, orderBy, Database));
        }
                                
		/// <summary>
		/// Return the count of AssemblyDescriptors
		/// </summary>
		public long CountAssemblyDescriptors()
        {
            return Bam.Net.CoreServices.AssemblyManagement.Data.Dao.AssemblyDescriptor.Count(Database);
        }

		/// <summary>
		/// Execute a query and return the number of results
		/// </summary>
		/// <param name="where">A WhereDelegate that receives a AssemblyDescriptorColumns 
		/// and returns a IQueryFilter which is the result of any comparisons
		/// between AssemblyDescriptorColumns and other values
		/// </param>
        public long CountAssemblyDescriptorsWhere(WhereDelegate<AssemblyDescriptorColumns> where)
        {
            return Bam.Net.CoreServices.AssemblyManagement.Data.Dao.AssemblyDescriptor.Count(where, Database);
        }
        
        public async Task BatchQueryAssemblyDescriptors(int batchSize, WhereDelegate<AssemblyDescriptorColumns> where, Action<IEnumerable<Bam.Net.CoreServices.AssemblyManagement.Data.AssemblyDescriptor>> batchProcessor)
        {
            await Bam.Net.CoreServices.AssemblyManagement.Data.Dao.AssemblyDescriptor.BatchQuery(batchSize, where, (batch) =>
            {
				batchProcessor(Wrap<Bam.Net.CoreServices.AssemblyManagement.Data.AssemblyDescriptor>(batch));
            }, Database);
        }
		
        public async Task BatchAllAssemblyDescriptors(int batchSize, Action<IEnumerable<Bam.Net.CoreServices.AssemblyManagement.Data.AssemblyDescriptor>> batchProcessor)
        {
            await Bam.Net.CoreServices.AssemblyManagement.Data.Dao.AssemblyDescriptor.BatchAll(batchSize, (batch) =>
            {
				batchProcessor(Wrap<Bam.Net.CoreServices.AssemblyManagement.Data.AssemblyDescriptor>(batch));
            }, Database);
        }

		
		/// <summary>
		/// Set one entry matching the specified filter.  If none exists 
		/// one is created; success depends on the nullability
		/// of the specified columns.
		/// </summary>
		public void SetOneAssemblyReferenceDescriptorWhere(WhereDelegate<AssemblyReferenceDescriptorColumns> where)
		{
			Bam.Net.CoreServices.AssemblyManagement.Data.Dao.AssemblyReferenceDescriptor.SetOneWhere(where, Database);
		}

		/// <summary>
		/// Set one entry matching the specified filter.  If none exists 
		/// one is created; success depends on the nullability
		/// of the specified columns.
		/// </summary>
		public void SetOneAssemblyReferenceDescriptorWhere(WhereDelegate<AssemblyReferenceDescriptorColumns> where, out Bam.Net.CoreServices.AssemblyManagement.Data.AssemblyReferenceDescriptor result)
		{
			Bam.Net.CoreServices.AssemblyManagement.Data.Dao.AssemblyReferenceDescriptor.SetOneWhere(where, out Bam.Net.CoreServices.AssemblyManagement.Data.Dao.AssemblyReferenceDescriptor daoResult, Database);
			result = daoResult.CopyAs<Bam.Net.CoreServices.AssemblyManagement.Data.AssemblyReferenceDescriptor>();
		}

		/// <summary>
		/// Get one entry matching the specified filter.  If none exists 
		/// one is created; success depends on the nullability
		/// of the specified columns.
		/// </summary>
		/// <param name="where"></param>
		public Bam.Net.CoreServices.AssemblyManagement.Data.AssemblyReferenceDescriptor GetOneAssemblyReferenceDescriptorWhere(WhereDelegate<AssemblyReferenceDescriptorColumns> where)
		{
			Type wrapperType = GetWrapperType<Bam.Net.CoreServices.AssemblyManagement.Data.AssemblyReferenceDescriptor>();
			return (Bam.Net.CoreServices.AssemblyManagement.Data.AssemblyReferenceDescriptor)Bam.Net.CoreServices.AssemblyManagement.Data.Dao.AssemblyReferenceDescriptor.GetOneWhere(where, Database)?.CopyAs(wrapperType, this);
		}

		/// <summary>
		/// Execute a query that should return only one result.  If no result is found null is returned.  If more
		/// than one result is returned a MultipleEntriesFoundException is thrown.  This method is most commonly used to retrieve a
		/// single AssemblyReferenceDescriptor instance by its Id/Key value
		/// </summary>
		/// <param name="where">A WhereDelegate that receives a AssemblyReferenceDescriptorColumns 
		/// and returns a IQueryFilter which is the result of any comparisons
		/// between AssemblyReferenceDescriptorColumns and other values
		/// </param>
		public Bam.Net.CoreServices.AssemblyManagement.Data.AssemblyReferenceDescriptor OneAssemblyReferenceDescriptorWhere(WhereDelegate<AssemblyReferenceDescriptorColumns> where)
        {
            Type wrapperType = GetWrapperType<Bam.Net.CoreServices.AssemblyManagement.Data.AssemblyReferenceDescriptor>();
            return (Bam.Net.CoreServices.AssemblyManagement.Data.AssemblyReferenceDescriptor)Bam.Net.CoreServices.AssemblyManagement.Data.Dao.AssemblyReferenceDescriptor.OneWhere(where, Database)?.CopyAs(wrapperType, this);
        }

		/// <summary>
		/// Execute a query and return the results. 
		/// </summary>
		/// <param name="where">A WhereDelegate that receives a Bam.Net.CoreServices.AssemblyManagement.Data.AssemblyReferenceDescriptorColumns 
		/// and returns a IQueryFilter which is the result of any comparisons
		/// between Bam.Net.CoreServices.AssemblyManagement.Data.AssemblyReferenceDescriptorColumns and other values
		/// </param>
		public IEnumerable<Bam.Net.CoreServices.AssemblyManagement.Data.AssemblyReferenceDescriptor> AssemblyReferenceDescriptorsWhere(WhereDelegate<AssemblyReferenceDescriptorColumns> where, OrderBy<AssemblyReferenceDescriptorColumns> orderBy = null)
        {
            return Wrap<Bam.Net.CoreServices.AssemblyManagement.Data.AssemblyReferenceDescriptor>(Bam.Net.CoreServices.AssemblyManagement.Data.Dao.AssemblyReferenceDescriptor.Where(where, orderBy, Database));
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
		/// <param name="where">A WhereDelegate that receives a AssemblyReferenceDescriptorColumns 
		/// and returns a IQueryFilter which is the result of any comparisons
		/// between AssemblyReferenceDescriptorColumns and other values
		/// </param>
		public IEnumerable<Bam.Net.CoreServices.AssemblyManagement.Data.AssemblyReferenceDescriptor> TopAssemblyReferenceDescriptorsWhere(int count, WhereDelegate<AssemblyReferenceDescriptorColumns> where)
        {
            return Wrap<Bam.Net.CoreServices.AssemblyManagement.Data.AssemblyReferenceDescriptor>(Bam.Net.CoreServices.AssemblyManagement.Data.Dao.AssemblyReferenceDescriptor.Top(count, where, Database));
        }

        public IEnumerable<Bam.Net.CoreServices.AssemblyManagement.Data.AssemblyReferenceDescriptor> TopAssemblyReferenceDescriptorsWhere(int count, WhereDelegate<AssemblyReferenceDescriptorColumns> where, OrderBy<AssemblyReferenceDescriptorColumns> orderBy)
        {
            return Wrap<Bam.Net.CoreServices.AssemblyManagement.Data.AssemblyReferenceDescriptor>(Bam.Net.CoreServices.AssemblyManagement.Data.Dao.AssemblyReferenceDescriptor.Top(count, where, orderBy, Database));
        }
                                
		/// <summary>
		/// Return the count of AssemblyReferenceDescriptors
		/// </summary>
		public long CountAssemblyReferenceDescriptors()
        {
            return Bam.Net.CoreServices.AssemblyManagement.Data.Dao.AssemblyReferenceDescriptor.Count(Database);
        }

		/// <summary>
		/// Execute a query and return the number of results
		/// </summary>
		/// <param name="where">A WhereDelegate that receives a AssemblyReferenceDescriptorColumns 
		/// and returns a IQueryFilter which is the result of any comparisons
		/// between AssemblyReferenceDescriptorColumns and other values
		/// </param>
        public long CountAssemblyReferenceDescriptorsWhere(WhereDelegate<AssemblyReferenceDescriptorColumns> where)
        {
            return Bam.Net.CoreServices.AssemblyManagement.Data.Dao.AssemblyReferenceDescriptor.Count(where, Database);
        }
        
        public async Task BatchQueryAssemblyReferenceDescriptors(int batchSize, WhereDelegate<AssemblyReferenceDescriptorColumns> where, Action<IEnumerable<Bam.Net.CoreServices.AssemblyManagement.Data.AssemblyReferenceDescriptor>> batchProcessor)
        {
            await Bam.Net.CoreServices.AssemblyManagement.Data.Dao.AssemblyReferenceDescriptor.BatchQuery(batchSize, where, (batch) =>
            {
				batchProcessor(Wrap<Bam.Net.CoreServices.AssemblyManagement.Data.AssemblyReferenceDescriptor>(batch));
            }, Database);
        }
		
        public async Task BatchAllAssemblyReferenceDescriptors(int batchSize, Action<IEnumerable<Bam.Net.CoreServices.AssemblyManagement.Data.AssemblyReferenceDescriptor>> batchProcessor)
        {
            await Bam.Net.CoreServices.AssemblyManagement.Data.Dao.AssemblyReferenceDescriptor.BatchAll(batchSize, (batch) =>
            {
				batchProcessor(Wrap<Bam.Net.CoreServices.AssemblyManagement.Data.AssemblyReferenceDescriptor>(batch));
            }, Database);
        }

		
		/// <summary>
		/// Set one entry matching the specified filter.  If none exists 
		/// one is created; success depends on the nullability
		/// of the specified columns.
		/// </summary>
		public void SetOneAssemblyRevisionWhere(WhereDelegate<AssemblyRevisionColumns> where)
		{
			Bam.Net.CoreServices.AssemblyManagement.Data.Dao.AssemblyRevision.SetOneWhere(where, Database);
		}

		/// <summary>
		/// Set one entry matching the specified filter.  If none exists 
		/// one is created; success depends on the nullability
		/// of the specified columns.
		/// </summary>
		public void SetOneAssemblyRevisionWhere(WhereDelegate<AssemblyRevisionColumns> where, out Bam.Net.CoreServices.AssemblyManagement.Data.AssemblyRevision result)
		{
			Bam.Net.CoreServices.AssemblyManagement.Data.Dao.AssemblyRevision.SetOneWhere(where, out Bam.Net.CoreServices.AssemblyManagement.Data.Dao.AssemblyRevision daoResult, Database);
			result = daoResult.CopyAs<Bam.Net.CoreServices.AssemblyManagement.Data.AssemblyRevision>();
		}

		/// <summary>
		/// Get one entry matching the specified filter.  If none exists 
		/// one is created; success depends on the nullability
		/// of the specified columns.
		/// </summary>
		/// <param name="where"></param>
		public Bam.Net.CoreServices.AssemblyManagement.Data.AssemblyRevision GetOneAssemblyRevisionWhere(WhereDelegate<AssemblyRevisionColumns> where)
		{
			Type wrapperType = GetWrapperType<Bam.Net.CoreServices.AssemblyManagement.Data.AssemblyRevision>();
			return (Bam.Net.CoreServices.AssemblyManagement.Data.AssemblyRevision)Bam.Net.CoreServices.AssemblyManagement.Data.Dao.AssemblyRevision.GetOneWhere(where, Database)?.CopyAs(wrapperType, this);
		}

		/// <summary>
		/// Execute a query that should return only one result.  If no result is found null is returned.  If more
		/// than one result is returned a MultipleEntriesFoundException is thrown.  This method is most commonly used to retrieve a
		/// single AssemblyRevision instance by its Id/Key value
		/// </summary>
		/// <param name="where">A WhereDelegate that receives a AssemblyRevisionColumns 
		/// and returns a IQueryFilter which is the result of any comparisons
		/// between AssemblyRevisionColumns and other values
		/// </param>
		public Bam.Net.CoreServices.AssemblyManagement.Data.AssemblyRevision OneAssemblyRevisionWhere(WhereDelegate<AssemblyRevisionColumns> where)
        {
            Type wrapperType = GetWrapperType<Bam.Net.CoreServices.AssemblyManagement.Data.AssemblyRevision>();
            return (Bam.Net.CoreServices.AssemblyManagement.Data.AssemblyRevision)Bam.Net.CoreServices.AssemblyManagement.Data.Dao.AssemblyRevision.OneWhere(where, Database)?.CopyAs(wrapperType, this);
        }

		/// <summary>
		/// Execute a query and return the results. 
		/// </summary>
		/// <param name="where">A WhereDelegate that receives a Bam.Net.CoreServices.AssemblyManagement.Data.AssemblyRevisionColumns 
		/// and returns a IQueryFilter which is the result of any comparisons
		/// between Bam.Net.CoreServices.AssemblyManagement.Data.AssemblyRevisionColumns and other values
		/// </param>
		public IEnumerable<Bam.Net.CoreServices.AssemblyManagement.Data.AssemblyRevision> AssemblyRevisionsWhere(WhereDelegate<AssemblyRevisionColumns> where, OrderBy<AssemblyRevisionColumns> orderBy = null)
        {
            return Wrap<Bam.Net.CoreServices.AssemblyManagement.Data.AssemblyRevision>(Bam.Net.CoreServices.AssemblyManagement.Data.Dao.AssemblyRevision.Where(where, orderBy, Database));
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
		/// <param name="where">A WhereDelegate that receives a AssemblyRevisionColumns 
		/// and returns a IQueryFilter which is the result of any comparisons
		/// between AssemblyRevisionColumns and other values
		/// </param>
		public IEnumerable<Bam.Net.CoreServices.AssemblyManagement.Data.AssemblyRevision> TopAssemblyRevisionsWhere(int count, WhereDelegate<AssemblyRevisionColumns> where)
        {
            return Wrap<Bam.Net.CoreServices.AssemblyManagement.Data.AssemblyRevision>(Bam.Net.CoreServices.AssemblyManagement.Data.Dao.AssemblyRevision.Top(count, where, Database));
        }

        public IEnumerable<Bam.Net.CoreServices.AssemblyManagement.Data.AssemblyRevision> TopAssemblyRevisionsWhere(int count, WhereDelegate<AssemblyRevisionColumns> where, OrderBy<AssemblyRevisionColumns> orderBy)
        {
            return Wrap<Bam.Net.CoreServices.AssemblyManagement.Data.AssemblyRevision>(Bam.Net.CoreServices.AssemblyManagement.Data.Dao.AssemblyRevision.Top(count, where, orderBy, Database));
        }
                                
		/// <summary>
		/// Return the count of AssemblyRevisions
		/// </summary>
		public long CountAssemblyRevisions()
        {
            return Bam.Net.CoreServices.AssemblyManagement.Data.Dao.AssemblyRevision.Count(Database);
        }

		/// <summary>
		/// Execute a query and return the number of results
		/// </summary>
		/// <param name="where">A WhereDelegate that receives a AssemblyRevisionColumns 
		/// and returns a IQueryFilter which is the result of any comparisons
		/// between AssemblyRevisionColumns and other values
		/// </param>
        public long CountAssemblyRevisionsWhere(WhereDelegate<AssemblyRevisionColumns> where)
        {
            return Bam.Net.CoreServices.AssemblyManagement.Data.Dao.AssemblyRevision.Count(where, Database);
        }
        
        public async Task BatchQueryAssemblyRevisions(int batchSize, WhereDelegate<AssemblyRevisionColumns> where, Action<IEnumerable<Bam.Net.CoreServices.AssemblyManagement.Data.AssemblyRevision>> batchProcessor)
        {
            await Bam.Net.CoreServices.AssemblyManagement.Data.Dao.AssemblyRevision.BatchQuery(batchSize, where, (batch) =>
            {
				batchProcessor(Wrap<Bam.Net.CoreServices.AssemblyManagement.Data.AssemblyRevision>(batch));
            }, Database);
        }
		
        public async Task BatchAllAssemblyRevisions(int batchSize, Action<IEnumerable<Bam.Net.CoreServices.AssemblyManagement.Data.AssemblyRevision>> batchProcessor)
        {
            await Bam.Net.CoreServices.AssemblyManagement.Data.Dao.AssemblyRevision.BatchAll(batchSize, (batch) =>
            {
				batchProcessor(Wrap<Bam.Net.CoreServices.AssemblyManagement.Data.AssemblyRevision>(batch));
            }, Database);
        }

		
		/// <summary>
		/// Set one entry matching the specified filter.  If none exists 
		/// one is created; success depends on the nullability
		/// of the specified columns.
		/// </summary>
		public void SetOneProcessRuntimeDescriptorWhere(WhereDelegate<ProcessRuntimeDescriptorColumns> where)
		{
			Bam.Net.CoreServices.AssemblyManagement.Data.Dao.ProcessRuntimeDescriptor.SetOneWhere(where, Database);
		}

		/// <summary>
		/// Set one entry matching the specified filter.  If none exists 
		/// one is created; success depends on the nullability
		/// of the specified columns.
		/// </summary>
		public void SetOneProcessRuntimeDescriptorWhere(WhereDelegate<ProcessRuntimeDescriptorColumns> where, out Bam.Net.CoreServices.AssemblyManagement.Data.ProcessRuntimeDescriptor result)
		{
			Bam.Net.CoreServices.AssemblyManagement.Data.Dao.ProcessRuntimeDescriptor.SetOneWhere(where, out Bam.Net.CoreServices.AssemblyManagement.Data.Dao.ProcessRuntimeDescriptor daoResult, Database);
			result = daoResult.CopyAs<Bam.Net.CoreServices.AssemblyManagement.Data.ProcessRuntimeDescriptor>();
		}

		/// <summary>
		/// Get one entry matching the specified filter.  If none exists 
		/// one is created; success depends on the nullability
		/// of the specified columns.
		/// </summary>
		/// <param name="where"></param>
		public Bam.Net.CoreServices.AssemblyManagement.Data.ProcessRuntimeDescriptor GetOneProcessRuntimeDescriptorWhere(WhereDelegate<ProcessRuntimeDescriptorColumns> where)
		{
			Type wrapperType = GetWrapperType<Bam.Net.CoreServices.AssemblyManagement.Data.ProcessRuntimeDescriptor>();
			return (Bam.Net.CoreServices.AssemblyManagement.Data.ProcessRuntimeDescriptor)Bam.Net.CoreServices.AssemblyManagement.Data.Dao.ProcessRuntimeDescriptor.GetOneWhere(where, Database)?.CopyAs(wrapperType, this);
		}

		/// <summary>
		/// Execute a query that should return only one result.  If no result is found null is returned.  If more
		/// than one result is returned a MultipleEntriesFoundException is thrown.  This method is most commonly used to retrieve a
		/// single ProcessRuntimeDescriptor instance by its Id/Key value
		/// </summary>
		/// <param name="where">A WhereDelegate that receives a ProcessRuntimeDescriptorColumns 
		/// and returns a IQueryFilter which is the result of any comparisons
		/// between ProcessRuntimeDescriptorColumns and other values
		/// </param>
		public Bam.Net.CoreServices.AssemblyManagement.Data.ProcessRuntimeDescriptor OneProcessRuntimeDescriptorWhere(WhereDelegate<ProcessRuntimeDescriptorColumns> where)
        {
            Type wrapperType = GetWrapperType<Bam.Net.CoreServices.AssemblyManagement.Data.ProcessRuntimeDescriptor>();
            return (Bam.Net.CoreServices.AssemblyManagement.Data.ProcessRuntimeDescriptor)Bam.Net.CoreServices.AssemblyManagement.Data.Dao.ProcessRuntimeDescriptor.OneWhere(where, Database)?.CopyAs(wrapperType, this);
        }

		/// <summary>
		/// Execute a query and return the results. 
		/// </summary>
		/// <param name="where">A WhereDelegate that receives a Bam.Net.CoreServices.AssemblyManagement.Data.ProcessRuntimeDescriptorColumns 
		/// and returns a IQueryFilter which is the result of any comparisons
		/// between Bam.Net.CoreServices.AssemblyManagement.Data.ProcessRuntimeDescriptorColumns and other values
		/// </param>
		public IEnumerable<Bam.Net.CoreServices.AssemblyManagement.Data.ProcessRuntimeDescriptor> ProcessRuntimeDescriptorsWhere(WhereDelegate<ProcessRuntimeDescriptorColumns> where, OrderBy<ProcessRuntimeDescriptorColumns> orderBy = null)
        {
            return Wrap<Bam.Net.CoreServices.AssemblyManagement.Data.ProcessRuntimeDescriptor>(Bam.Net.CoreServices.AssemblyManagement.Data.Dao.ProcessRuntimeDescriptor.Where(where, orderBy, Database));
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
		/// <param name="where">A WhereDelegate that receives a ProcessRuntimeDescriptorColumns 
		/// and returns a IQueryFilter which is the result of any comparisons
		/// between ProcessRuntimeDescriptorColumns and other values
		/// </param>
		public IEnumerable<Bam.Net.CoreServices.AssemblyManagement.Data.ProcessRuntimeDescriptor> TopProcessRuntimeDescriptorsWhere(int count, WhereDelegate<ProcessRuntimeDescriptorColumns> where)
        {
            return Wrap<Bam.Net.CoreServices.AssemblyManagement.Data.ProcessRuntimeDescriptor>(Bam.Net.CoreServices.AssemblyManagement.Data.Dao.ProcessRuntimeDescriptor.Top(count, where, Database));
        }

        public IEnumerable<Bam.Net.CoreServices.AssemblyManagement.Data.ProcessRuntimeDescriptor> TopProcessRuntimeDescriptorsWhere(int count, WhereDelegate<ProcessRuntimeDescriptorColumns> where, OrderBy<ProcessRuntimeDescriptorColumns> orderBy)
        {
            return Wrap<Bam.Net.CoreServices.AssemblyManagement.Data.ProcessRuntimeDescriptor>(Bam.Net.CoreServices.AssemblyManagement.Data.Dao.ProcessRuntimeDescriptor.Top(count, where, orderBy, Database));
        }
                                
		/// <summary>
		/// Return the count of ProcessRuntimeDescriptors
		/// </summary>
		public long CountProcessRuntimeDescriptors()
        {
            return Bam.Net.CoreServices.AssemblyManagement.Data.Dao.ProcessRuntimeDescriptor.Count(Database);
        }

		/// <summary>
		/// Execute a query and return the number of results
		/// </summary>
		/// <param name="where">A WhereDelegate that receives a ProcessRuntimeDescriptorColumns 
		/// and returns a IQueryFilter which is the result of any comparisons
		/// between ProcessRuntimeDescriptorColumns and other values
		/// </param>
        public long CountProcessRuntimeDescriptorsWhere(WhereDelegate<ProcessRuntimeDescriptorColumns> where)
        {
            return Bam.Net.CoreServices.AssemblyManagement.Data.Dao.ProcessRuntimeDescriptor.Count(where, Database);
        }
        
        public async Task BatchQueryProcessRuntimeDescriptors(int batchSize, WhereDelegate<ProcessRuntimeDescriptorColumns> where, Action<IEnumerable<Bam.Net.CoreServices.AssemblyManagement.Data.ProcessRuntimeDescriptor>> batchProcessor)
        {
            await Bam.Net.CoreServices.AssemblyManagement.Data.Dao.ProcessRuntimeDescriptor.BatchQuery(batchSize, where, (batch) =>
            {
				batchProcessor(Wrap<Bam.Net.CoreServices.AssemblyManagement.Data.ProcessRuntimeDescriptor>(batch));
            }, Database);
        }
		
        public async Task BatchAllProcessRuntimeDescriptors(int batchSize, Action<IEnumerable<Bam.Net.CoreServices.AssemblyManagement.Data.ProcessRuntimeDescriptor>> batchProcessor)
        {
            await Bam.Net.CoreServices.AssemblyManagement.Data.Dao.ProcessRuntimeDescriptor.BatchAll(batchSize, (batch) =>
            {
				batchProcessor(Wrap<Bam.Net.CoreServices.AssemblyManagement.Data.ProcessRuntimeDescriptor>(batch));
            }, Database);
        }


	}
}																								
