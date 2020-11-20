using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Bam.Net.CommandLine;

namespace Bam.Net
{
    /// <summary>
    /// Provides information about the current process.
    /// </summary>
    public class ProcessInfo
    {
        private readonly Process _currentProcess;
        public ProcessInfo()
        {
            _currentProcess = Process.GetCurrentProcess();
            MachineName = Environment.MachineName;
            ProcessId = _currentProcess.Id;
            StartTime = _currentProcess.StartTime;
            FilePath = new FileInfo(_currentProcess.MainModule.FileName).FullName;
            EntryAssembly = Assembly.GetEntryAssembly().GetFilePath();
            CommandLineArgs = Environment.GetCommandLineArgs();
            CommandLine = Environment.CommandLine;
        }

        private static ProcessInfo _current;
        static readonly object _currentLock = new object();
        public static ProcessInfo Current
        {
            get { return _currentLock.DoubleCheckLock(ref _current, () => new ProcessInfo()); }
        }
        
        public string MachineName { get; set; }
        public int ProcessId { get; set; }
        public DateTime StartTime { get; set; }
        public string FilePath { get; set; }
        public string EntryAssembly { get; set; }
        public string CommandLine { get; set; }

        public string[] CommandLineArgs { get; set; }

        public ProcessStartInfo ToStartInfo()
        {
            return ToStartInfo(CommandLineArgs);
        }
        
        public ProcessStartInfo ToStartInfo(params string[] commandLineArgs)
        {
            commandLineArgs ??= CommandLineArgs;
            ProcessStartInfo startInfo = _currentProcess.GetStartInfo(commandLineArgs);
            startInfo.FileName = FilePath;
            startInfo.Arguments = string.Join(" ", commandLineArgs);
            return startInfo;
        }

        public ProcessOutput ReRun(params string[] newCommandLineArgs)
        {
            ProcessInfo copy = this.CopyAs<ProcessInfo>();
            copy.CommandLineArgs = newCommandLineArgs;
            return copy.Run();
        }
        
        public ProcessOutput Run()
        {
            return ToStartInfo().Run();
        }
        
        public ProcessOutput Run(params string[] commandLineArgs)
        {
            return ToStartInfo(commandLineArgs).Run();
        }
    }
}
