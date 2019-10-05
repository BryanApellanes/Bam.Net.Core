using System;
using Bam.Net;
using Bam.Net.Application;
using Bam.Net.Data;
using Bam.Net.UserAccounts.Data;
using Bam.Shell;

namespace Bam.Shell.Users
{
    public class RoleProvider: ShellProvider
    {
        public string[] RawArguments { get; private set; }

        public override void RegisterArguments(string[] args)
        {
            base.RegisterArguments(args);
            RawArguments = args;
            AddValidArgument("roleName", "Roles: the name of the role");
        }
        
        public override void List(Action<string> output = null, Action<string> error = null)
        {
            Database userDatabase = ServiceTools.GetUserDatabase();
            RoleCollection roles = Role.LoadAll(userDatabase);
            int num = 1;
            foreach (Role role in roles)
            {
                output($"{num}. {role.Name}\r\n");
            }
        }

        public override void Add(Action<string> output = null, Action<string> error = null)
        {
            string roleName = GetArgument("roleName", "Please enter the name of the role to add");
            Role role = new Role()
            {
                Name = roleName
            };
            role.Save(GetUserDb());

            output("*** role added ***");
            output(role.ToYaml());
        }

        public override void Show(Action<string> output = null, Action<string> error = null)
        {
            throw new NotImplementedException();
        }

        public override void Remove(Action<string> output = null, Action<string> error = null)
        {
            throw new NotImplementedException();
        }

        public override void Run(Action<string> output = null, Action<string> error = null)
        {
            throw new NotImplementedException();
        }
    }
}