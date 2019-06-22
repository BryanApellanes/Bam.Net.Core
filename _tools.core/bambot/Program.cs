using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Bam.Net.Testing;

namespace Bam.Net.Application
{
    [Serializable]
    public class Program : CommandLineTestInterface
    {
        static void Main(string[] args)
        {
            TryWritePid(true);
            IsolateMethodCalls = false;
            AddValidArgument("host", false, false, "The host name to deploy to");
            AddSwitches(typeof(ConsoleActions));
            AddConfigurationSwitches();
            Initialize(args, (a) =>
            {
                OutLineFormat("Error parsing arguments: {0}", ConsoleColor.Red, a.Message);
                Environment.Exit(1);
            });

            if (!ExecuteSwitches(Arguments, new ConsoleActions()))
            {
                Interactive();
            }
        }
    }
}
