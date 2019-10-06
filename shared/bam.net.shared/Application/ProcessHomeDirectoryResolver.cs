using System.IO;
using Bam.Net.CommandLine;

namespace Bam.Net.Application
{
    public class ProcessHomeDirectoryResolver : IHomeDirectoryResolver
    {
        public string ResolveHomeDirectory()
        {
            return RuntimeSettings.ProcessProfileDir;
        }

        public string GetHomePath(string path)
        {
            while (path.StartsWith("/") || path.StartsWith("~"))
            {
                path = path.TruncateFront(1);
            }
            return Path.Combine(ResolveHomeDirectory(), path);
        }
    }
}