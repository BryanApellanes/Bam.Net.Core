using System.IO;

namespace Bam.Net.CoreServices.AssemblyManagement
{
    public interface INetStandardVersionResolver
    {
        string ResolveVersion(DirectoryInfo packageRefRoot);
    }
}