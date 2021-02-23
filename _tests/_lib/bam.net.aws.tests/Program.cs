using System;
using Bam.Net.CommandLine;
using Bam.Net.Testing;

namespace Bam.Net.Aws.Tests
{
    class Program : CommandLineTool
    {
        static void Main(string[] args)
        {
            ExecuteMainOrInteractive(args,(a) =>
            {
                Message.PrintLine("Error parsing arguments: {0}", ConsoleColor.Red, a.Message);
                Environment.Exit(1);
            });
        }
    }
}