using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Bam.Net.ServiceProxy;

namespace Bam.Net.Data.Repositories
{
    public class DefaultDatabaseProviderRepositoryResolver : RepositoryResolver, IDataDirectoryResolver
    {
        public override IRepository GetRepository(IHttpContext context)
        {
            DefaultDataDirectoryProvider dataSettings = ResolveDataDirectoryProvider(context);
            return dataSettings.GetSysRepository();
        }

        public DefaultDataDirectoryProvider ResolveDataDirectoryProvider(IHttpContext context)
        {
            return new DefaultDataDirectoryProvider(ProcessModeResolver.Resolve(context.Request.Url));
        }
    }
}
