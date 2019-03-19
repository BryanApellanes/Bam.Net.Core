using System;
using System.Reflection;
using System.Threading;
using Bam.Net.CommandLine;
using Bam.Net.Testing;

namespace Bam.Net.Application
{
    [Serializable]
    public class ExternalMenu: CommandLineTestInterface
    {
        [ConsoleAction("menu", "Display console menu for the specified assembly file")]
        public void ConsoleMenu()
        {
            string assemblyPath =
                GetArgument("menu", "Please enter the path to the assembly to show a console menu for");
            
            ShowMenu(Assembly.LoadFrom(assemblyPath), new ConsoleMenu[]{}, assemblyPath);
        }
    }
}