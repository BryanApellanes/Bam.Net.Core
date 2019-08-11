/*
	Copyright © Bryan Apellanes 2015  
*/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Bam.Net.Data;

namespace Bam.Net.Services.DataReplication.Data
{
    [Serializable]
	public class QueryOperation: Operation
	{
        public virtual List<DataPropertyFilter> PropertyFilters { get; set; }
		public override object Execute(IDistributedRepository repository)
		{
            return repository.Query(this);
		}

        public static QueryOperation Where(Type type, dynamic queryProperties)
        {
            return Where(type, ((object)queryProperties).ToDictionary());
        }

        public static QueryOperation Where(Type type, Dictionary<string, object> properties)
        {
            QueryOperation operation = For<QueryOperation>(type);
            operation.PropertyFilters = new List<DataPropertyFilter>();
            properties.Keys.Each(key =>
            {
                operation.PropertyFilters.Add(new DataPropertyFilter { Name = key, Value = properties[key], Operator =  QueryOperator.Equals});
            });
            return operation;
        }

        public static QueryOperation Where<T>(params DataPropertyFilter[] filters)
        {
	        return Where(typeof(T), filters);
        }

        public static QueryOperation Where(Type type, params DataPropertyFilter[] filters)
        {
	        QueryOperation operation = For<QueryOperation>(type);
	        operation.PropertyFilters = new List<DataPropertyFilter>(filters);
	        return operation;
        }

        public static QueryOperation Where(Type type, object property, QueryOperator queryOperator, object value)
        {
	        QueryOperation operation = For<QueryOperation>(type);
	        operation.PropertyFilters = new List<DataPropertyFilter>(new DataPropertyFilter[]
	        {
		        DataPropertyFilter.Where(property, queryOperator, value)
	        });
	        return operation;
        }
	}
}
