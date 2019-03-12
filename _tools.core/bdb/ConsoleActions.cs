using Bam.Net.CommandLine;
using Bam.Net.Configuration;
using Bam.Net.Data.Repositories;
using Bam.Net.Server;
using Bam.Net.Testing;
using System;
using System.Collections.Generic;

namespace Bam.Net.Application
{
    [Serializable]
    public class ConsoleActions : CommandLineTestInterface
    {
        static string contentRootConfigKey = "ContentRoot";
        static string defaultContentRoot = BamPaths.ContentPath;
        static BamDbServer trooServer;

        [ConsoleAction("startTrooServer", "Start the troo server")]
        public void StartConsole()
        {
            StartBamDbServer(GetLogger(), GetRepository());
            Pause("Troo is running");
        }

        [ConsoleAction("killTrooServer", "Kill the troo server")]
        public void StopConsole()
        {
            if (trooServer != null)
            {
                trooServer.Stop();
                Pause("Troo stopped");
            }
            else
            {
                OutLine("Troo server not running");
            }
        }

        public static void StartBamDbServer(ConsoleLogger logger, IRepository repo)
        {
            BamConf conf = BamConf.Load(DefaultConfiguration.GetAppSetting(contentRootConfigKey).Or(defaultContentRoot));
            trooServer = new BamDbServer(conf, logger, repo)
            {
                HostPrefixes = new HashSet<HostPrefix>(HostPrefix.FromDefaultConfiguration()),
                MonitorDirectories = DefaultConfiguration.GetAppSetting("MonitorDirectories").DelimitSplit(",", ";")
            };
            trooServer.Start();
        }

        private static IRepository GetRepository()
        {
            return new DaoRepository();
        }
        private static ConsoleLogger GetLogger()
        {
            ConsoleLogger logger = new ConsoleLogger();
            logger.AddDetails = false;
            logger.UseColors = true;
            logger.StartLoggingThread();
            return logger;
        }
    }
}