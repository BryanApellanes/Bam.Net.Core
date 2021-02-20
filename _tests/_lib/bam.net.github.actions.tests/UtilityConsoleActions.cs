﻿using System;
using Bam.Net.CommandLine;
using Bam.Net.Github.Actions.Tests;

namespace Bam.Net.Github.Actions
{
    [Serializable]
    public class UtilityConsoleActions : CommandLineTool
    {
        [ConsoleAction]
        public void ListArtifactNames()
        {
            GithubActionsClient client = new GithubActionsClient(DataConstants.RepoOwnerUserName, DataConstants.RepoName);
            int number = 1;
            client.GetArtifacts().Each(artifact =>
            {
                Message.PrintLine($"{number++}. {artifact.Name}");
            });
        }

        [ConsoleAction]
        public void DeleteArtifactsNamedWrong()
        {
            GithubActionsClient client = new GithubActionsClient(DataConstants.RepoOwnerUserName, DataConstants.RepoName);
            int number = 1;
            client.GetArtifacts().Each(artifact =>
            {
                string name = artifact.Name;
                string[] nameSegments = name.Split('-');
                if (nameSegments[^1].Length != 7)
                {
                    Message.PrintLine($"{number++}. {artifact.Name} {artifact.Id}");
                    if (Confirm("Delete?"))
                    {
                        if (client.DeleteArtifact(artifact.Id))
                        {
                            Message.PrintLine("Deleted artifact {0}", ConsoleColor.Yellow, artifact.Name);
                        }
                    }
                }
            });
        }
    }
}