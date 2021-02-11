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
            GithubArtifact[] artifacts = client.GetArtifacts("BryanApellanes", "Bam.Net.Core").ToArray();
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
            Vault testVault = Vault.Create(new FileInfo($"./{nameof(WillAddAuthorizationHeader)}.sqlite"), nameof(WillAddAuthorizationHeader));
            string testHeaderValue = $"{nameof(WillAddAuthorizationHeader)}_".RandomLetters(8);
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
            GithubArtifact[] artifacts = client.GetArtifacts("BryanApellanes", "Bam.Net.Core").ToArray();
            artifacts.Length.ShouldBeGreaterThan(0, "No artifacts were returned");
            GithubArtifact artifact = artifacts.First();
            string fileName = $"./{nameof(CanDownloadArtifact)}.zip";
            FileInfo fileInfo = artifact.DownloadTo(fileName);
            fileInfo.Exists.ShouldBeTrue("file doesn't exists after attempted download");
            
            fileInfo.Length.ShouldBeGreaterThanOrEqualTo(artifact.SizeInBytes);
            Message.PrintLine("Artifact Size: {0}, File Size: {1}", artifact.SizeInBytes, fileInfo.Length);
            Pass(nameof(CanDownloadArtifact));
        }
    }
}