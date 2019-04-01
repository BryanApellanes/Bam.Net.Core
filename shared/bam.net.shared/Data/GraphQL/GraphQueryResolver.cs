using System;
using Bam.Net.Data.Repositories;
using GraphQL.Types;

namespace Bam.Net.Data.GraphQL
{
    public class GraphQueryResolver : ObjectGraphType
    {
        public GraphQueryResolver() : this(DataProvider.Current.GetSysRepository())
        {
        }
        
        public GraphQueryResolver(IRepository dataSourceRepository)
        {
            DataSourceRepository = dataSourceRepository;
        }
        
        public IRepository DataSourceRepository { get; set; }

        public virtual T Resolve<T>(ResolveFieldContext<T> ctx)
        {
            // call this from generated GraphQueriesContext resolve lambda function
            throw new NotImplementedException();
        }

        public virtual QueryArguments GetQueryArguments<T>()
        {
            // create a new QueryArgument for all "columns/properties" of the specified type; see the implementation of DaoRepository to determine appropriate property filter
            // new QueryArguments(new QueryArgument<StringGraphType> { Name = "{{PropertyName}}" }, ... )
            throw new NotImplementedException();
        }
    }
}