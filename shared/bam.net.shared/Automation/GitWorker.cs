using System;
using System.IO;

namespace Bam.Net.Automation
{
    public abstract class GitWorker : ProcessWorker
    {
        public GitWorker()
        {
            GetRepoDirectory = (repoName) => new DirectoryInfo(Path.Combine(Repos.FullName, repoName)).FullName;
            
            BamSettings = BamSettings.Load();
            BuildSettings = Config.Load<BuildSettings>();
            CommandName = BamSettings.GitPath;
            
            Builds = Workspace.CreateDirectory("builds");
            Repos = Workspace.CreateDirectory("repos");
        }
        
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
        
        public BamSettings BamSettings { get; set; }
        public BuildSettings BuildSettings { get; set; }

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
    }
}