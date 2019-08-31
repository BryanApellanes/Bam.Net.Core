using Bam.Net.CommandLine;
using Bam.Net.Testing;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Threading;
using Bam.Net.CoreServices;

namespace Bam.Net.Application
{
    [Serializable]
    public class ConsoleActions : CommandLineTestInterface
    {
        [ConsoleAction("discover",
            "Read a specified directory and discover csproj files in the child directories therein, writing a recipe for all projects found.")]
        public bool Discover()
        {
            string directoryPath = GetArgument("discover", "Please enter the path to the root of the projects folder");
            if (string.IsNullOrEmpty(directoryPath))
            {
                OutLine("Directory not specified", ConsoleColor.Yellow);
                return false;
            }

            DirectoryInfo rootDir = new DirectoryInfo(directoryPath);
            if (!rootDir.Exists)
            {
                OutLineFormat("Specified directory does not exist: {0}", ConsoleColor.Yellow, directoryPath);
                return false;
            }

            List<string> projectFilePaths = GetProjectFilePaths(rootDir);

            if (Arguments.Contains("recipePerProject"))
            {
                WriteRecipes(directoryPath, projectFilePaths);
                return true;
            }
            else
            {
                Recipe recipe = new Recipe
                {
                    ProjectRoot = directoryPath,
                    ProjectFilePaths = projectFilePaths.ToArray()
                };

                string recipeFile = Arguments.Contains("outputRecipe") ? Arguments["outputRecipe"] : "./recipe.json";
                FileInfo file = ReadRecipe(recipeFile, recipe);

                if (Arguments.Contains("output"))
                {
                    recipe.OutputDirectory = GetArgument("output");
                }

                WriteRecipe(recipe, file);
                return true;
            }
        }

        [ConsoleAction("all", "Discover tools projects and build")]
        public void DiscoverAndBuild()
        {
            if (!Arguments.Contains("discover"))
            {
                Arguments["discover"] = GetArgument("all");
            }

            if (Discover())
            {
                string recipeFile = Arguments.Contains("recipe") ? Arguments["recipe"] : "./recipe.json";
                Recipe discovered = recipeFile.FromJsonFile<Recipe>();
                Recipe toUse = discovered;
                if (Arguments.Contains("recipe"))
                {
                    Recipe specified = Arguments["recipe"].FromJsonFile<Recipe>();
                    specified.ProjectRoot = discovered.ProjectRoot;
                    specified.ProjectFilePaths = discovered.ProjectFilePaths;
                    toUse = specified;
                }

                if (Arguments.Contains("output"))
                {
                    toUse.OutputDirectory = GetArgument("output");
                }

                string tempRecipe = $"./temp_recipe_{6.RandomLetters()}.json";
                toUse.ToJsonFile(tempRecipe);
                Arguments["recipe"] = new FileInfo(tempRecipe).FullName;
                BakeRecipe();
            }
        }

        [ConsoleAction("clean", "clean the projects specified by a given recipe")]
        public void CleanRecipe()
        {
            Recipe recipe = GetRecipe();
            BamSettings settings = BamSettings.Load();
            foreach(string projectFile in recipe.ProjectFilePaths)
            {
                string dotNetArgs = $"clean {projectFile}";
                ProcessStartInfo startInfo = settings.DotNetPath.ToStartInfo(dotNetArgs);
                startInfo.Run(msg => OutLine(msg, ConsoleColor.Cyan));
                OutLineFormat("clean command finished for project {0}", projectFile, ConsoleColor.Blue);
            }
        }

        [ConsoleAction("recipe", "bake the specified recipe")]
        public void BakeRecipe()
        {
            // build each csproj with dotnet publish
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
                if (projectName.Equals("bake")) // don't try to bake bake in case it is the current process
                {
                    continue;
                }
                string dotNetArgs =
                    $"publish {projectFile} -c {recipe.BuildConfig.ToString()} -r {RuntimeNames[recipe.OsName]} -o {outputDirectory}";
                ProcessStartInfo startInfo = settings.DotNetPath.ToStartInfo(dotNetArgs);
                startInfo.Run(msg => OutLine(msg, ConsoleColor.DarkYellow));
                OutLineFormat("publish command finished for project {0}, output directory = {1}", ConsoleColor.Blue, projectFile, outputDirectory);
            }
        }

