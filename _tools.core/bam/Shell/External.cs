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
    public class External: CommandLineTestInterface
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

        [ArgZero("new")]
        public void New()
        {
            
        }
        
        [ArgZero("run")]
        public void Run()
        {
            string assemblyPath = GetArgument("assembly",
                "Please specify the path to the assembly containing cli switches to execute");
            string className = GetArgument("class",
                "Please enter the name of the class containing the cli switch to execute");
            if (!File.Exists(assemblyPath))
            {
                OutLineFormat("Specified assembly does not exist: {0}", ConsoleColor.Magenta, assemblyPath);
                Exit(1);
            }

            FileInfo file = new FileInfo(assemblyPath);
            Assembly targetAssembly = Assembly.LoadFile(file.FullName);
            Type targetType = targetAssembly.GetType(className);
            if (targetType == null)
            {
                targetType = targetAssembly.GetTypes()
                    .FirstOrDefault(t => $"{t.Namespace}.{t.Name}".Equals(className) || className.Equals(t.Name));
                if (targetType == null)
                {
                    OutLineFormat("Specified class was not found in the specified assembly: className = {0}", ConsoleColor.Magenta, className);
                    Exit(1);
                }
            }

            ConsoleLogger logger = new ConsoleLogger {AddDetails = false};
            ExecuteSwitches(Arguments, targetType, false, logger);
        }
    }
}