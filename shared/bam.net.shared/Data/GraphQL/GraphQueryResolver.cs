using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using Bam.Net.Data.Repositories;
using Bam.Net.Logging;
using GraphQL.Types;

namespace Bam.Net.Data.GraphQL
{
    public abstract class GraphQueryResolver : ObjectGraphType
    {
        public GraphQueryResolver() : this(DataProvider.Current.GetSysRepository())
        {
        }
        
        public GraphQueryResolver(IRepository dataSourceRepository, ILogger logger = null)
        {
            DataSourceRepository = dataSourceRepository;
            Logger = logger ?? Log.Default;
        }
        
        public IRepository DataSourceRepository { get; set; }

        public ILogger Logger { get; set; }
        
        public virtual T Resolve<T>(ResolveFieldContext<T> ctx) where T: class, new()
        {
            // call this from generated GraphQueriesContext resolve lambda function
            IEnumerable<T> results = DataSourceRepository.Query<T>(ctx.Arguments);
            if (results.Count() > 1)
            {
                Logger.Warning("Top level GraphQuery returned more than one result for the specified arguments ({0})", GetArgumentString(ctx.Arguments));
            }

            return results.FirstOrDefault() ?? new T();
        }

        private string GetArgumentString(Dictionary<string, object> arguments)
        {
            StringBuilder result = new StringBuilder();
            bool first = true;
            foreach (string key in arguments.Keys)
            {
                if (!first)
                {
                    result.Append(",");
                }
                result.AppendFormat("{0}={1}", key, arguments[key]);
                first = false;
            }

            return result.ToString();
        }
    }
}