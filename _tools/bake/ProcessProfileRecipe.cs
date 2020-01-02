using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using Bam.Net.Application;
using Bam.Net.Automation;
using Bam.Net.Bake;
using Bam.Net.CommandLine;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using YamlDotNet.Serialization;

namespace Bam.Net.Bake
{
    /// <summary>
    /// A Recipe that resolves '~' to the home directory of the owner of the current process.
    /// </summary>
    public class ProcessProfileRecipe: Recipe, IHomeDirectoryResolver
    {
        private readonly ProcessHomeDirectoryResolver _homeDirectoryResolver;
        public ProcessProfileRecipe(Recipe recipe)
        {
            _homeDirectoryResolver = new ProcessHomeDirectoryResolver();
            OriginalRecipe = recipe;
            this.CopyProperties(recipe);
            ResolveRecipePaths();
        }

        /// <summary>
        /// The kernels finest,... :) jk
        /// </summary>
        public Recipe OriginalRecipe { get; set; }
        
        public string ResolveHomeDirectory()
        {
            string result = _homeDirectoryResolver.ResolveHomeDirectory();
            return result;
        }

        public string GetHomePath(string path)
        {
            string result = _homeDirectoryResolver.GetHomePath(path);
            return result;
        }

        private void ResolveRecipePaths()
        {
            if (ProjectRoot.StartsWith("~"))
            {
                string path = ProjectRoot.TruncateFront(1);
                ProjectRoot = GetHomePath(path);
            }

            if (OutputDirectory.StartsWith("~"))
            {
                string path = OutputDirectory.TruncateFront(1);
                OutputDirectory = GetHomePath(path);
            }

            if (NugetOutputDirectory.StartsWith("~"))
            {
                string path = NugetOutputDirectory.TruncateFront(1);
                NugetOutputDirectory = GetHomePath(path);
            }
        }
    }
}