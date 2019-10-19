using System;
using System.IO;

namespace Bam.Net.Application.Files
{
    public class AppFileIdentifier : FileIdentifier
    {
        public AppFileIdentifier(string appName, FileInfo file)
        {
            Kind = FileIdentifierKinds.App;
            throw new NotImplementedException();
        }
    }
}