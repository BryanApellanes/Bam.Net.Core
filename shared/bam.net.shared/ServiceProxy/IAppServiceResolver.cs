using System;
using System.IO;
using Bam.Net.Server;

namespace Bam.Net.ServiceProxy
{
    public interface IAppServiceResolver
    {
        AppServiceAssembly CompileAppServices(AppConf appConf);
        DirectoryInfo GetAppServicesDirectory(AppConf appConf);
        DirectoryInfo GetAppServicesSourceDirectory(AppConf appConf);
        DirectoryInfo GetCommonServicesBinDirectory(BamConf bamConf);
        DirectoryInfo GetServicesBinDirectory(Fs fsRoot);
        void ForEachApplicationProxiedClass(AppConf appConf, Action<Type> forEach);
        void ForEachCommonProxiedClass(BamConf bamConf, Action<Type> forEachProxiedClass);
        void ForEachProxiedClass(string searchPattern, DirectoryInfo serviceDir, Action<Type> forEachProxiedClass);
    }
}