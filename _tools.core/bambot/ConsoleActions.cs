using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Permissions;
using System.Threading;
using Bam.Net.Automation;
using Bam.Net.CommandLine;
using Bam.Net.CoreServices;
using Bam.Net.Server;
using Bam.Net.Services;
using Bam.Net.Services.Automation;
using Bam.Net.Testing;
using InterSystems.Data.CacheClient.Gateway;


namespace Bam.Net.Application
{
    [Serializable]
    public class ConsoleActions : CommandLineTestInterface
    {
        public ServiceProxyServer ServiceProxyServer { get; set; }
        
        [ConsoleAction("S", "Start bambot server")]
        public void StartServer()
        {
            ApplicationServiceRegistry appRegistry = BambotServiceRegistry.ForCurrentProcessMode();
            HashSet<HostPrefix> hostPrefixes = new HashSet<HostPrefix>();
            HostPrefix.FromBamProcessConfig().Each(hp => hostPrefixes.Add(hp));
            ServiceProxyServer = appRegistry.Serve(hostPrefixes.ToArray());
            
            OutLine($"Config file: {Config.Current.File.FullName}", ConsoleColor.DarkCyan);
            OutLine(Config.Current.File.ReadAllText());
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