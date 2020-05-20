using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Bam.Net.Data.Dynamic
{
    public class DynamicDataSaveResultEventArgs: EventArgs
    {
        public DynamicDataSaveResult Result { get; set; }
        public FileInfo File { get; set; }
        public Bam.Net.Logging.Counters.Timer Timer { get; set; }
    }
}
