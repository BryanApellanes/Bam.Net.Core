using System;
using Bam.Net.CommandLine;
using Bam.Net.Testing;
using Bam.Net.Testing.Integration;
using Bam.Net.Testing.Unit;
using Bam.Remote.Deployment;

namespace Bam.Net.Application.Tests.UnitTests
{
    public class RemoteHostTests: CommandLineTestInterface
    {
        [ConsoleAction]
        [IntegrationTest]
        public void CanListUsers()
        {
            RemoteSshHost sshRemoteHost = GetRemoteHost();
            string[] users = sshRemoteHost.ListUsers();
            Expect.IsNotNull(users);
            Expect.IsTrue(users.Length > 0);
            users.Each(user => Message.PrintLine(user, ConsoleColor.Cyan));
        }

        [ConsoleAction]
        [IntegrationTest]
        public void CanAddUser()
        {
            RemoteSshHost sshRemoteHost = GetRemoteHost();
            string userNameToAdd = "Test_User_".RandomLetters(4);
            string[] existingUsers = sshRemoteHost.ListUsers();
            Expect.IsNotNull(existingUsers);
            bool added = sshRemoteHost.AddUser(userNameToAdd, "test_password_".RandomLetters(12));
            Expect.IsTrue(added);
            string[] usersAfterAdd = sshRemoteHost.ListUsers();
            Expect.IsNotNull(usersAfterAdd);
            Expect.AreEqual(existingUsers.Length + 1, existingUsers.Length);
            sshRemoteHost.DeleteUser(userNameToAdd);
        }
        
        [ConsoleAction]
        [IntegrationTest]
        public void CanListNetworkInterfaces()
        {
            RemoteSshHost sshRemoteHost = new RemoteSshHost();
            string host = "chumbucket4";
            string loginUser = "bam";
            string loginPassword = "bamP455w0rd1!";
            string[] nicNames = sshRemoteHost.ListNetworkInterfaces(host, 22, loginUser, loginPassword);
            
            foreach (string nicName in nicNames)
            {
                Message.PrintLine("{0}", nicName);
            }
        }

        [ConsoleAction]
        [IntegrationTest]
        public void CanGetMacAddresses()
        {
            RemoteSshHost sshRemoteHost = new RemoteSshHost();
            string host = "chumbucket4";
            string loginUser = "bam";
            string loginPassword = "bamP455w0rd1!";
            string macAddress = sshRemoteHost.GetMacAddress(host, 22, loginUser, loginPassword, "eno1");
            Message.PrintLine(macAddress);
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