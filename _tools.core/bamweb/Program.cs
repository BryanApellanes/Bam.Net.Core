/*
	Copyright © Bryan Apellanes 2015  
*/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using Bam.Net;
using Bam.Net.CommandLine;
using Bam.Net.Logging;
using Bam.Net.Incubation;
using Bam.Net.Configuration;
using System.IO;
using Bam.Net.Yaml;
using Bam.Net.Testing;
using System.Reflection;
using Bam.Net.UserAccounts;
using Bam.Net.Data;
using Bam.Net.ServiceProxy;
using Bam.Net.Server;

namespace Bam.Net.Application
{
    [Serializable]
    class Program : CommandLineTestInterface
    {
        static void Main(string[] args)
        {
            TryWritePid(true);
            IsolateMethodCalls = false;
			Type type = typeof(Program);
            AddValidArgument("content", false, false, "The path to the content root directory");
			AddSwitches(type);
			DefaultMethod = type.GetMethod(nameof(Interactive));
            Initialize(args);

            if (Arguments.Length > 0)
			{
				ExecuteSwitches(Arguments, type, null, null);
			}
			else
			{
				Interactive();
			}
        }
        
        static BamServer _server;
        static object _serverLock = new object();
        public static BamServer Server
        {
            get
            {
                return _serverLock.DoubleCheckLock(ref _server, () => new BamServer(BamConf.Load(GetArgument("content", "Enter the path to the content root"))));
            }
        }

        [ConsoleAction("S", "Start default server")]
        public static void StartDefaultServer()
        {
            ConsoleLogger logger = new ConsoleLogger() { AddDetails = false };
            Server.Subscribe(logger);
            Log.Default = logger;
            LogResponses();
            Server.Start();
            BamConf conf = Server.GetCurrentConf();
            StringBuilder configurationMessage = new StringBuilder();
            configurationMessage.AppendLine("***");
            foreach(AppConf appConfig in conf.AppConfigs)
            {
                configurationMessage.AppendLine(appConfig.Name);
                foreach(HostPrefix hostPrefix in appConfig.Bindings)
                {
                    configurationMessage.AppendFormat("\t{0}\r\n", hostPrefix.ToString());
                }
                configurationMessage.AppendLine();
            }
            configurationMessage.AppendLine(new string('*', 40));
            logger.AddEntry(configurationMessage.ToString());
			Pause("Default server started");
        }

        [ConsoleAction("K", "Stop (Kill) default server")]
        public static void StopDefaultServer()
        {
            Server.Stop();
			_server = null;
			Pause("Default server stopped");
        }

        [ConsoleAction("R", "Restart default server")]
        public static void RestartDefaultServer()
        {
            Server.Stop();
            _server = null; // force reinitialization
            Server.Start();
			Pause("Default server re-started");
        }

        [ConsoleAction]
        public static void LogResponses()
        {
            Server.Responded += (s, res, req) =>
            {
                StringBuilder messageFormat = new StringBuilder();
                messageFormat.Append("Responded: ClientIp={0}, Path={1}");
                messageFormat.Append("\tUserAgent: {2}");
                messageFormat.Append("\tUserLanguages: {3}");
                Logger.Info(messageFormat.ToString(), req?.GetClientIp() ?? "[null]", req?.Url?.ToString() ?? "[null]", req?.UserAgent, string.Join(",", req?.UserLanguages));
            };
        }
    }
}
