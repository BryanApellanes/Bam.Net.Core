using System;
using Bam.Net.CoreServices;

namespace Bam.Net.Server
{
    public class AppServerConf
    {
        public AppServerConf(ServerKinds serverKind)
        {
            ServerKind = serverKind;
            Init();
        }
        public ServerKinds ServerKind { get; set; }
        public string Command { get; set; }
        public string[] Arguments { get; set; }

        private void Init()
        {
            switch (ServerKind)
            {
                case ServerKinds.Invalid:
                case ServerKinds.Bam:
                    Command = "bamweb";
                    Arguments = new string[] {"/S", $"/content:{GetContentPath()}", "/verbose"};
                    break;
                case ServerKinds.External:
                    Command = String.Empty;
                    Arguments = new string[] { };
                    break;
            }
        }

        private string GetContentPath()
        {
            switch (OSInfo.Current)
            {
                case OSNames.Invalid:
                case OSNames.Windows:
                    return "/c/bam/content";
                case OSNames.Linux:
                case OSNames.OSX:
                    return "/opt/bam/content";
                    break;
            }

            return "/opt/bam/content";
        }
    }
}