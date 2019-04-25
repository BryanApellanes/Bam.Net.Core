using Bam.Net.CoreServices;
using Bam.Net.Testing;

namespace Bam.Net.Services
{
    public class CommandLineServiceInterface : CommandLineTestInterface
    {
        public CommandLineServiceInterface()
        {
            ServiceHostSettings = new ServiceHostSettings();
        }
        public ServiceHostSettings ServiceHostSettings { get; set; }
    }
}