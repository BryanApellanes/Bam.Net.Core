using System;
using Bam.Net.CommandLine;
using Bam.Net.Testing;

namespace Bam.Net.Application
{
    [Serializable]
    public class ConsoleActions : CommandLineTestInterface
    {
        [ConsoleAction("build", "Builds the BamFramework")]
        public void Build()
        {
            // checkout latest /branch:[name]
            
            // build toolkit with bake
            
            // build tests with bake, output to /opt/bam/tests
        }

        [ConsoleAction("test", "Tests the BamFramework")]
        public void Test()
        {
            // run tests bte
        }
    }
}