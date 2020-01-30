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
        [ConsoleAction("ref","for a given recipe, update project file references to bam.net.core, value may be either 'Project' or 'Package-{Version}'")]
        public void Ref()
        {
            Recipe recipe = GetRecipe();
            string projectOrPackage = GetArgument("ref", "REF: Please specify 'Project' or 'Package-{Version}'");
            if (projectOrPackage.StartsWith("Package"))
            {
                string version = !projectOrPackage.Contains("-") ? Prompt("Please specify what package version to reference.") : projectOrPackage.Split("-", 2)[1];
                recipe.ReferenceAsPackage("bam.net.core", version);
            }
            else
            {
                recipe.ReferenceAsProject("bam.net.core", "../../_lib/bam.net.core/");
            }
        } 
    }
}