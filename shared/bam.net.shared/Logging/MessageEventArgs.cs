using System;

namespace Bam.Net.Logging
{
    public class MessageEventArgs: EventArgs
    {
        public LogEventType LogEventType { get; set; }
        public LogMessage LogMessage { get; set; }
    }
}