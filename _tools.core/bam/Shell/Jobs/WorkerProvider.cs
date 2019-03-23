using System;
using System.Text;
using Bam.Net;
using Bam.Net.Automation;
using Bam.Net.Data;
using Bam.Net.Data.Repositories;
using CsQuery.ExtensionMethods;

namespace Bam.Shell.Jobs
{
    public class WorkerProvider : ShellProvider
    {
        JobManagerService _jobManagerService;
        public JobManagerService JobManagerService
        {
            get
            {
                if (_jobManagerService == null)
                {
                    _jobManagerService = new JobManagerService(ProcessApplicationNameProvider.Current, DataProvider.Current);
                }
                return _jobManagerService; 
                
            }
            set { _jobManagerService = value; }
        }
        
        public override void List(Action<string> output = null, Action<string> error = null)
        {
            try
            {
                StringBuilder workers = new StringBuilder();
                if (Arguments.Contains("jobName"))
                {
                    string jobName = GetArgument("jobName", "Please enter the name of the job to list workers for");
                    JobConf jobConf = GetJobConf(jobName);
                    
                    foreach (string workerName in jobConf.ListWorkers())
                    {
                        workers.AppendLine(workerName);
                    }
                }
                else
                {
                    JobManagerService.GetWorkerTypes().ForEach(wt=> workers.AppendLine(wt));
                }
                
                OutLine(workers.ToString(), ConsoleColor.Cyan);
                Exit(0);
            }
            catch (Exception ex)
            {
                OutLineFormat("Error listing workers: {0}", ConsoleColor.Magenta, ex.Message);
                Exit(1);
            }
        }

        public override void Add(Action<string> output = null, Action<string> error = null)
        {
            try
            {
                string workerName = GetArgument("name", "Please enter the name of the worker to add");
                string jobName = GetArgument("jobName", "Please enter the name of the job to add a worker to");
                
                JobConf jobConf = GetJobConf(jobName);
                string[] workerTypeNames = JobManagerService.GetWorkerTypes();
                int workerType = SelectFrom(workerTypeNames, "Please select a worker type");
                JobManagerService.AddWorker(jobName, workerTypeNames[workerType], workerName);
                
                OutLineFormat("Added worker {0} to job {1}", ConsoleColor.Cyan, workerName, jobName);
                Exit(0);
            }
            catch (Exception ex)
            {
                OutLineFormat("Error adding worker: {0}", ConsoleColor.Magenta, ex.Message);
                Exit(1);
            }
        }

        public override void Show(Action<string> output = null, Action<string> error = null)
        {
            throw new NotImplementedException();
        }

        public override void Set(Action<string> output = null, Action<string> error = null)
        {
            throw new NotImplementedException();
        }

        public override void Remove(Action<string> output = null, Action<string> error = null)
        {
            throw new NotImplementedException();
        }

        public override void Run(Action<string> output = null, Action<string> error = null)
        {
            throw new NotImplementedException();
        }
        
        private JobConf GetJobConf(string jobName)
        {
            JobConf jobConf = JobManagerService.GetJob(jobName, false);
            if (jobConf == null)
            {
                OutLineFormat("The specified job was not found: {0}", ConsoleColor.Magenta, jobName);
                Exit(1);
            }

            return jobConf;
        }
    }
}