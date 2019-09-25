using Bam.Net.CoreServices;
using Bam.Net.Logging;
using Bam.Net.Server;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Bam.Net.ServiceProxy.Secure;

namespace Bam.Net.Application
{
    [Serializable]
    [ApiKeyRequired]
    [Proxy("processMonitor")]
    public class DaemonProcessMonitorService : ProxyableService
    {
        public DaemonProcessMonitorService(ILogger logger)
        {
            _monitors = new Dictionary<string, DaemonProcessMonitor>();
            Logger = logger;
        }

        private readonly FileInfo _daemonProcessConfig;
        public DaemonProcessMonitorService(ILogger logger, FileInfo daemonProcessConfig): this(logger)
        {
            _daemonProcessConfig = daemonProcessConfig;
        }
        
        public override object Clone()
        {
            DaemonProcessMonitorService clone = new DaemonProcessMonitorService(Logger);
            clone.CopyProperties(this);
            clone.CopyEventHandlers(this);
            return clone;
        }
        
        public void Start()
        {
            ConfigFile = GetDaemonProcessConfigFilePath();
            if (!ConfigFile.Exists)
            {
                Logger.AddEntry("{0} not found: {1}", ConfigFile.Name, ConfigFile.FullName);
            }
            else
            {
                Start(ConfigFile);
            }
        }

        public void Start(FileInfo configFile)
        {
            Processes = configFile.FullName.FromJsonFile<DaemonProcess[]>() ?? new DaemonProcess[] { };
            Expect.IsNotNull(Processes, $"No processes defined in {configFile.Name}");
            Logger.AddEntry("{0} processes in {1}", Processes.Length.ToString(), configFile.Name);
            Parallel.ForEach(Processes, StartProcess);
        }

        public static DaemonProcessMonitorService For(BamConf bamConf, ILogger logger = null)
        {
            logger = logger ?? bamConf.GetMainLogger(out Type ignore);
            DaemonProcess.SaveDefaultConfig(bamConf);
            return Start(logger, DaemonProcess.DefaultConfig);
        }
        
        public static DaemonProcessMonitorService Start(ILogger logger, FileInfo daemonProcessConfigFile)
        {
            DaemonProcessMonitorService svc = new DaemonProcessMonitorService(logger, daemonProcessConfigFile);
            svc.Start();
            return svc;
        }
        
        public void Stop()
        {
            Parallel.ForEach(_monitors.Keys, (key) =>
            {
                try
                {
                    Logger.AddEntry("Stopping {0}", key);
                    _monitors[key].Process.Kill();
                    Logger.AddEntry("Stopped {0}", key);
                }
                catch (Exception ex)
                {
                    Logger.AddEntry("Exception stopping process {0}: {1}", ex, key, ex.Message);
                }
            });          
        }

        public FileInfo ConfigFile { get; set; }

        public virtual List<DaemonProcessInfo> GetProcesses()
        {
            return MonitoredProcesses.Select(m => m.Process.CopyAs<DaemonProcessInfo>()).ToList();
        }

        public virtual CoreServiceResponse AddProcess(DaemonProcessInfo processInfo)
        {
            try
            {
                DaemonProcess process = processInfo.CopyAs<DaemonProcess>();
                List<DaemonProcess> current = new List<DaemonProcess>(Processes)
                {
                    process
                };
                Processes = current.ToArray();
                Processes.ToJsonFile(ConfigFile);
                StartProcess(process);
                return new CoreServiceResponse { Success = true };
            }
            catch(Exception ex)
            {
                return new CoreServiceResponse { Success = false, Message = ex.Message };
            }
        }

        public virtual CoreServiceResponse KillProcess(string name)
        {
            try
            {
                if (_monitors.ContainsKey(name))
                {
                    _monitors[name].Process.Kill();
                    return new CoreServiceResponse { Success = true };
                }
                return new CoreServiceResponse { Success = false, Message = $"Process with the specified name was not found: {name}" };
            }
            catch (Exception ex)
            {
                return new CoreServiceResponse { Success = false, Message = ex.Message };
            }
        }

        public List<DaemonProcessMonitor> MonitoredProcesses => _monitors.Values?.ToList() ?? new List<DaemonProcessMonitor>();

        public DaemonProcess[] Processes { get; set; }

        readonly Dictionary<string, DaemonProcessMonitor> _monitors;
        private void StartProcess(DaemonProcess process)
        {
            try
            {
                string key = process.ToString();
                Logger.AddEntry("Starting {0}", key);
                process.Subscribe(Logger);
                _monitors.Add(key, DaemonProcessMonitor.Start(process));
            }
            catch (Exception ex)
            {
                Logger.AddEntry("Error starting process {0}: {1}", ex, process?.ToString(), ex.Message);
            }
        }
        
        private FileInfo GetDaemonProcessConfigFilePath()
        {
            if (_daemonProcessConfig == null)
            {
                string configRoot = Path.Combine(ServiceConfig.ContentRoot, "conf");
                string fileName = $"{nameof(DaemonProcess).Pluralize()}.json";
                return new FileInfo(Path.Combine(configRoot, fileName));
            }

            return _daemonProcessConfig;
        }
    }
}
