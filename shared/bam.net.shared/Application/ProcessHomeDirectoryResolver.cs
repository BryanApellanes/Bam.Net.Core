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
            while (path.StartsWith("~"))
            {
                path = path.TruncateFront(1);
            }
            while (path.StartsWith("/"))
            {
                path = path.TruncateFront(1);
            }
            string result = Path.Combine(ResolveHomeDirectory(), path);
            return result;
        }
    }
}