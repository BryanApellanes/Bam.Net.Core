namespace Bam.Net.Automation
{
    public class SetJobDataWorker : Worker
    {
        protected override WorkState Do(WorkState currentWorkState)
        {
            currentWorkState[Key] = currentWorkState[Value];
            return currentWorkState;
        }

        public string Key { get; set; }
        public string Value { get; set; }
        
        public override string[] RequiredProperties
        {
            get { return new string[] {"Key", "Value"}; }
        }
    }
}