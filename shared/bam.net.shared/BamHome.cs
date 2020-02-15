using Bam.Net;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Bam.Net
{
    public class BamHome // TODO: refactor this into BamHome.[home paths] and BamProfile.[profile paths]
    {
        public static string Path => System.IO.Path.Combine(HomeSegments);

        /// <summary>
        /// The root of the bam installation, the same as BamHome
        /// </summary>
        public static string Root => Path;

        public static string Local => System.IO.Path.Combine(Path, "local");
        
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

        public static string Profile => System.IO.Path.Combine(UserHome, ".bam");
        
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

        public static string SystemRuntime => System.IO.Path.Combine(System.IO.Path.Combine(ReferenceRuntimeSegments), "System.Runtime.dll");

        public static string[] ReferenceRuntimeSegments
        {
            get
            {
                return new List<string>
                {
                    Path, "nuget", "global", $"runtime.{OSInfo.ReferenceRuntime}.microsoft.netcore.app",
                    OSInfo.CoreVersion, "runtimes", OSInfo.ReferenceRuntime, "lib", OSInfo.DefaultLibSubfolder,
                }.ToArray();
            }
        }

        public static string Build => System.IO.Path.Combine(Path, "build");

        public static string PublicPath => System.IO.Path.Combine(Path, "public");

        public static string ToolkitPath => System.IO.Path.Combine(ToolkitSegments);

        public static string[] ToolkitSegments => new List<string>() {UserHome, ".bam", "toolkit"}.ToArray();

        public static string CurrentRuntimeToolkitPath => System.IO.Path.Combine(CurrentRuntimeToolkitSegments);
        public static string[] CurrentRuntimeToolkitSegments => new List<string>(ToolkitSegments) {OSInfo.CurrentRuntime}.ToArray();
        
        public static string NugetOutputPath => System.IO.Path.Combine(NugetOutputSegments);
        
        public static string[] NugetOutputSegments => new List<string>() {UserHome, ".bam", "nupkg"}.ToArray();
        
        /// <summary>
        /// The path where third party tools are found, including sysinternals and opencover.
        /// </summary>
        public static string Tools => System.IO.Path.Combine(ToolsSegments);
        public static string[] ToolsSegments => new List<string>() {Path, "bin", "tools"}.ToArray();
        
        public static string Tests => System.IO.Path.Combine(TestsSegments);

        public static string[] TestsSegments => new List<string>() {UserHome, ".bam", "tests"}.ToArray();

        public static string Content => System.IO.Path.Combine(ContentSegments);

        public static string[] ContentSegments => new List<string>(HomeSegments) {"content"}.ToArray();

        public static string Apps => System.IO.Path.Combine(AppsSegments);

        public static string[] AppsSegments => new List<string>(ContentSegments) {"apps"}.ToArray();

        public static string SvcScriptsSrcPath => System.IO.Path.Combine(SvcScriptsSrcSegments);

        public static string[] SvcScriptsSrcSegments => new List<string>(HomeSegments) {"svc", "scripts"}.ToArray();

        public static string Conf => System.IO.Path.Combine(ConfSegments);

        public static string[] ConfSegments => new List<string>(HomeSegments) {"conf"}.ToArray();

        public static string DataPath => System.IO.Path.Combine(DataSegments);

        public static string[] DataSegments => new List<string>(HomeSegments) {"data"}.ToArray();
    }
}
