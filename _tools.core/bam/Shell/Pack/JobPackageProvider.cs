using System;
using System.IO;
using System.Linq;
using System.Reflection;
using Bam.Net;
using Bam.Net.Application;
using Bam.Net.Automation;
using Bam.Net.CommandLine;
using Bam.Net.CoreServices;


namespace Bam.Shell.Pack
{
    public class JobPackageProvider : Bam.Shell.Pack.PackageProvider
    {
        public JobManagerService JobManagerService
        {
            get
            {
                return JobManagerService.Current;
            }
        }
        public override void RegisterArguments(string[] args)
        {
            AddValidArgument("job", "The name of the job to work with");
        }

        public override void Build(Action<string> output = null, Action<string> error = null)
        {
            throw new NotImplementedException("Build is not implemented");
        }
        
        public override void Pack(Action<string> output = null, Action<string> error = null)
        {
            throw new NotImplementedException("Pack is not implemented");
        }        
        
        public override void Push(Action<string> output = null, Action<string> error = null)
        {
            try
            {
                string jobName = GetArgument("job", "Please enter the name of the job to push.");
                JobConf conf = JobManagerService.GetJob(jobName);

                ProxyFactory factory = new ProxyFactory();
                string remote = Config.Current["Server"].Or("bambot.bamapps.net");
                int port = Config.Current["Port"].Or("80").ToInt();
                JobManagerService remoteSvc = factory.GetProxy<JobManagerService>();
                remoteSvc.SaveJob(conf);
                output($"Job {jobName} pushed to {remote}:{port}");
            }
            catch (Exception ex)
            {
                error(ex.Message);
                Exit(1);
            }
        }
        
        public override void Pull(Action<string> output = null, Action<string> error = null)
        {
            throw new NotImplementedException("Pull is not implemented");
        }        
    }
}