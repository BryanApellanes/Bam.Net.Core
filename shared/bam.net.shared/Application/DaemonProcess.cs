using Bam.Net.CommandLine;
using Bam.Net.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Bam.Net.Server;

namespace Bam.Net.Application
{
    public class DaemonProcess: Loggable
    {
        public DaemonProcess()
        {
            _arguments = string.Empty;
            WorkingDirectory = "./";
            StandardOutSoFar = string.Empty;
            StandardErrorSoFar = string.Empty;
            StandardOut += (o, a) =>
            {
                DaemonProcessEventArgs args = (DaemonProcessEventArgs)a;
                StandardOutSoFar += $"\r\n{args.ConsoleMessage}";
                StandardOutLineCount++;
            };

            ErrorOut += (o, a) =>
            {
                DaemonProcessEventArgs args = (DaemonProcessEventArgs)a;
                StandardErrorSoFar += $"\r\n{args.ConsoleMessage}";
                ErrorOutLineCount++;
            };
        }

        public DaemonProcess(string commandLine) : this()
        {
            string[] split = commandLine.Split(new char[] { ' ' }, 2);
            FileName = split[0];
            if (split.Length > 1)
            {
                Arguments = split[1];
            }
        }

        public DaemonProcess(string command, params string[] args) : this()
        {
            FileName = command;
            ArgumentsArray = args;
        }

        public static FileInfo DefaultConfig
        {
            get
            {
                string configRoot = Path.Combine(ServiceConfig.ContentRoot, "conf");
                string fileName = $"{nameof(DaemonProcess).Pluralize()}.json";
                return new FileInfo(Path.Combine(configRoot, fileName));
            }
        }

        public static void SaveDefaultConfig(BamConf bamConf)
        {
            ForBamConf(bamConf).ToArray().ToJson(true).SafeWriteToFile(DefaultConfig.FullName, true);
        }
        
        public static DaemonProcess[] FromDefaultConfig()
        {
            return DefaultConfig.FullName.FromJsonFile<DaemonProcess[]>() ?? new DaemonProcess[] { };
        }

        public static DaemonProcess[] FromBamConf(BamConf bamConf)
        {
            SaveDefaultConfig(bamConf);
            return ForBamConf(bamConf).ToArray();
        }
        
        public static IEnumerable<DaemonProcess> ForBamConf(BamConf bamConf)
        {
            Args.ThrowIfNull(bamConf, "bamConf");

            yield return ForDefaultServerConf();

            foreach (DaemonProcess externallyServedApp in ForExternallyServedApps(bamConf))
            {
                yield return externallyServedApp;
            }
        }

        public static DaemonProcess ForDefaultServerConf()
        {
            return AppServerConf.Default.ToDaemonProcess(Path.Combine(BamPaths.CurrentRuntimeToolkitPath, "bamweb"));
        }
        
        public static DaemonProcess[] ForExternallyServedApps(BamConf bamConf)
        {
            Args.ThrowIfNull(bamConf, "bamConf");

            return bamConf.AppsServedExternally.Select(ToServe).ToArray();
        }
        
        /// <summary>
        /// Return a DaemonProcess configured to mirror the ServerConf section of the specified AppConf.
        /// </summary>
        /// <param name="appConf"></param>
        /// <returns></returns>
        public static DaemonProcess ToServe(AppConf appConf)
        {
            Args.ThrowIfNull(appConf.ServerConf, "appConf.ServerConf");
            
            AppServerConf serverConf = appConf.ServerConf;
            return new DaemonProcess(serverConf.Command, serverConf.Arguments)
            {
                WorkingDirectory = appConf.AppRoot.Root
            };
        }
        
        [Verbosity(VerbosityLevel.Information, MessageFormat = "StandardOut: {ConsoleMessage}")]
        public event EventHandler StandardOut;

        [Verbosity(VerbosityLevel.Information, MessageFormat = "ErrorOut: {ConsoleMessage}")]
        public event EventHandler ErrorOut;        

        string _name;
        public string Name
        {
            get => _name ?? FileName;
            set => _name = value;
        }

        public string FileName { get; set; }

        private string _arguments;

        public string Arguments
        {
            get => _arguments;
            set
            {
                _arguments = value;
                ArgumentsArray = _arguments.DelimitSplit(" ");
            }
        }
        public string WorkingDirectory { get; set; }

        private string[] _argumentsArray;
        protected string[] ArgumentsArray
        {
            get => _argumentsArray;
            set
            {
                _argumentsArray = value;
                string argsTemp = string.Join(' ',_argumentsArray.Select(arg => arg.Contains(" ") ? $"\"{arg}\"" : arg).ToArray());
                if (!_arguments.Equals(argsTemp))
                {
                    _arguments = argsTemp;
                }
            }
        }
        
        [JsonIgnore]
        public ProcessOutput ProcessOutput { get; set; }

        public void Kill()
        {
            ProcessOutput.Process.Kill();
        }

        public ProcessOutput Start(EventHandler onExit = null)
        {
            try
            {
                ExitHandler = onExit;
                ProcessStartInfo startInfo = new ProcessStartInfo(FileName, Arguments)
                {
                    WorkingDirectory = WorkingDirectory,
                    UseShellExecute = false,
                    ErrorDialog = false,
                    CreateNoWindow = true
                };
                Log.AddEntry("Starting {0} working directory: {1}", FileName, WorkingDirectory);
                startInfo.RedirectStandardOutput = true;
                startInfo.RedirectStandardError = true;
                ProcessOutputCollector collector = new ProcessOutputCollector((data) => FireEvent(StandardOut, new DaemonProcessEventArgs { DaemonProcess = this, ConsoleMessage = data }), (error) => FireEvent(ErrorOut, new DaemonProcessEventArgs { DaemonProcess = this, ConsoleMessage = error }));
                ProcessOutput = startInfo.Run(ExitHandler, collector);
                return ProcessOutput;
            }
            catch (Exception ex)
            {
                FireEvent(ErrorOut, new DaemonProcessEventArgs { DaemonProcess = this, ConsoleMessage = ex.Message });
                return null;
            }
        }

        public string ToCommandLine()
        {
            return $"{FileName} {Arguments}";
        }

        public override string ToString()
        {
            return $"{WorkingDirectory}:{Name} > {FileName} {Arguments}";
        }

        internal int StandardOutLineCount { get; set; }
        internal int ErrorOutLineCount { get; set; }

        internal string StandardOutSoFar { get; set; }
        internal string StandardErrorSoFar { get; set; }

        protected int RetryCount { get; set; }
        protected EventHandler ExitHandler { get; set; }        
    }
}
