using Bam.Net.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bam.Net
{
    [Obsolete("Use of this class should be migrated to BamPaths")]
    public static class Paths
    { 
        static Paths()
        {
            Root = Net.BamHome.Path;

            SystemDrive = Path.Combine(Root, "public"); 
            WindowsDrive = OSInfo.Current == OSNames.Windows ? "/bam/public" : SystemDrive;
        }

        static string _root;
        [Obsolete("Use BamPaths.BamHome instead")]
        public static string Root
        {
            get => _root;
            set
            {
                _root = value;
                SetPaths();
            }
        }

        static string _pubRoot = @"//bam/public";
        
        [Obsolete("Use BamPaths.Public instead")]
        public static string PubRoot
        {
            get
            {
                return _pubRoot;
            }
            set
            {
                _pubRoot = value;
            }
        }

        public static string SystemDrive { get; set; }
        public static string WindowsDrive { get; set; }

        public static string Apps { get; private set; }
        public static string Local { get; private set; }
        public static string Content { get; private set; }
        public static string Conf { get; private set; }
        public static string Sys { get; private set; }
        
        public static string Generated { get; private set; }
        public static string Proxies { get; private set; }

        public static string Logs { get; private set; }
        public static string Data { get; private set; }
        public static string Tools { get; private set; }
        
        public static string Tests { get; private set; }
        public static string Builds { get; set; }
        public static string NugetPackages { get; private set; }

        public static string AppData
        {
            get { return RuntimeSettings.ProcessDataFolder; }
            set { RuntimeSettings.ProcessDataFolder = value; }
        }
        
        public static string BamHome
        {
            get { return Path.Combine(BamHomeSegments); }
        }

        public static string[] BamHomeSegments
        {
            get
            {
                if (OSInfo.Current == OSNames.Windows)
                {
                    return new string[] {"C:", "bam"};
                }

                return new string[] {"/", "opt", "bam"};
            }
        }

        public static string ToolkitPath { get { return Path.Combine(ToolkitSegments); } }

        public static string[] ToolkitSegments
        {
            get { return new List<string>(BamHomeSegments) {"toolkit"}.ToArray(); }
        }

        public static string ContentPath { get { return Path.Combine(ContentSegments); } }

        public static string[] ContentSegments
        {
            get { return new List<string>(BamHomeSegments) {"content"}.ToArray(); }
        }

        public static string RpcScriptsSrcPath
        {
            get { return Path.Combine(RpcScriptsSrcSegments); }
        }
        
        public static string[] RpcScriptsSrcSegments
        {
            get { return new List<string>(BamHomeSegments) {"rpc", "scripts"}.ToArray(); }
        }
        
        public static string ConfPath
        {
            get { return Path.Combine(ConfSegments); }
        }

        public static string[] ConfSegments
        {
            get { return new List<string>(BamHomeSegments) {"conf"}.ToArray(); }
        }
        
        public static string DataPath
        {
            get { return Path.Combine(DataSegments); }
        }

        public static string[] DataSegments
        {
            get { return new List<string>(BamHomeSegments) {"data"}.ToArray(); }
        }
        
        private static void SetPaths()
        {
            Apps = Path.Combine(Root, "apps");
            Local = Path.Combine(Root, "local");
            Content = Path.Combine(Root, "content");
            Conf = Path.Combine(Root, "conf");
            Sys = Path.Combine(Root, "sys");
            Logs = Path.Combine(Root, "logs");
            Data = Path.Combine(Root, "data");
            Tools = Path.Combine(Root, "tools");
            Tests = Path.Combine(Root, "tests");            
            NugetPackages = Path.Combine(Root, "nuget", "packages");

            Generated = Path.Combine(Root, "src", "_gen");
            Proxies = Path.Combine(Sys, "_proxies");
            Builds = Path.Combine(PubRoot, "Builds");            
        }
    }
}
