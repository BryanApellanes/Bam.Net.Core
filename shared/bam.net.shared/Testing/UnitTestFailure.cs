using System;

namespace Bam.Net.Testing
{
    public class UnitTestFailure
    {
        public UnitTestFailure()
        {
        }

        public UnitTestFailure(TestMethod testMethod, Exception ex)
        {
            this.TestMethod = testMethod;
            this.Exception = ex;
        }

        public UnitTestFailure(TestExceptionEventArgs testExceptionEventArgs)
        {
            this.CopyProperties(testExceptionEventArgs);
        }
        
        public Exception Exception { get; set; }
        public TestMethod TestMethod { get; set; }
    }
}