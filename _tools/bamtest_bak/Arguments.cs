namespace Bam.Net.Testing
{
    public partial class Program
    {
        public static void AddArguments()
        {
            AddValidArgument("results", "On /Recipe, specifies the directory where the test result sqlite file is saved, default is './BamTests'.");
            AddValidArgument("search", false, description: "The search pattern to use to locate test assemblies, the default is *Tests.* if not specified.");
            AddValidArgument("testFile", false, description: "The path to the assembly containing tests to run");
            AddValidArgument("dir", false, description: "The directory to look for test assemblies in");
            AddValidArgument("debug", true, description: "If specified, the runner will pause to allow for a debugger to be attached to the process");
            AddValidArgument("data", false, description: "The path to save the results to, default is the current directory if not specified");
            AddValidArgument("dataPrefix", true, description: "The file prefix for the sqlite data file or 'BamTests' if not specified");
            AddValidArgument("type", false, description: "The type of tests to run [Unit | Integration], default is Unit.");
            AddValidArgument("testReportHost", false, description: "The hostname of the test report service");
            AddValidArgument("testReportPort", false, description: "The port that the test report service is listening on");

            AddValidArgument("projects", false, false, "On /recipe, optionally specify a comma separated list of project names to test from the specified recipe.");

            AddValidArgument(_exitOnFailure, true);
            AddSwitches(typeof(Program));
        }
    }
}