using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.Data;
using System.Data.Common;
using System.Data.Sql;
using System.Data.SqlClient;
using System.IO;
using Bam.Net.CommandLine;
using Bam.Net;
using Bam.Net.Testing;
using Bam.Net.Encryption;
using Bam.Net.Logging;
using Bam.Net.Configuration;
using Bam.Net.Server;
using System.Threading;

namespace Bam.Net.Application
{
    [Serializable]
    class Program : CommandLineTestInterface
    {
        static void Main(string[] args)
        {
            Resolver.Register();
            TryWritePid(true);

            IsolateMethodCalls = false;
            AddValidArgument("conf", "Specify an alternate config file to use.  Should be a json file containing an array of DaemonProcess definitions.");
            
            ExecuteMain(args, (a) =>
            {
                OutLineFormat("Error parsing arguments: {0}", ConsoleColor.Red, a.Message);
                Environment.Exit(1);
            });
        }

        [ConsoleAction("S", "Start bam daemon server")]
        public static void StartDaemonServer()
        {
            DaemonService.Start();
            Pause("Daemon server started");
        }

        [ConsoleAction("K", "Stop (Kill) bam daemon server")]
        public static void StopDaemonServer()
        {
            DaemonService.Stop();
            Pause("Daemon server stopped");
        }

        [ConsoleAction("R", "Restart bam daemon server")]
        public static void RestartDaemonServer()
        {
            DaemonService.Stop();            
            DaemonService.Start();
            Pause("Daemon server re-started");
        }
    }
}
