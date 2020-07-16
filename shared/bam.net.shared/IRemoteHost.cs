namespace Bam.Net
{
    public interface IRemoteHost
    {
        string HostName { get; set; }
        int Port { get; set; }
        string Execute(string command);
        bool Upload(string localFilePath, string remoteFilePath);
        bool Download(string remoteFilePath, string localFilePath);
    }
}