using Bam.Net.Services.Clients;

namespace Bam.Net.Automation
{
    public abstract class ScriptWorkerContext
    {
        public ApplicationContext ApplicationContext { get; set; }
        public Job Job { get; set; }
        public WorkState WorkState { get; set; }
        public IWorker Self { get; set; }

        public abstract void Execute(WorkState workState = null);
    }
}