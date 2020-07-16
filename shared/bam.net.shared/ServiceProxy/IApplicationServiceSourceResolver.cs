using System;
using System.IO;
using Bam.Net.Server;

namespace Bam.Net.ServiceProxy
{
    /// <summary>
    /// When implemented, provides a mechanism to resolve the location of all service source files for compilation.
    /// </summary>
    public interface IApplicationServiceSourceResolver
    {
        event EventHandler CompilationException;
        ApplicationServiceAssembly CompileAppServices(AppConf appConf);
        DirectoryInfo GetAppServicesDirectory(AppConf appConf);
        DirectoryInfo GetAppServicesSourceDirectory(AppConf appConf);
        DirectoryInfo GetCommonServicesBinDirectory(BamConf bamConf);
        FileInfo GetAppServicesAssemblyFile(AppConf appConf);
        DirectoryInfo GetServicesBinDirectory(Fs fsRoot);
        void ForEachApplicationProxiedClass(AppConf appConf, Action<Type> forEachProxiedClass);
        void ForEachCommonProxiedClass(BamConf bamConf, Action<Type> forEachProxiedClass);
        void ForEachProxiedClass(string searchPattern, DirectoryInfo serviceDir, Action<Type> forEachProxiedClass);
    }
}