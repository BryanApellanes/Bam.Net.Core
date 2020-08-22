/*
	Copyright © Bryan Apellanes 2015  
*/
using Bam.Net.CommandLine;
using Bam.Net.Configuration;
using Bam.Net.Logging;
using Bam.Net.Server;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using Bam.Net.Application;
using Bam.Net.Testing;

namespace Bam.Net.System
{
    public class DaemonService : CommandLineTool
    { 
        static DaemonServer _server;
        static object _serverLock = new object();
        public static DaemonServer Server
        {
            get
            {
                return _serverLock.DoubleCheckLock(ref _server, () =>
                {
                    ILogger logger = GetDaemonServiceLogger();
                    try
                    {
                        ProcessMonitorService = ResolveProcessMonitorService(logger);
                        DaemonServer server = new DaemonServer(BamConf, ProcessMonitorService, logger)
                        {
                            HostPrefixes = new HashSet<HostPrefix>(GetConfiguredHostPrefixes()),
                            MonitorDirectories = DefaultConfiguration.GetAppSetting("MonitorDirectories").DelimitSplit(",", ";")
                        };
                        logger.AddEntry("Created Server of Type {0}: {1}", typeof(DaemonServer).FullName, server.PropertiesToString());
                        return server;
                    }
                    catch (Exception ex)
                    {
                        logger.AddEntry("Exception occurred: {0}", ex, ex.Message);
                    }
                    return null;
                });
            }
        }
        
        public static void Start()
        {
            try
            {
                Log.AddLogger(GetDaemonServiceLogger());
                Log.AddEntry("{0} starting", "BamDaemonService");
                Server.Start();
                ProcessMonitorService.Start();
            }
            catch (Exception ex)
            {
                Log.AddEntry("Error starting bam daemon service: {0}", ex, ex.Message);
            }
        }

        public static void Stop()
        {
            try
            {
                Log.AddEntry("{0} stopping", "BamDaemonService");
                Server.Stop();
                ProcessMonitorService.Stop();
            }
            catch (Exception ex)
            {
                Log.AddEntry("Error stopping {0}: {1}", LogEventType.Warning, "BamDaemonService", ex.Message);
            }
        }

        private static HostPrefix[] GetConfiguredHostPrefixes()
        {
            return ServiceConfig.GetConfiguredHostPrefixes();
        }

        static ILogger _daemonServiceLogger;
        static readonly object _daemonServiceLoggerLock = new object();
        private static ILogger GetDaemonServiceLogger()
        {
            return _daemonServiceLoggerLock.DoubleCheckLock(ref _daemonServiceLogger, () => ServiceConfig.GetMultiTargetLogger(Log.CreateLogger("Console")));
        }
        private static BamConf _bamConf;
        private static readonly object _bamConfLock = new object();

        private static BamConf BamConf
        {
            get { return _bamConfLock.DoubleCheckLock(ref _bamConf, () => BamConf.Load(ServiceConfig.ContentRoot)); }
        }

        private static DaemonProcessMonitorService _daemonProcessMonitorService;
        private static readonly object _daemonProcessMonitorServiceLock = new object();

        private static DaemonProcessMonitorService ResolveProcessMonitorService(ILogger logger)
        {
            if (ParsedArguments.Current.Contains("conf"))
            {
                FileInfo configFile = new FileInfo(ParsedArguments.Current["conf"]);
                if (!configFile.Exists)
                {
                    OutLineFormat("Specified conf file does not exist ({0})", ConsoleColor.Red, configFile.FullName);
                    Exit(1);
                }
                return _daemonProcessMonitorServiceLock.DoubleCheckLock(ref _daemonProcessMonitorService, () => DaemonProcessMonitorService.Start(logger, configFile));
            }
            return _daemonProcessMonitorServiceLock.DoubleCheckLock<DaemonProcessMonitorService>(ref _daemonProcessMonitorService, () => DaemonProcessMonitorService.For(BamConf, logger));
        }

        private static DaemonProcessMonitorService ProcessMonitorService { get; set; }
    }
}
