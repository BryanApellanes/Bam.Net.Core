using System.IO;
using System.Net;
using Amazon.S3.Model;
using Bam.Net.CoreServices.Files;
using Bam.Net.Data.Repositories;

namespace Bam.Net.Aws.Data
{
    public class S3ChunkDescriptor : CompositeKeyAuditRepoData
    {
        [CompositeKey] 
        public string BucketName { get; set; }

        [CompositeKey]
        public string Hash { get; set; }

        /// <summary>
        /// Advises where to save the file or where it was in the file system at the time of construction of the current S3ChunkDescriptor.
        /// </summary>
        public string FullPath { get; set; }

        public PutObjectRequest ToPutRequest()
        {
            if (!File.Exists(FullPath))
            {
                throw new FileNotFoundException(FullPath);
            }
            return new PutObjectRequest
            {
                BucketName = BucketName,
                Key = Hash,
                ContentBody = File.ReadAllBytes(FullPath).ToBase64()
            };
        }
        
        public static S3ChunkDescriptor ForFile(FileInfo file)
        {
            return ForFile(S3FileSystem.DefaultBucketName, file);
        }

        public static S3ChunkDescriptor ForFile(string bucketName, FileInfo fileInfo)
        {
            return new S3ChunkDescriptor
            {
                BucketName = bucketName,
                Hash = fileInfo.Sha256(),
                FullPath = fileInfo.FullName
            };
        }
    }
}