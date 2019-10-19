using System.IO;
using System.Text;
using Bam.Net.CoreServices.Files;

namespace Bam.Net.Aws
{
    public class S3Chunk : IChunk
    {
        public S3Chunk()
        {
            Encoding = Encoding.UTF8;
        }
        
        public string BucketName { get; set; }

        public Encoding Encoding { get; set; }
        
        private Stream _responseStream;
        /// <summary>
        /// The stream from s3 that represents the file content.
        /// </summary>
        public Stream ResponseStream 
        {
            get => _responseStream;
            set
            {
                _responseStream = value;
                _data = _responseStream.ReadToEnd().ToBytes();
            }
        }
        public string Hash { get; set; }

        private byte[] _data;

        public byte[] Data
        {
            get => _data ?? (_data = ResponseStream.ReadToEnd().ToBytes());
            set => _data = value;
        }

        public string Content => Encoding.GetString(Data);

        public static S3Chunk FromFile(FileInfo fileInfo)
        {
            return FromFile(S3FileSystem.DefaultBucketName, fileInfo);
        }

        public static S3Chunk FromFile(string bucketName, FileInfo fileInfo)
        {
            return new S3Chunk
            {
                BucketName = bucketName,
                Hash = fileInfo.Sha256(),
                Data = File.ReadAllBytes(fileInfo.FullName)
            };
        }
    }
}