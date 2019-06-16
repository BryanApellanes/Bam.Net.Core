using System;
using Bam.Net.CommandLine;
using Bam.Net.Testing;

namespace Bam.Net.Adhoc
{
    [Serializable]
    public class ConsoleActions : CommandLineTestInterface
    {
        [ConsoleAction("A Random test")]
        public void TestSomething()
        {
            OutLine("It worked!", ConsoleColor.Green);
        }
    }
}