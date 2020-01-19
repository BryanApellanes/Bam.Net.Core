using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Xml;
using System.Xml.Linq;
using System.Xml.XPath;
using Bam.Net.Application;
using Bam.Net.Automation.SourceControl;
using Bam.Net.CommandLine;
using Bam.Net.Testing;

namespace Bam.Net.Bake
{
    public partial class ConsoleActions
    {
        [ConsoleAction("version", "Update the package version of each project referenced by a recipe; also (over)writes the SemanticAssemblyInfo.cs file for all projects in the recipe.")]
        public void Version()
        {
            string prompt = "Please specify 'major', 'minor' or 'patch' to increment version component.";
            string versionArg = GetArgument("version", true, prompt);

            //TODO : change how this gets the recipe because /recipe is a console exec switch to bake the specified recipe. Consider /versionRecipe
            Recipe recipe = GetRecipe();

            foreach (string projectFile in recipe.ProjectFilePaths)
            {
                FileInfo projectFileInfo = new FileInfo(projectFile);
                FileSystemSemanticVersion currentVersion = FileSystemSemanticVersion.Find(projectFileInfo.Directory);
                FileSystemSemanticVersion newVersion = GetNewVersion(currentVersion, versionArg, projectFile);
                OutLineFormat("Current version in semver directory {0}: {1}", newVersion.SemverDirectory, currentVersion.ToString());
                OutLineFormat("New version in semver directory {0}: {1}", newVersion.SemverDirectory, newVersion.ToString());
                
                XDocument xdoc = XDocument.Load(projectFile);
                XElement versionElement = xdoc.Element("Project").Element("PropertyGroup").Element("Version");
                
                if (versionElement != null)
                {
                    string version = newVersion.ToString();
                    OutLineFormat("Setting version for {0} to {1}", projectFile, version);
                    versionElement.Value = version;
                    XmlWriterSettings settings = new XmlWriterSettings {Indent = true, OmitXmlDeclaration = true};
                    using (XmlWriter xw = XmlWriter.Create(projectFile, settings))
                    {
                        xdoc.Save(xw);
                    }
                }
                else
                {
                    OutLineFormat("Version element not found in project file: {0}", ConsoleColor.Yellow, projectFile);
                }
                newVersion.Save();
                AssemblySemanticVersion.WriteProjectSemanticAssemblyInfo(projectFile);
            }
        }

        private FileSystemSemanticVersion GetNewVersion(FileSystemSemanticVersion currentVersion, string versionArg, string projectFile)
        {
            FileSystemSemanticVersion newVersion = currentVersion.CopyAs<FileSystemSemanticVersion>();
            if (!string.IsNullOrEmpty(versionArg))
            {
                if (SemanticVersion.TryParse(versionArg, out SemanticVersion parsedVersion))
                {
                    newVersion.CopyProperties(parsedVersion);
                    return newVersion;
                }
                VersionSpec versionSpec = versionArg.ToEnum<VersionSpec>();
                newVersion.Increment(versionSpec);
            }
            if (Arguments.Contains("major"))
            {
                newVersion.Increment(VersionSpec.Major);
            }

            if (Arguments.Contains("minor"))
            {
                newVersion.Increment(VersionSpec.Minor);
            }

            if (Arguments.Contains("patch"))
            {
                newVersion.Increment(VersionSpec.Patch);
            }

            if (Arguments.Contains("dev"))
            {
                string gitRepo = new FileInfo(projectFile).Directory.FullName;
                GitLog gitLog = GitLog.Get(gitRepo, 1).First();
                newVersion.SetSuffix(gitLog.AbbreviatedCommitHash);
                newVersion.GitLog = gitLog;
            }

            if (Arguments.Contains("test"))
            {
                newVersion.SetSuffix("test");
            }

            if (Arguments.Contains("staging"))
            {
                string gitRepo = new FileInfo(projectFile).Directory.FullName;
                GitLog gitLog = GitLog.Get(gitRepo, 1).First();
                newVersion.ReleasePrefix = "rc";
                newVersion.Build = gitLog.AbbreviatedCommitHash;
                newVersion.GitLog = gitLog;
            }

            if (Arguments.Contains("release"))
            {
                newVersion.ClearSuffix();
            }

            return newVersion;
        }
    }
}