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
        [ConsoleAction("recipe", "bake the specified recipe")]
        public void Recipe()
        {
            // build each csproj with dotnet publish
            string startDir = Environment.CurrentDirectory;
            Recipe recipe = GetRecipe();
            if (Arguments.Contains("output"))
            {
                recipe.OutputDirectory = GetArgument("output");
            }
            BamSettings settings = BamSettings.Load();
            string outputDirectory = GetOutputDirectory(recipe);
            foreach (string projectFile in recipe.ProjectFilePaths)
            {
                string projectName = Path.GetFileNameWithoutExtension(projectFile);
                DirectoryInfo projectDirectory = new FileInfo(projectFile).Directory;
                Environment.CurrentDirectory = projectDirectory.FullName;
                DirectoryInfo projectOutputDirectory = new DirectoryInfo(Path.Combine(outputDirectory, projectName));
                string outputDirectoryPath = projectOutputDirectory.FullName;
                string dotNetArgs = $"publish {projectFile} -c {recipe.BuildConfig.ToString()} -r {RuntimeNames[recipe.OsName]} -o {outputDirectoryPath}";
                ProcessStartInfo startInfo = settings.DotNetPath.ToStartInfo(dotNetArgs);
                startInfo.Run(msg => OutLine(msg, ConsoleColor.DarkYellow));
                OutLineFormat("publish command finished for project {0}, output directory = {1}", ConsoleColor.Blue, projectFile, outputDirectoryPath);
            }

            Environment.CurrentDirectory = startDir;
        }
    }
}