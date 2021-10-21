using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Bam.Net.CommandLine;
using Bam.Net.CoreServices.AccessControl;
using Bam.Net.Encryption;
using Bam.Net.Github.Actions;
using Bam.Net.Testing.Integration;

namespace Bam.Net.Github.Actions.Tests
{
    [Serializable]
    public class GithubActionsClientIntegrationTests : CommandLineTool
    {
        [ConsoleAction]
        [IntegrationTest]
        public void CanGetArtifacts()
        {
            GithubActionsClient client = new GithubActionsClient();
            GithubArtifactInfo[] artifacts = client.ListArtifactInfos(DataConstants.RepoOwnerUserName, DataConstants.RepoName).ToArray();
            artifacts.ShouldNotBeNull("artifacts was null");
            artifacts.Length.ShouldBeGreaterThan(0, "No artifacts were returned");
            artifacts.Each(artifact =>
            {
                Message.PrintLine(artifact.ToJson(true), ConsoleColor.Cyan);
            });
            Pass(nameof(CanGetArtifacts));
        }

        [ConsoleAction]
        [IntegrationTest]
        public void CanGetArtifact()
        {
            GithubActionsClient client = new GithubActionsClient(DataConstants.RepoOwnerUserName, DataConstants.RepoName);
            GithubArtifactInfo[] artifacts = client.ListArtifactInfos(DataConstants.RepoOwnerUserName, DataConstants.RepoName).ToArray();
            uint artifactId = artifacts[0].Id;
            GithubArtifactInfo artifactInfo = client.GetArtifactInfo(artifactId);
            artifactInfo.ShouldNotBeNull();
            Message.PrintLine(artifactInfo.ToJson(true));
        }
        
        [ConsoleAction]
        [IntegrationTest]
        public void WillAddAuthorizationHeader()
        {
            Vault testVault = Vault.Create(new FileInfo($"./{nameof(WillAddAuthorizationHeader)}.sqlite"), nameof(WillAddAuthorizationHeader));
            string testHeaderValue = $"{nameof(WillAddAuthorizationHeader)}_".RandomLetters(8);
            TestGithubActionsClient client = new TestGithubActionsClient(testHeaderValue, testVault);
            Dictionary<string, string> header = client.CallGetHeaders(true);
            header.ContainsKey("Authorization");
            header["Authorization"].ShouldBeEqualTo($"token {testHeaderValue}");
            Pass(nameof(WillAddAuthorizationHeader));
        }
        
        [ConsoleAction]
        [IntegrationTest]
        public void WillAddAuthorizationHeaderUsingParameterlessConstructor()
        {
            Vault testVault = Vault.Create(new FileInfo($"./{nameof(WillAddAuthorizationHeaderUsingParameterlessConstructor)}.sqlite"), nameof(WillAddAuthorizationHeaderUsingParameterlessConstructor));
            string testHeaderValue = $"{nameof(WillAddAuthorizationHeaderUsingParameterlessConstructor)}_".RandomLetters(8);
            TestGithubActionsClient client = new TestGithubActionsClient();
            client.SetTestValue(testHeaderValue);
            Dictionary<string, string> header = client.CallGetHeaders(true);
            header.ContainsKey("Authorization");
            header["Authorization"].ShouldBeEqualTo($"token {testHeaderValue}");
            Pass(nameof(WillAddAuthorizationHeader));
        }

        [ConsoleAction]
        [IntegrationTest]
        public void ConfigKeyShouldBeSet()
        {
            GithubActionsClient client = new GithubActionsClient();
            IAuthorizationHeaderProvider authorizationHeaderProvider = client.AuthorizationHeaderProvider;
            string configKey = authorizationHeaderProvider.ConfigKey; 
            configKey.ShouldNotBeNull();
            configKey.ShouldNotBeBlank();
            Pass(nameof(ConfigKeyShouldBeSet));
        }

        [ConsoleAction]
        [IntegrationTest]
        public void CanDownloadArtifact()
        {
            GithubActionsClient client = new GithubActionsClient();
            GithubArtifactInfo[] artifacts = client.ListArtifactInfos(DataConstants.RepoOwnerUserName, DataConstants.RepoName).ToArray();
            artifacts.Length.ShouldBeGreaterThan(0, "No artifacts were returned");
            GithubArtifactInfo artifactInfo = artifacts.First();
            string fileName = $"./{nameof(CanDownloadArtifact)}.zip";
            FileInfo fileInfo = artifactInfo.DownloadTo(fileName);
            fileInfo.Exists.ShouldBeTrue("file doesn't exist after attempted download");

            Message.PrintLine("Artifact Size Unzipped: {0}, File Size Zipped: {1}", artifactInfo.SizeInBytes, fileInfo.Length);
            fileInfo.Delete();
            Pass(nameof(CanDownloadArtifact));
        }
    }
}