using Amazon.S3;
using Bam.Net.CoreServices.Files;

namespace Bam.Net.Aws
{
    public class S3ChunkStorage : IChunkStorage
    {
        public S3ChunkStorage(AmazonS3Client s3Client)
        {
            S3Client = s3Client;
        }
        
        protected AmazonS3Client S3Client { get; set; }
        
        public IChunk GetChunk(string hash)
        {
            throw new System.NotImplementedException();
        }

        public void SetChunk(IChunk chunk)
        {
            throw new System.NotImplementedException();
        }
    }
}