using System;
using Bam.Net;
using Bam.Net.Automation;
using Bam.Net.Data.Repositories;
using Bam.Net.Testing;

namespace Bam.Shell
{
    public class Jobs : CommandLineTestInterface
    {
        static Jobs()
        {
            Register();
        }
        
        [ArgZero("add")]
        public void AddJob()
        {
            try
            {
                string jobName = GetArgument("job", "Please enter the name of the job to create");
                
                JobConf jobConf = JobManagerService.GetJob(jobName);
                jobConf.AddWorker(typeof(BuildWorker), "BuildWorker", true);
                JobManagerService.SaveJob(jobConf);
                OutLineFormat("Created job {0} in directory {1}", ConsoleColor.Cyan, jobConf.Name, jobConf.JobDirectory);
            }
            catch (Exception ex)
            {
                OutLineFormat("Exception adding job: {0}", ex.Message);
            }
            Exit(0);
        }

        [ArgZero("runJob")]
        public void RunJob()
        {
            
        }
        
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
        
        public static void Register()
        {
            AddValidArgument("job", "Add a new job");
            
            // TODO: move lifecycle actions to Shell
            //AddValidArgument("app");
            //AddValidArgument("page");
        }
    }
}