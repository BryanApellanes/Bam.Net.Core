using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Bam.Net.Server
{
    public class HttpServerEventArgs: EventArgs
    {
        public HostPrefix[] HostPrefixes { get; set; }
        public string HostPrefixString
        {
            get
            {
                return string.Join("\r\n", HostPrefixes.Select(hp => hp.ToString()).ToArray());
            }
        }
    }
}
