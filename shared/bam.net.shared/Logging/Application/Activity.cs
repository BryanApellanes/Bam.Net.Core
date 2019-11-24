using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Bam.Net.Configuration;
using Bam.Net.Data.Repositories;
using Bam.Net.Logging.Application.Data;
using Bam.Net.Logging.Debug;
using Bam.Net.Server;
using Bam.Net.UserAccounts.Data;

namespace Bam.Net.Logging.Application
{
    public class Activity
    {
        static Activity()
        {
            if (Repository == null)
            {
                Repository = new DaoRepository();
                Repository.AddType<ActivityDescriptor>();
            }

            if (ApplicationNameProvider == null)
            {
                ApplicationNameProvider = EnvironmentApplicationNameProvider.Instance;
            }
        }
        
        public static IApplicationNameProvider ApplicationNameProvider { get; set; }
        
        public static ILogger Logger { get; set; }
        public static IRepository Repository { get; set; }

        public static void UserLog(User user, string activityName, string messageSignature, params object[] args)
        {
            string appName = ApplicationNameProvider.GetApplicationName();
            Add(ActivityType.User, appName, appName, user.UserName, activityName, messageSignature, args);
        }
        
        public static void AppLog(string activityName, string messageSignature, params object[] args)
        {
            string appName = ApplicationNameProvider?.GetApplicationName();
            Add(ActivityType.System, appName, appName, User.Anonymous.UserName, activityName, messageSignature, args);
        }
        
        public static void SysLog(string activityName, string messageSignature, params object[] args)
        {
            string appName = ApplicationNameProvider?.GetApplicationName();
            Add(ActivityType.System, appName, appName, User.Anonymous.UserName, activityName, messageSignature, args);
        }
                
        public static void Add(AppConf appConf, string activityName, string messageSignature, params object[] args)
        {
            string appName = appConf.Name;
            string displayName = appConf.DisplayName;
                
            Add(appName, displayName, activityName, messageSignature, args);
        }
        
        public static void Add(string appName, string appDisplayName, string activityName, string messageSignature, params object[] args)
        {
            Add(appName, appDisplayName, User.Anonymous.UserName, activityName, messageSignature, args);
        }

        public static void Add(string appName, string appDisplayName, string userName, string activityName, string messageSignature, params object[] args)
        {
            Add(ActivityType.Application, appName, appDisplayName, userName, activityName, messageSignature, args);
        }

        public static void Add(ActivityType activityType, string appName, string appDisplayName, string userName, string activityName, string messageSignature, params object[] args)
        {
            try
            {
                string signature = $"[{appName}({appDisplayName})]::({activityName}):{messageSignature}";
                string[] formatArgs = args.Select(a => a.ToString()).ToArray();
                (Logger ?? Bam.Net.Logging.Log.Default)?.AddEntry(signature, formatArgs);

                Repository?.Save(new ActivityDescriptor
                {
                    Type = activityType,
                    Name = activityName,
                    ApplicationName = appName,
                    Message = string.Format(signature, formatArgs)
                });
            }
            catch (Exception ex)
            {
                Bam.Net.Logging.Log.Trace("Exception logging activity: {0}", ex, ex.Message);
            }
        }

        public static IEnumerable<ActivityDescriptor> Retrieve(string appName, string activityName)
        {
            return Repository?.Query<ActivityDescriptor>(new {Type = ActivityType.Application.ToString(), Name = activityName});
        }
        
        public static IEnumerable<ActivityDescriptor> Retrieve(string activityName)
        {
            return Repository?.Query<ActivityDescriptor>(new {Name = activityName});
        }
    }
}