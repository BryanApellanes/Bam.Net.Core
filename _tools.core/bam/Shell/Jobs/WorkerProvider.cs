using System;
using System.Text;
using bam.Shell;
using Bam.Net;
using Bam.Net.Automation;
using Bam.Net.CommandLine;
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

        public override void RegisterArguments()
        {
            base.RegisterArguments();
            AddValidArgument("worker", "The name of the worker to work with");
            AddValidArgument("workerType", "When adding a worker, the type of the worker to add");
            
            AddValidArgument("details", "Show worker details");
        }

        protected override ProviderArguments GetProviderArguments()
        {
            return GetProviderArguments(true, false);
        }
        
        protected ProviderArguments GetProviderArguments(bool callBase = true, bool getJobName = false)
        {
            ProviderArguments baseArgs = callBase ? base.GetProviderArguments(): new JobProviderArguments();
            JobProviderArguments providerArguments = baseArgs.CopyAs<JobProviderArguments>();
            providerArguments.WorkerName = providerArguments.ProviderContextTarget;
            if (getJobName)
            {
                providerArguments.JobName = GetTypeNameArgument("job", "Please enter the name of the job");
                providerArguments.ShowWorkerDetails = true;
            }

            return providerArguments;
        }

        public override void List(Action<string> output = null, Action<string> error = null)
        {
            try
            {
                StringBuilder workers = new StringBuilder();
                if (Arguments.Contains("job"))
                {
                    string jobName = Arguments["job"];
                    JobConf jobConf = GetJobConf(jobName);
                    
                    foreach (string workerName in jobConf.ListWorkerNames())
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
                JobProviderArguments providerArguments = GetProviderArguments(true, true) as JobProviderArguments;
                string workerName = providerArguments.WorkerName;
                string jobName = providerArguments.JobName;
                
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
            try
            {
                JobProviderArguments providerArguments = GetProviderArguments(false, true) as JobProviderArguments;
                JobConf jobConf = GetJobConf(providerArguments.JobName);
                if (!string.IsNullOrEmpty(providerArguments.WorkerName))
                {
                    WorkerConf workerConf = jobConf.GetWorkerConf(providerArguments.WorkerName);
                    if (workerConf == null)
                    {
                        OutLineFormat("Specified worker {0} was not a part of the specified job {1}", providerArguments.WorkerName, providerArguments.JobName);
                        Exit(1);
                    }
                    string serialized = Serialize(workerConf);
                    OutLineFormat("***\r\n{0}\r\n***", ConsoleColor.Blue, serialized);
                }
                else
                {
                    jobConf.ReloadWorkers();
                    StringBuilder stringBuilder = new StringBuilder();
                    foreach (string key in jobConf.WorkerConfs.Keys)
                    {
                        stringBuilder.AppendLine(key);
                        stringBuilder.AppendLine(Serialize(jobConf.WorkerConfs[key]));
                    }
                    OutLineFormat("***\r\n{0}\r\n***", stringBuilder.ToString());
                }
                
                Exit(0);
            }
            catch (Exception ex)
            {
                OutLineFormat("error showing worker: {0}", ConsoleColor.Magenta, ex.Message);
                Exit(1);
            }
        }

        public override void Remove(Action<string> output = null, Action<string> error = null)
        {
            try
            {
                JobProviderArguments providerArguments = GetProviderArguments(true) as JobProviderArguments;
                JobConf jobConf = GetJobConf(providerArguments.JobName);
                WorkerConf worker = jobConf.GetWorkerConf(providerArguments.WorkerName);
                if (worker == null)
                {
                    OutLineFormat("Specified worker {0} was not a part of the specified job {1}", providerArguments.WorkerName, providerArguments.JobName);
                    Exit(1);
                }
                jobConf.RemoveWorker(providerArguments.WorkerName);
                OutLineFormat("Removed worker {0}", ConsoleColor.Yellow, providerArguments.WorkerName);
                Exit(0);
            }
            catch (Exception ex)
            {
                OutLineFormat("error removing worker: {0}", ConsoleColor.Magenta, ex.Message);
                Exit(1);
            }
        }

        public override void Run(Action<string> output = null, Action<string> error = null)
        {
            try
            {
                JobProviderArguments providerArguments = GetProviderArguments(true) as JobProviderArguments;
                JobConf jobConf = GetJobConf(providerArguments.JobName);
                WorkerConf worker = jobConf.GetWorkerConf(providerArguments.WorkerName);
                WorkState result = worker.CreateWorker(null).Do();
                OutLine(result.ToYaml(), ConsoleColor.Yellow);
                Exit(0);
            }
            catch (Exception ex)
            {
                OutLineFormat("error running worker: {0}", ConsoleColor.Magenta, ex.Message);
                Exit(1);
            }
        }

        public override void Edit(Action<string> output = null, Action<string> error = null)
        {
            try
            {
                JobProviderArguments providerArguments = GetProviderArguments(true, true) as JobProviderArguments;
                JobConf jobConf = GetJobConf(providerArguments.JobName);
                WorkerConf workerConf = jobConf.GetWorkerConf(providerArguments.WorkerName, true);
                if (workerConf == null)
                {
                    OutLineFormat("specified worker was not found: {0}", providerArguments.WorkerName);
                }

                ProcessOutput processOutput = ShellSettings.Current.Editor.Start(workerConf.LoadedFrom);
                Exit(0);
            }
            catch (Exception ex)
            {
                OutLineFormat("error editing worker: {0}", ConsoleColor.Magenta, ex.Message);
                Exit(1);
            }
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