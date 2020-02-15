using System;
using System.Collections.Generic;

namespace Bam.Net
{
    /// <summary>
    /// Paths rooted in the .bam folder of the current directory.
    /// </summary>
    public static class BamDir
    {
        /// <summary>
        /// The path to the .bam directory in the home directory of the current process' user.
        /// This value is the same as BamHome.Profile.
        /// </summary>
        public static string Path => System.IO.Path.Combine(Environment.CurrentDirectory, ".bam");
        
        
        public static string ToolkitPath => System.IO.Path.Combine(ToolkitSegments);
        public static string[] ToolkitSegments => new List<string>() {Path, "toolkit"}.ToArray();
        public static string NugetOutputPath => System.IO.Path.Combine(NugetOutputSegments);
        
        public static string[] NugetOutputSegments => new List<string>() {Path, "nupkg"}.ToArray();
        
        public static string Config => System.IO.Path.Combine(ConfigSegments);
        public static string[] ConfigSegments => new List<string>() {Path, "config"}.ToArray();

        public static string Tests => System.IO.Path.Combine(TestSegments);
        public static string[] TestSegments => new List<string>() {Path, "tests"}.ToArray();
        public static string Content => System.IO.Path.Combine(Content);
        public static string[] ContentSegments => new List<string>() {Path, "content"}.ToArray();

        public static string Apps => System.IO.Path.Combine(AppsSegments);
        public static string[] AppsSegments => new List<string>(ContentSegments) {"apps"}.ToArray();
        
        public static string SvcScriptsSrcPath => System.IO.Path.Combine(SvcScriptsSrcSegments);
        public static string[] SvcScriptsSrcSegments => new List<string>() {Path, "svc", "scripts"}.ToArray();

        public static string Data => System.IO.Path.Combine(DataSegments);

        public static string[] DataSegments => new List<string>() {Path, "data"}.ToArray();

        public static string Recipes => System.IO.Path.Combine(RecipeSegments);
        public static string[] RecipeSegments => new List<string>() {Path, "recipes"}.ToArray();
    }
}