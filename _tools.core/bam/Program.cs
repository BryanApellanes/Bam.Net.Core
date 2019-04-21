using Bam.Net.Application;
using Bam.Net.CommandLine;
using Bam.Net.Testing;
using System;
using System.Linq;
using Bam.Net.Data;
using Bam.Net.Data.Repositories;
using Bam.Net.Data.Repositories.Handlebars;
using Bam.Net.Logging;
using Bam.Shell;
using Bam.Shell.Conf;
using Bam.Shell.ShellGeneration.Data;

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

            RegisterArgZeroProviders<ShellProvider>(args);
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

        [ConsoleAction]
        public void TestDaoRepoHbGen()
        {
            Database db = DataProvider.Current.GetAppDatabaseFor(ProcessApplicationNameProvider.Current, this);
            DaoRepository repo = new DaoRepository(db);
            repo.BaseNamespace = typeof(ShellDescriptor).Namespace;
            repo.RequireCuid = true;
            repo.AddType<ShellDescriptor>();
            ShellDescriptor d = new ShellDescriptor(){AssemblyName = "Ass", NameSpace = "Ns"};
            d = repo.Save(d);

            ShellDescriptor queried = repo.Query<ShellDescriptor>(c => c.Id == d.Id).FirstOrDefault();
            Expect.IsNotNull(queried);
            
            Expect.AreEqual(d, queried);

            ShellDescriptor retrieved = repo.Retrieve<ShellDescriptor>(d.Id);
            Expect.IsNotNull(retrieved);
            
            Expect.AreEqual(d, retrieved);
            
            Pass("yay");
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
