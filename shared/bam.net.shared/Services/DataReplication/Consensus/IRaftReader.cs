using System.Collections.Generic;
using Bam.Net.Data.Repositories;
using Bam.Net.Services.DataReplication.Data;
using Bam.Net.Testing;

namespace Bam.Net.Services.DataReplication.Consensus
{
    public interface IRaftReader
    {
        T ReadInstance<T>(ulong compositeKey) where T : CompositeKeyAuditRepoData, new();
        object Retrieve(RetrieveOperation retrieveOperation);
        IEnumerable<object> Query(QueryOperation query);
    }
}