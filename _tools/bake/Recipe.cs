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
        private ProcessHomeDirectoryResolver _homeDirectoryResolver;
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
    
    public class Recipe
    {
        public Recipe()
        {
            Name = "Recipe_".RandomLetters(6);
            NameIsDefault = true;
            ProjectRoot = "_tools";
            OsName = OSInfo.Current;
            OutputDirectory = Path.Combine(BamPaths.ToolkitPath, OsName.ToString());
            NugetOutputDirectory = Path.Combine(BamPaths.NugetOutputPath, OsName.ToString());
        }
        
        /// <summary>
        /// Create a recipe containing a single project.
        /// </summary>
        /// <param name="projectFilePath"></param>
        /// <param name="outputDirectory"></param>
        /// <returns></returns>
        public static Recipe FromProject(string projectFilePath, string outputDirectory = "./baked-goods")
        {
            FileInfo project = new FileInfo(projectFilePath);
            return new Recipe()
            {
                ProjectRoot = project.DirectoryName,
                ProjectFilePaths = new string[]{projectFilePath},
                OutputDirectory =  outputDirectory,
                NugetOutputDirectory =  Path.Combine(outputDirectory, "nupkg")
            };
        }

        public bool NameIsDefault { get; set; }
        private string _name;

        /// <summary>
        /// A logical name to refer to this recipe.
        /// </summary>
        public string Name
        {
            get => _name;
            set
            {
                _name = value;
                NameIsDefault = false;
            }
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

        [YamlIgnore]
        [XmlIgnore]
        [JsonIgnore]
        [Exclude]
        public string[] UnixProjectFilePaths => ProjectFilePaths.Select(ToUnixPath).ToArray();

        public bool ReferencesPackage(string projectFilePath, string referencedProjectName)
        {
            ProjectInfo projectInfo = GetProjectByPath(projectFilePath);
            return projectInfo.ReferencesPackage(referencedProjectName);
        }

        public bool ReferencesProject(string projectFilePath, string projectOrPackageName)
        {
            return ReferencesProject(projectFilePath, projectOrPackageName, out ReferenceInfo ignore);
        }
        
        public bool ReferencesProject(string projectFilePath, string projectOrPackageName, out ReferenceInfo referenceInfo)
        {
            ProjectInfo projectInfo = GetProjectByPath(projectFilePath);
            return projectInfo.ReferencesAsProject(projectOrPackageName, out referenceInfo);
        }
        
        protected ProjectInfo[] ProjectInfos => UnixProjectFilePaths.Select(ProjectInfo.FromProjectFilePath).ToArray();
        
        protected ProjectInfo GetProjectByPath(string projectPath)
        {
            return UnixProjectFilePaths
                .Where(p => p.Equals(projectPath, StringComparison.InvariantCultureIgnoreCase))
                .Select(ProjectInfo.FromProjectFilePath)
                .FirstOrDefault();
        }
        
        protected ProjectInfo GetProjectByName(string projectName)
        {
            return UnixProjectFilePaths
                .Where(p => projectName.Equals(Path.GetFileNameWithoutExtension(p)))
                .Select(ProjectInfo.FromProjectFilePath)
                .FirstOrDefault();
        }

        /// <summary>
        /// For all the projects in this recipe, reference the specified project as a package.
        /// </summary>
        /// <param name="projectName"></param>
        public void ReferenceAsPackage(string projectName, string version = null)
        {
            ProjectInfos.Each(pi => pi.ReferenceAsPackage(projectName, version));
        }

        /// <summary>
        /// For all the project in this recipe, reference the specified project as a project.
        /// </summary>
        /// <param name="projectName"></param>
        /// <param name="referenceProjectDirectory"></param>
        public void ReferenceAsProject(string projectName, string referenceProjectDirectory = "")
        {
            ProjectInfos.Each(pi => pi.ReferenceAsProject(projectName, referenceProjectDirectory));
        }
        
        /// <summary>
        /// Ensure that the specified projectFilePath references the project specified by projectName as a
        /// package and not a project.  Uses "(1.0.3,)" (any version higher than 1.0.3) as the default version value.
        /// </summary>
        /// <param name="projectPathToModify"></param>
        /// <param name="projectName"></param>
        public void SetPackageReference(string projectPathToModify, string projectName)
        {
            ProjectInfo projectInfo = GetProjectByPath(projectPathToModify);
            projectInfo.ReferenceAsPackage(projectName);
        }

        public void SetProjectReference(string projectPathToModify, string projectName, string referenceProjectDirectory)
        {
            ProjectInfo projectInfo = GetProjectByPath(projectPathToModify);
            projectInfo.ReferenceAsProject(projectName, referenceProjectDirectory);
        }
        
        private string ToUnixPath(string filePath)
        {
            if (filePath.StartsWith("c:", StringComparison.InvariantCultureIgnoreCase))
            {
                filePath = $"/c{filePath.TruncateFront(2)}";
            }

            return filePath.Replace("\\", "/");
        }
    }
}
