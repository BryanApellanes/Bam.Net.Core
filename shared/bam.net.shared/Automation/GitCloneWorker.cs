namespace Bam.Net.Automation
{
    public class GitCloneWorker : ProcessWorker
    {
        public GitCloneWorker()
        {
            BamSettings = BamSettings.Load();
            BuildSettings = Config.Load<BuildSettings>();
            CommandName = BamSettings.GitPath;
            
        }
        
        public BamSettings BamSettings { get; set; }
        public BuildSettings BuildSettings { get; set; }
    }
}