using Bam.Net.Application;

namespace Bam.Net
{
    public class RuntimeConfig
    {
        public string ReferenceAssembliesDir { get; set; }
        public string GenDir { get; set; }
        public string SysHomeDir { get; set; }
        public string SysDir { get; set; }
        public string ProcessHomeDir { get; set; }
    }
}