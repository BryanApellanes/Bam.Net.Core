using System.IO;

namespace Bam.Net.Application.Files
{
    public class HashFileIdentifier : FileIdentifier
    {
        public HashFileIdentifier(FileInfo file)
        {
            Kind = FileIdentifierKinds.Hash;
            Value = file.Sha256();
        }

        public HashFileIdentifier(string fileHash)
        {
            Kind = FileIdentifierKinds.Hash;
            Value = fileHash;
        }

        public HashFileIdentifier(FileIdentifier fileIdentifier) : base(fileIdentifier)
        {
            Kind = FileIdentifierKinds.Hash;
        }
    }
}