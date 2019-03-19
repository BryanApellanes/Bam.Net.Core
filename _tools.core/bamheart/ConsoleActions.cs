using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Bam.Net.CommandLine;
using Bam.Net;
using Bam.Net.Testing;
using Bam.Net.Encryption;
using Bam.Net.Server;
using Bam.Net.Configuration;
using Bam.Net.ServiceProxy;
using Bam.Net.Logging;
using System.Reflection;
using System.Security.Permissions;
using Bam.Net.Services;
using Bam.Net.CoreServices;
using System.Threading;
using Bam.Net.Automation;

namespace Bam.Net.Application
{
    [Serializable]
    public class ConsoleActions : CommandLineTestInterface
    {        
        static ServiceProxyServer server;

        [ConsoleAction("killHeartServer", "Kill the Heart server")]
        public static void StopServer()
        {
            if (server != null)
            {
                server.Stop();
                OutLine("Heart stopped", ConsoleColor.Yellow);
            }
            else
            {
                OutLine("Heart server not running");
            }
        }

        [ConsoleAction("startHeartServer", "Start the Heart server")]
        public static void StartServerAndPause()
        {
            ServiceRegistry serviceRegistry = StartServer(out HostPrefix[] hostPrefixes);
            hostPrefixes.Each(h => OutLine(h.ToString(), ConsoleColor.Cyan));
            Pause($"Heart server is serving service registry {serviceRegistry.Name}");
        }

        [ConsoleAction("deployHeartServer", "Deploy the Heart server to a kubernetes cluster")]
        public static void DeployServer()
        {
            string serviceInfo = Arguments.Contains("deployHeartServer") ? Arguments["deployHeartServer"] : string.Empty;
            if (string.IsNullOrEmpty(serviceInfo))
            {
                serviceInfo = Arguments.Contains("dhs") ? Arguments["dhs"] : string.Empty;
            }

            if(string.IsNullOrEmpty(serviceInfo))
            {
                OutLineFormat("Please specify the path to the service info file.");
                Exit(1);
            }
            FileInfo svcInfoFile = new FileInfo(serviceInfo);
            if (!svcInfoFile.Exists)
            {
                OutLineFormat("Specified service info file was not found: {0}", svcInfoFile.FullName);
                Exit(1);
            }
            ConsoleLogger logger = new ConsoleLogger
            {
                AddDetails = false
            };
            logger.Info("Starting deployment");

            throw new NotImplementedException("This service needs to be updated to deploy to a kubernetes cluster");
        }

        internal static ServiceRegistry StartServer(out HostPrefix[] hostPrefixes)
        {
            HostPrefix[] prefixes = GetConfiguredHostPrefixes();
            ILogger logger = GetLogger();
            Log.Default = logger;
            ServiceRegistry serviceRegistry = CoreServiceRegistryContainer.Create();
            server = serviceRegistry.Serve(prefixes, logger);
            hostPrefixes = prefixes;
            return serviceRegistry;
        }

        public static HostPrefix[] GetConfiguredHostPrefixes()
        {
            int port = int.Parse(DefaultConfiguration.GetAppSetting("Port", "80"));
            bool ssl = DefaultConfiguration.GetAppSetting("Ssl").IsAffirmative();
            List<HostPrefix> results = new List<HostPrefix>();
            foreach(string hostName in DefaultConfiguration.GetAppSetting("HostNames").Or("localhost").DelimitSplit(",", true))
            {
                HostPrefix hostPrefix = new HostPrefix()
                {
                    HostName = hostName,
                    Port = port,
                    Ssl = ssl
                };
                results.Add(hostPrefix);
            }
            return results.ToArray();
        }

        private static ILogger GetLogger()
        {
            ConsoleLogger logger = new ConsoleLogger()
            {
                AddDetails = false,
                UseColors = true
            };
            logger.StartLoggingThread();
            Log.AddLogger(logger);
            return Log.Default;
        }
    }
}