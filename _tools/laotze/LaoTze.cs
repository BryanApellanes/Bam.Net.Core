namespace Bam.Net.Data
{
    public partial class LaoTze
    {
        public static void AddValidArguments()
        {
            AddValidArgument("assembly", false, false, "On /reverseDao, the path to the assembly to analyse for dao types.");
            AddValidArgument("nameSpace", false, false, "On /reverseDao, the namespace to check for dao types.");
        }
    }
}