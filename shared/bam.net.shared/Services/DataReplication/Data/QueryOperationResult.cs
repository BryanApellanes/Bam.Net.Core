using Bam.Net.Data.Repositories;

namespace Bam.Net.Services.DataReplication.Data
{
    public class QueryOperationResult : AuditRepoData
    {
        public string QueryOperationIdentifier { get; set; }
        public string TypeName { get; set; }
        public string Data { get; set; }
    }
}