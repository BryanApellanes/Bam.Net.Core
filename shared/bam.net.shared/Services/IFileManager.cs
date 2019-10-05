using System.IO;

namespace Bam.Net.Services
{
    public interface IFileManager
    {
        void StoreFiles(DirectoryInfo directoryInfo);
        void StoreFiles(string description, params FileInfo[] files);
        FileInfo RestoreFile(string fileNameOrHash);
        ChunkedFileInfo[] ListFiles(string originalDirectory);
    }
}