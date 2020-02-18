using System.Collections.Generic;
using Bam.Net.Application;
using Bam.Net.Logging;

namespace Bam.Net.Server
{
    public class BamRoutingServer: BamServer
    {
        public BamRoutingServer(BamConf conf) : base(conf)
        {
        }
        
        
        public Dictionary<string, AppRouteHandlerManager> ReloadAppRouteHandlerManagers()
        {
            _appRouteHandlerManagers = null;
            return AppRouteHandlerManagers;
        }

        private Dictionary<string, AppRouteHandlerManager> _appRouteHandlerManagers;
        private readonly object _appRouteHandlerManagersLock = new object();

        public Dictionary<string, AppRouteHandlerManager> AppRouteHandlerManagers
        {
            get {
                return _appRouteHandlerManagersLock.DoubleCheckLock(ref _appRouteHandlerManagers, () =>
                {
                    Dictionary<string, AppRouteHandlerManager> result = new Dictionary<string, AppRouteHandlerManager>();
                    foreach (AppConf appToServe in GetCurrentConf().AppsToServe)
                    {
                        if (string.IsNullOrEmpty(appToServe.Name))
                        {
                            Log.Warn("AppRouteHandlerManagers: Application name not specified in AppConf: \r\n{0}", appToServe.ToJson(true));
                        }

                        if (result.ContainsKey(appToServe.Name))
                        {
                            Log.Warn("AppRouteHandlerManagers: Duplicate app names found ({0})", appToServe.Name);
                        }
                        
                        result.Add(appToServe.Name, new AppRouteHandlerManager(appToServe));
                    }

                    return result;
                }); 
            }
        }
    }
}