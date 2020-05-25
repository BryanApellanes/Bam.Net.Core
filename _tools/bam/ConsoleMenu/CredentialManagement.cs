using System;
using Bam.Net.CommandLine;
using Bam.Net.Testing;
using Bambot.Deployment;
using Bambot.Deployment.Data;
using Bambot.Etc;

namespace Bam.Net.ConsoleActions
{
    public class CredentialManagement : CommandLineTestInterface
    {
        [ConsoleAction("addUser", "Add the bambot user to the specified host")]
        public void AddUser()
        {
            string hostName = GetArgument("host");
            string userNameToAdd = GetArgument("addUser");
            string password = GetPasswordArgument("password");
            int port = int.Parse(GetArgumentOrDefault("port", "22"));
            
        }
        
        [ConsoleAction("setPassword", "Set the password for bambot to a deterministic value on the specified host")]
        public void SetPassword()
        {
            // ssh to the specified hostname and issue 
        }

        [ConsoleAction("listUsers", "List the users of the specified remote host")]
        public void ListUsers()
        {
            SshRemoteHost remoteHost = GetRemoteHost();
            
        }
        
        protected GeneratedPassword GetPasswordFor(string hostName, double? julianDate = null)
        {
            throw new NotImplementedException();
        }

        private SshRemoteHost GetRemoteHost()
        {
            return new SshRemoteHost
            {
                HostName = GetArgument("host"),
                Port = int.Parse(GetArgumentOrDefault("port", "22")),
                LoginUserName = GetArgument("loginUser"),
                LoginPassword = GetPasswordArgument("password"),
            };
        }
    }
}