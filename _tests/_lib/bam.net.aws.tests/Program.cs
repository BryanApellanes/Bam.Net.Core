using System;
using Bam.Net.Testing;

namespace Bam.Net.Aws.Tests
{
    class Program : CommandLineTool
    {
        static void Main(string[] args)
        {
            ExecuteMain(args,
                () => { },
                parsedArguments => { });
        }
    }
}