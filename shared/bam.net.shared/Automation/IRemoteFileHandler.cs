using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using Bam.Net.Application.Network;

namespace Bam.Net.Automation
{
    /// <summary>
    /// Defines methods used to read and write remote host file systems.
    /// </summary>
    public interface IRemoteFileHandler
    {
        void CopyTo(Remote remote, FileSystemInfo localData, string localPathOnRemote = null);
        void CopyTo(string host, FileSystemInfo localData, string localPathOnRemote = null);
        void Delete(string host, string localPathOnRemote);
        bool Exists(string host, string localPathOnRemote);
    }
}
