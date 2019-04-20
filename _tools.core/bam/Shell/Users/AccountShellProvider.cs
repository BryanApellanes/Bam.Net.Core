using Bam.Net.Application;
using Bam.Net.Data;
using Bam.Net.UserAccounts.Data;

namespace Bam.Shell.Users
{
    public abstract class AccountShellProvider : ShellProvider
    {
        protected Role GetRole()
        {
            string roleName = GetArgument("roleName", "Please enter the name of the role.");
            return Role.FirstOneWhere(c => c.Name == roleName, GetUserDb());
        }
        
        protected User GetUser()
        {
            string userName = GetArgument("userName", "Please enter the name of the user.");
            return User.FirstOneWhere(c => c.UserName == userName, GetUserDb());
        }

        protected Database GetUserDb()
        {
            return ServiceTools.GetUserDatabase();
        }
    }
}