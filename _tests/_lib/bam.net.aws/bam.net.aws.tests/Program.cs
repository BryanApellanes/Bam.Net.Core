using System;
using Bam.Net.Testing;

namespace Bam.Net.Aws.Tests
{
    class Program : CommandLineTestInterface
    {
        static void Main(string[] args)
        {
            ExecuteMain(args,
                () => { },
                parsedArguments => { });
        }
    }
}