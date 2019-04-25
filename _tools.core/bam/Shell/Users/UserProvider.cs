using System;
using System.IO;
using System.Linq;
using Bam.Net;
using Bam.Net.Application;
using Bam.Net.CommandLine;
using Bam.Net.Data;
using Bam.Net.UserAccounts.Data;
using Bam.Shell;

namespace Bam.Shell.Users
{
    public class UserProvider : ShellProvider
    {
        public string[] RawArguments { get; private set; }

        public override void RegisterArguments(string[] args)
        {
            base.RegisterArguments(args);
            RawArguments = args;
            AddValidArgument("userName", "User: the name of the user to.");
            AddValidArgument("email", "User: the name email address of the user.");
        }
        
        public override void List(Action<string> output = null, Action<string> error = null)
        {
            Database userDatabase = ServiceTools.GetUserDatabase();
            UserCollection users = User.LoadAll(userDatabase);
            int num = 1;
            foreach (User user in users)
            {
                output($"{num}. ({user.Email}) {user.UserName}");
                output($"\tRoles: {string.Join(",", user.Roles.Select(r => r.Name).ToArray())}");
            }
        }

        public override void Add(Action<string> output = null, Action<string> error = null)
        {
            Database userDatabase = ServiceTools.GetUserDatabase();
            string userName = GetArgument("name", "Please enter the name for the new user");
            string emailAddress = GetArgument("email","Please enter the new user's email address");

            User user = User.Create(userName, emailAddress, ServiceTools.ConfirmPasswordPrompt().Sha1());
            output($"*** user created ***\r\n{user.ToYaml()}");
        }

        public override void Show(Action<string> output = null, Action<string> error = null)
        {
            Database userDatabase = ServiceTools.GetUserDatabase();
            string userName = GetArgument("name", "Please enter the user name to show.");
            User user = User.FirstOneWhere(n => n.UserName == userName);
            if (user == null)
            {
                error($"User ({userName}) was not found");
                Exit(1);
            }

            output(user.ToYaml());
        }

        public override void Remove(Action<string> output = null, Action<string> error = null)
        {
            Database userDatabase = ServiceTools.GetUserDatabase();
            if (!Confirm("Whoa, whoa, hold your horses cowboy!! Are you sure you know what you're doing?", ConsoleColor.DarkYellow))
            {
                return;
            }
            OutLineFormat("This might not work depending on the state of the user's activity and related data.  Full scrub of user's is not implemented to help ensure data integrity into the future.", ConsoleColor.DarkYellow);
            if (!Confirm("Continue?", ConsoleColor.DarkYellow))
            {
                return;
            }
            string email = Prompt("Please enter the user's email address");
            User toDelete = User.FirstOneWhere(u => u.Email == email, userDatabase);
            if (toDelete == null)
            {
                OutLineFormat("Unable to find the user with the email address {0}", ConsoleColor.Magenta, email);
                return;
            }

            try
            {
                if (!Confirm($"Last chance to turn back!! About to delete this user:\r\n{toDelete.ToJsonSafe().ToJson(true)}", ConsoleColor.Yellow))
                {
                    return;
                }
                toDelete.Delete(userDatabase);
                OutLineFormat("User deleted", ConsoleColor.DarkMagenta);
            }
            catch (Exception ex)
            {
                OutLineFormat("Delete user failed: {0}", ConsoleColor.Magenta, ex.Message);
            }
        }

        public override void Edit(Action<string> output = null, Action<string> error = null)
        {
            User user = GetUser();
            string fileName = $"./user_{user.UserName}.yaml";
            user.ToYamlFile(fileName);
            ShellSettings.Current.Editor.Start(fileName);
            User edited = fileName.FromYamlFile<User>();
            edited.Id = user.Id;
            edited.Uuid = user.Uuid;
            edited.Cuid = user.Cuid;
            edited.Save(GetUserDb());
            File.Delete(fileName);
        }

        public override void Run(Action<string> output = null, Action<string> error = null)
        {
            error("Run a user? What does that even mean?");
        }
    }
}