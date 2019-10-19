using System;
using System.Threading.Tasks;
using Amazon.S3;
using Amazon.S3.Model;
using Bam.Net.CoreServices.Files;

namespace Bam.Net.Aws
{
    public class S3ChunkStorage : IChunkStorage
    {
        public S3ChunkStorage(string defaultBucketName, AmazonS3Client s3Client)
        {
            DefaultBucketName = defaultBucketName;
            S3Client = s3Client;
        }
        
        public string DefaultBucketName { get; set; }
        protected AmazonS3Client S3Client { get; set; }

        public async Task<S3Chunk> GetObjectAsChunk(string bucketName, string key)
        {
            GetObjectRequest getObjectRequest = new GetObjectRequest
            {
                BucketName = bucketName,
                Key = key
            };
            GetObjectResponse response = await S3Client.GetObjectAsync(getObjectRequest);
            return new S3Chunk()
            {
                BucketName = bucketName,
                Hash = key,
                ResponseStream = response.ResponseStream
            };
        }
        
        public IChunk GetChunk(string hash)
        {
            return GetChunk(DefaultBucketName, hash).Result;
        }

        public async Task<IChunk> GetChunk(string bucketName, string hash)
        {
            return await GetObjectAsChunk(bucketName, hash);
        }
        
        public void SetChunk(IChunk chunk)
        {
            Args.ThrowIfNull(chunk);
            Args.ThrowIfNullOrEmpty(DefaultBucketName, "DefaultBucketName");
            
            SetChunk(DefaultBucketName, chunk);
        }

        public void SetChunk(string bucketName, IChunk chunk)
        {
            Args.ThrowIfNull(chunk, "chunk");
            Args.ThrowIfNullOrEmpty(chunk.Hash, "chunk.Hash");

            PutObjectRequest putObjectRequest = new PutObjectRequest
            {
                BucketName = bucketName,
                Key = chunk.Hash,
                ContentBody = chunk.Data.ToBase64()
            };

            S3Client.PutObjectAsync(putObjectRequest);
        }
    }
}