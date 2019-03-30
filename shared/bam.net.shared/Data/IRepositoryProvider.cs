using Bam.Net.Data.Repositories;
using Bam.Net.Logging;

namespace Bam.Net.Data
{
    public interface IRepositoryProvider
    {
        IRepository GetSysRepository();
        DaoRepository GetSysDaoRepository(ILogger logger = null, string schemaName = null);
    }
}