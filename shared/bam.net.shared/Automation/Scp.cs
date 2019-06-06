using System;
using System.ComponentModel.Design;
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
            return $"{Path} {source} {destination}".Run();
        }
        
        public static void SetPath()
        {
            string fileName = "scp";
            Path = OSInfo.TryGetPath(fileName, out string path) ? path : OSInfo.DefaultToolPath(fileName);
        }
    }
}