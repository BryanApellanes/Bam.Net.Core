using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Bam.Net.Bake
{
    public class ProjectInfo
    {
        public ProjectInfo()
        {
        }
        
        public ProjectInfo(string projectFilePath)
        {
            Name = Path.GetFileNameWithoutExtension(projectFilePath);
            Project = projectFilePath.FromXmlFile<Project>();
            ProjectFilePath = projectFilePath;
        }
        
        public string Name { get; set; }
        public Project Project { get; set; }
        public string ProjectFilePath { get; set; }

        ReferenceInfo[] _referenceInfos;
        readonly object _referenceInfosLock = new object();
        public ReferenceInfo[] ReferenceInfos => _referenceInfosLock.DoubleCheckLock(ref _referenceInfos, GetReferenceInfos);

        public ReferenceInfo[] PackageReferences => ReferenceInfos.Where(ri => ri.Kind == ReferenceKind.Package).ToArray();

        public ReferenceInfo[] ProjectReferences => ReferenceInfos.Where(ri => ri.Kind == ReferenceKind.Project).ToArray();

        /// <summary>
        /// If the current project references a project with the specified package name, change the reference to a package
        /// reference.
        /// </summary>
        /// <param name="packageName"></param>
        public void SetToPackageReference(string packageName)
        {
            
        }
        
        public bool ReferencesPackage(string packageName, string version = null)
        {
            return ReferencesPackage(packageName, out ReferenceInfo ignore, version);
        }

        public bool ReferencesPackage(string packageName, out ReferenceInfo packageReferenceInfo, string version = null)
        {
            packageReferenceInfo = PackageReferences.FirstOrDefault(ri => !string.IsNullOrEmpty(ri.Include) && ri.Include.Equals(packageName));
            if (!string.IsNullOrEmpty(version))
            {
                return packageReferenceInfo?.Version != null && packageReferenceInfo.Version.Equals(version);
            }
            return packageReferenceInfo != null;
        }
        
        public bool ReferencesProject(string projectPath)
        {
            return ReferencesProject(projectPath, out ReferenceInfo ignore);
        }
        
        public bool ReferencesProject(string projectPath, out ReferenceInfo projectReferenceInfo)
        {
            projectReferenceInfo = ProjectReferences.FirstOrDefault(ri => !string.IsNullOrEmpty(ri.ProjectPath) && ri.ProjectPath.Equals(projectPath));
            return projectReferenceInfo != null;
        }

        public bool References(string name)
        {
            return References(name, out ReferenceInfo ignore);
        }
        
        /// <summary>
        /// Determines if the current project references a package or project by the specified name.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="referenceInfo"></param>
        /// <returns></returns>
        public bool References(string name, out ReferenceInfo referenceInfo)
        {
            referenceInfo = ReferenceInfos.FirstOrDefault(ri => Path.GetFileNameWithoutExtension(ri.ProjectPath).Equals(name) || ri.Include.Equals(name));
            return referenceInfo != null;
        }

        public ProjectInfo ReferenceAsProject(string name, string projectDirectory)
        {
            if (References(name, out ReferenceInfo referenceInfo))
            {
                if (referenceInfo.Kind != ReferenceKind.Project)
                {
                    ProjectItemGroup projectItemGroup = referenceInfo.ItemGroup;
                    List<ProjectItemGroupPackageReference> packageReferences = new List<ProjectItemGroupPackageReference>(projectItemGroup.PackageReference);
                    packageReferences.Remove(referenceInfo.PackageReference);
                    projectItemGroup.PackageReference = packageReferences.ToArray();
                    
                    ProjectItemGroupProjectReference projectReference = referenceInfo.ToProjectReference(projectDirectory);
                    List<ProjectItemGroupProjectReference> projectReferences = new List<ProjectItemGroupProjectReference>(projectItemGroup.ProjectReference)
                    {
                        projectReference
                    };
                    projectItemGroup.ProjectReference = projectReferences.ToArray();
                    Save();
                }
            }
            return this;
        }

        /// <summary>
        /// If the specified project/package is referenced as a project then change the reference to a package reference
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public ProjectInfo ReferenceAsPackage(string name, string version = null)
        {
            if (References(name, out ReferenceInfo referenceInfo))
            {
                if (referenceInfo.Kind != ReferenceKind.Package)
                {
                    ProjectItemGroup projectItemGroup = referenceInfo.ItemGroup;
                    List<ProjectItemGroupProjectReference> projectReferences = new List<ProjectItemGroupProjectReference>(projectItemGroup.ProjectReference);
                    projectReferences.Remove(referenceInfo.ProjectReference);
                    projectItemGroup.ProjectReference = projectReferences.ToArray();

                    ProjectItemGroupPackageReference packageReference = referenceInfo.ToPackageReference(version);
                    List<ProjectItemGroupPackageReference> packageReferences = new List<ProjectItemGroupPackageReference>(projectItemGroup.PackageReference)
                    {
                        packageReference
                    };
                    projectItemGroup.PackageReference = packageReferences.ToArray();
                    Save();
                }
            }

            return this;
        }
        
        public void Save()
        {
            Project.ToXmlFile(ProjectFilePath);
        }
        
        private ReferenceInfo[] GetReferenceInfos()
        {
            Project project = ProjectFilePath.FromXmlFile<Project>();
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
                                Kind = ReferenceKind.Package, 
                                Include = packageReference.Include,
                                Version = packageReference.Version,
                                Name = packageReference.Include,
                                PackageReference = packageReference,
                                ItemGroup = itemGroup,
                                Project = project,
                                ProjectPath = ProjectFilePath
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
                                Include = projectReference.Include,
                                ProjectReference = projectReference,
                                Name = Path.GetFileNameWithoutExtension(projectReference.Include),
                                ItemGroup = itemGroup,
                                Project = project,
                                ProjectPath = ProjectFilePath
                            });
                        }
                    }
                }
            }
            
            return results.ToArray();
        }
        
        public static ProjectInfo FromProjectFilePath(string projectFilePath)
        {
            return new ProjectInfo()
            {
                Name = Path.GetFileNameWithoutExtension(projectFilePath),
                Project = projectFilePath.FromXmlFile<Project>(),
                ProjectFilePath = projectFilePath
            };
        }
    }
}