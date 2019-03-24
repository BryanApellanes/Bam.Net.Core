using System;

namespace Bam.Net.Automation
{
    public class InitialWorkState : WorkState
    {
        public InitialWorkState(IWorker worker) : base(worker)
        {
        }

        public InitialWorkState(IWorker worker, Exception ex) : base(worker, ex)
        {
        }
    }
}