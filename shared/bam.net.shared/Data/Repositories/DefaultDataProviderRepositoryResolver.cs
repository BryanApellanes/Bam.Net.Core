using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Bam.Net.ServiceProxy;

namespace Bam.Net.Data.Repositories
{
    public class DefaultDataProviderRepositoryResolver : RepositoryResolver, IDataProviderResolver
    {
        public override IRepository GetRepository(IHttpContext context)
        {
            IDataProvider dataSettings = ResolveDataProvider(context);
            return dataSettings.GetSysRepository();
        }

        public IDataProvider ResolveDataProvider(IHttpContext context)
        {
            return new DefaultDataProvider(ProcessModeResolver.Resolve(context.Request.Url));
        }
    }
}
