using System;
using System.IO;
using System.Xml.Serialization;
using Bam.Net.Application;
using Bam.Net.Automation;
using Bam.Net.CommandLine;
using Bam.Net.Logging;
using Newtonsoft.Json;
using YamlDotNet.Serialization;

namespace Bam.Net.Automation
{
    public class FullBuildWorker: Worker
    {
        public FullBuildWorker()
        {
            BamSettings = BamSettings.Load();
            BuildSettings = Bam.Net.Config.Load<BuildSettings>();
            Builds = Workspace.CreateDirectory("builds");
            Repos = Workspace.CreateDirectory("repos");
            Tools = Workspace.CreateDirectory("tools");
            SetBuildSettings();
        }

        Workspace _workspace;
        public Workspace Workspace
        {
            get
            {
                if (_workspace == null)
                {
                    _workspace = Workspace.ForType(this.GetType());
                }

                return _workspace;
            }
        }
        
        public BamSettings BamSettings { get; set; }
        public BuildSettings BuildSettings { get; set; }

        DirectoryInfo _builds;
        public DirectoryInfo Builds
        {
            get { return _builds; }
            set
            {
                _builds = value;
                _buildOutput = _builds.FullName;
            }
        }
        
        public DirectoryInfo Repos { get; set; }
        public DirectoryInfo Tools { get; set; }
        
        public bool TryGetBuildRunner(out FileInfo fileInfo)
        {
            try
            {
                fileInfo = GetBuildRunner();
                return true;
            }
            catch (Exception ex)
            {
                fileInfo = null;
                Log.Error("Failed to get build runner: {0}", ex, ex.Message);
                return false;
            }
        }
        
        public FileInfo GetBuildRunner()
        {
            Args.ThrowIf(string.IsNullOrEmpty(BuildSettings.BuildRunner), "BuildSettings.BuildRunner not specified");
            
            FileInfo buildRunner = new FileInfo(Path.Combine(Tools.FullName, BuildSettings.BuildRunner));
            if (!buildRunner.Exists)
            {
                buildRunner = new FileInfo(OSInfo.GetPath(BuildSettings.BuildRunner));
                if (!buildRunner.Exists)
                {
                    Args.Throw<InvalidOperationException>("Unable to locate specified build runner: {0}", BuildSettings.BuildRunner);
                }
            }

            return buildRunner;
        }
        
        public BuildResult Clone(Action<string> output, Action<string> error = null)
        {
            return Clone(BuildSettings, output, error);
        }
        
        public BuildResult Clone(BuildSettings settings = null, Action<string> output = null, Action<string> error= null)
        {
            settings = (settings ?? BuildSettings) ?? new BuildSettings();
            Info("Clone:: Using BuildConfig:\r\n\r\n{0}", settings.ToYaml());
            ProcessOutput processOutput =
                BamSettings.GitPath.Start($"clone {settings.RepoPath} {GetRepoDirectory(settings.RepoName).FullName}", output,
                    error);
            return new BuildResult()
            {
                Success = processOutput.ExitCode == 0,
                Data = processOutput
            };
        }

        private DirectoryInfo GetRepoDirectory(string repoName)
        {
            return new DirectoryInfo(Path.Combine(Repos.FullName, repoName));
        }
        
