using Bam.Net;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Bam.Net
{
    public class BamPaths
    {
        public static string[] PathSegments
        {
            get
            {
                return new string[] { "/", "opt", "bam" };
            }
        }

        public static string ToolkitPath { get { return Path.Combine(ToolkitSegments); } }

        public static string[] ToolkitSegments
        {
            get
            {
                return new List<string>(PathSegments) { "toolkit" }.ToArray();
            }
        }

        public static string ContentPath { get { return Path.Combine(ContentSegments); } }

        public static string[] ContentSegments
        {
            get
            {
                return new List<string>(PathSegments) { "content" }.ToArray();
            }
        }
        
        public static string DataPath
        {
            get { return Path.Combine(DataSegments); }
        }

        public static string[] DataSegments
        {
            get
            {
                return new List<string>(PathSegments) { "data"}.ToArray();
            }
        }
    }
}
