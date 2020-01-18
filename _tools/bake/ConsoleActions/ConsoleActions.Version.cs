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
        [ConsoleAction("version", "Update the package version of each project referenced by a recipe.")]
        public void Version()
        {
            FileSystemSemanticVersion currentVersion = FileSystemSemanticVersion.Find();
            string prompt = "Please specify 'major', 'minor' or 'patch' to increment version component.";
            string versionArg = GetArgument("version", true, prompt);

            SemanticVersion newVersion = GetNewVersion(currentVersion, versionArg);
            Recipe recipe = GetRecipe();
            OutLineFormat("Current version in semver directory: {0}", currentVersion.ToString());
            OutLineFormat("New version in semver directory: {0}", newVersion.ToString());
            
            foreach (string projectFile in recipe.ProjectFilePaths)
            {
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
                    OutLineFormat("Version element not found for in project file: {0}", ConsoleColor.Yellow, projectFile);
                }
            }
        }

        private SemanticVersion GetNewVersion(FileSystemSemanticVersion currentVersion, string versionArg)
        {
            SemanticVersion newVersion = currentVersion.CopyAs<SemanticVersion>();
            if (!string.IsNullOrEmpty(versionArg))
            {
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
                string gitRepo = GetArgument("gitRepo", "Please specify the path to the git repo");
                GitLog gitLog = GitLog.Get(gitRepo, 1).First();
                newVersion.SetSuffix(gitLog.CommitHash.First(6));
            }

            if (Arguments.Contains("test"))
            {
                newVersion.SetSuffix("test");
            }

            if (Arguments.Contains("staging"))
            {
                string gitRepo = GetArgument("gitRepo", "Please specify the path to the git repo");
                GitLog gitLog = GitLog.Get(gitRepo, 1).First();
                newVersion.PreRelease = "rc";
                newVersion.Build = gitLog.CommitHash.First(6);
            }

            if (Arguments.Contains("release"))
            {
                newVersion.ClearSuffix();
            }

            return newVersion;
        }
    }
}