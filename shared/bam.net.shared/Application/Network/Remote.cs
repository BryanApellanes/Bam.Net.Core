using System;
using System.Collections.Generic;
using System.Linq;
using Bam.Net.Automation;

namespace Bam.Net.Application.Network
{
    public class Remote
    {
        public Remote()
        {
            ScanReport = new Lazy<string>(() => Nmap.Run(HostName, "-O").StandardOutput);
        }
        public string HostName { get; set; }

        /// <summary>
        /// The admin share of the remote host if it is running windows.  This is invalid if the remote is NOT running Windows OS.
        /// </summary>
        public string AdminShare
        {
            get
            {
                if (OSInfo.Current == OSNames.Windows)
                {
                    return $"\\\\{HostName}\\C$";
                }
                else
                {
                    return $"//{HostName}/c$";
                }
            }
        }
        
        protected Lazy<string> ScanReport { get; set; }

        public OSNames OS
        {
            get
            {
                string osDetail = GetScanReportLines().FirstOrDefault(line =>
                    line.StartsWith("OS details", StringComparison.InvariantCultureIgnoreCase));
                if (osDetail.Contains("Windows", StringComparison.InvariantCultureIgnoreCase))
                {
                    return OSNames.Windows;
                }

                if (osDetail.Contains("Linux", StringComparison.InvariantCultureIgnoreCase))
                {
                    return OSNames.Linux;
                }

                return OSNames.OSX;
            }
        }

        protected IEnumerable<string> GetScanReportLines()
        {
            foreach (string line in ScanReport?.Value.DelimitSplit("\r", "\n"))
            {
                yield return line;
            }
        }
    }
}