using Bam.Net.Data.Repositories;

namespace Bam.Net.Services.DataReplication.Consensus.Data
{
    public class RaftFollowerWriteLog: AuditRepoData
    {
        public ulong NodeIdentifier { get; set; }
        public ulong LogEntryIdentifier { get; set; }

        public static RaftFollowerWriteLog For(string hostName, int port, RaftLogEntryWriteRequest writeRequest)
        {
            return new RaftFollowerWriteLog()
            {
                NodeIdentifier = RaftNodeIdentifier.KeyFor(hostName, port),
                LogEntryIdentifier = writeRequest.LogEntry.GetId()
            };
        }
    }
}