        /// <summary>
        /// Gets the latest code by checking if the repo has already been cloned locally then cloning it if it hasn't
        /// and checking out the target branch.  Will do "git pull" in all cases.
        /// </summary>
        /// <param name="settings"></param>
        /// <param name="output"></param>
        /// <param name="error"></param>
        /// <returns></returns>
        public BuildResult GetLatest(BuildSettings settings = null, Action<string> output = null,
            Action<string> error = null)
        {
            string startDirectory = Environment.CurrentDirectory;
            try
            {
                settings = (settings ?? BuildSettings) ?? new BuildSettings();
                Info("GetLatest:: Using BuildConfig:\r\n\r\n{0}", settings.ToYaml());
                DirectoryInfo repo = GetRepoDirectory(settings.RepoName);
                if (!repo.Exists || !Directory.Exists(Path.Combine(repo.FullName, ".git")))
                {
                    BuildResult actionResult = Clone(settings, output, error);
                    if (!actionResult.Success)
                    {
                        ProcessOutput processOutput = actionResult.Data.Cast<ProcessOutput>();
                        output(processOutput.StandardOutput);
                        output(processOutput.StandardError);
                        return actionResult;
                    }
                }

                Environment.CurrentDirectory = repo.FullName;
                ProcessOutput checkoutOutput = BamSettings.GitPath.Start($"checkout -f {settings.BranchName}", output, error);
                if (checkoutOutput.ExitCode > 0)
                {
                    return new BuildResult()
                    {
                        Success = checkoutOutput.ExitCode == 0,
                        Data = checkoutOutput
                    };
                }

                ProcessOutput pullOutput = BamSettings.GitPath.Start("pull", output, error);
                Environment.CurrentDirectory = startDirectory;
                return new BuildResult()
                {
                    Success = pullOutput.ExitCode == 0,
                    Data = pullOutput
                };
            }
            finally
            {
                Environment.CurrentDirectory = startDirectory;
            }
        }

        public BuildResult Build(BuildSettings settings = null, Action<string> output = null,
            Action<string> error = null)
        {
            string startDirectory = Environment.CurrentDirectory;
            try
            {
                BuildResult getLatest = GetLatest(settings, output, error);
                if (!getLatest.Success)
                {
                    return getLatest;
                }

                settings = (settings ?? BuildSettings) ?? new BuildSettings();
                Environment.CurrentDirectory = GetRepoDirectory(settings.RepoName).FullName;
                ProcessOutput buildOutput = GetBuildRunner().FullName.Start($"{settings.BuildArguments} /output:{settings.BuildOutput}", output, error);
                if (buildOutput.ExitCode != settings.BuildSuccessExitCode)
                {
                    return new BuildResult()
                    {
                        Success = false,
                        Message = $"Build exited with code {buildOutput.ExitCode}"
                    };
                }

                WorkState($"{Name}_BuildOutput", settings.BuildOutput);
                WorkState($"{Name}_BuildResult", buildOutput);
                return new BuildResult()
                {
                    Success =  true
                };
            }
            finally
            {
                Environment.CurrentDirectory = startDirectory;
            }
        }

        protected override WorkState Do(WorkState currentWorkState)
        {
            if (currentWorkState != null)
            {
                if (currentWorkState.Status == Status.Failed && !currentWorkState.ContinueOnFailure)
                {
                    Warn("Current workstate status is Failed.  Exiting");
                    return currentWorkState;
                }
            }

            Build(GetBuildSettings(), o => Console(o), e => Console(e));
            return new WorkState(this);
        }

        public override string[] RequiredProperties
        {
            get
            {
                return new string[] {"BuildOutput", "RepoName", "RepoPath", "BranchName", "Config", "Runtime"};
            }
        }

        public string RepoName { get; set; }
        public string RepoPath { get; set; }
        public string BranchName { get; set; }
        public string Config { get; set; }
        public string OSName { get; set; }
        
        string _buildOutput;
        public string BuildOutput
        {
            get { return _buildOutput; }
            set
            {
                _buildOutput = value;
                if (string.IsNullOrEmpty(_buildOutput))
                {
                    _buildOutput = Workspace.Path("builds");
                }
                _builds = new DirectoryInfo(_buildOutput);
            }
        }

        public void SetBuildSettings(BuildSettings settings = null)
        {
            settings = settings ?? new BuildSettings();
            this.CopyProperties(settings);
            Config = settings.Config.ToString();
            OSName = settings.Runtime.OsName.ToString();
        }
        
        public virtual BuildSettings GetBuildSettings()
        {
            return new BuildSettings()
            {
                RepoName = RepoName,
                RepoPath = RepoPath,
                BranchName = BranchName,
                Config = Config.ToEnum<BuildConfig>(),
                Runtime = Bam.Net.Application.Runtime.For(OSName),
                BuildOutput = BuildOutput
            };
        }
    }
}