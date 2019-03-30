using System;
using System.IO;

namespace Bam.Net
{
    public class ConfigChangedEventArgs: EventArgs
    {
        public FileInfo File { get; set; }
        public Config OldConfig { get; set; }
        public Config NewConfig { get; set; }
    }
}