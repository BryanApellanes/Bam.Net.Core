using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Bam.Net.Testing;

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
        
        public bool ReferencesAsProject(string projectOrPackageName)
        {
            return ReferencesAsProject(projectOrPackageName, out ReferenceInfo ignore);
        }
        
        public bool ReferencesAsProject(string projectOrPackageName, out ReferenceInfo projectReferenceInfo)
        {
            bool result = References(projectOrPackageName, out projectReferenceInfo);
            return result && projectReferenceInfo.Kind == ReferenceKind.Project;
        }

        public bool References(string name)
        {
            return References(name, out ReferenceInfo ignore);
        }
        
        /// <summary>
        /// Determines if the current project references a package or project of the specified name.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="referenceInfo"></param>
        /// <returns></returns>
        public bool References(string name, out ReferenceInfo referenceInfo)
        {
            referenceInfo = ReferenceInfos.FirstOrDefault(ri => ri.Name.Equals(name) || ri.Include.Equals(name));
            return referenceInfo != null;
        }

        public ProjectInfo ReferenceAsProject(string name, string projectDirectory)
        {
            if (References(name, out ReferenceInfo referenceInfo))
            {
                if (referenceInfo.Kind != ReferenceKind.Project)
                {
                    RemovePackageReference(name);
                    AddProjectReference(Path.Combine(projectDirectory, $"{name}.csproj"));
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
                    RemoveProjectReference(referenceInfo.Include);
                    AddPackageReference(name, version);
                    Save();
                }
            }

            return this;
        }

        public void AddPackageReference(string packageName, string version = "1.0.0")
        {
            ProjectItemGroup itemGroup = new ProjectItemGroup();
            ProjectItemGroupPackageReference packageReference = new ProjectItemGroupPackageReference{Include = packageName, Version = version ?? ReferenceInfo.DefaultVersion};
            itemGroup.PackageReference = new ProjectItemGroupPackageReference[]{packageReference};
            Project.ItemGroup = new List<ProjectItemGroup>(Project.ItemGroup) {itemGroup}.ToArray();
        }
        
        public void RemovePackageReference(string packageName)
        {
            List<ProjectItemGroup> itemGroups = new List<ProjectItemGroup>();
            foreach (ProjectItemGroup itemGroup in Project.ItemGroup)
            {
                if (itemGroup.PackageReference == null)
                {
                    itemGroups.Add(itemGroup);
                    continue;
                }
                List<ProjectItemGroupPackageReference> packageReferences = new List<ProjectItemGroupPackageReference>();
                foreach (ProjectItemGroupPackageReference packageReference in itemGroup.PackageReference)
                {
                    if (!packageReference.Include.Equals(packageName))
                    {
                        packageReferences.Add(packageReference);
                    }
                }

                if (packageReferences.Count > 0)
                {
                    itemGroup.PackageReference = packageReferences.ToArray();
                    itemGroups.Add(itemGroup);
                }
            }

            Project.ItemGroup = itemGroups.ToArray();
        }

        public void AddProjectReference(string projectPath)
        {
            ProjectItemGroup itemGroup = new ProjectItemGroup();
            ProjectItemGroupProjectReference projectReference = new ProjectItemGroupProjectReference{Include = projectPath};
            itemGroup.ProjectReference = new ProjectItemGroupProjectReference[] {projectReference};
            Project.ItemGroup = new List<ProjectItemGroup>(Project.ItemGroup) {itemGroup}.ToArray();
        }
        
        public void RemoveProjectReference(string projectPath)
        {
            List<ProjectItemGroup> itemGroups = new List<ProjectItemGroup>();
            foreach (ProjectItemGroup itemGroup in Project.ItemGroup)
            {
                if (itemGroup.ProjectReference == null)
                {
                    itemGroups.Add(itemGroup);
                    continue;
                }
                List<ProjectItemGroupProjectReference> projectReferences = new List<ProjectItemGroupProjectReference>();
                foreach (ProjectItemGroupProjectReference projectReference in itemGroup.ProjectReference)
                {
                    if (!projectReference.Include.Equals(projectPath))
                    {
                        projectReferences.Add(projectReference);
                    }
                }
                if (projectReferences.Count > 0)
                {
                    itemGroup.ProjectReference = projectReferences.ToArray();
                    itemGroups.Add(itemGroup);
                }
            }

            Project.ItemGroup = itemGroups.ToArray();
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
                                ParentProjectPath = ProjectFilePath
                            });
                        }
                    }

                    if (itemGroup.ProjectReference != null && itemGroup.ProjectReference.Length > 0)
                    {
                        foreach (ProjectItemGroupProjectReference projectReference in itemGroup.ProjectReference)
                        {
                            FileInfo referencedProject = new FileInfo(projectReference.Include.Replace("\\", "/"));
                            results.Add(new ReferenceInfo()
                            {
                                Kind = ReferenceKind.Project,
                                Include = projectReference.Include,
                                ProjectReference = projectReference,
                                Name = Path.GetFileNameWithoutExtension(referencedProject.Name),
                                ItemGroup = itemGroup,
                                Project = project,
                                ParentProjectPath = ProjectFilePath
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