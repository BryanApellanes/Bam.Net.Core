using System;
using Bam.Net.CommandLine;
using Bam.Net.Testing;
using DNS.Server;

namespace Bam.Net.Services
{
    [Serializable]
    public class ConsoleActions : CommandLineTestInterface
    {
        private static SimpleDnsServer _server;

        [ConsoleAction("S", "Start the Dns server")]
        public static void StartServer()
        {
            _server = new SimpleDnsServer();
            _server.Start();
            Pause("badns started", () =>
            {
                OutLine("badns started", ConsoleColor.Green);
            });
        }

        [ConsoleAction("K", "Kill the Dns server")]
        public static void KillServer()
        {
            _server?.Stop();
            OutLine("badns stopped");
        }
        
    }
}