using System;
using System.IO;
using Bam.Net.Automation;
using Bam.Net.CommandLine;
using Bam.Net.Testing;

namespace Bam.Net.Application.Network
{
    public class ScpRemoteFileHandler: CommandLineTestInterface, IRemoteFileHandler
    {
        public string UserName { get; set; }

        public void CopyTo(Remote remote, FileSystemInfo localData, string localPathOnRemote = null)
        {
            CopyTo(remote.HostName, localData, localPathOnRemote);
        }

        public void CopyTo(string host, FileSystemInfo localData, string localPathOnRemote = null)
        {
            string options = "";
            if (Directory.Exists(localData.FullName))
            {
                options = "-r";
            }

            OutLineFormat("If your local public key is not in the ~/.ssh/authorized_keys file of the remote, the password prompt will prevent success, and this command will timeout in 60 seconds", ConsoleColor.Yellow);
            ProcessOutput output = Scp.Run(options, localData.FullName, $"{UserName}@{host}:{localPathOnRemote}");
            
            OutLineFormat("*** out ***\r\n{0}\r\n*** / out ***", ConsoleColor.Cyan, output.StandardOutput);
            OutLineFormat("*** err ***\r\n{0}\r\n*** / err ***", ConsoleColor.Yellow, output.StandardError);
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