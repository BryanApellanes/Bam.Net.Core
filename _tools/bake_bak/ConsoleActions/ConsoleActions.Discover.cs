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
        [ConsoleAction("discover","Read a specified directory and discover csproj files in the child directories therein, writing a recipe for all projects found.")]
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
                OutLineFormat("Specified directory does not exist: {0}", ConsoleColor.Yellow, rootDir.FullName);
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
                    ProjectFilePaths = projectFilePaths.ToArray(),
                    BuildConfig = TryGetBuildConfig()
                };
                
                string recipeFile = Arguments.Contains("outputRecipe") ? Arguments["outputRecipe"] : DefaultRecipeFile;
                FileInfo file = ReadRecipe(recipeFile, recipe);

                if (Arguments.Contains("output"))
                {
                    recipe.OutputDirectory = GetArgument("output");
                }

                WriteRecipe(recipe, file);
                return true;
            }
        }
        
        private BuildConfig TryGetBuildConfig()
        {
            try
            {
                if (Arguments.Contains("buildConfig"))
                {
                    if (Arguments["buildConfig"].TryToEnum<BuildConfig>(out BuildConfig config))
                    {
                        return config;
                    }
                }
            }
            catch (Exception ex)
            {
                Error($"Error occurred getting BuildConfig: {ex.Message}", ex);
            }
                
            return BuildConfig.Debug;
        }
    }
}