using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading;
using Bam.Net;
using Bam.Net.CommandLine;
using Bam.Net.Data;
using Bam.Net.Logging;
using Bam.Net.Testing;
using Bam.Shell;

namespace Bam.Net.Application
{
    /// <summary>
    /// Used to display ConsoleActions from a given assembly.
    /// </summary>
    [Serializable]
    public class ExternalProvider: CommandLineTool
    {
        [ArgZero("menu")]
        public void Menu()
        {
            FileInfo file = typeof(Adhoc.Adhoc).Assembly.GetFileInfo();
            string assemblyPath = file.FullName;
            if (Arguments.Contains("assembly"))
            {
                assemblyPath = Arguments["assembly"];
            }
            
            FileInfo assemblyFile = new FileInfo(assemblyPath);
            ShowMenu(Assembly.LoadFile(assemblyFile.FullName), new ConsoleMenu[]{}, assemblyPath);
            Exit(0);
        }
    }
}