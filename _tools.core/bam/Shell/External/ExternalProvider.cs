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

namespace Bam.Shell
{
    [Serializable]
    public class ExternalProvider: CommandLineTestInterface
    {
        [ArgZero("menu")]
        public void Menu()
        {
            string assemblyPath =
                GetArgument("assembly", "Please enter the path to the assembly to show a console menu for");
            
            FileInfo assemblyFile = new FileInfo(assemblyPath);
            ShowMenu(Assembly.LoadFile(assemblyFile.FullName), new ConsoleMenu[]{}, assemblyPath);
            Exit(0);
        }
    }
}