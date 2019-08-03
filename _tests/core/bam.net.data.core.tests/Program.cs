using Bam.Net.Testing;
using System;
using Bam.Net.CommandLine;
using Bam.Net.Logging;

namespace Bam.Net.Data.Tests
{
    [Serializable]
    class Program : CommandLineTestInterface
    {
        static void Main(string[] args)
        {
            AddSwitches(typeof(DatabaseConfigTests));
            AddConfigurationSwitches();
            Initialize(args);
            if (Arguments.Length > 0 && !Arguments.Contains("i"))
            {
                ExecuteSwitches(Arguments, typeof(DatabaseConfigTests), false, new ConsoleLogger());
            }
        }
    }
}
