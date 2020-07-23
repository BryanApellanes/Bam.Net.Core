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
        [ConsoleAction("zip", "Zip the OutputDirectory specified by a recipe")]
        public void ZipRecipe()
        {
            Recipe recipe = GetRecipe();
            if (!Directory.Exists(recipe.OutputDirectory))
            {
                try
                {
                    Directory.CreateDirectory(recipe.OutputDirectory);
                }
                catch (Exception ex)
                {
                    OutLineFormat("Failed to create output directory ({0}), directory does not exist: {1}", recipe.OutputDirectory, ex.Message);
                    Exit(1);
                }
            }
            DirectoryInfo dirInfo = new DirectoryInfo(recipe.OutputDirectory);
            string defaultFileName = $"{recipe.Name}.zip";
            string fileName = GetArgumentOrDefault("zip", defaultFileName);
            string output = $"./{fileName}";
            FileInfo outputFile = new FileInfo(output);
            if (Arguments.Contains("output"))
            {
                output = GetPathArgument("output");
                outputFile = new FileInfo(Path.Combine(output, "..", fileName));
            }

            if (outputFile.Exists && outputFile.Name.Equals(defaultFileName))
            {
                Message.PrintLine("File {0} exists, deleting...", ConsoleColor.DarkYellow, outputFile.FullName);
                Thread.Sleep(300);
                File.Delete(outputFile.FullName);
                Thread.Sleep(300);
            }

            if (File.Exists(outputFile.FullName))
            {
                File.Move(outputFile.FullName, outputFile.GetNextFile().FullName);
            }
            Message.PrintLine("Zipping {0} to {1}...", ConsoleColor.DarkYellow, recipe.OutputDirectory, outputFile.FullName);
            ZipFile.CreateFromDirectory(new DirectoryInfo(recipe.OutputDirectory).FullName, outputFile.FullName);
            Message.PrintLine("\r\nZipped {0} to {1}", ConsoleColor.Green, recipe.OutputDirectory, outputFile.FullName);
            Thread.Sleep(300);
        }
    }
}