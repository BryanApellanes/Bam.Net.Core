using System;
using System.Linq;
using Bam.Net.CommandLine;
using bam.net.github.actions;
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
    }
}