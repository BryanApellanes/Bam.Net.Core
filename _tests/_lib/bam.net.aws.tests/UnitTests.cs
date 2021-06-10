using System;
using Bam.Net.CommandLine;
using Bam.Net.Testing;
using Bam.Net.Testing.Unit;

namespace Bam.Net.Aws.Tests
{
    [Serializable]
    public class UnitTests : CommandLineTool
    {
        [UnitTest]
        public void CanListS3Files()
        {
            Message.PrintLine("This test is not fully implemented");
        }
    }
}