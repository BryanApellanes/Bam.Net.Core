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
using Bam.Net.Logging;
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
            SemanticVersion specifiedVersion = GetVersionArg(versionArg);
            SemanticVersion nextVersion = GetNextVersionFrom(specifiedVersion);

            string recipePath = Arguments["versionRecipe"];
            if (string.IsNullOrEmpty(recipePath))
            {
                OutLineFormat("Please specify /versionRecipe:<path_to_recipe_to_update_version>");
                Exit(1);
            }

            Recipe recipe = recipePath.FromJsonFile<Recipe>();

            foreach (string projectFile in recipe.ProjectFilePaths)
            {
                FileInfo projectFileInfo = new FileInfo(projectFile);
                FileSystemSemanticVersion currentProjectVersion = FileSystemSemanticVersion.Find(projectFileInfo.Directory);
                SemanticVersion nextProjectVersion = GetNextVersionFrom(currentProjectVersion);
                SetGitLog(nextProjectVersion, projectFile);
                SemanticVersion versionToUse = nextVersion >= nextProjectVersion ? nextVersion : nextProjectVersion;
                OutLineFormat("Project: {0}", ConsoleColor.Cyan, projectFileInfo.FullName);
                OutLineFormat("Current version in semver directory {0}: {1}", currentProjectVersion.SemverDirectory, currentProjectVersion.ToString());
                OutLineFormat("Next project version: {0}", nextProjectVersion.ToString());
                OutLineFormat("Using version: {0}", versionToUse.ToString());

                XDocument xdoc = XDocument.Load(projectFile);
                XElement versionElement = xdoc.Element("Project").Element("PropertyGroup").Element("Version");
                
                if (versionElement != null)
                {
                    string version = versionToUse.ToString();
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
                
                AssemblySemanticVersion.WriteProjectSemanticAssemblyInfo(projectFile, versionToUse);
            }
        }

        private SemanticVersion GetVersionArg(string versionArg)
        {
            if (SemanticVersion.TryParse(versionArg, out SemanticVersion parsedVersion))
            {
                return parsedVersion;
            }
            SemanticVersion result = new SemanticVersion();
            Log.Warn("Couldn't parse versionArg {0}, using default version {1}", versionArg, result.ToString());
            return result;
        }
        
        private SemanticVersion GetNextVersionFrom(SemanticVersion currentVersion)
        {
            SemanticVersion newVersion = currentVersion.CopyAs<SemanticVersion>();
           
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
                newVersion.Lifecycle = SemanticLifecycle.Dev;
            }

            if (Arguments.Contains("test"))
            {
                newVersion.Lifecycle = SemanticLifecycle.Test;
            }

            if (Arguments.Contains("staging"))
            {
                newVersion.Lifecycle = SemanticLifecycle.Staging;
            }

            if (Arguments.Contains("release"))
            {
                newVersion.Lifecycle = SemanticLifecycle.Release;
            }

            if (newVersion.Equals(currentVersion))
            {
                newVersion.Increment(VersionSpec.Patch);
            }
            
            return newVersion;
        }

        private static void SetGitLog(SemanticVersion newVersion, string projectFile)
        {
            string gitRepo = new FileInfo(projectFile).Directory.FullName;
            GitLog gitLog = GitLog.Get(gitRepo, 1).First();
            newVersion.Build = gitLog.AbbreviatedCommitHash;
        }
    }
}