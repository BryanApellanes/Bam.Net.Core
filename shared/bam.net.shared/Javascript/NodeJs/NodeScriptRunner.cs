/*
	Copyright © Bryan Apellanes 2015  
*/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Bam.Net;
using Bam.Net.CommandLine;

namespace Bam.Net.Javascript.NodeJs
{
	public class NodeScriptRunner
	{
        public NodeScriptRunner()
        {
            this.NodePath = OSInfo.Current == OSNames.Windows ? @"C:\Program Files\nodejs\node.exe": "/usr/local/bin/node";
        }

        public string NodePath { get; set; }

        public ProcessOutput Run(string nodeScriptPath)
        {
            return "{0} {1}"._Format(NodePath, nodeScriptPath).Run();
        }
	}
}
