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
    [Serializable]
    public partial class ConsoleActions : CommandLineTestInterface
    {
        public const string DefaultRecipeFile = "./default-recipe.json";
        
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

                if (Arguments.Contains("output"))
                {
                    recipe.OutputDirectory = GetArgument("output");
                }
                
                string projectName = Path.GetFileNameWithoutExtension(projectFilePath);
                string recipeFilePath = Path.Combine("recipes", $"{projectName}-{fileNameSuffix}.json");
                FileInfo recipeFile = ReadRecipe(recipeFilePath, recipe);
                WriteRecipe(recipe, recipeFile);
            }
        }
        
        private static void WriteRecipe(Recipe recipe, FileInfo file)
        {
            if (recipe.NameIsDefault)
            {
                recipe.Name = Path.GetFileNameWithoutExtension(file.Name);
            }
            string json = recipe.ToJson(true);
            json.SafeWriteToFile(file.FullName, true);
            Message.PrintLine(json, ConsoleColor.Cyan);
            Message.PrintLine("Wrote recipe file: {0}", ConsoleColor.DarkCyan, file.FullName);
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

            if (Arguments.Contains("nuget"))
            {
                recipePath = GetArgument("nuget", "NUGET: please enter the path to the recipe file to use");
            }
            if (!File.Exists(recipePath))
            {
                OutLineFormat("Specified recipe file does not exist: {0}", ConsoleColor.Yellow, new FileInfo(recipePath).FullName);
                Exit(1);
            }
            Recipe recipe = recipePath.FromJsonFile<Recipe>();
            return new ProcessProfileRecipe(recipe);
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
