using Bam.Net.Services;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Bam.Net.Server
{
    public class IncludesResolver : IIncludesResolver
    {
        public IncludesResolver(BamServer server)
        {
            BamServer = server;
        }

        public BamServer BamServer { get; set; }

        public Includes ResolveApplicationIncludes(string applicationName, string contentRoot)
        {
            FileInfo includesFile = new FileInfo(Path.Combine(contentRoot, "apps", applicationName, "includes.yml"));
            if (includesFile.Exists)
            {
                return includesFile.FromFile<Includes>();
            }
            return new Includes();
        }

        public Includes ResolveCommonIncludes(string contentRoot)
        {
            FileInfo includesFile = new FileInfo(Path.Combine(contentRoot, "includes.yml"));
            if (includesFile.Exists)
            {
                return includesFile.FromFile<Includes>();
            }
            return new Includes();
        }
    }
}
