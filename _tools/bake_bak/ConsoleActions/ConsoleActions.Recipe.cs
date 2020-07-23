using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using System.Threading;
using Bam.Net.Application;
using Bam.Net.Automation;
using Bam.Net.CommandLine;
using Bam.Net.Testing;

namespace Bam.Net.Bake
{
    public partial class ConsoleActions
    {
        [ConsoleAction("recipe", "bake the specified recipe")]
        public void BakeRecipe()
        {
            if (Arguments.Contains("version")) // don't bake the recipe if all we're doing is updating the version
            {
                return;
            }
            // build each csproj with dotnet publish
            string startDir = Environment.CurrentDirectory;
            Recipe recipe = GetRecipe();
            if (Arguments.Contains("output"))
            {
                recipe.OutputDirectory = GetArgument("output");
            }
            BamSettings settings = BamSettings.Load();
            string outputDirectory = GetOutputDirectory(recipe);
            string buildConfigString = recipe.BuildConfig.ToString();
            if (Arguments.Contains("buildConfig"))
            {
                buildConfigString = Arguments["buildConfig"];
                OutLineFormat("Recipe BuildConfig = {0}, Specified BuildConfig = {1}", ConsoleColor.DarkYellow, recipe.BuildConfig.ToString(), buildConfigString);
            }
            BuildConfig buildConfig = BuildConfig.Debug;
            if (!BuildConfig.TryParse(buildConfigString, out buildConfig))
            {
                OutLineFormat("Unable to parse specified buildConfig (should be either Debug or Release: {0}", ConsoleColor.Magenta, buildConfigString);
                Exit(1);
            }
            foreach (string projectFile in recipe.ProjectFilePaths)
            {
                string projectName = Path.GetFileNameWithoutExtension(projectFile);
                DirectoryInfo projectDirectory = new FileInfo(projectFile).Directory;
                Environment.CurrentDirectory = projectDirectory.FullName;
                DirectoryInfo projectOutputDirectory = new DirectoryInfo(Path.Combine(outputDirectory, projectName));
                if (!projectOutputDirectory.Exists)
                {
                    projectOutputDirectory.Create();
                }
                string outputDirectoryPath = projectOutputDirectory.FullName;
                string dotNetArgs = $"publish {projectFile} -c {buildConfig.ToString()} -o {outputDirectoryPath}";
                OutLineFormat("dotnet {0}", ConsoleColor.Blue, dotNetArgs);
                ProcessStartInfo startInfo = settings.DotNetPath.ToStartInfo(dotNetArgs);
                startInfo.Run(msg => OutLine(msg, ConsoleColor.DarkYellow));
                OutLineFormat("publish command finished for project {0}, output directory = {1}", ConsoleColor.Blue, projectFile, outputDirectoryPath);
            }

            Environment.CurrentDirectory = startDir;
        }
    }
}