using System;
using Bam.Net;
using Bam.Net.CommandLine;
using Bam.Net.Server;
using Bam.Net.Testing;
using Bam.Net.Services.Clients;
using Bam.Net.Logging;
using Bam.Net.UserAccounts;
using Bam.Net.Automation;
using System.IO;
using System.Collections.Generic;
using System.Reflection;
using Bam.Net.CoreServices;
using Bam.Net.UserAccounts.Data;
using Bam.Net.Data;
using System.Linq;
using Bam.Net.Messaging;
using Bam.Net.Testing.Integration;

namespace Bam.Net.Application
{
    // TODO: convert this to a ShellProvider
    /// <summary>
    /// User administrative actions
    /// </summary>
    /// <seealso cref="CommandLineTool" />
    [Serializable]
    public class LocalUserAdministrationActions : CommandLineTool
    {
        /// <summary>
        /// Adds the user to role.
        /// </summary>
        [ConsoleAction("localAddUserToRole", "LOCAL: add user to role")]
        public void AddUserToRole()
        {
            Database userDatabase = ServiceTools.GetUserDatabase();
            string email = Prompt("Please enter the user's email address");
            User user = User.FirstOneWhere(u => u.Email == email, userDatabase);
            if (user == null)
            {
                OutLine("Unable to find a user with the specified address", ConsoleColor.Yellow);
                return;
            }
            string role = Prompt("Please enter the role to add the user to");
            Role daoRole = Role.FirstOneWhere(r => r.Name == role, userDatabase);
            if (daoRole == null)
            {
                daoRole = new Role(userDatabase)
                {
                    Name = role
                };
                daoRole.Save(userDatabase);
            }
            Role existing = user.Roles.FirstOrDefault(r => r.Name.Equals(daoRole.Name));
            if (existing == null)
            {
                user.Roles.Add(daoRole);
                user.Save(userDatabase);
                Message.PrintLine("User ({0}) added to role ({1})", user.UserName, daoRole.Name);
            }
            else
            {
                OutLine("User already in specified role");
            }
        }
    }
}
