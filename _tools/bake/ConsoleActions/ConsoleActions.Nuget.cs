using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using System.Threading;
using System.Xml.Linq;
using Bam.Net.Application;
using Bam.Net.Automation;
using Bam.Net.CommandLine;
using Bam.Net.Testing;

namespace Bam.Net.Bake
{
    public partial class ConsoleActions
    {
        [ConsoleAction("nuget", "pack the specified recipe as nuget packages")]
        public void Nuget()
        {
            Recipe recipe = GetRecipeWithNugetOutput();
            BamSettings settings = BamSettings.Load();
            string nugetDirectory = GetNugetDirectory(recipe);
            foreach (string projectFile in recipe.ProjectFilePaths)
            {
                string projectName = Path.GetFileNameWithoutExtension(projectFile);
                if (projectName.Equals("bake"))
                {
                    continue;
                }

                string dotNetArgs = $"pack {projectFile} -c {recipe.BuildConfig} -o {recipe.NugetOutputDirectory}";
                if (Arguments.Contains("packageVersion"))
                {
                    string packageVersion = Arguments["packageVersion"];
                    dotNetArgs = $"{dotNetArgs} -p:PackageVersion={packageVersion}";
                }

                ProcessStartInfo startInfo = settings.DotNetPath.ToStartInfo(dotNetArgs);
                startInfo.Run(msg => OutLine(msg, ConsoleColor.DarkCyan));
                OutLineFormat("pack command finished for project {0}, output directory = {1}", ConsoleColor.Blue, projectFile, nugetDirectory);
            }
        }

        [ConsoleAction("nugetPush", "push the nuget packages that result from a specified recipe; bake /nuget must be called first.")]
        public void NugetPush()
        {
            string nugetApiKey = GetArgumentOrDefault("nugetApiKey", "");
            if (string.IsNullOrEmpty(nugetApiKey))
            {
                OutLineFormat("nugetApiKey not specified", ConsoleColor.Red);
                Exit(1);
            }
            string nugetSource = GetArgumentOrDefault("nugetSource", "nuget.org");
            Recipe recipe = GetRecipeWithNugetOutput();
            BamSettings settings = BamSettings.Load();
            string nugetDirectory = GetNugetDirectory(recipe);
            foreach (string projectFile in recipe.ProjectFilePaths)
            {
                string projectName = GetProjectName(projectFile);
                string projectVersion = GetProjectVersion(projectFile);
                string nupkgName = $"{projectName}.{projectVersion}.nupkg";
                string nupkgPath = Path.Combine(nugetDirectory, nupkgName);

                string dotNetArgs = $"nuget push {nupkgPath} -s {nugetSource} -k {nugetApiKey}";

                ProcessStartInfo startInfo = settings.DotNetPath.ToStartInfo(dotNetArgs);
                startInfo.Run(msg => OutLine(msg, ConsoleColor.DarkCyan));
                OutLineFormat("dotnet nuget push command finished for project {0}", ConsoleColor.Blue, projectFile);
            }
        }

        [ConsoleAction("nugetRestore", "call dotnet restore for the projects in a specified recipe")]
        public void NugetRestore()
        {
            string nugetSource = GetArgumentOrDefault("nugetSource", "nuget.org");
            Recipe recipe = GetRecipeWithNugetOutput();
            BamSettings settings = BamSettings.Load();
            string nugetDirectory = GetNugetDirectory(recipe);
            string currentDirectory = Environment.CurrentDirectory;
            foreach (string projectFile in recipe.ProjectFilePaths)
            {
                string dotNetArgs = $"restore -s {nugetSource}";
                FileInfo file = new FileInfo(projectFile);
                Environment.CurrentDirectory = file.Directory.FullName;
                ProcessStartInfo startInfo = settings.DotNetPath.ToStartInfo(dotNetArgs);
                startInfo.Run(msg => OutLine(msg, ConsoleColor.DarkCyan));
                OutLineFormat("dotnet nuget restore command finished for project {0}", ConsoleColor.Blue, projectFile);
            }
        }
        
        private static Recipe GetRecipeWithNugetOutput()
        {
            Recipe recipe = GetRecipe();
            if (Arguments.Contains("nugetOutput"))
            {
                recipe.NugetOutputDirectory = GetArgument("nugetOutput");
            }

            return recipe;
        }

        private string GetProjectVersion(string projectFile)
        {
            XDocument xdoc = XDocument.Load(projectFile);
            XElement versionElement = xdoc.Element("Project")?.Element("PropertyGroup")?.Element("Version");
            return versionElement?.Value;
        }

        private string GetProjectName(string projectFile)
        {
            FileInfo file = new FileInfo(projectFile);
            return Path.GetFileNameWithoutExtension(file.Name);
        }
    }
}