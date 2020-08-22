using Bam.Net.Logging;
using Bam.Net.Testing;
using System;
using Bam.Shell;
using Bam.Shell.CodeGen;
using Bam.Shell.Data;

namespace Bam.Net.Application
{
    [Serializable]
    class Program : ArgZero
    {
        static void Main(string[] args)
        {
            Log.Default = Workspace.Current.CreateLogger<TextFileLogger>();
            TryWritePid();

            DefaultMethod = typeof(Program).GetMethod(nameof(AfterInitialize));
            IsolateMethodCalls = false;
            AddValidArgument("pause", true, addAcronym: false, description: "pause before exiting, only valid if command line switches are specified");
            AddSwitches();
            AddConfigurationSwitches();
            
            BamEnvironmentVariables.SetBamVariable("ApplicationName", "bamdb");
            RegisterArgZeroProviders<DataShellProvider>(args);
            RegisterArgZeroProviders<CodeGenProvider>(args);
            RegisterArgZeroProviders<DaoProvider>(args);
            
            ArgumentAdder.AddArguments(args);
            ExecuteArgZero(args);
            
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
                ExecuteSwitches(Arguments, typeof(BamDbConsoleActions), false, Log.Default);
            }
            else if (Arguments.Contains("i"))
            {
                Interactive();
            }
        }
    }
}
