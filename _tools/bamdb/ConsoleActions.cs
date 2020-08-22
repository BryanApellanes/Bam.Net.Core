using Bam.Net.CommandLine;
using Bam.Net.Configuration;
using Bam.Net.Data.Repositories;
using Bam.Net.Server;
using Bam.Net.Testing;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Bam.Net.Data;
using Bam.Net.Data.Dynamic;
using Bam.Net.Data.Npgsql;
using Bam.Shell;
using Bam.Shell.CodeGen;

namespace Bam.Net.Application
{
    [Serializable]
    public class ConsoleActions : CommandLineTool
    {
        static string contentRootConfigKey = "ContentRoot";
        static string defaultContentRoot = BamHome.Content;
        static BamDbServer bamDbServer;

        [ConsoleAction("S", "Start the BamDb server")]
        public void StartConsole()
        {
            StartBamDbServer(GetLogger(), GetRepository());
            Pause("BamDb is running");
        }

        [ConsoleAction("K", "Kill the BamDb server")]
        public void StopConsole()
        {
            if (bamDbServer != null)
            {
                bamDbServer.Stop();
                Pause("BamDb stopped");
            }
            else
            {
                OutLine("BamDb server not running");
            }
        }
        public const string AppDataFolderName = "AppData";
        
        static DirectoryInfo _appData;
        static object _appDataLock = new object();
        static DirectoryInfo AppData
        {
            get
            {
                return _appDataLock.DoubleCheckLock(ref _appData, () => new DirectoryInfo(Path.Combine(Environment.CurrentDirectory, AppDataFolderName)));
            }
        }
                
        [ConsoleAction("import", "Import data files from AppData (csv, json and yaml)")]
        public void ImportDataFiles()
        {
            DynamicDataManager mgr = new DynamicDataManager();
            DirectoryInfo appData = AppData;
            if (Arguments.Contains("AppData"))
            {
                appData = new DirectoryInfo(Arguments["appData"]);
            }
            mgr.ProcessDataFiles(appData);
        }

        [ConsoleAction("printConfig")]
        public void PrintConfig()
        {
            Config config = Config.Current;
            OutLine(config.File.FullName);
            StringBuilder msg = new StringBuilder();
            foreach(string key in config.AppSettings.Keys)
            {
                msg.AppendLine($"{key} = {config[key]}");
            }

            OutLine(msg.ToString());
        }

        public static void StartBamDbServer(ConsoleLogger logger, IRepository repo)
        {
            BamConf conf = BamConf.Load(GetArgument("content", "Enter the path to the content root").Or(defaultContentRoot));
            bamDbServer = new BamDbServer(conf, logger, repo)
            {
                HostPrefixes = new HashSet<HostPrefix>(HostPrefix.FromBamProcessConfig()),
                MonitorDirectories = Config.Current["MonitorDirectories"].DelimitSplit(",", ";")
            };
            bamDbServer.Start();
        }

        private static IRepository GetRepository()
        {
            return new DaoRepository();
        }
        
        private static ConsoleLogger GetLogger()
        {
            ConsoleLogger logger = new ConsoleLogger {AddDetails = false, UseColors = true};
            logger.StartLoggingThread();
            return logger;
        }
    }
}