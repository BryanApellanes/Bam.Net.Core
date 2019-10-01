/*
	Copyright © Bryan Apellanes 2015  
*/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Bam.Net.Logging;

namespace Bam.Net.Automation
{
    /// <summary>
    /// Used to communicate the state of the currently running job.  Gets passed
    /// from worker to worker during job execution.  A new instance may be instantiated
    /// by each worker so one should not rely on the beginning WorkState to be the same
    /// as the end.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class WorkState<T>: WorkState
    {
        public WorkState(IWorker worker, T data)
            : base(worker)
        {
            this.Data = data;
        }
        public WorkState(IWorker worker, Exception ex) : base(worker, ex) { }

        public T Data { get; set; }

        public override bool HasValue => Data != null;
    }

    /// <summary>
    /// Used to communicate the state of the currently running job.  Gets passed
    /// from worker to worker during job execution.  A new instance may be instantiated
    /// by each worker so one should not rely on the beginning WorkState to be the same
    /// at the end.
    /// </summary>
    public class WorkState : Loggable
    {
        internal WorkState()
        {
            JobData = new Dictionary<string, object>();
        }

        public WorkState(IWorker worker):this()
        {
            Args.ThrowIfNull(worker, "worker");

            this.WorkerName = worker.Name;
            this.StepNumber = worker.StepNumber;
            this.WorkTypeName = worker.GetType().AssemblyQualifiedName;

            if (worker.Job != null)
            {
                this.JobName = worker.Job.Name;
            }
        }

        public WorkState(IWorker worker, Exception ex)
            : this(worker)
        {
            this.Status = Status.Failed;
            string message = ex.GetInnerException().Message;
            this.Message = !string.IsNullOrEmpty(ex.StackTrace) ? string.Format("{0}:\r\n\r\n{1}", message, ex.StackTrace) : message;
        }

        public WorkState PreviousWorkState { get; set; }
        public Dictionary<string, object> JobData { get; set; }
        public int StepNumber { get; set; }
        public string JobName { get; set; }
        public string WorkerName { get; set; }
        public string Message { get; set; }

        public string WorkTypeName { get; set; }

        public bool ContinueOnFailure { get; set; }

        [Verbosity(VerbosityLevel.Information)]
        public event EventHandler Notify;

        internal void Starting()
        {
            Status = Status.Starting;
            FireEvent(Notify, this, new WorkStateEventArgs(this));
        }

        internal void Finished()
        {
            Status = Status.Finished;
            FireEvent(Notify, this, new WorkStateEventArgs(this));
        }

        internal void Failed()
        {
            Status = Status.Failed;
            FireEvent(Notify, this, new WorkStateEventArgs(this));
        }

        internal void Succeeded()
        {
            Status = Status.Succeeded;
            FireEvent(Notify, this, new WorkStateEventArgs(this));
        }

        internal void Suspended()
        {
            Status = Status.Suspended;
            FireEvent(Notify, this, new WorkStateEventArgs(this));
        }
        
        Status _success;
        public Status Status
        {
            get
            {
                return _success;
            }
            set
            {
                _success = value;
            }
        }

        public object this[string jobProperty]
        {
            get => JobData[jobProperty];
            set
            {
                if (JobData.ContainsKey(jobProperty))
                {
                    JobData[jobProperty] = value;
                }
                else
                {
                    JobData.Add(jobProperty, value);
                }
            }
        }
        
        public virtual bool HasValue { get { return false; } }
    }
}
