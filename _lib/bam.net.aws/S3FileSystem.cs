using System;
using System.Data.SQLite;
using System.IO;
using Amazon.S3;
using Amazon.S3.Model;
using Bam.Net.Application.Files;
using Bam.Net.Automation;
using Bam.Net.CoreServices.Files;
using Bam.Net.Data;
using Bam.Net.Data.Repositories;
using Bam.Net.Server;
using Bam.Net.Services;
using Microsoft.AspNetCore.Mvc.Internal;
using Microsoft.Win32.SafeHandles;

namespace Bam.Net.Aws
{
    public class S3FileSystem : FileManager, IFileSystem
    {
        public const string DefaultBucketName = "bam.fs";

        public S3FileSystem(IDataDirectoryProvider dataDirectoryProvider, AmazonS3Client amazonS3Client) : this(DefaultBucketName, dataDirectoryProvider, amazonS3Client)
        {
        }

        public S3FileSystem(string bucketName, IDataDirectoryProvider dataDirectoryProvider, AmazonS3Client amazonS3Client) : base(dataDirectoryProvider, new S3FileService(bucketName, amazonS3Client))
        {
            BucketName = DefaultBucketName;
            Repository = new DaoRepository();
            LocalFs = new Fs(DataDirectoryProvider.GetFilesDirectory());
        }
        
        public StorageWriteStrategy DefaultStorageWriteStrategy { get; set; }
        public string BucketName { get; set; }
        
        public Stream ReadFile(string fileContentHash)
        {
            Stream resultStream = null;
            Stream localStream = ReadLocalFile(fileContentHash);
            if (localStream.Length == 0)
            {
                resultStream = ReadS3File(fileContentHash);
                // TODO: handle storage
                throw new NotImplementedException();
            }

            return resultStream;
        }
        
        public Stream ReadLocalFile(string fullPath, bool writeToStorage = false)
        {
            if (!File.Exists(fullPath))
            {
                return new MemoryStream();
            }

            if (writeToStorage)
            {
                S3FileService.WriteContentToS3(new FileInfo(fullPath));
            }
            
            return new FileStream(fullPath, FileMode.Open);
        }

        public Stream ReadS3File(string fileContentHash)
        {
            return ReadS3File(BucketName, fileContentHash);
        }
        
        public Stream ReadS3File(S3FileIdentifier fileIdentifier)
        {
            return ReadS3File(fileIdentifier.Bucket, fileIdentifier.Key);
        }
        
        public Stream ReadS3File(string bucketName, string key)
        {
            return S3FileService.ReadContentFromS3(bucketName, key);
        }
        
        public IRepository Repository { get; set; }
        public Fs LocalFs { get; set; }
        public Stream ReadFile(FileIdentifier fileIdentifier)
        {
            EnsureValidFileIdentifier(fileIdentifier);

            S3FileIdentifier s3FileIdentifier = (S3FileIdentifier) fileIdentifier;
            return S3FileService.GetFileStream(s3FileIdentifier.Bucket, s3FileIdentifier.Key);
        }

        protected S3FileService S3FileService => (S3FileService) FileService;

        public FileInfo GetFile(FileIdentifier fileIdentifier)
        {
            Stream fileStream = ReadFile(fileIdentifier);
            FileInfo result = LocalFs.GetFile($".bam/temp/{fileIdentifier.Value}");
            using (StreamWriter streamWriter = new StreamWriter(result.FullName))
            {
                streamWriter.Write(fileStream);
            }
            result.Refresh();
            return result;
        }

        public void WriteFile(FileIdentifier fileIdentifier, byte[] content)
        {
            EnsureValidFileIdentifier(fileIdentifier);

            S3FileService.WriteContentToS3(fileIdentifier, content);
        }
        
        private static void EnsureValidFileIdentifier(FileIdentifier fileIdentifier)
        {
            Args.ThrowIfNull(fileIdentifier, nameof(fileIdentifier));
            if (fileIdentifier.GetType() != typeof(S3FileIdentifier))
            {
                throw new InvalidOperationException("Unsupported fileIdentifier, must be an S3FileIdentifier");
            }
        }
    }
}