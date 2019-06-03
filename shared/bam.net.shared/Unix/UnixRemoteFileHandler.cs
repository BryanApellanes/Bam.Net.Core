using System.IO;
using Bam.Net.Automation;

namespace Bam.Net.Unix
{
    public class UnixRemoteFileHandler: IRemoteFileHandler
    {
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