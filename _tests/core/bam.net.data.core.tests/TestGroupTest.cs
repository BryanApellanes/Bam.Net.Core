using System;
using Bam.Net.Testing;
using Bam.Net.Testing.Unit;

namespace Bam.Net.Data.Tests
{
    public class TestGroupTest: CommandLineTestInterface
    {
        [UnitTest]
        [TestGroup("TestTestGroup")]
        public void TestTestGroup()
        {
            Pass("Test group passed");
        }
    }
}