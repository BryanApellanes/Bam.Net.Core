using System;
using System.IO;
using Bam.Net.Automation;
using Bam.Net.CoreServices.ApplicationRegistration.Data;
using Bam.Net.Services;
using Bam.Net.Application.Network;
using Bam.Net.Services.Automation;
using Bam.Net.Windows;
using Bam.Net.Logging;

namespace Bam.Net.Application.Verbs
{
    public class CopyContext
    {
        public CopyContext()
        {
            SetupHostScan();
        }

        private static bool _setup;
        protected internal static void SetupHostScan()
        {
            if (!_setup)
            {
                _setup = true;
                Scan.SetHostScanFor<IRemoteFileHandler>(hostName =>
                {
                    Network.Remote remote = Network.Remote.For(hostName);
                    switch (remote.OS)
                    {
                        case OSNames.Windows:
                            return new WindowsRemoteFileHandler();
                            break;
                        case OSNames.Linux:
                        case OSNames.OSX:
                            return new ScpRemoteFileHandler();
                        case OSNames.Invalid:
                        default:
                            return new CommandServiceClientRemoteFileHandler(hostName, CommandService.DefaultPort);
                    }
                });
            }
        }

        protected bool IsFile => File.Exists(LocalPath);

        protected bool IsDirectory => Directory.Exists(LocalPath);

        protected FileSystemInfo GetFileSystemInfo()
        {
            if (IsFile)
            {
                return new FileInfo(LocalPath);
            }
            else if (IsDirectory)
            {
                return new DirectoryInfo(LocalPath);
            }

            return null;
        }

        public string LocalPath { get; set; }
        public string RemotePath { get; set; }
        public Remote Remote { get; set; }
        
        public bool Execute()
        {
            try
            {
                FileSystemInfo fsInfo = GetFileSystemInfo();
                if (fsInfo != null)
                {
                    IRemoteFileHandler fileHandler = Scan.HostFor<IRemoteFileHandler>(Remote.HostName);
                    fileHandler.CopyTo(Remote.HostName, fsInfo);
                    return true;
                }
            }
            catch (Exception ex)
            {
                Log.Error("Exception copying {0} to remote host {1}", LocalPath, Remote?.HostName);
            }

            return false;
        }
    }
}