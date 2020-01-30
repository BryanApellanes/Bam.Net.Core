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
        [ConsoleAction("clean", "clean the projects specified by a given recipe")]
        public void Clean()
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
    }
}