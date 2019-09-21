using System;
using System.Diagnostics;
using System.IO;
using Bam.Net;

namespace Bam.Net.Server
{
    public class HostProcessInfo
    {
        public HostProcessInfo()
        {
            Process currentProcess = Process.GetCurrentProcess();
            ProcessId = currentProcess.Id;
            CommandLineArgs = string.Join(" ", Environment.GetCommandLineArgs());
            MainExe = Path.Combine(currentProcess.MainModule.FileName);
        }
        public int ProcessId { get; set; }
        public string CommandLineArgs { get; set; }
        public string MainExe { get; set; }
        public ProcessMode ProcessMode { get; set; }
    }
}