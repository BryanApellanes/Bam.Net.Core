using System.IO;
using Bam.Net.Application.Files;

namespace Bam.Net.Aws
{
    public class FsFileIdentifier : FileIdentifier
    {
        public FsFileIdentifier(FileInfo file)
        {
            FileInfo = file;
            Hash = FileInfo.Sha256();
            Kind = FileIdentifierKinds.OS;
            Value = Hash;
        }
        
        public FileInfo FileInfo { get; set; }

        public string Hash
        {
            get;
            set;
        }
    }
}