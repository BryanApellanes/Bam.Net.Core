using System;
using System.IO;

namespace Bam.Net
{
    public class FileSystemSemanticVersion : SemanticVersion
    {
        public string SemverDirectory { get; set; }
        public string VersionFile { get; set; }
        public static FileSystemSemanticVersion Find()
        {
            DirectoryInfo currentDirectory = new DirectoryInfo(Environment.CurrentDirectory);
            DirectoryInfo semverDirectory = new DirectoryInfo(Path.Combine(currentDirectory.FullName, "semver"));
            while (!semverDirectory.Exists)
            {
                if (currentDirectory.Parent == null)
                {
                    break;
                }

                currentDirectory = currentDirectory.Parent;
                semverDirectory = new DirectoryInfo(Path.Combine(currentDirectory.FullName, "semver"));
            }

            if (!semverDirectory.Exists)
            {
                throw new DirectoryNotFoundException("The semver directory was not found");
            }
            
            FileInfo versionFile = new FileInfo(Path.Combine(semverDirectory.FullName, "version"));
            if (!versionFile.Exists)
            {
                throw new FileNotFoundException($"The version file was not present in directory {semverDirectory.FullName}");
            }

            string content = versionFile.ReadAllText().Trim();
            FileSystemSemanticVersion semanticVersion = SemanticVersion.Parse(content).CopyAs<FileSystemSemanticVersion>();
            semanticVersion.SemverDirectory = semverDirectory.FullName;
            semanticVersion.VersionFile = versionFile.FullName;
            return semanticVersion;
        }
    }
}