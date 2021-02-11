using System;
using System.IO;
using Bam.Net.CommandLine;
using Bam.Net.Testing;

namespace Bam.Net.Application
{
    [Serializable]
    class Program : CommandLineTool
    {
        static void Main(string[] args)
        {
            IsolateMethodCalls = false;
            Type type = typeof(Program);
            AddValidArgument("src", false, false, "The path to the source directory");
            AddValidArgument("dst", false, false, "The path to the destination directory");
            AddValidArgument("config", false, false, "The path to a configuration file to use.  If config is specified src and dst are ignored");
            AddSwitches(type);
            DefaultMethod = type.GetMethod(nameof(Interactive));
            Initialize(args);

            if (Arguments.Length > 0)
            {
                ExecuteSwitches(Arguments, type, null, null);
            }
            else
            {
                Interactive();
            }
        }
        
        [ConsoleAction("Copy matching subfolders; \r\n\tWhere <source> has subfolders \r\n\tmatching in name to subfolders in <destination>")]
        public static void CopyMatchingSubfolders()
        {
            if (Arguments.Contains("config"))
            {
                string configFile = GetArgument("config");
                
                if (!File.Exists(configFile))
                {
                    Message.PrintLine("Config file {0} not found.", ConsoleColor.Yellow, configFile);
                    OutLine("Starting configuration...", ConsoleColor.Cyan);
                    Configure();
                }
            }
            else
            {
                string src = GetArgument("src", "Please enter the path to the source directory");
                string dst = GetArgument("dst", "Please enter the path to the destination directory");
                
                DirectoryInfo sourceDir = new DirectoryInfo(src);
                DirectoryInfo destinationDir = new DirectoryInfo(dst);                

                if (Exists(sourceDir) && Exists(destinationDir))
                {
                    DirectoryInfo[] dirsToMatch = destinationDir.GetDirectories();
                    for (int i = 0; i < dirsToMatch.Length; i++)
                    {
                        DirectoryInfo currentDirToMatch = dirsToMatch[i];
                        string sourcePath = Path.Combine(sourceDir.FullName, currentDirToMatch.Name);
                        DirectoryInfo currentSource = new DirectoryInfo(sourcePath);
                        if (currentSource.Exists)
                        {
                            currentSource.Copy(currentDirToMatch.FullName, true,
                                (srcFile, destFile) =>
                                {
                                    FileInfo d = new FileInfo(destFile);
                                    if (d.Exists)
                                    {
                                        if (d.IsReadOnly)
                                        {
                                            d.IsReadOnly = false;
                                        }
                                        d.Delete();
                                        d.Refresh();
                                    }

                                    Message.PrintLine("Copying {0} -> {1}", ConsoleColor.Yellow, srcFile, destFile);
                                });
                        }
                    }
                }
            }
        }
        
        [ConsoleAction("Configure")]
        public static void Configure()
        {
            string configFile = GetArgument("config", "Please enter the path to the config file to use");
            string source = Prompt("Enter the path to the source folder");
            string destination = Prompt("Enter the path to the destination folder");
            MasucoConfig config = new MasucoConfig();
            config.Source = source;
            config.Destination = destination;
            config.ToJsonFile(configFile);
            if (Confirm("Start copying?"))
            {
                CopyMatchingSubfolders();
            }
        }

        [ConsoleAction("Show configuration")]
        public static void ShowConfiguration()
        {
            string configFile = GetArgument("config", "Please enter the path to the config file to use");
            if (!File.Exists(configFile))
            {
                Message.PrintLine("Config file {0} not found.", ConsoleColor.Yellow, configFile);
                Message.PrintLine("Starting configuration...", ConsoleColor.Cyan);
                Configure();
            }
            else
            {
                MasucoConfig config = configFile.SafeReadFile().FromJson<MasucoConfig>();
                Message.PrintLine("Source: {0}", ConsoleColor.Cyan, config.Source);
                Message.PrintLine("Destination: {0}", ConsoleColor.Cyan, config.Destination);
            }
        }

        private static bool Exists(DirectoryInfo dir)
        {
            bool result = dir.Exists;
            if (!result)
            {
                Message.PrintLine("{0} was not found", ConsoleColor.Red, dir.FullName);
            }

            return result;
        }
    }
}