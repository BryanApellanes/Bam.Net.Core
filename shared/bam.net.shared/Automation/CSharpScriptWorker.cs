namespace Bam.Net.Automation
{
    public class CSharpScriptWorker : Worker
    {
        protected override WorkState Do(WorkState currentWorkState)
        {
            throw new System.NotImplementedException();
        }
        
        public string Script { get; set; }

        public override string[] RequiredProperties
        {
            get { return new string[] {"Script"}; }
        }
    }
}