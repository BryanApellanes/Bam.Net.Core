using Bam.Net.Application;

namespace Bam.Net
{
    public class RuntimeConfig
    {
        public string ReferenceAssembliesDir { get; set; }
        public string GenDir { get; set; }
        public string BamHomeDir { get; set; }
        public string BamDir { get; set; }
        public string ProcessHomeDir { get; set; }
    }
}