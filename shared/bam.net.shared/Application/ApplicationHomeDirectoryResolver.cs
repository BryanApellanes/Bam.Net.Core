using System.IO;
using Bam.Net.Server;

namespace Bam.Net.Application
{
    public class ApplicationHomeDirectoryResolver : IHomeDirectoryResolver
    {
        public ApplicationHomeDirectoryResolver(AppConf appConf)
        {
            AppConf = appConf;
        }

        public AppConf AppConf { get; set; }
        
        public string ResolveHomeDirectory()
        {
            Args.ThrowIfNull(AppConf, "AppConf");
            Args.ThrowIfNull(AppConf.AppRoot, "AppConf.AppRoot");
            Args.ThrowIfNull(AppConf.AppRoot.RootDir, "AppConf.AppRoot.RootDir");

            return Path.Combine(AppConf.AppRoot.RootDir.FullName);
        }

        public string GetHomePath(string path)
        {
            return Path.Combine(ResolveHomeDirectory(), path);
        }
    }
}