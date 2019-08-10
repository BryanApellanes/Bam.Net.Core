using Bam.Net.Data.Repositories;

namespace Bam.Net.Services.DataReplication.Data
{
    public class RetrieveOperationResult : AuditRepoData
    {
        public string RetrieveOperationIdentifier { get; set; }
        
        public string TypeName { get; set; }
        public string Data { get; set; }
    }
}