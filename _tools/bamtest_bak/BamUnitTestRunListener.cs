using System;
using System.Collections.Generic;
using Bam.Net.CommandLine;
using Bam.Net.Logging;
using Bam.Net.Testing.Unit;

namespace Bam.Net.Testing
{
    public class BamUnitTestRunListener: UnitTestRunListener
    {
        public BamUnitTestRunListener(string resultDirectory) : base(resultDirectory, "BamUnitTests")
        {
            FailedTests = new HashSet<UnitTestFailure>();
        }

        public bool FailuresOccurred => FailedTests?.Count > 0;

        public HashSet<UnitTestFailure> FailedTests { get; set; }
        
        public Action<TestExceptionEventArgs> TestFailedAction { get; set; }
        public Action<TestEventArgs<UnitTestMethod>> TestsFinishedAction { get; set; }

        public override void TestFailed(object sender, TestExceptionEventArgs args)
        {
            FailedTests.Add(new UnitTestFailure(args));
            base.TestFailed(sender, args);
            TestFailedAction?.Invoke(args);
        }

        public override void TestsFinished(object sender, TestEventArgs<UnitTestMethod> args)
        {
            base.TestsFinished(sender, args);
            TestsFinishedAction?.Invoke(args);
        }

        public void LogSummary()
        {
            LogSummary(Log.Default);
        }
        
        public void LogSummary(ILogger logger)
        {
            List<ConsoleMessage> messages = new List<ConsoleMessage>();
            foreach (UnitTestFailure failure in FailedTests)
            {
                messages.Add(GetMessage(failure));
            }
            Message.Log(messages.ToArray());
        }

        private ConsoleMessage GetMessage(UnitTestFailure unitTestFailure)
        {
            string methodName = unitTestFailure?.TestMethod?.Method?.Name;
            string information = unitTestFailure?.TestMethod?.Information;
            string exceptionMessage = unitTestFailure?.Exception?.Message;
            string stackTrace = unitTestFailure?.Exception?.StackTrace;
            ConsoleMessage message = new ConsoleMessage
            {
                Colors = new ConsoleColorCombo(ConsoleColor.Yellow, ConsoleColor.Red)
            };
            message.SetText("({0})[\"{1}\"]: *** {2} ***\r\n\tStackTrace: {3}\r\n", methodName, information, exceptionMessage, stackTrace);
            return message;
        }
    }
}