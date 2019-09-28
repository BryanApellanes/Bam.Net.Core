using System;
using System.IO;
using Bam.Net.Application;
using Bam.Net.CoreServices;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Bam.Net.Server
{
    public class AppServerConf
    {
        public AppServerConf(ServerKinds serverKind)
        {
            ServerKind = serverKind;
            Init();
        }

        private static AppServerConf _default;
        private static readonly object _defaultLock = new object();
        public static AppServerConf Default
        {
            get { return _defaultLock.DoubleCheckLock(ref _default, () => new AppServerConf(ServerKinds.Bam)); }
        }
        
        [JsonConverter(typeof(StringEnumConverter))]
        public ServerKinds ServerKind { get; set; }
        public string Command { get; set; }
        public string[] Arguments { get; set; }

        public DaemonProcess ToDaemonProcess(string workingDirectory = "./")
        {
            return new DaemonProcess(Path.Combine(workingDirectory, Command), Arguments) {WorkingDirectory = workingDirectory};
        }
        
        private void Init()
        {
            switch (ServerKind)
            {
                case ServerKinds.Invalid:
                case ServerKinds.Bam:
                    Command = "bamweb";
                    Arguments = new string[] {"/S", $"/content:{GetContentPath()}", "/verbose"};
                    break;
                case ServerKinds.Node:
                    Command = "node";
                    Arguments = new string[] {"server.js"};
                    break;
                case ServerKinds.Python:
                    Command = "python";
                    Arguments = new string[] {"server.py"};
                    break;
                case ServerKinds.External:
                    Command = String.Empty;
                    Arguments = new string[] { };
                    break;
            }
        }

        private string GetContentPath()
        {
            switch (OSInfo.Current)
            {
                case OSNames.Invalid:
                case OSNames.Windows:
                    return "/c/bam/content";
                case OSNames.Linux:
                case OSNames.OSX:
                    return "/opt/bam/content";
                    break;
            }

            return "/opt/bam/content";
        }
    }
}