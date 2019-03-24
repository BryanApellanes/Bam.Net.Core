using System.Collections.Generic;

namespace Bam.Shell.Jobs
{
    public class ProviderArguments
    {
        public string ProviderType { get; set; }
        public string ProviderContextTarget { get; set; }
        public Dictionary<string, string> Values { get; set; } 
    }
}