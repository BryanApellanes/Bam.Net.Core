using System.IO;
using Amazon.S3.Model;
using Bam.Net.Data.Repositories;
using Bam.Net.Server;
using GraphQL.Types;

namespace Bam.Net.Aws.Data
{
    /// <summary>
    /// Meta data about a file stored in S3.
    /// </summary>
    public class S3FileDescriptor : CompositeKeyAuditRepoData
    {
        public string S3ChunkDescriptorKey { get; set; }
        public string FullPath { get; set; }
        public string Hash { get; set; }

        public FileInfo ToFileInfo()
        {
            return  new FileInfo(FullPath);
        }

        public static S3FileDescriptor FromFile(string fullPath)
        {
            return FromFile(new FileInfo(fullPath));
        }

        public static S3FileDescriptor FromFile(FileInfo fileInfo)
        {
            return FromFile(new AppConf(), fileInfo);
        }

        public static S3FileDescriptor FromFile(AppConf appConf, FileInfo fileInfo)
        {
            S3ChunkDescriptor s3ChunkDescriptor = S3ChunkDescriptor.ForFile(appConf.GetApplicationName(), fileInfo);
            return new S3FileDescriptor
            {
                S3ChunkDescriptorKey = s3ChunkDescriptor.CompositeKey,
                FullPath = fileInfo.FullName,
                Hash = fileInfo.Sha256()
            };
        }
    }
}