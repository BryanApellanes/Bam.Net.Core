using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bam.Net.Services.Clients
{
    public class HeartClientInfo
    {
        public HeartClientInfo()
        {
            HeartHostName = "heart.bamapps.net";
            HeartPort = 80;
            ContentRoot = "/opt/bam/content";
        }
        public string HeartHostName { get; set; }
        public int HeartPort { get; set; }
        public string ContentRoot { get; set; }
    }
}
