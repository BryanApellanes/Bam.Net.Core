namespace Bam.Net
{
    public interface IRemoteSshHost: IRemoteHost
    {
        string LoginUserName { get; set; }
        string LoginPassword { get; set; }
        string[] ListUsers();
        bool AddUser(string userName, string password);
        bool DeleteUser(string userName);
        string[] ListGroups();
        bool DeleteGroup(string groupName);
    }
}