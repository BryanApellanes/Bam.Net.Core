using Bam.Net.Services;

namespace Bam.Net.Automation
{
    public class CSharpScriptWorkerModel
    {
        public string Namespace { get; set; }
        public string Name { get; set; }
        public string Script { get; set; }
    }
    
    public class CSharpScriptWorker : Worker
    {
        public CSharpScriptWorker()
        {
            DynamicTemplatedClassManager = new DynamicTemplatedClassManager();
        }
        
        public DynamicTemplatedClassManager DynamicTemplatedClassManager { get; set; }
        
        protected override WorkState Do(WorkState currentWorkState)
        {
            CSharpScriptWorkerModel model = new CSharpScriptWorkerModel()
            {
                Namespace = Namespace,
                Name = Name,
                Script = Script
            };
            ScriptWorkerContext workerContext =
                DynamicTemplatedClassManager.Compile<ScriptWorkerContext>("CSharpScriptWorker", model);
            workerContext.Execute(currentWorkState);
            return currentWorkState;
        }
        
        public string Namespace { get; set; }
        
        public string Script { get; set; }

        public override string[] RequiredProperties
        {
            get { return new string[] {"Script", "Namespace", "Name"}; }
        }
    }
}