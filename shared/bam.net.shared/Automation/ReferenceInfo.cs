using System.IO;
using Bam.Net.Automation;

namespace Bam.Net
{
    public class ReferenceInfo
    {
        public const string DefaultVersion = "(1.0.3,)";
        public ReferenceKind Kind { get; set; }
        public string Include { get; set; }
        public string Version { get; set; }

        protected internal string Name { get; set; }

        protected internal ProjectItemGroupPackageReference PackageReference { get; set; }
        protected internal ProjectItemGroupProjectReference ProjectReference { get; set; }

        protected internal ProjectItemGroup ItemGroup { get; set; }
        protected internal Project Project { get; set; }
        
        protected internal string ParentProjectPath { get; set; }

        protected internal ProjectItemGroupPackageReference ToPackageReference(string version = null)
        {
            version = version ?? DefaultVersion;
            if (Kind == ReferenceKind.Package)
            {
                return PackageReference;
            }
            return new ProjectItemGroupPackageReference()
            {
                Include = Name,
                Version = version
            };
        }

        protected internal ProjectItemGroupProjectReference ToProjectReference(string projectDirectory)
        {
            if (Kind == ReferenceKind.Project)
            {
                return ProjectReference;
            }

            return new ProjectItemGroupProjectReference()
            {
                Include = Path.Combine(projectDirectory, $"{Include}.csproj")
            };
        }
    }
}