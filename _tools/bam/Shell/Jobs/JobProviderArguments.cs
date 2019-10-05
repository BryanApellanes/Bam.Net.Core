namespace Bam.Shell.Jobs
{
    public class JobProviderArguments : ProviderArguments
    {
        public string JobName { get; set; }
        public string WorkerName{ get; set; }
        
        public bool ShowWorkerDetails { get; set; }
    }
}