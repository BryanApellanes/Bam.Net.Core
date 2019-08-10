/*
	Copyright © Bryan Apellanes 2015  
*/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bam.Net.Services.DataReplication.Data
{
    [Serializable]
	public class QueryOperation: Operation
	{
        public List<DataPropertyFilter> PropertyFilters { get; set; }
		public override object Execute(IDistributedRepository repository)
		{
            return repository.Query(this);
		}

        public static QueryOperation For(Type type, dynamic queryProperties)
        {
            return For(type, ((object)queryProperties).ToDictionary());
        }

        public static QueryOperation For(Type type, Dictionary<string, object> properties)
        {
            QueryOperation operation = For<QueryOperation>(type);
            operation.PropertyFilters = new List<DataPropertyFilter>();
            properties.Keys.Each(key =>
            {
                operation.PropertyFilters.Add(new DataPropertyFilter { Name = key, Value = properties[key], Operator =  QueryOperator.Equals});
            });
            return operation;
        }
	}
}
