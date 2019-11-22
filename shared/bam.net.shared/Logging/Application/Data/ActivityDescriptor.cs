using System;
using Bam.Net.Data.Repositories;
using Bam.Net.Server;

namespace Bam.Net.Logging.Application.Data
{
    public class ActivityDescriptor : KeyedRepoData
    {
        public ActivityDescriptor()
        {
            ProcessInfo = Net.ProcessInfo.Current.ToYaml();
        }
        
        [CompositeKey]
        public ActivityType Type { get; set; }
        
        /// <summary>
        /// A logical name to refer to the activity.
        /// </summary>
        [CompositeKey]
        public string Name { get; set; }
        [CompositeKey]
        public string ApplicationName { get; set; }
        [CompositeKey]
        public string ProcessInfo { get; set; }
        [CompositeKey]
        public string Message { get; set; }
    }
}