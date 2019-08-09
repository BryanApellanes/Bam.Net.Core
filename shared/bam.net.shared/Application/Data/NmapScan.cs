using Bam.Net.Application.Network;
using Bam.Net.Data.Repositories;

namespace Bam.Net.Application.Data
{
    public class NmapScan : NamedAuditRepoData
    {
        public string Raw { get; set; }

        public static NmapScan For(string hostName)
        {
            return new NmapScan()
            {
                Raw = new Remote(hostName).ScanReport
            };
        }
    }
}