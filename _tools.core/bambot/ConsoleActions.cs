using System;
using System.IO;
using System.Threading;
using Bam.Net.Automation;
using Bam.Net.CommandLine;
using Bam.Net.Server;
using Bam.Net.Services;
using Bam.Net.Services.Automation;
using Bam.Net.Testing;


namespace Bam.Net.Application
{
    [Serializable]
    public class ConsoleActions : CommandLineTestInterface
    {
        public ServiceProxyServer ServiceProxyServer { get; set; }
        
        [ConsoleAction("S", "Start bambot server")]
        public void StartServer()
        {
            ServiceProxyServer = new CommandService().Serve();
            foreach (HostPrefix hostPrefix in ServiceProxyServer.HostPrefixes)
            {
                OutLineFormat("\t{0}", ConsoleColor.Cyan, hostPrefix.ToString());
            }
            Pause("Bambot service started");
        }

        [ConsoleAction("K", "Kill bambot server")]
        public void KillServer()
        {
            if (ServiceProxyServer != null)
            {
                ServiceProxyServer.Stop();
                Pause("Bambot service stopped");
            }
        }
    }
}