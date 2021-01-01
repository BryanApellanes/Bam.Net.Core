using System;
using System.IO;
using System.Linq;
using Bam.Net.CommandLine;
using Bam.Net.Github.Actions;
using Bam.Net.Testing.Integration;

namespace Bam.Net.Github.Actions.Tests
{
    [Serializable]
    public class IntegrationTests
    {
        [ConsoleAction]
        [IntegrationTest]
        public void CanGetArtifacts()
        {
            GithubActionsClient client = new GithubActionsClient();
            GithubArtifact[] artifacts = client.GetArtifacts("BryanApellanes", "Bam.Net.Core").ToArray();
            artifacts.ShouldNotBeNull("artifacts was null");
            artifacts.Length.ShouldBeGreaterThan(0, "No artifacts were returned");
            artifacts.Each(artifact =>
            {
                Message.PrintLine(artifact.ToJson(true), ConsoleColor.Cyan);
            });
        }

        [ConsoleAction]
        [IntegrationTest]
        public void CanDownloadArtifact()
        {
            GithubActionsClient client = new GithubActionsClient();
            GithubArtifact[] artifacts = client.GetArtifacts("BryanApellanes", "Bam.Net.Core").ToArray();
            artifacts.Length.ShouldBeGreaterThan(0, "No artifacts were returned");
            GithubArtifact artifact = artifacts.First();
            string fileName = $"./{nameof(CanDownloadArtifact)}.zip";
            FileInfo fileInfo = artifact.DownloadTo(fileName);
            fileInfo.Exists.ShouldBeTrue("file doesn't exists after attempted download");
            
            fileInfo.Length.ShouldBeGreaterThanOrEqualTo(artifact.SizeInBytes);
            Message.PrintLine("Artifact Size: {0}, File Size: {1}", artifact.SizeInBytes, fileInfo.Length);
        }
    }
}