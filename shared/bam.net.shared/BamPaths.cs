using Bam.Net;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Bam.Net
{
    public class BamPaths
    {
        public static string BamHome => Path.Combine(BamHomeSegments);

        /// <summary>
        /// The root of the bam installation, the same as BamHome
        /// </summary>
        public static string Root => BamHome;

        /// <summary>
        /// The path segments for BamHome
        /// </summary>
        public static string[] BamHomeSegments
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
                    return Environment.GetEnvironmentVariable("$HOME");
                }
            }
        }
        
        public static string Build => Path.Combine(BamHome, "build");

        public static string PublicPath => Path.Combine(BamHome, "public");

        public static string ToolkitPath => Path.Combine(ToolkitSegments);

        public static string[] ToolkitSegments => new List<string>() {UserHome, ".bam", "toolkit"}.ToArray();

        public static string TestsPath => Path.Combine(TestsSegments);

        public static string[] TestsSegments => new List<string>() {UserHome, ".bam", "tests"}.ToArray();

        public static string ContentPath => Path.Combine(ContentSegments);

        public static string[] ContentSegments => new List<string>(BamHomeSegments) {"content"}.ToArray();

        public static string Apps => Path.Combine(AppsSegments);

        public static string[] AppsSegments => new List<string>(ContentSegments) {"apps"}.ToArray();

        public static string RpcScriptsSrcPath => Path.Combine(RpcScriptsSrcSegments);

        public static string[] RpcScriptsSrcSegments => new List<string>(BamHomeSegments) {"rpc", "scripts"}.ToArray();

        public static string ConfPath => Path.Combine(ConfSegments);

        public static string[] ConfSegments => new List<string>(BamHomeSegments) {"conf"}.ToArray();

        public static string DataPath => Path.Combine(DataSegments);

        public static string[] DataSegments => new List<string>(BamHomeSegments) {"data"}.ToArray();
    }
}
