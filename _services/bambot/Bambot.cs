using System;
using System.IO;
using Bam.Net.CommandLine;
using Bam.Net.Logging;

namespace Bam.Net.Application
{
    public class Bambot: Loggable
    {
        public Bambot()
        {
            BamSettings = BamSettings.Load();
            BuildSettings = Config.Load<BuildSettings>();
            Builds = Workspace.CreateDirectory("builds");
            Repos = Workspace.CreateDirectory("repos");
            Tools = Workspace.CreateDirectory("tools");
        }

        public Workspace Workspace
        {
            get
            {
                return Workspace.ForClass(this.GetType());
            }
        }
        
        public BamSettings BamSettings { get; set; }
        public BuildSettings BuildSettings { get; set; }
        public DirectoryInfo Builds { get; set; }
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
        
        public BambotActionResult Clone(Action<string> output, Action<string> error = null)
        {
            return Clone(BuildSettings, output, error);
        }
        
        public BambotActionResult Clone(BuildSettings settings = null, Action<string> output = null, Action<string> error= null)
        {
            settings = (settings ?? BuildSettings) ?? new BuildSettings();
            Info("Clone:: Using BambotBuildConfig:\r\n\r\n{0}", settings.ToYaml());
            ProcessOutput processOutput =
                BamSettings.GitPath.Start($"clone {settings.RepoPath} {Workspace.CreateDirectory(settings.RepoName).FullName}", output,
                    error);
            return new BambotActionResult()
            {
                Success = processOutput.ExitCode == 0,
                Data = processOutput
            };
        }

        /// <summary>
        /// Gets the latest code by checking if the repo has already been cloned locally then cloning it if it hasn't
        /// and checking out the target branch.  Will do "git pull" in all cases.
        /// </summary>
        /// <param name="settings"></param>
        /// <param name="output"></param>
        /// <param name="error"></param>
        /// <returns></returns>
        public BambotActionResult GetLatest(BuildSettings settings = null, Action<string> output = null,
            Action<string> error = null)
        {
            settings = (settings ?? BuildSettings) ?? new BuildSettings();
            Info("GetLatest:: Using BambotBuildConfig:\r\n\r\n{0}", settings.ToYaml());
            DirectoryInfo repo = Workspace.CreateDirectory(settings.RepoName);
            if (!repo.Exists)
            {
                BambotActionResult actionResult = Clone(settings, output, error);
                if (!actionResult.Success)
                {
                    ProcessOutput processOutput = actionResult.Data.Cast<ProcessOutput>();
                    output(processOutput.StandardOutput);
                    output(processOutput.StandardError);
                    return actionResult;
                }
            }

            string startDirectory = Environment.CurrentDirectory;
            Environment.CurrentDirectory = repo.FullName;
            ProcessOutput checkoutOutput = BamSettings.GitPath.Start($"checkout -f {settings.BranchName}", output, error);
            if (checkoutOutput.ExitCode > 0)
            {
                return new BambotActionResult()
                {
                    Success = checkoutOutput.ExitCode == 0,
                    Data = checkoutOutput
                };
            }

            ProcessOutput pullOutput = BamSettings.GitPath.Start("pull", output, error);
            Environment.CurrentDirectory = startDirectory;
            return new BambotActionResult()
            {
                Success = pullOutput.ExitCode == 0,
                Data = pullOutput
            };
        }

        public BambotActionResult Build(BuildSettings settings = null, Action<string> output = null,
            Action<string> error = null)
        {
            BambotActionResult getLatest = GetLatest(settings, output, error);
            if (!getLatest.Success)
            {
                return getLatest;
            }

            ProcessOutput buildOutput = settings.BuildRunner.Start(settings.BuildArguments, output, error);
            if (buildOutput.ExitCode != settings.BuildSuccessExitCode)
            {
                return new BambotActionResult()
                {
                    Success = false,
                    Message = $"Build exited with code {buildOutput.ExitCode}"
                };
            }

            ProcessOutput testOutput = settings.TestRunner.Start(settings.TestArguments, output, error);
            if (testOutput.ExitCode != settings.TestSuccessExitCode)
            {
                return new BambotActionResult()
                {
                    Success = false,
                    Message = $"Test exited with code {testOutput.ExitCode}"
                };
            }
            
            
            return new BambotActionResult()
            {
                Success = true,
                Message = "Build and test completed successfully"
            };
        }
    }
}