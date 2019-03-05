using Bam.Net;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Bam.Net
{
    public class BamHome
    {
        public static string[] PathSegments
        {
            get
            {
                if(OSInfo.Current == OSNames.Windows)
                {
                    return new string[] { "C:", "bam" };
                }
                else
                {
                    return new string[] { "~/", ".bam" };
                }
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
    }
}
