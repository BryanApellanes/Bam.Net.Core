using System;
using Bam.Net.CommandLine;

namespace Bam.Net.Automation
{
    public class Scp
    {
        static Scp()
        {
            SetPath();
        }
        
        public static string Path { get; set; }

        public static ProcessOutput Run(string source, string destination)
        {
            throw new NotImplementedException();
        }
        
        public static void SetPath()
        {
            string fileName = "Scp";
            Path = OSInfo.TryGetPath(fileName, out string path) ? path : OSInfo.DefaultToolPath(fileName);
        }
    }
}