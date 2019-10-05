namespace Bam.Net.Services
{
    public class ChunkedFileInfo // Property compatible with ChunkedFileDescriptor
    {
        public string FileHash { get; set; }
        public string FileName { get; set; }
        public string Description { get; set; }
        public string OriginalDirectory { get; set; }
        public long FileLength { get; set; }
        public long ChunkCount { get; set; }
        /// <summary>
        /// The specified ChunkLength at the time
        /// of chunking
        /// </summary>
        public int ChunkLength { get; set; }
    }
}