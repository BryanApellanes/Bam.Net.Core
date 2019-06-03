using Bam.Net.CoreServices.ApplicationRegistration.Data;

namespace Bam.Net.Application.Verbs
{
    public class CopyContext
    {
        public string LocalFilePath { get; set; }
        public string RemoteFilePath { get; set; }
        public Machine Machine { get; set; }
    }
}