using System.IO;
using Bam.Net.Services;

namespace Bam.Net.Application.Files
{
    public interface IFileSystem : IFileManager
    {
        Stream ReadFile(FileIdentifier fileIdentifier);
        FileInfo GetFile(FileIdentifier fileIdentifier);

        void WriteFile(FileIdentifier fileIdentifier, byte[] content);
    }
}