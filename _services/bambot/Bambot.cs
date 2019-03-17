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
            BuildSettings = Config.Load<BambotBuildSettings>();
            Builds = Workspace.Directory("builds");
            Repos = Workspace.Directory("repos");
            Tools = Workspace.Directory("tools");
            Bake = Workspace.File("tools", "bake.exe");
        }

        public Workspace Workspace
        {
            get
            {
                return Workspace.Current;
            }
        }
        
        public BamSettings BamSettings { get; set; }
        public FileInfo Bake { get; set; }
        public BambotBuildSettings BuildSettings { get; set; }

        public DirectoryInfo Builds { get; set; }
        public DirectoryInfo Repos { get; set; }
        public DirectoryInfo Tools { get; set; }

        public BambotActionResult Clone(Action<string> output, Action<string> error = null)
        {
            return Clone(BuildSettings, output, error);
        }
        
        public BambotActionResult Clone(BambotBuildSettings settings = null, Action<string> output = null, Action<string> error= null)
        {
            settings = (settings ?? BuildSettings) ?? new BambotBuildSettings();
            Info("Clone:: Using BambotBuildConfig:\r\n\r\n{0}", settings.ToYaml());
            ProcessOutput processOutput =
                BamSettings.GitPath.Start($"clone {settings.RepoPath} {Workspace.Directory(settings.RepoName).FullName}", output,
                    error);
            return new BambotActionResult()
            {
                Success = processOutput.ExitCode == 0,
                Data = processOutput
            };
        }

        public BambotActionResult GetLatest(BambotBuildSettings settings = null, Action<string> output = null,
            Action<string> error = null)
        {
            settings = (settings ?? BuildSettings) ?? new BambotBuildSettings();
            Info("GetLatest:: Using BambotBuildConfig:\r\n\r\n{0}", settings.ToYaml());
            DirectoryInfo repo = Workspace.Directory(settings.RepoName);
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

        public BambotActionResult Build(BambotBuildSettings settings = null, Action<string> output = null,
            Action<string> error = null)
        {
            BambotActionResult getLatest = GetLatest(settings, output, error);
            if (!getLatest.Success)
            {
                return getLatest;
            }
            
            return new BambotActionResult()
            {
                Success = false,
                Message = "This method is not fully implemented"
            };
        }
    }
}