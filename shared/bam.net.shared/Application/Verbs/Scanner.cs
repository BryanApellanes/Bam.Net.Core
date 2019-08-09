using System;
using System.Collections.Generic;

namespace Bam.Net.Application.Verbs
{
    public class Scanner<T> : Scanner
    {
        Dictionary<string, T> _scanResults;

        public Scanner()
        {
            _scanResults = new Dictionary<string, T>();
        }
        
        public new Func<string, T> Scan { get; set; }
        public new Dictionary<string, T> ScanResults { get; }
        
        public new T GetResult(string hostName)
        {
            if (!ScanResults.ContainsKey(hostName))
            {
                ScanResults.AddMissing(hostName, Scan(hostName));
            }

            return ScanResults[hostName];
        }
    }

    public class Scanner
    {
        Dictionary<string, object> _scanResults;

        public Scanner()
        {
            _scanResults = new Dictionary<string, object>();
        }
        
        public Func<string, object> Scan { get; set; }
        public Dictionary<string, object> ScanResults { get; }
        
        public object GetResult(string hostName)
        {
            if (!ScanResults.ContainsKey(hostName))
            {
                ScanResults.AddMissing(hostName, Scan(hostName));
            }

            return ScanResults[hostName];
        }
    }
}