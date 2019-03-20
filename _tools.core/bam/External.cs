using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading;
using Bam.Net.CommandLine;
using Bam.Net.Data;
using Bam.Net.Logging;
using Bam.Net.Testing;

namespace Bam.Net.Application
{
    [Serializable]
    public class External: CommandLineTestInterface
    {
        [ConsoleAction("menu", "Display console menu for the specified assembly file")]
        public void ConsoleMenu()
        {
            string assemblyPath =
                GetArgument("menu", "Please enter the path to the assembly to show a console menu for");
            
            FileInfo assemblyFile = new FileInfo(assemblyPath);
            ShowMenu(Assembly.LoadFile(assemblyFile.FullName), new ConsoleMenu[]{}, assemblyPath);
        }

        [ConsoleAction("switch", "Execute command line switches from a class in a specified assembly.  Switches are specified in the form /switch:[name1]-[value1],[name2]-[value2]")]
        public void CommandLineSwitch()
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

            string switchesArg = GetArgument("switch");
            Dictionary<string, string> switches = switchesArg.ToDictionary("-", ",");
            List<string> args = new List<string>();
            List<string> argNames = new List<string>();
            foreach (string key in switches.Keys)
            {
                if (!string.IsNullOrEmpty(switches[key]))
                {
                    args.Add($"/{key}:{switches[key]}");
                }
                else
                {
                    args.Add($"/{key}");
                }
                argNames.Add(key);
            }
            ParsedArguments arguments = new ParsedArguments(args.ToArray(), ArgumentInfo.FromStringArray(argNames.ToArray(), true));
            ConsoleLogger logger = new ConsoleLogger {AddDetails = false};
            Arguments = arguments;
            ExecuteSwitches(arguments, targetType, false, logger);
        }
    }
}