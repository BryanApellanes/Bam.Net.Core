using System;
using System.IO;

namespace Bam.Net.Automation
{
    public class GitCloneWorker : GitWorker
    {
        public GitCloneWorker() : base()
        {
        }
        
        protected override WorkState Do(WorkState currentWorkState)
        {
            string repoDirectory = GetRepoDirectory(BuildSettings.RepoName);
            if (!Directory.Exists(repoDirectory) || !Directory.Exists(Path.Combine(repoDirectory, ".git")))
            {
                Arguments = $"clone {BuildSettings.RepoPath} {repoDirectory}";
                base.Do(currentWorkState);
            }
            else
            {
                Info("Repository already exists: {0}", repoDirectory);
            }

            return currentWorkState;
        }
    }
}