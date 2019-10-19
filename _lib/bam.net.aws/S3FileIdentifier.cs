using System.IO;
using Bam.Net.Application.Files;
using Google.Protobuf.WellKnownTypes;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Bam.Net.Aws
{
    public class S3FileIdentifier : FileIdentifier
    {
        public S3FileIdentifier(string bucket, string key)
        {
            Kind = FileIdentifierKinds.App;
            Bucket = bucket;
            Key = key;
            Hash = key;
        }

        public string Bucket { get; set; }
        public string Key { get; set; }

        public string Hash { get; set; }
        
        public override string Value
        {
            get => $"s3://{Bucket}/{Key}";
        }

        public static S3FileIdentifier FromFile(FileInfo fileInfo)
        {
            return FromFile(S3FileSystem.DefaultBucketName, fileInfo);
        }
        
        public static S3FileIdentifier FromFile(string bucket, FileInfo fileInfo)
        {
            return new S3FileIdentifier(bucket, fileInfo.Sha256());
        }
    }
}