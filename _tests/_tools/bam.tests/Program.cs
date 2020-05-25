using System;
using Bam.Net;

namespace Bam.Tests
{
    class Program :CommandLineTool
    {
        static void Main(string[] args)
        {
            ExecuteMain(args);
        }

        static void AddArguments()
        {
            AddValidArgument("host", "For credential management of a remote system, the host to manage");
            AddValidArgument("port", "For credential management of a remote system, the port that the ssh daemon is listening on.  The default is 22");
            AddValidArgument("loginUser", "For credential management of a remote system, the user name to login as");
            AddValidArgument("password", "For credential management of a remote system, the login password for a remote host");
        }
    }
}