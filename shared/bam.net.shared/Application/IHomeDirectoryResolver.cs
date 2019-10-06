using System.Collections.Generic;

namespace Bam.Net.Application
{
    public interface IHomeDirectoryResolver
    {
        string ResolveHomeDirectory();

        string GetHomePath(string path);
    }
}