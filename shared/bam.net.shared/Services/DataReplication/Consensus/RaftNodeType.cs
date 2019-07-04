namespace Bam.Net.Services.DataReplication.Consensus
{
    public enum RaftNodeType
    {
        Invalid,
        Follower,
        Candidate,
        Leader
    }
}