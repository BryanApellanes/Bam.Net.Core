using System;
using System.IO;
using Bam.Net.CommandLine;

namespace Bam.Net.Automation
{
    public class BakeWorker : Worker
    {
        public BakeWorker()
        {
            GetRepoDirectory = (repoName) => new DirectoryInfo(Path.Combine(Repos.FullName, repoName)).FullName;
            
            BamSettings = BamSettings.Load();
            BuildSettings = Config.Load<BuildSettings>();
            Tools = Workspace.CreateDirectory("tools");
            
            Builds = Workspace.CreateDirectory("builds");
            Repos = Workspace.CreateDirectory("repos");
        }
        
        public string OutputDirectory { get; set; }
        
        public Func<string, string> GetRepoDirectory { get; set; }
        
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

        public BuildSettings BuildSettings { get; set; }
        
        public DirectoryInfo Tools { get; set; }
        
        public BamSettings BamSettings { get; set; }
        
        DirectoryInfo _builds;
        public DirectoryInfo Builds
        {
            get { return _builds; }
            set
            {
                _builds = value;
            }
        }
        
        public DirectoryInfo Repos { get; set; }
        
        public virtual FileInfo GetBuildRunner()
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

        protected override WorkState Do(WorkState currentWorkState)
        {
            string startDirectory = Environment.CurrentDirectory;
            try
            {
                ProcessOutput buildOutput = GetBuildRunner().FullName
                    .Start($"{BuildSettings.BuildArguments} /output:{OutputDirectory}", o => Info(o),
                        e => Warn(e));

                currentWorkState.Status = buildOutput.ExitCode == 0 ? Status.Succeeded : Status.Failed;
            }
            finally
            {
                Environment.CurrentDirectory = startDirectory;
            }

            return currentWorkState;
        }

        public override string[] RequiredProperties
        {
            get { return new string[] {"OutputDirectory"}; }
        }
    }
}