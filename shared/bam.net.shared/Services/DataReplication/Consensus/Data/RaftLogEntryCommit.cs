using Bam.Net.Data.Repositories;

namespace Bam.Net.Services.DataReplication.Consensus.Data
{
    public class RaftLogEntryCommit: RepoData
    {
        public ulong RaftLogEntryId { get; set; }
        public ulong Seq { get; set; }
    }
}