using Bam.Net.CommandLine;
using Bam.Net.Testing;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using System.Text;
using System.Threading;

namespace Bam.Net.Application
{
    [Serializable]
    public class ConsoleActions : CommandLineTestInterface
    {
        [ConsoleAction("toolkit", "Bake the BamToolkit")]
        public void BuildToolkit()
        {
            // build each csproj with dotnet publish
            string recipePath = GetArgument("recipe", "Please enter the path to the recipe file to use");
            Recipe recipe = recipePath.FromJsonFile<Recipe>();
            BamSettings settings = BamSettings.Load(true);
            foreach (string projectFile in recipe.ProjectFilePaths)
            {
                ProcessStartInfo startInfo = settings.DotNetPath.ToStartInfo($"publish {projectFile} -c Release -r {RuntimeNames[OSInfo.Current]} -o {recipe.OutputDirectory}");
                startInfo.Run(msg => OutLine(msg, ConsoleColor.DarkYellow));
                OutLineFormat("publish command finished for project {0}, output directory = {1}", ConsoleColor.Blue, projectFile, recipe.OutputDirectory);
            }
            FileInfo file = new FileInfo(Path.Combine(".", "bamtk.zip"));
            ZipFile.CreateFromDirectory(recipe.OutputDirectory, file.FullName);
            OutLineFormat("Created {0}", ConsoleColor.DarkGreen, file.FullName);
        }

        [ConsoleAction("discover", "Read a specified recipe file and discover csproj files")]
        public void Discover()
        {
            string recipePath = GetArgument("recipe", "Please enter the path to the recipe file to use");
            Recipe recipe = recipePath.FromJsonFile<Recipe>();
            DirectoryInfo rootDir = new DirectoryInfo(recipe.ProjectRoot);
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
            recipe.ProjectNames = projectNames.ToArray();
            recipe.ProjectFilePaths = projectFilePaths.ToArray();
            string json = recipe.ToJson(true);
            json.SafeWriteToFile(recipePath, true);
            OutLine(json, ConsoleColor.Cyan);
            Thread.Sleep(300);
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
