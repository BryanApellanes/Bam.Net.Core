using System.IO;
using Bam.Net.Logging;

namespace Bam.Net
{
    public class AssemblySemanticVersion: FileSystemSemanticVersion
    {
        /// <summary>
        /// The same as Revision.
        /// </summary>
        public int BuildNumber
        {
            get => Revision;
            set => _revision = value;
        }

        private int? _revision;
        /// <summary>
        /// The revision number; this value is based on the
        /// Build or Commit.  The Revision is the SHA1 of the Build
        /// converted to an integer.
        /// </summary>
        public int Revision
        {
            get
            {
                if (_revision == null)
                {
                    _revision = (Commit?.ToSha1Int()).Value;
                }

                return _revision ?? 0;
            }
        }

        private string _commit;
        public string Commit
        {
            get
            {
                if (string.IsNullOrEmpty(_commit))
                {
                    _commit = GitLog.AbbreviatedCommitHash;
                }

                return _commit;
            }
            set
            {
                _commit = value;
            } 
        }

        private string _build;
        public override string Build
        {
            get => _build;
            set => _build = value;
        }

        public override string ToString()
        {
            return $"{Major}.{Minor}.{BuildNumber}.{Revision}";
        }

        public static void WriteProjectSemanticAssemblyInfo(string projectFilePath)
        {
            WriteProjectSemanticAssemblyInfo(new FileInfo(projectFilePath));
        }
        
        public static void WriteProjectSemanticAssemblyInfo(FileInfo projectFile)
        {
            if (FileSystemSemanticVersion.TryFind(projectFile.Directory.FullName,
                out FileSystemSemanticVersion currentVersion))
            {
                From(currentVersion).WriteSemanticAssemblyInfo(projectFile.Directory.FullName);
            }
            else
            {
                Log.Warn("Failed to find semantic version for project file: {0}", projectFile?.FullName ?? "[null]");
            }
        }
        
        public static AssemblySemanticVersion From(SemanticVersion semanticVersion)
        {
            return semanticVersion.CopyAs<AssemblySemanticVersion>();
        }
        
        /// <summary>
        /// Write this AssemblySemanticVersion to a SemanticAssemblyInfo.cs file
        /// </summary>
        /// <param name="overwrite"></param>
        /// <param name="path"></param>
        public void WriteSemanticAssemblyInfo(string path = ".", bool overwrite = true)
        {
            FileInfo file = new FileInfo(Path.Combine(path, "SemanticAssemblyInfo.cs"));
            Log.Info("Writing file {0}", file.FullName);
            ToSemanticAssemblyInfo().SafeWriteToFile(file.FullName, overwrite);
        }
        
        public string ToSemanticAssemblyInfo()
        {
            return $"using System.Reflection;\r\n" +
                   $"using Bam.Net;\r\n\r\n" +
                   $"[assembly: AssemblyVersion(\"{Major}.{Minor}.{Patch}.0\")]\r\n" +
                   $"[assembly: AssemblyFileVersion(\"{Major}.{Minor}.{Patch}.{Revision}\")]\r\n" +
                   $"[assembly: AssemblyCommit(\"{Commit}\")]\r\n" +
                   $"[assembly: AssemblySemanticVersion(\"{base.ToString()}\")]\r\n";
        }
    }
}