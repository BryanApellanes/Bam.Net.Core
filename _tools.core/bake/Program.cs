using Bam.Net.Application;
using Bam.Net.CommandLine;
using Bam.Net.Testing;
using System;

namespace Bam.Net
{
    [Serializable]
    class Program : CommandLineTestInterface
    {
        static void Main(string[] args)
        {
            AddArguments();
            
            DefaultMethod = typeof(Program).GetMethod("Start");

            Initialize(args);
        }

        public static void AddArguments()
        {
            AddSwitches(typeof(ConsoleActions));
            AddValidArgument("pause", true, addAcronym: false, description: "pause before exiting, only valid if command line switches are specified");
            AddValidArgument("output", false, true, "Specify the directory to build to");
            AddValidArgument("projects", false, false, "Sepcify the recipe file containing project file paths to clean");
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
            if (ExecuteSwitches(Arguments, typeof(ConsoleActions), false, logger))
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
