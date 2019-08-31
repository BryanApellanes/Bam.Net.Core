using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Bam.Net.Automation;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Bam.Net.Application
{
    public class Recipe
    {
        public Recipe()
        {
            ProjectRoot = "_tools.core";
            OsName = OSInfo.Current;
            OsName = OSNames.Windows;
            OutputDirectory = Path.Combine(BamPaths.ToolkitPath, OsName.ToString());
            NugetOutputDirectory = Path.Combine(BamPaths.NugetOutputPath, OsName.ToString());
        }
        /// <summary>
        /// Gets or sets the root directory where the bam toolkit
        /// project directories are found.
        /// </summary>
        public string ProjectRoot { get; set; }
        public string[] ProjectFilePaths { get; set; }
        public string OutputDirectory { get; set; }
        
        /// <summary>
        /// The directory to output nuget packages to.
        /// </summary>
        public string NugetOutputDirectory { get; set; }
        
        [JsonConverter(typeof(StringEnumConverter))]
        public BuildConfig BuildConfig { get; set; }
        
        [JsonConverter(typeof(StringEnumConverter))]
        public OSNames OsName { get; set; }
    }
}
