using System;
using Bam.Net.CommandLine;
using Bam.Net.Testing;
using Bam.Remote.Deployment;
using Bam.Remote.Deployment.Data;
using Bam.Remote.Etc;

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
            RemoteSshHost remoteHost = GetRemoteHost();
            
        }
        
        protected ManagedPassword GetPasswordFor(string hostName, double? julianDate = null)
        {
            throw new NotImplementedException();
        }

        private RemoteSshHost GetRemoteHost()
        {
            return new RemoteSshHost
            {
                HostName = GetArgument("host"),
                Port = int.Parse(GetArgumentOrDefault("port", "22")),
                LoginUserName = GetArgument("loginUser"),
                LoginPassword = GetPasswordArgument("password"),
            };
        }
    }
}