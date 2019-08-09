using System.IO;
using Bam.Net.Automation;

namespace Bam.Net.Application.Network
{
    public class CommandServiceClientRemoteFileHandler: IRemoteFileHandler
    {
        public CommandServiceClientRemoteFileHandler(string commandServerHostName, int commandServerPort)
        {
            CommandServerHostName = commandServerHostName;
            CommandServerPort = commandServerPort;
        }
        
        public string CommandServerHostName { get; set; }
        public int CommandServerPort { get; set; }
        public void CopyTo(Remote remote, FileSystemInfo localData, string localPathOnRemote = null)
        {
            throw new System.NotImplementedException();
        }

        public void CopyTo(string host, FileSystemInfo localData, string localPathOnRemote = null)
        {
            throw new System.NotImplementedException();
        }

        public void Delete(string host, string localPathOnRemote)
        {
            throw new System.NotImplementedException();
        }

        public bool Exists(string host, string localPathOnRemote)
        {
            throw new System.NotImplementedException();
        }
    }
}