using System.Collections.Generic;
using System.IO;
using Amazon.S3;
using Amazon.S3.Model;
using Bam.Net.CoreServices;

namespace Bam.Net.Aws
{
    public class S3FileService : FileService
    {
        public S3FileService(string defaultBucketName, AmazonS3Client amazonS3Client)
        {
            DefaultBucketName = defaultBucketName;
            AmazonS3Client = amazonS3Client;
            ClearStorage();
            S3ChunkStorage = new S3ChunkStorage(defaultBucketName, amazonS3Client);
            AddStorage(S3ChunkStorage);
        }
        
        protected Dictionary<string, S3Chunk> ChunkCache { get; }
        
        protected string DefaultBucketName { get; }
        
        public AmazonS3Client AmazonS3Client { get; private set; }
        public S3ChunkStorage S3ChunkStorage { get; set; }

        public void WriteContentToS3(FileInfo fileInfo)
        {
            WriteContentToS3(DefaultBucketName, fileInfo);
        }
        
        public void WriteContentToS3(string bucketName, FileInfo fileInfo)
        {
            S3FileIdentifier s3FileIdentifier = S3FileIdentifier.FromFile(fileInfo);
            
            WriteContentToS3(bucketName, File.ReadAllBytes(fileInfo.FullName));
        }

        public void WriteContentToS3(string bucketName, byte[] bytes)
        {
            PutObjectRequest putObjectRequest = new PutObjectRequest
            {
                BucketName = bucketName,
                Key = bytes.Sha256(),
                ContentBody = bytes.ToBase64()
            };

            AmazonS3Client.PutObjectAsync(putObjectRequest);
        }
        
        public Stream ReadContentFromS3(string bucketName, string key)
        {
            return GetFileStream(bucketName, key);
        }
        
        public Stream ReadContentFromS3(S3FileIdentifier fileIdentifier)
        {
            return GetFileStream(fileIdentifier.Bucket, fileIdentifier.Key);
        }
        
        public string GetContent(string bucketName, string key)
        {
            return GetFileStream(bucketName, key).ReadToEnd();
        }
        
        public Stream GetFileStream(string bucketName, string key)
        {
            if (!ChunkCache.ContainsKey(GetS3Url(bucketName, key)))
            {
                
                S3Chunk chunk = S3ChunkStorage.GetObjectAsChunk(bucketName, key).Result;
                ChunkCache.Add(GetS3Url(bucketName, key), chunk);
                return chunk.ResponseStream;
            }

            return ChunkCache[GetS3Url(bucketName, key)].ResponseStream;
        }

        private string GetS3Url(string bucketName, string key)
        {
            return $"s3://{bucketName}/{key}";
        }
    }
}