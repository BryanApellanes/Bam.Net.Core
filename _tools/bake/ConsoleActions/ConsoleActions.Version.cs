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
        [ConsoleAction("version", "Update the package version of each project referenced by a recipe.")]
        public void Version()
        {
            Recipe recipe = GetRecipe();
            string versionArg = GetArgument("version", true, "Please specify 'major', 'minor' or 'patch' to increment version component.");
            
            throw new NotImplementedException("This is not fully implemented");
        } 
    }
}