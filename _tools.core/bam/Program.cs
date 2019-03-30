using Bam.Net.Application;
using Bam.Net.CommandLine;
using Bam.Net.Testing;
using System;
using Bam.Net.Logging;
using Bam.Shell;

namespace Bam.Net
{
    [Serializable]
    class Program : ArgZero
    {
        static void Main(string[] args)
        {
            Log.Default = Workspace.Current.CreateLogger<TextFileLogger>();
           
            AddArguments();
            AddValidArgument("pause", true, addAcronym: false, description: "pause before exiting, only valid if command line switches are specified");
            
            BamEnvironmentVariables.SetBamVariable("ApplicationName", "bam.exe");
            ExecuteArgZero(args);
            
            DefaultMethod = typeof(Program).GetMethod("Start");

            Initialize(args);
        }

        public static void AddArguments()
        {
            AddSwitches(typeof(Bam.Shell.LifeCycleProvider));
            AddSwitches(typeof(ExternalProvider));
            AddValidArgument("config", "The path to a config file to use");
            AddValidArgument("schemaName", false, addAcronym: true, description: "The name to use for the generated schema");
            
            AddValidArgument("assembly", "When executing command line switches in an external assembly, the path to the assembly");
            AddValidArgument("class", "When executing command line switches in an external assembly, the name of the class");
        }

        #region do not modify
        public static void Start()
        {
            ConsoleLogger logger = new ConsoleLogger
            {
                AddDetails = false
            };
            logger.StartLoggingThread();
            if (Arguments.Contains("pause"))
            {
                Pause("paused..."); // for debugging
            }
            if (ExecuteSwitches(Arguments, typeof(Bam.Shell.LifeCycleProvider), false, logger) ||
                ExecuteSwitches(Arguments, typeof(ExternalProvider), false, logger))
            {
                logger.BlockUntilEventQueueIsEmpty();                
            }
            else
            {
                Interactive();
            }
        }
        #endregion
    }
}