        [ConsoleAction("nuget", "pack the specified recipe")]
        public void PackRecipe()
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
                ProcessStartInfo startInfo = settings.DotNetPath.ToStartInfo(dotNetArgs);
                startInfo.Run(msg => OutLine(msg, ConsoleColor.DarkCyan));
                OutLineFormat("pack command finished for project {0}, output directory = {1}", ConsoleColor.Blue, projectFile, nugetDirectory);
            }
        }

        [ConsoleAction("zip", "Zip the OutputDirectory specified by a recipe")]
        public void ZipRecipe()
        {
            Recipe recipe = GetRecipe();
            if (!Directory.Exists(recipe.OutputDirectory))
            {
                OutLineFormat("Output directory does not exist: {0}", recipe.OutputDirectory);
                Exit(1);
            }
            DirectoryInfo dirInfo = new DirectoryInfo(recipe.OutputDirectory);
            string fileName = $"{dirInfo.Name}.zip";
            string output = $"./{fileName}";
            FileInfo outputFile = new FileInfo(output);
            if (Arguments.Contains("output"))
            {
                output = GetArgument("output");
                outputFile = new FileInfo(output);
                if (!outputFile.HasExtension(".zip"))
                {
                    dirInfo = new DirectoryInfo(output);
                    outputFile = new FileInfo(Path.Combine(dirInfo.FullName, fileName));
                }
            }

            if (outputFile.Exists)
            {
                OutLineFormat("File {0} exists, deleting...", outputFile.FullName);
                Thread.Sleep(300);
                File.Delete(outputFile.FullName);
                Thread.Sleep(300);
            }
            ZipFile.CreateFromDirectory(recipe.OutputDirectory, outputFile.FullName);
            OutLineFormat("\r\nZipped {0} to {1}", ConsoleColor.Green, recipe.OutputDirectory, outputFile.FullName);
            Thread.Sleep(300);
        }
        
        private static string GetOutputDirectory(Recipe recipe)
        {
            string outputDirectory = recipe.OutputDirectory;
            return CleanDirectoryPath(outputDirectory);
        }

        private static string GetNugetDirectory(Recipe recipe)
        {
            string nugetDirectory = recipe.NugetOutputDirectory;
            return CleanDirectoryPath(nugetDirectory);
        }
        
        private static string CleanDirectoryPath(string outputDirectory)
        {
            if (outputDirectory.StartsWith("C:"))
            {
                outputDirectory = outputDirectory.TruncateFront(2);
            }

            outputDirectory.Replace("\\", "/");
            return outputDirectory;
        }

        private static List<string> GetProjectFilePaths(DirectoryInfo rootDir)
        {
            List<string> projectFilePaths = new List<string>();
            foreach (DirectoryInfo projectDir in rootDir.GetDirectories())
            {
                string projectPath = Path.Combine(projectDir.FullName, $"{projectDir.Name}.csproj");
                if (!File.Exists(projectPath))
                {
                    OutLineFormat("No project found in {0}", ConsoleColor.Yellow, projectDir.FullName);
                    continue;
                }

                projectFilePaths.Add(projectPath);
            }

            return projectFilePaths;
        }

        private static FileInfo ReadRecipe(string recipeFile, Recipe recipe)
        {
            FileInfo file = new FileInfo(recipeFile);
            if (file.Exists)
            {
                Recipe fromFile = file.FromJsonFile<Recipe>();
                recipe.OutputDirectory = fromFile.OutputDirectory;
                recipe.BuildConfig = fromFile.BuildConfig;
                recipe.OsName = fromFile.OsName;
            }

            return file;
        }

        private static void WriteRecipes(string projectRoot, List<string> projectPaths)
        {
            FileInfo outputFile = new FileInfo(Arguments.Contains("outputRecipe") ? Arguments["outputRecipe"] : "recipe");
            string fileNameSuffix = Path.GetFileNameWithoutExtension(outputFile.Name);
            foreach (string projectFilePath in projectPaths)
            {
                FileInfo projectFile = new FileInfo(projectFilePath);
                Recipe recipe = new Recipe
                {
                    ProjectRoot = projectRoot,
                    ProjectFilePaths = new string[]{projectFilePath} 
                };

                string projectName = Path.GetFileNameWithoutExtension(projectFilePath);
                string recipeFilePath = Path.Combine("recipes", $"{projectName}-{fileNameSuffix}.json");
                FileInfo recipeFile = ReadRecipe(recipeFilePath, recipe);
                WriteRecipe(recipe, recipeFile);
            }
        }
        
        private static void WriteRecipe(Recipe recipe, FileInfo file)
        {
            string json = recipe.ToJson(true);
            json.SafeWriteToFile(file.FullName, true);
            OutLine(json, ConsoleColor.Cyan);
            OutLineFormat("Wrote recipe file: {0}", ConsoleColor.DarkCyan, file.FullName);
            Thread.Sleep(300);
        }

        private static Recipe GetRecipe()
        {
            string recipePath = "./recipe.json";
            if (Arguments.Contains("recipe"))
            {
                recipePath = GetArgument("recipe", "RECIPE: Please enter the path to the recipe file to use");
            }
            if (Arguments.Contains("clean"))
            {
                recipePath = GetArgument("clean", "CLEAN: Please enter the path to the recipe file to use");
            }
            if (Arguments.Contains("zipRecipe"))
            {
                recipePath = GetArgument("zipRecipe", "ZIP: Please enter the path to the zip file to use");
            }

            if (Arguments.Contains("pack"))
            {
                recipePath = GetArgument("pack", "PACK: please enter the path to the recipe file to use");
            }
            if (!File.Exists(recipePath))
            {
                OutLineFormat("Specified file does not exist: {0}", ConsoleColor.Yellow, recipePath);
                Exit(1);
            }
            Recipe recipe = recipePath.FromJsonFile<Recipe>();
            return recipe;
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
