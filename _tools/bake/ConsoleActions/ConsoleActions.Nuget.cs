using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using System.Threading;
using Bam.Net.Application;
using Bam.Net.CommandLine;
using Bam.Net.Testing;

namespace Bam.Net.Bake
{
    public partial class ConsoleActions
    {
        [ConsoleAction("nuget", "pack the specified recipe as a nuget package")]
        public void Nuget()
        {
            Recipe recipe = GetRecipe();
            if (Arguments.Contains("nugetOutput"))
            {
                recipe.NugetOutputDirectory = GetArgument("nugetOutput");
            }
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

        [ConsoleAction("addNugetSource", "Add the default NugetOutputPath to the local nuget sources")]
        public void AddNugetSource()
        {
            string path = GetArgument("addNugetSource").Or(Recipe.DefaultNugetOutputDirectory);
            string sourceName = GetArgument("name").Or("BamPackages");
            string nugetArgs = $"sources Add -Name \"{sourceName}\" -Source {path}";
            BamSettings settings = BamSettings.Load();
            ProcessStartInfo startInfo = settings.NugetPath.ToStartInfo(nugetArgs);
            startInfo.Run(msg => OutLine(msg, ConsoleColor.DarkCyan));
            OutLineFormat("addNugetSource command finished", ConsoleColor.Blue);
        }
    }
}