using Bam.Net.Logging;
using Bam.Net.Testing;
using System;

namespace Bam.Net.Application
{
    [Serializable]
    class Program : CommandLineTestInterface
    {
        static void Main(string[] args)
        {
            TryWritePid();

            DefaultMethod = typeof(Program).GetMethod(nameof(AfterInitialize));
            IsolateMethodCalls = false;
            AddValidArgument("pause", true, addAcronym: false, description: "pause before exiting, only valid if command line switches are specified");
            AddSwitches(typeof(UtilityActions));
            AddConfigurationSwitches();
            ArgumentAdder.AddArguments(args);

            Initialize(args, (a) =>
            {
                OutLineFormat("Error parsing arguments: {0}", ConsoleColor.Red, a.Message);
                Environment.Exit(1);
            });
        }

        public static void AfterInitialize()
        {
            if (Arguments.Contains("pause"))
            {
                Pause("paused..."); // for debugging
            }
            if (Arguments.Length > 0 && !Arguments.Contains("i"))
            {
                ExecuteSwitches(Arguments, typeof(UtilityActions), false, Log.Default);
            }
            else if (Arguments.Contains("i"))
            {
                Interactive();
            }
        }
    }
}
