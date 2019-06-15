using System.Threading.Tasks;
using DNS.Client.RequestResolver;
using DNS.Protocol;

namespace Bam.Net.Application
{
    public class BamDnsRequestResolver : IRequestResolver
    {
        public Task<IResponse> Resolve(IRequest request)
        {
            throw new System.NotImplementedException();
        }
    }
}