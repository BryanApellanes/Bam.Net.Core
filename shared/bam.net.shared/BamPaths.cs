using Bam.Net;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Bam.Net
{
    public class BamPaths // TODO: refactor this into BamHome.[home paths] and BamProfile.[profile paths]
    {
        public static string Home => Path.Combine(HomeSegments);

        /// <summary>
        /// The root of the bam installation, the same as BamHome
        /// </summary>
        public static string Root => Home;

        /// <summary>
        /// The path segments for BamHome
        /// </summary>
        public static string[] HomeSegments
        {
            get
            {
                if (OSInfo.Current == OSNames.Windows)
                {
                    return new string[] {"C:", "bam"};
                }
                else
                {
                    return new string[] {"/", "opt", "bam"};
                }
            }
        }

        public static string Profile => Path.Combine(UserHome, ".bam");
        
        public static string UserHome
        {
            get
            {
                if (OSInfo.Current == OSNames.Windows)
                {
                    return Environment.ExpandEnvironmentVariables("%HOMEDRIVE%%HOMEPATH%");
                }
                else
                {
                    return Environment.GetEnvironmentVariable("HOME");
                }
            }
        }

        public static string SystemRuntime => Path.Combine(Path.Combine(ReferenceRuntimeSegments), "System.Runtime.dll");

        public static string[] ReferenceRuntimeSegments
        {
            get
            {
                return new List<string>
                {
                    Home, "nuget", "global", $"runtime.{OSInfo.ReferenceRuntime}.microsoft.netcore.app",
                    OSInfo.CoreVersion, "runtimes", OSInfo.ReferenceRuntime, "lib", OSInfo.DefaultLibSubfolder,
                }.ToArray();
            }
        }

        public static string Build => Path.Combine(Home, "build");

        public static string PublicPath => Path.Combine(Home, "public");

        public static string ToolkitPath => Path.Combine(ToolkitSegments);

        public static string[] ToolkitSegments => new List<string>() {UserHome, ".bam", "toolkit"}.ToArray();

        public static string CurrentRuntimeToolkitPath => Path.Combine(CurrentRuntimeToolkitSegments);
        public static string[] CurrentRuntimeToolkitSegments => new List<string>(ToolkitSegments) {OSInfo.CurrentRuntime}.ToArray();
        
        public static string NugetOutputPath => Path.Combine(NugetOutputSegments);
        
        public static string[] NugetOutputSegments => new List<string>() {UserHome, ".bam", "nupkg"}.ToArray();
        
        /// <summary>
        /// The path where third party tools are found, including sysinternals and opencover.
        /// </summary>
        public static string Tools => Path.Combine(ToolsSegments);
        public static string[] ToolsSegments => new List<string>() {Home, "bin", "tools"}.ToArray();
        
        public static string Tests => Path.Combine(TestsSegments);

        public static string[] TestsSegments => new List<string>() {UserHome, ".bam", "tests"}.ToArray();

        public static string Content => Path.Combine(ContentSegments);

        public static string[] ContentSegments => new List<string>(HomeSegments) {"content"}.ToArray();

        public static string Apps => Path.Combine(AppsSegments);

        public static string[] AppsSegments => new List<string>(ContentSegments) {"apps"}.ToArray();

        public static string SvcScriptsSrcPath => Path.Combine(SvcScriptsSrcSegments);

        public static string[] SvcScriptsSrcSegments => new List<string>(HomeSegments) {"svc", "scripts"}.ToArray();

        public static string Conf => Path.Combine(ConfSegments);

        public static string[] ConfSegments => new List<string>(HomeSegments) {"conf"}.ToArray();

        public static string DataPath => Path.Combine(DataSegments);

        public static string[] DataSegments => new List<string>(HomeSegments) {"data"}.ToArray();
    }
}
