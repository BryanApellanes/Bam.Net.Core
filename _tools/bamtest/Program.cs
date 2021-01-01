using Bam.Net.Automation.Testing;
using Bam.Net.CommandLine;
using Bam.Net.Configuration;
using Bam.Net.Testing.Integration;
using Bam.Net.Testing.Unit;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;

namespace Bam.Net.Testing
{
    [Serializable]
    public partial class Program : CommandLineTool
    {
        private const string _exitOnFailure = "exitOnFailure";
        private const string _programName = "bamtestrunner";
        
        static void Main(string[] args)
        {
            IsolateMethodCalls = false;
            PreInit();
            Initialize(args);
            CheckBamDebugSetting();
            string resultDirectory = Arguments.Contains("data") ? Arguments["data"] : ".";
            string filePrefix = Arguments["dataPrefix"].Or("BamTests");
            UnitTestRunListeners = new HashSet<ITestRunListener<UnitTestMethod>>
            {
                new UnitTestRunListener(resultDirectory, $"{filePrefix}_{DateTime.Now.Date:MM_dd_yyyy}")
            };
            ConsoleLogger logger = new ConsoleLogger() { AddDetails = false, ShowTime = true, ApplicationName = "bamtest", UseColors = true };
            logger.StartLoggingThread();
            if(ExecuteSwitches(Arguments, typeof(Program), false, logger))
            {
                return;
            }
            else
            {
                Start();
            }
        }

        public static void PreInit()
        {
            AddArguments();
            TestAction = RunUnitTests;
        }
        
        public static void Start()
        {
            Enum.TryParse<TestType>(Arguments["type"].Or("Unit"), out TestType testType);

            Setup(out string startDirectory, out DirectoryInfo testDirectory , out FileInfo[] testAssemblies);
            
            switch (testType)
            {
                case TestType.Unit:
                    TestAction = RunUnitTests;
                    break;
                case TestType.Integration:
                    TestAction = RunIntegrationTests;
                    break;
            }

            if (testAssemblies.Length == 0)
            {
                Message.PrintLine("No test assemblies were found in test directory ({0})", ConsoleColor.Yellow, testDirectory.FullName);
            }
            else
            {
                TestAction(startDirectory, testAssemblies);
            }
        }

        public static HashSet<ITestRunListener<UnitTestMethod>> UnitTestRunListeners { get; set; }

        protected static Action<string, FileInfo[]> TestAction { get; set; }

        protected static void RunIntegrationTests(string startDirectory, FileInfo[] files)
        {
            bool exceptionOccurred = false;
            for (int i = 0; i < files.Length; i++)
            {
                FileInfo fi = files[i];
                RunIntegrationTestsInFile(fi.FullName, startDirectory);
            }
            
            if (_failedCount > 0 || exceptionOccurred)
            {
                Exit(1);
            }
            else
            {
                Exit(0);
            }
        }
        static int? _failedCount;
        static int? _passedCount;
        /// <summary>
        /// Runs the unit tests found in the specified files.
        /// </summary>
        /// <param name="startDirectory">The start directory.</param>
        /// <param name="files">The files.</param>
        protected static void RunUnitTests(string startDirectory, FileInfo[] files)
        {
            bool exceptionOccurred = false;
            for (int i = 0; i < files.Length; i++)
            {
                FileInfo fi = files[i];
                RunUnitTestsInFile(fi.FullName, startDirectory);                
            }

            Message.PrintLine("Passed: {0}", ConsoleColor.Green, _passedCount);
            Message.PrintLine("Failed: {0}", ConsoleColor.Red, _failedCount);

            if (_failedCount > 0 || exceptionOccurred)
            {
                Exit(1);
            }
            else
            {
                Exit(0);
            }
        }

        private static void HandleException(Exception ex)
        {
            Message.PrintLine("{0}: {1}", ConsoleColor.DarkRed, _programName, ex.Message);
            Message.PrintLine("Stack: {0}", ConsoleColor.DarkRed, ex.StackTrace);
            if (Arguments.Contains(_exitOnFailure))
            {
                Exit(1);
            }
        }
        
        private static void Setup()
        {
            Setup(out string ignoreDirectory, out FileInfo[] ignoreFiles);
        }

        private static void Setup(out string startDirectory, out FileInfo[] testAssemblies)
        {
            Setup(out startDirectory, out DirectoryInfo ignore, out testAssemblies);
        }
        
        private static void Setup(out string startDirectory, out DirectoryInfo testDir, out FileInfo[] testAssemblies)
        {
            string reportHost = Arguments["testReportHost"];
            if (string.IsNullOrEmpty(reportHost))
            {
                reportHost = DefaultConfiguration.GetAppSetting("TestReportHost", string.Empty);
            }
            if (!string.IsNullOrEmpty(reportHost))
            {
                if(!int.TryParse(Arguments["testReportPort"].Or(DefaultConfiguration.GetAppSetting("TestReportPort", string.Empty)).Or("80"), out int port))
                {
                    port = 80;
                }
                UnitTestRunListeners.Add(new UnitTestRunReportingListener(reportHost, port));
            }

            GetUnitTestRunListeners = () => UnitTestRunListeners;
            startDirectory = Environment.CurrentDirectory;
            testDir = GetTestDirectory();
            Environment.CurrentDirectory = testDir.FullName;

            testAssemblies = GetTestAssemblies(testDir);          
        }

        private static FileInfo[] GetTestAssemblies(DirectoryInfo testDir)
        {
            Message.PrintLine("Getting test files from: {0}", ConsoleColor.DarkCyan, testDir.FullName);
            FileInfo[] files = new FileInfo[] { };
            if (Arguments.Contains("search"))
            {
                string search = Arguments["search"];
                Message.PrintLine("/search switch specified: {0}", ConsoleColor.DarkCyan, search);
                files = testDir.GetFiles(search);
            }
            else if (Arguments.Contains("testFile"))
            {
                string testFile = Arguments["testFile"];
                Message.PrintLine("/testFile switch specified: {0}", ConsoleColor.DarkCyan, testFile);
                FileInfo file = new FileInfo(testFile);
                if (!file.Exists)
                {
                    throw new InvalidOperationException($"The specified test file was not found: {file.FullName}");
                }
                files = new FileInfo[] { file };
            }
            else
            {
                OutLineFormat("Getting default tests");
                List<FileInfo> tmp = new List<FileInfo>();
                tmp.AddRange(testDir.GetFiles("*tests.exe"));
                tmp.AddRange(testDir.GetFiles("*tests.dll"));
                files = tmp.ToArray();
            }
            OutLineFormat("retrieved ({0}) files", files.Length);
            return files;
        }
       
        private static DirectoryInfo GetTestDirectory()
        {
            OutLine("Getting test directory");
            DirectoryInfo testDir = new DirectoryInfo(".");
            if (Arguments.Contains("dir"))
            {
                string dir = Arguments["dir"];
                testDir = new DirectoryInfo(dir);
                if (!testDir.Exists)
                {
                    OutLineFormat("The specified directory ({0}) was not found", ConsoleColor.Magenta, dir);
                    Exit(1);
                }
            }
            OutLineFormat("Got test directory: {0}", testDir.FullName);
            return testDir;
        }
    }

}
