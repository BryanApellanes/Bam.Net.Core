using System.IO;

namespace Bam.Net.Application.Files
{
    public class FileIdentifier
    {
        public static implicit operator string(FileIdentifier fileIdentifier)
        {
            return fileIdentifier.Value;
        }

        public static implicit operator FileIdentifier(string fileIdentifier)
        {
            return new FileIdentifier {Value = fileIdentifier, Kind = FileIdentifierKinds.App};
        }
        
        protected FileIdentifier()
        {
        }

        public FileIdentifier(FileInfo file)
        {
            Kind = FileIdentifierKinds.OS;
            Value = file.FullName;
        }

        public FileIdentifier(FileIdentifier fileIdentifier)
        {
            Kind = fileIdentifier.Kind;
            Value = fileIdentifier.Value;
        }
      
        public FileIdentifierKinds Kind { get; set; }
        
        public virtual string Value { get; set; }
    }
}