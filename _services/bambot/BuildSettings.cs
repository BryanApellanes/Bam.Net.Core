using Bam.Net.Automation;

namespace Bam.Net.Application
{
    public class BuildSettings
    {
        public BuildSettings()
        {
            RepoPath = $"https://github.com/BryanApellanes/Bam.Net.Core.git";
            RepoName = "Bam.Net.Core";
            BranchName = "master";
            Config = BuildConfig.Release;
            Runtime = Runtime.Windows;
            
            BuildRunner = "bake";
            BuildArguments = "/all:./_tools.core/";

            TestRunner = "bamtest";
            TestArguments = "see bamtest for more information";
        }

        public BuildSettings(string repoName) : this()
        {
            RepoName = repoName ?? "Bam.Net.Core";
            RepoPath = $"https://github.com/BryanApellanes/{RepoName}.git";
        }

        public string RepoName { get; set; }
        public string RepoPath { get; set; }
        public string BranchName { get; set; }
        public BuildConfig Config { get; set; }
        public Runtime Runtime { get; set; }

        public string BuildRunner { get; set; }
        public string BuildArguments { get; set; }
        
        public int BuildSuccessExitCode { get; set; }
        
        public string TestRunner { get; set; }
        public string TestArguments { get; set; }
        public int TestSuccessExitCode { get; set; }
        
        public string BuildOutputRoot { get; set; }
    }
}