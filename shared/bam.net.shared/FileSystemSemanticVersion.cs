using System;
using System.IO;
using System.Linq;
using Bam.Net.Automation.SourceControl;
using UnityEngine;

namespace Bam.Net
{
    public class FileSystemSemanticVersion : SemanticVersion
    {
        public FileSystemSemanticVersion()
        {
            SemverDirectory = ".";
            VersionFile = "./version";
        }
        public string SemverDirectory { get; private set; }
        public string VersionFile { get; set; }

        private GitLog _gitLog;
        private readonly object _gitLogLock = new object();

        public GitLog GitLog
        {
            get { return _gitLogLock.DoubleCheckLock<GitLog>(ref _gitLog, () => GitLog.Get(SemverDirectory, 1).First()); }
            set => _gitLog = value;
        }

        public void Save()
        {
            this.ToString().SafeWriteToFile(VersionFile, true);
        }

        public static bool TryFind(string directoryPath, out FileSystemSemanticVersion fileSystemSemanticVersion)
        {
            fileSystemSemanticVersion = null;
            try
            {
                fileSystemSemanticVersion = Find(directoryPath);
                return true;
            }
            catch (Exception ex)
            {
                Bam.Net.Logging.Log.Warn("Exception finding semver for directory {0}: {1}", directoryPath, ex.Message);
                return false;
            }
        }
        
        public static FileSystemSemanticVersion Find()
        {
            DirectoryInfo currentDirectory = new DirectoryInfo(Environment.CurrentDirectory);
            return Find(currentDirectory);
        }

        public static FileSystemSemanticVersion Find(string fromDirectory)
        {
            return Find(new DirectoryInfo(fromDirectory));
        }
        
        public static FileSystemSemanticVersion Find(DirectoryInfo currentDirectory)
        {
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