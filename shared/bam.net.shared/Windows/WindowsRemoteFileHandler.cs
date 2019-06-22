using System.IO;
using Bam.Net.Application.Network;
using Bam.Net.Automation;
using Bam.Net.Logging;
using Bam.Net.Server;

namespace Bam.Net.Windows
{
    public class WindowsRemoteFileHandler : IRemoteFileHandler
    {
        public void CopyTo(Remote remote, FileSystemInfo localData, string localPathOnRemote = null)
        {
            CopyTo(remote.HostName, localData, localPathOnRemote);
        }

        public void CopyTo(string host, FileSystemInfo localData, string localPathOnRemote = null)
        {
            if (localData is DirectoryInfo directoryInfo)
            {
                directoryInfo.CopyTo(host, localPathOnRemote);
            }
            else if (localData is FileInfo fileInfo)
            {
                fileInfo.CopyTo(host, localPathOnRemote);
            }
        }

        public void Delete(string host, string localPathOnRemote)
        {
            if (Exists(host, localPathOnRemote, out FileSystemInfo fileSystemInfo))
            {
                Log.Info("Deleting {0}", fileSystemInfo.FullName);
                fileSystemInfo.Delete();
            }
        }

        public bool Exists(string host, string localPathOnRemote)
        {
            return Exists(host, localPathOnRemote, out FileSystemInfo ignore);
        }
        
        public bool Exists(string host, string localPathOnRemote, out FileSystemInfo fileSystemInfo)
        {
            DirectoryInfo directoryInfo = new DirectoryInfo(host.GetAdminShareDirectoryPath(localPathOnRemote));
            fileSystemInfo = null;
            if (directoryInfo.Exists)
            {
                fileSystemInfo = directoryInfo;
            }
            else
            {
                FileInfo fileInfo = new FileInfo(host.GetAdminShareFilePath(localPathOnRemote));
                if (fileInfo.Exists)
                {
                    fileInfo.Delete();
                    Log.Info("Deleting {0}", fileInfo.FullName);
                    fileSystemInfo = fileInfo;
                }
            }

            return fileSystemInfo != null && fileSystemInfo.Exists;
        }
    }
}