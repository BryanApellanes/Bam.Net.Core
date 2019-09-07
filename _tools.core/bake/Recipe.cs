using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using Bam.Net.Automation;
using Bam.Net.CommandLine;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using YamlDotNet.Serialization;

namespace Bam.Net.Application
{
    public class Recipe
    {
        public Recipe()
        {
            ProjectRoot = "_tools.core";
            OsName = OSInfo.Current;
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

        [YamlIgnore]
        [XmlIgnore]
        [JsonIgnore]
        [Exclude]
        public string[] UnixProjectFilePaths
        {
            get { return ProjectFilePaths.Select(path => ToUnixPath(path)).ToArray(); }
        }

        public static void SetPackageReference(string projectFilePath, string projectName)
        {
            // find the project reference
            // remove it
            // replace with package reference
        }

        public static void SetProjectReference(string projectFilePath, string projectName)
        {
            // find packageReference
            // remove it
            // replace with project reference
        }
        
        public static ReferenceInfo[] GetReferenceInfo(string projectFilePath)
        {
            Project project = projectFilePath.FromXmlFile<Project>();
            List<ReferenceInfo> results = new List<ReferenceInfo>();
            if (project.ItemGroup != null && project.ItemGroup.Length > 0)
            {
                foreach (ProjectItemGroup itemGroup in project.ItemGroup)
                {
                    if (itemGroup.PackageReference != null && itemGroup.PackageReference.Length > 0)
                    {
                        foreach (ProjectItemGroupPackageReference packageReference in itemGroup.PackageReference)
                        {
                            results.Add(new ReferenceInfo()
                            {
                                Kind = ReferenceKind.Project, 
                                Name = packageReference.Include,
                                Version = packageReference.Version,
                                PackageReference = packageReference,
                                Project = project
                            });
                        }
                    }

                    if (itemGroup.ProjectReference != null && itemGroup.ProjectReference.Length > 0)
                    {
                        foreach (ProjectItemGroupProjectReference projectReference in itemGroup.ProjectReference)
                        {
                            results.Add(new ReferenceInfo()
                            {
                                Kind = ReferenceKind.Project,
                                Name = projectReference.Include,
                                ProjectReference = projectReference,
                                Project = project
                            });
                        }
                    }
                }
            }
            
            return results.ToArray();
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
