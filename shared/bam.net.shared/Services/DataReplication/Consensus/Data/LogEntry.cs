using Bam.Net.Data.Repositories;

namespace Bam.Net.Services.DataReplication.Consensus.Data
{
    public class LogEntry: AuditRepoData
    {
        public string Base64Key { get; set; }
        public string Base64Value { get; set; }
    }
}