using Bam.Net.CommandLine;

namespace Bam.Net.Automation
{
    public class Nmap
    {
        static Nmap()
        {
            SetPath();
        }

        public static string Path { get; set; }

        public static ProcessOutput Run(string computerName, string options = "", int timeout = 600000)
        {
            return Path.ToStartInfo($"{computerName} {options}").RunAndWait();
        }
        
        private static void SetPath()
        {
            string fileName = "nmap";
            Path = OSInfo.TryGetPath(fileName, out string path) ? path : OSInfo.DefaultToolPath(fileName);
        }
    }
}