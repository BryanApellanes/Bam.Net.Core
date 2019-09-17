using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using Bam.Net.Server;

namespace Bam.Net.ServiceProxy
{
    /// <summary>
    /// Responsible for resolving paths where proxyable services are found.
    /// </summary>
    public class ServiceResolver : IServiceResolver
    {
        private const string ServicesRelativePath = "~/services";

        public AppServiceAssembly CompileAppServices(AppConf appConf)
        {
            DirectoryInfo sourceDir = GetAppServicesSourceDirectory(appConf);
            if (sourceDir.Exists)
            {
                DirectoryInfo binDir = GetServicesBinDirectory(appConf.AppRoot);
                RoslynCompiler compiler = new RoslynCompiler();
                string assemblyName = $"{appConf.Name}.Services";
                return new AppServiceAssembly()
                {
                    AssemblyData = compiler.Compile(assemblyName, sourceDir.GetFiles("*.cs", SearchOption.AllDirectories)),
                    Name = assemblyName
                };
            }

            return null;
        }

        public DirectoryInfo GetAppServicesDirectory(AppConf appConf)
        {
            Args.ThrowIfNull(appConf, "appConf");
            return new DirectoryInfo(appConf.AppRoot.GetAbsolutePath(ServicesRelativePath));
        }

        public DirectoryInfo GetAppServicesSourceDirectory(AppConf appConf)
        {
            Args.ThrowIfNull(appConf, "appConf");
            return new DirectoryInfo(Path.Combine(GetAppServicesDirectory(appConf).FullName, "src"));
        }

        public DirectoryInfo GetCommonServicesBinDirectory(BamConf bamConf)
        {
            Args.ThrowIfNull(bamConf, "bamConf");
            return GetServicesBinDirectory(bamConf.Fs);
        }

        public DirectoryInfo GetServicesBinDirectory(Fs fsRoot)
        {
            return new DirectoryInfo(Path.Combine(fsRoot.GetAbsolutePath(ServicesRelativePath), "bin"));
        }

        public DirectoryInfo GetAppServicesBinDirectory(AppConf appConf)
        {
            Args.ThrowIfNull(appConf, "appConf");
            return new DirectoryInfo(Path.Combine(GetAppServicesDirectory(appConf).FullName, "bin"));
        }

        public void ForEachApplicationProxiedClass(AppConf appConf, Action<Type> forEachProxiedClass)
        {
            Args.ThrowIfNull(appConf, "appConf");
            BamConf bamConf = appConf.BamConf;
            HashSet<string> searchPatterns = new HashSet<string>();
            appConf.ServiceSearchPattern.Each(searchPattern => searchPatterns.Add(searchPattern));
            DirectoryInfo serviceBin = GetAppServicesBinDirectory(appConf);
            ForEachProxiedClass(bamConf, serviceBin, searchPatterns, forEachProxiedClass);
        }

        public void ForEachCommonProxiedClass(BamConf bamConf, Action<Type> forEachProxiedClass)
        {
            Args.ThrowIfNull(bamConf, "bamConf");
            HashSet<string> searchPatterns = new HashSet<string>();
            bamConf?.ServiceSearchPattern.DelimitSplit(",", "|").Each(searchPattern => searchPatterns.Add(searchPattern));
            DirectoryInfo commonServicesBin = GetCommonServicesBinDirectory(bamConf);
            ForEachProxiedClass(bamConf, commonServicesBin, searchPatterns, forEachProxiedClass);
        }

        public void ForEachProxiedClass(string searchPattern, DirectoryInfo serviceDir, Action<Type> forEachProxiedClass)
        {
            ForEachProxiedClass(null, searchPattern, serviceDir, forEachProxiedClass);
        }

        public static void ForEachProxiedClass(BamConf bamConf, DirectoryInfo serviceBin, HashSet<string> searchPatterns, Action<Type> forEachProxiedClass)
        {
            if (serviceBin.Exists)
            {
                foreach (string searchPattern in searchPatterns)
                {
                    ForEachProxiedClass(bamConf, searchPattern, serviceBin, forEachProxiedClass);
                }
            }
        }

        public static void ForEachProxiedClass(BamConf bamConf, string searchPattern, DirectoryInfo serviceBin, Action<Type> forEachProxiedClass)
        {
            FileInfo[] files = serviceBin.GetFiles(searchPattern);
            int ol = files.Length;
            for (int i = 0; i < ol; i++)
            {
                FileInfo file = files[i];
                Assembly.LoadFrom(file.FullName)
                    .GetTypes()
                    .Where(type => type.HasCustomAttributeOfType<ProxyAttribute>())
                    .Each(t =>
                    {
                        ProxyAttribute attr = t.GetCustomAttributeOfType<ProxyAttribute>();
                        if (!string.IsNullOrEmpty(attr.VarName))
                        {
                            bamConf?.AddProxyAlias(attr.VarName, t);
                        }

                        forEachProxiedClass(t);
                    });
            }
        }
    }
}