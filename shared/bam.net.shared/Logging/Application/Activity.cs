using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Bam.Net.Data.Repositories;
using Bam.Net.Logging.Application.Data;
using Bam.Net.Logging.Debug;
using Bam.Net.Server;

namespace Bam.Net.Logging.Application
{
    public class Activity
    {
        static Activity()
        {
            ActivityRepository = new DaoRepository();
            ActivityRepository.AddType<ActivityDescriptor>();
        }
        
        public static ILogger Logger { get; set; }
        public static IRepository ActivityRepository { get; set; }

        public static void Log(AppConf appConf, string activityName, string messageSignature, params object[] args)
        {
            try
            {
                string signature = $"[{appConf.Name}({appConf.DisplayName})]::({activityName}):{messageSignature}";
                string[] formatArgs = args.Select(a => a.ToString()).ToArray();
                (Logger ?? Bam.Net.Logging.Log.Default)?.AddEntry(signature, formatArgs);

                ActivityRepository?.Save(new ActivityDescriptor()
                {
                    Name = activityName,
                    ApplicationName = appConf.Name,
                    Message = string.Format(signature, formatArgs)
                });
            }
            catch (Exception ex)
            {
                Bam.Net.Logging.Log.Trace("Exception logging activity: {0}", ex, ex.Message);
            }
        }

        public static IEnumerable<ActivityDescriptor> RetrieveDescriptors(string activityName)
        {
            return ActivityRepository.Query<ActivityDescriptor>(new {Name = activityName});
        }
    }
}