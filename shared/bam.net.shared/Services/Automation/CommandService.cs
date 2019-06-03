using Bam.Net.CommandLine;
using Bam.Net.ServiceProxy;
using Bam.Net.ServiceProxy.Secure;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Bam.Net.Services;
using Bam.Net.Data.Repositories;

namespace Bam.Net.Services.Automation
{
    [Encrypt]
    [ApiKeyRequired]
    [Proxy("commandSvc")]
    [ServiceSubdomain("command")]
    public class CommandService : AsyncProxyableService
    {
        public const int DefaultPort = 8413;
        
        public CommandService()
        {
        }

        [RoleRequired("/", "Admin")]
        public ServiceResponse<CommandInfo> Start(CommandRequest commandRequest)
        {
            try
            {
                UserIsLoggedInOrDie();
                string command = commandRequest.Command;
                
                CommandInfo info = new CommandInfo { Command = command };
                IRepository repo = RepositoryResolver.GetRepository(HttpContext);
                info = repo.Save(info);
                
                // TODO: fix this to redirect output to specified urls in commandrequest
                Task<ProcessOutput> task = command.RunAsync();
                task.ContinueWith((t) =>
                {
                    Logger.AddEntry("Command completed: {0}", command);
                    info.StandardOut = t.Result.StandardOutput;
                    info.StandardError = t.Result.StandardError;
                    info = repo.Save(info);
                });
                Logger.AddEntry("Running command: {0}", command);
                return new ServiceResponse<CommandInfo>
                {
                    Success = true,
                    Data = info
                };
            }
            catch (Exception ex)
            {
                Logger.AddEntry("Error in CommandService: {0}", ex, ex.Message);
                return new ServiceResponse<CommandInfo>
                {
                    Success = false,
                    Message = ex.Message
                };
            }
        }

        public override object Clone()
        {
            CommandService clone = new CommandService();
            clone.CopyProperties(this);
            clone.CopyEventHandlers(this);
            return clone;
        }
    }
}
