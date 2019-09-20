using System;
using Bam.Net.Data.Repositories;
using Bam.Net.Logging;
using Bam.Net.Server;

namespace Bam.Net.Application.Activity.Data
{
    public class ActivityDescriptor : KeyedRepoData
    {
        public string Name { get; set; }
        public LogEvent LogEvent { get; set; }

        public static void Add(AppConf appConf, string message)
        {
            throw new NotImplementedException();
        }
    }
}