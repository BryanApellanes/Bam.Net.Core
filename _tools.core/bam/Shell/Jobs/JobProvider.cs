using System;
using System.Text;
using Bam.Net;
using Bam.Net.Automation;
using Bam.Net.Data.Repositories;

namespace Bam.Shell.Jobs
{
    public class JobProvider : ShellProvider
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
        }
        protected override ProviderArguments GetProviderArguments()
        {
            return GetProviderArguments(false);
        }
        
        protected ProviderArguments GetProviderArguments(bool full = false)
        {
            ProviderArguments baseArgs = base.GetProviderArguments();
            JobProviderArguments providerArguments = baseArgs.CopyAs<JobProviderArguments>();
            providerArguments.JobName = providerArguments.ProviderContextTarget;
            return providerArguments;
        }

        public override void List(Action<string> output = null, Action<string> error = null)
        {
            try
            {
                PrintMessage();
                StringBuilder jobs = new StringBuilder();
                foreach (string jobName in JobManagerService.ListJobNames())
                {
                    jobs.AppendLine(jobName);
                }
                OutLine(jobs.ToString(), ConsoleColor.Blue);
            }
            catch (Exception ex)
            {
                OutLineFormat("Exception listing jobs: {0}", ex.Message);
                Exit(1);
            }

            Exit(0);
        }

        public override void Add(Action<string> output = null, Action<string> error = null)
        {
            try
            {
                PrintMessage();
                JobProviderArguments arguments = GetProviderArguments() as JobProviderArguments;
                string jobName = arguments.JobName;
                JobConf jobConf = JobManagerService.GetJob(jobName);
                
                string[] workerTypes = JobManagerService.GetWorkerTypes();
                int workerIndex = SelectFrom(workerTypes);
                string workerName = GetArgument("workerName", "Please enter a name for the worker");
                JobManagerService.AddWorker(jobName, workerTypes[workerIndex], workerName);
                JobManagerService.SaveJob(jobConf);
                OutLine(jobConf.ToYaml(), ConsoleColor.Cyan);
                OutLineFormat("Created job {0} in directory {1}", ConsoleColor.Cyan, jobConf.Name, jobConf.JobDirectory);
            }
            catch (Exception ex)
            {
                OutLineFormat("Exception adding job: {0}", ex.Message);
                Exit(1);
            }
            Exit(0);
        }

        public override void Show(Action<string> output = null, Action<string> error = null)
        {
            try
            {
                PrintMessage();
                JobProviderArguments arguments = GetProviderArguments() as JobProviderArguments;
                string jobName = arguments.JobName;

                JobConf jobConf = JobManagerService.GetJob(jobName);
                OutLine(jobConf.ToYaml(), ConsoleColor.Cyan);
            }
            catch (Exception ex)
            {
                OutLineFormat("Exception getting job: {0}", ex.Message);
                Exit(1);
            }

            Exit(0);
        }

        public override void Pack(Action<string> output = null, Action<string> error = null)
        {
            OutLineFormat("Pack is not implemented for the JobProvider", ConsoleColor.Yellow);
        }

        public override void Remove(Action<string> output = null, Action<string> error = null)
        {
            try
            {
                JobProviderArguments providerArguments = GetProviderArguments() as JobProviderArguments;
                JobManagerService.RemoveJob(providerArguments.JobName);
                OutLineFormat("Job {0} was deleted", ConsoleColor.Yellow, providerArguments.JobName);
            }
            catch (Exception ex)
            {
                OutLineFormat("Exception removing job: {0}", ConsoleColor.Magenta, ex.Message);
                Exit(1);
            }

            Exit(0);
        }

        public override void Run(Action<string> output = null, Action<string> error = null)
        {
            try
            {
                PrintMessage();
                JobProviderArguments providerArguments = GetProviderArguments() as JobProviderArguments;
                if (JobManagerService.JobExists(providerArguments.JobName))
                {
                    bool? jobFinished = false;
                    JobManagerService.JobFinished += (sender, args) =>
                    {
                        jobFinished = (args.Cast<WorkStateEventArgs>())?.WorkState?.JobName?.Equals(providerArguments.JobName);
                        if (jobFinished != null && jobFinished.Value == true)
                        {
                            OutLineFormat("Job {0} finished", providerArguments.JobName);
                            Unblock();
                        }
                    };
                    JobManagerService.StartJob(providerArguments.JobName);
                    OutLineFormat("Job {0} was queued to start", ConsoleColor.DarkGreen, providerArguments.JobName);
                    Block();
                }
                else
                {
                    OutLineFormat("Specified job {0} does not exist", ConsoleColor.Magenta, providerArguments.JobName);
                }
            }
            catch (Exception ex)
            {
                OutLineFormat("Exception running job: {0}", ConsoleColor.Magenta, ex.Message);
                Exit(1);
            }
            Exit(0);
        }

        private void PrintMessage()
        {
            OutLineFormat("Jobs directory: {0}", ConsoleColor.Yellow, JobManagerService.JobsDirectory);
        }
    }
}