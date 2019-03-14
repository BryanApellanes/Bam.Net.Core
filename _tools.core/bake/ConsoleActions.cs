using Bam.Net.CommandLine;
using Bam.Net.Testing;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using System.Text;
using System.Threading;
using Bam.Net.CoreServices;

namespace Bam.Net.Application
{
    [Serializable]
    public class ConsoleActions : CommandLineTestInterface
    {
        [ConsoleAction("discover", "Read a specified directory and discover csproj files in the child directories therein.")]
        public void Discover()
        {
            string directoryPath = GetArgument("discover");
            if (string.IsNullOrEmpty(directoryPath))
            {
                OutLine("Directory not specified", ConsoleColor.Yellow);
                return;
            }
            DirectoryInfo rootDir = new DirectoryInfo(directoryPath);
            if (!rootDir.Exists)
            {
                OutLineFormat("Specified directory does not exist: {0}", ConsoleColor.Yellow, directoryPath);
                return;
            }
            
            List<string> projectNames = new List<string>();
            List<string> projectFilePaths = new List<string>();
            foreach (DirectoryInfo projectDir in rootDir.GetDirectories())
            {
                string projectPath = Path.Combine(projectDir.FullName, $"{projectDir.Name}.csproj");
                if (!File.Exists(projectPath))
                {
                    OutLineFormat("No project found in {0}", ConsoleColor.Yellow, projectDir.FullName);
                    continue;
                }
                projectNames.Add(projectDir.Name);
                projectFilePaths.Add(projectPath);
            }

            Recipe recipe = new Recipe
            {
                ProjectRoot = directoryPath,
                ProjectNames = projectNames.ToArray(),
                ProjectFilePaths = projectFilePaths.ToArray()
            };
            
            string json = recipe.ToJson(true);
            FileInfo file = new FileInfo("./recipe.json");
            json.SafeWriteToFile(file.FullName, true);
            OutLine(json, ConsoleColor.Cyan);
            OutLineFormat("Wrote recipe file: {0}", ConsoleColor.DarkCyan, file.FullName);
            Thread.Sleep(300);
        }

        [ConsoleAction("toolkit", "Bake the BamToolkit")]
        public void BuildToolkit()
        {
            // build each csproj with dotnet publish
            string recipePath = GetArgument("toolkit", "Please enter the path to the recipe file to use");
            if (!File.Exists(recipePath))
            {
                OutLineFormat("Specified file does not exist: {0}", ConsoleColor.Yellow, recipePath);
                Exit(1);
            }
            Recipe recipe = recipePath.FromJsonFile<Recipe>();
            BamSettings settings = BamSettings.Load(true);
            foreach (string projectFile in recipe.ProjectFilePaths)
            {
                ProcessStartInfo startInfo = settings.DotNetPath.ToStartInfo($"publish {projectFile} -c Release -r {RuntimeNames[OSInfo.Current]} -o {recipe.OutputDirectory}");
                startInfo.Run(msg => OutLine(msg, ConsoleColor.DarkYellow));
                OutLineFormat("publish command finished for project {0}, output directory = {1}", ConsoleColor.Blue, projectFile, recipe.OutputDirectory);
            }
            FileInfo file = new FileInfo(Path.Combine(".", $"bamtoolkit-{OSInfo.Current.ToString()}.zip"));
            ZipFile.CreateFromDirectory(recipe.OutputDirectory, file.FullName);
            OutLineFormat("Created {0}", ConsoleColor.DarkGreen, file.FullName);
        }
        
        static Dictionary<OSNames, string> RuntimeNames
        {
            get
            {
                return new Dictionary<OSNames, string>()
                {
                    { OSNames.Invalid, "win10-x64" },
                    { OSNames.Linux, "ubuntu.16.10-x64" },
                    { OSNames.Windows, "win10-x64" },
                    { OSNames.OSX, "osx-x64" },
                };
            }
        }            
    }
}
