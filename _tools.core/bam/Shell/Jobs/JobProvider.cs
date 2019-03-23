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
            AddValidArgument("jobName", "The name of a job to work with");
        }

        public override void List(Action<string> output = null, Action<string> error = null)
        {
            try
            {
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
                string jobName = GetArgument("jobName", "Please enter the name of the job to create");
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
                string jobName = GetArgument("name", "Please enter the name of the job to show");

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

        public override void Set(Action<string> output = null, Action<string> error = null)
        {
            OutLineFormat("Set is not implemented for the JobProvider", ConsoleColor.Yellow);
        }

        public override void Remove(Action<string> output = null, Action<string> error = null)
        {
            try
            {
                string jobName = GetArgument("jobName", "Please enter the name of the job to remove");
                JobManagerService.RemoveJob(jobName);
                OutLineFormat("Job {0} was deleted", ConsoleColor.Yellow, jobName);
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
                string jobName = GetArgument("jobName", "Please enter the name of the job to run");
                JobManagerService.StartJob(jobName);
                OutLineFormat("Job {0} was queued to start", ConsoleColor.DarkGreen, jobName);
            }
            catch (Exception ex)
            {
                OutLineFormat("Exception running job: {0}", ConsoleColor.Magenta, ex.Message);
                Exit(1);
            }
            Exit(0);
        }
    }
}