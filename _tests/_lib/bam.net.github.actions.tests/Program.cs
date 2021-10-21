using System;
using Bam.Net.CommandLine;

namespace Bam.Net.Github.Actions.Tests
{
    public class Program: CommandLineTool
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