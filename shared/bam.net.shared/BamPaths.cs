﻿using Bam.Net;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Bam.Net
{
    public class BamPaths
    {
        public static string BamHome
        {
            get { return Path.Combine(BamHomeSegments); }
        }

        /// <summary>
        /// The root of the bam installation, the same as BamHome
        /// </summary>
        public static string Root
        {
            get { return BamHome; }
        }
        
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
                else if (OSInfo.Current == OSNames.OSX)
                {
                    return new string[] {"/", "opt", "bam"};
                }

                return new string[] {"/", "usr", "local", "bam"};
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
        
        public static string Build
        {
            get { return Path.Combine(BamHome, "build"); }
        }
        
        public static string PublicPath
        {
            get { return Path.Combine(BamHome, "public"); }
        }
        
        public static string ToolkitPath { get { return Path.Combine(ToolkitSegments); } }

        public static string[] ToolkitSegments
        {
            get
            {
                return new List<string>() {UserHome, ".bam", "toolkit"}.ToArray();
            }
        }

        public static string ContentPath { get { return Path.Combine(ContentSegments); } }

        public static string[] ContentSegments
        {
            get { return new List<string>(BamHomeSegments) {"content"}.ToArray(); }
        }

        public static string Apps
        {
            get { return Path.Combine(AppsSegments); }
        }
        
        public static string[] AppsSegments
        {
            get { return new List<string>(BamHomeSegments) {"apps"}.ToArray(); }
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
    }
}
