using System;
using Bam.Net.Services;
using Bam.Net.Testing;

namespace Bam.Net.System
{
    [Serializable]
    public class Program : CommandLineTestInterface
    {
        static void Main(string[] args)
        {
            TryWritePid(true);
            IsolateMethodCalls = false;
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
