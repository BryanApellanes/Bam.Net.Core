using System.IO;
using Bam.Net.Application.Network;
using Bam.Net.Application.Verbs;

namespace Bam.Net.Automation
{
    public class ScanningRemoteFileHandler : IRemoteFileHandler
    {
        public ScanningRemoteFileHandler()
        {
            CopyContext.SetupHostScan();
        }

        public void CopyTo(Remote remote, FileSystemInfo localData, string localPathOnRemote = null)
        {
            throw new System.NotImplementedException();
        }

        public void CopyTo(string host, FileSystemInfo localData, string localPathOnRemote = null)
        {
            IRemoteFileHandler remoteFileHandler = Scan.HostFor<IRemoteFileHandler>(host);
            remoteFileHandler.CopyTo(host, localData, localPathOnRemote);
        }

        public void Delete(string host, string localPathOnRemote)
        {
            IRemoteFileHandler remoteFileHandler = Scan.HostFor<IRemoteFileHandler>(host);
            remoteFileHandler.Delete(host, localPathOnRemote);
        }

        public bool Exists(string host, string localPathOnRemote)
        {
            IRemoteFileHandler remoteFileHandler = Scan.HostFor<IRemoteFileHandler>(host);
            return remoteFileHandler.Exists(host, localPathOnRemote);
        }
    }
}