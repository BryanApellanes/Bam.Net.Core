namespace Bam.Net.Data
{
    public interface IDatabaseCredentials
    {
        string UserId { get; set; }
        string Password { get; set; }
    }
}