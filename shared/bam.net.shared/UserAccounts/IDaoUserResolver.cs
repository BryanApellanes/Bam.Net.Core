using Bam.Net.Data;
using Bam.Net.ServiceProxy;
using Bam.Net.UserAccounts.Data;

namespace Bam.Net.UserAccounts
{
    public interface IDaoUserResolver: IUserResolver, IUserProvider
    {
        Database Database { get; set; }
        
        void SetUser(IHttpContext context, User user, bool isAuthenticated, Database db = null);
    }
}