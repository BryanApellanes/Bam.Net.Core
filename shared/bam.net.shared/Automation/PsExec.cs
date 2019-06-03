using Bam.Net.CommandLine;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bam.Net.Automation
{
    public class PsExec
    {
        static PsExec()
        {
            SetPath();
        }

        public static string Path { get; set; }

        public static ProcessOutput Run(string computerName, string command, int timeout = 600000)
        {
            return $"{Path} \\\\{computerName} {command}".Run(timeout);
        }

        public static ProcessOutput Run(string computerName, string command, Action<string> standout, Action<string> errorout, int timeout = 600000)
        {
            return $"{Path} \\\\{computerName} {command}".Run(standout, errorout, timeout);
        }

        private static void SetPath()
        {
            string fileName = "PsExec.exe";
            Path = OSInfo.TryGetPath(fileName, out string path) ? path : OSInfo.DefaultToolPath(fileName);
        }
    }
}
