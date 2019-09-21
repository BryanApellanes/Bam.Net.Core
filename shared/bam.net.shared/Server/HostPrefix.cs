/*
	Copyright © Bryan Apellanes 2015  
*/
using Bam.Net.Configuration;
using Bam.Net.ServiceProxy;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bam.Net.Server
{
    public class HostPrefix
    {
        public HostPrefix()
        {
            this.HostName = "localhost";
            this.Port = 8080;
        }

        public HostPrefix(string hostName, int port)
        {
            this.HostName = hostName;
            this.Port = port;
        }

        public string HostName { get; set; }
        public int Port { get; set; }

        public bool Ssl { get; set; }

        public override string ToString()
        {
            string protocol = Ssl ? "https://": "http://";
            return $"{protocol}{HostName}:{Port}/";
        }

        public override bool Equals(object obj)
        {
            if (obj is HostPrefix compareTo)
            {
                return compareTo.ToString().Equals(this.ToString());
            }
            return base.Equals(obj);
        }

        public override int GetHashCode()
        {
            return ToString().ToSha1Int();
        }

        public HostPrefix FromServiceSubdomain(ServiceSubdomainAttribute attr)
        {
            HostPrefix result = this.CopyAs<HostPrefix>();
            result.HostName = $"{attr.Subdomain}.{HostName}";
            return result;
        }

        public static HostPrefix[] FromHostAppMaps(IEnumerable<HostAppMap> hostAppMaps)
        {
            return hostAppMaps.Select(hm => new HostPrefix { HostName = hm.Host, Port = 80 }).ToArray();
        }

        public static HostPrefix[] FromBamProcessConfig(string defaultHostName = "localhost", int defaultPort = 80)
        {
            int port = int.Parse(Config.Current["Port", defaultPort.ToString()]);
            bool ssl = Config.Current["Ssl"].IsAffirmative();
            List<HostPrefix> results = new List<HostPrefix>();
            foreach (string hostName in Config.Current["HostNames"].Or(defaultHostName).DelimitSplit(",", true))
            {
                AddHostPrefix(hostName, port, ssl, results);
            }
            return results.ToArray();
        }

        public static HostPrefix[] FromDefaultConfiguration(string defaultHostName = "localhost", int defaultPort = 80)
        {
            int port = int.Parse(DefaultConfiguration.GetAppSetting("Port", defaultPort.ToString()));
            bool ssl = DefaultConfiguration.GetAppSetting("Ssl").IsAffirmative();
            List<HostPrefix> results = new List<HostPrefix>();
            foreach (string hostName in DefaultConfiguration.GetAppSetting("HostNames").Or(defaultHostName).DelimitSplit(",", true))
            {
                AddHostPrefix(hostName, port, ssl, results);
            }
            return results.ToArray();
        }
        
        private static void AddHostPrefix(string hostName, int port, bool ssl, List<HostPrefix> results)
        {
            HostPrefix hostPrefix = new HostPrefix()
            {
                HostName = hostName,
                Port = port,
                Ssl = ssl
            };
            results.Add(hostPrefix);
            Trace.Write($"Default Config Hostname: {hostPrefix.ToString()}");
        }
    }
}