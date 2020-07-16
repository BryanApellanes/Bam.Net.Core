using System;
using System.Linq;
using System.Text;
using System.Threading;
using Bam.Net;
using Bam.Net.CommandLine;
using Bam.Net.Server;
using Bam.Net.ServiceProxy;
using Bam.Net.Testing;
using Bam.Net.Testing.Unit;

namespace Bam.ConsoleActions
{
    [Serializable]
    public class ApplicationManagement: CommandLineTestInterface
    {
        [ConsoleAction]
        public void DeployContainer()
        {
            // copy files to remote
            // execute remote process
            
            throw new NotImplementedException();
        }
        
        [ConsoleAction("compileAppServices", "Compile application specific services given an AppConf or BamConf")]
        public void CompileAppServices()
        {
            BamConf bamConf = BamConf.Load(GetPathArgument("bamConf","Please enter the path to the content root where the BamConf is found."));
            if (bamConf == null)
            {
                OutLineFormat("BamConf not found.", ConsoleColor.Magenta);
                Exit(1);
            }
            
            ApplicationServiceSourceResolver sourceResolver = new ApplicationServiceSourceResolver();
            sourceResolver.CompilationException += (sender, args) =>
            {
                RoslynCompilationExceptionEventArgs eventArgs = (RoslynCompilationExceptionEventArgs) args;
                StringBuilder message = new StringBuilder();
                message.AppendFormat("{0}: {1}\r\n", eventArgs.AppConf.Name, eventArgs.AppConf.AppRoot.Root);
                message.AppendFormat(eventArgs.Exception.Message);
                OutLine(message.ToString(), ConsoleColor.Magenta);
                Thread.Sleep(300);
            };

            if (Arguments.Contains("app"))
            {
                AppConf appConf = bamConf.AppConfigs.FirstOrDefault(a => a.Name.Equals(Arguments["app"]));
                sourceResolver.CompileAppServices(appConf);
            }
            else
            {
                sourceResolver.CompileAppServices(bamConf);
            }
            
            Thread.Sleep(300);
        }
    }
}