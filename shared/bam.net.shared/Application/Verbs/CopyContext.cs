using System;
using Bam.Net.Automation;
using Bam.Net.CoreServices.ApplicationRegistration.Data;
using Bam.Net.Services;
using Bam.Net.Application.Network;
using Bam.Net.Services.Automation;
using Bam.Net.Unix;
using Bam.Net.Windows;
using DefaultNamespace;

namespace Bam.Net.Application.Verbs
{
    public class CopyContext
    {
        public CopyContext()
        {
            Setup();
        }

        private static bool _setup;
        protected static void Setup()
        {
            if (!_setup)
            {
                _setup = true;
                Scan.Prepare<IRemoteFileHandler>(hostName =>
                {
                    Network.Remote remote = Network.Remote.For(hostName);
                    switch (remote.OS)
                    {
                        case OSNames.Windows:
                            return new WindowsRemoteFileHandler();
                            break;
                        case OSNames.Linux:
                        case OSNames.OSX:
                            return new UnixRemoteFileHandler();
                        case OSNames.Invalid:
                            return new CommandServiceClientRemoteFileHandler(hostName, CommandService.DefaultPort);
                    }
                });
            }
        }
        
        public string LocalPath { get; set; }
        public string RemotePath { get; set; }
        public Remote Remote { get; set; }
        
        public bool Execute()
        {
            IRemoteFileHandler fileHandler = Scan.For<IRemoteFileHandler>(Remote.HostName);
            throw new NotImplementedException();
        }
    }
}