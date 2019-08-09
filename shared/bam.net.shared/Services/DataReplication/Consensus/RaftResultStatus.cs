namespace Bam.Net.Services.DataReplication.Consensus
{
    public enum RaftResultStatus
    {
        Invalid = -1,
        Error = 0,
        Success = 1,
        Accepted = 2,
    }
}