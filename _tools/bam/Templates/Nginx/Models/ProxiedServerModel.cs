namespace Bam.Templates.Nginx.Models
{
    public class ProxiedServerModel 
    {
        public int ExternalPort { get; set; }
        public string SpaceSeparatedHostNames { get; set; }
        public string InternalUrl { get; set; }
        public int InternalPort { get; set; }
    }
}