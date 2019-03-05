using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Bam.Net.Application
{
    public class Recipe
    {
        public Recipe()
        {
            ProjectRoot = "_tools.core";
            OutputDirectory = Path.Combine(BamHome.ToolkitSegments);
        }
        /// <summary>
        /// Gets or sets the root directory where the bam toolkit
        /// project directories are found.
        /// </summary>
        public string ProjectRoot { get; set; }
        public string[] ProjectNames { get; set; }
        public string[] ProjectFilePaths { get; set; }
        public string OutputDirectory { get; set; }
    }
}
