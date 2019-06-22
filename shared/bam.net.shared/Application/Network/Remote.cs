using System;
using System.Collections.Generic;
using System.Linq;
using Bam.Net.Automation;
using Bam.Net.Testing;

namespace Bam.Net.Application.Network
{
    public class Remote
    {
        public Remote() : this(null)
        {

        }

        public Remote(string hostName)
        {
            HostName = hostName;
        }

        public static Remote For(string hostName)
        {
            return new Remote(hostName);
        }

        string _scanReport;
        object _scanReportLock = new object();
        public string ScanReport
        {
            get
            {
                return _scanReportLock.DoubleCheckLock(ref _scanReport, () =>
                {
                    if (!string.IsNullOrEmpty(HostName))
                    {
                        return Nmap.Run(HostName, "-O").StandardOutput;
                    }

                    return null;
                });
            }
        }

        public string HostName { get; set; }

        /// <summary>
        /// The admin share of the remote host if it is running windows.  This is invalid if the remote is NOT running Windows OS.
        /// </summary>
        [Exclude]
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
        
        [Exclude]
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

        protected List<string> GetScanReportLines()
        {
            List<string> lines = new List<string>();
            foreach (string line in ScanReport?.DelimitSplit("\r", "\n"))
            {
                lines.Add(line);
            }

            return lines;
        }
    }
}