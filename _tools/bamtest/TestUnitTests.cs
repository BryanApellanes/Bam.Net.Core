using Bam.Net.Testing.Unit;
using System;

namespace Bam.Net.Testing
{
    [Serializable]
    public class TestUnitTests: CommandLineTool
    {
        [UnitTest]
        public void PassingTest()
        {
            Expect.IsTrue(true);
            Pass("Passing test should pass");
        }

        [UnitTest]
        public void FailingTest()
        {
            Expect.IsTrue(false);            
        }
    }
}
