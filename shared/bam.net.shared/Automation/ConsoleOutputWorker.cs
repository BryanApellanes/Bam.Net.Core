namespace Bam.Net.Automation
{
    public class ConsoleOutputWorker : Worker
    {
        protected override WorkState Do(WorkState currentWorkState)
        {
            WorkState result = new WorkState(this);
            Console($"Worker {Name}, previous worker status = {currentWorkState.Status.ToString()}: {currentWorkState.Message}");
            return result;
        }

        public override string[] RequiredProperties { get; }
    }
}