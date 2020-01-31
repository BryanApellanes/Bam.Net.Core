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
        [ConsoleAction("all", "Discover tools projects and build")]
        public void All()
        {
            if (!Arguments.Contains("discover"))
            {
                Arguments["discover"] = GetArgument("all", "Please enter the path to the root of the projects folder");
            }

            if (Discover())
            {
                string recipeFile = Arguments.Contains("recipe") ? Arguments["recipe"] : DefaultRecipeFile;
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
    }
}