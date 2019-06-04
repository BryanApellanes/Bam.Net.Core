﻿using Bam.Net.CommandLine;
using Bam.Net.Testing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using Bam.Net.Logging;
using Bam.Net.Testing.Integration;
using System.IO;
using System.Diagnostics;
using Bam.Net.Data;
using Bam.Net.Automation.Testing;
using Bam.Net.Testing.Unit;

namespace Bam.Net.Testing
{
    public partial class Program
    {
        /// <summary>
        /// Gets the path to OpenCover.Console.exe.
        /// </summary>
        /// <value>
        /// The open cover.
        /// </value>
        protected static string OpenCover
        {
            get
            {
                return "/bam/tools/OpenCover/OpenCover.Console.exe";
            }
        }

        /// <summary>
        /// Gets the output root.
        /// </summary>
        /// <value>
        /// The output root.
        /// </value>
        protected static string OutputRoot
        {
            get
            {
                return "/bam/tests/";
            }
        }

        /// <summary>
        /// Runs the tests with coverage.
        /// </summary>
        [ConsoleAction("TestsWithCoverage", "Run tests with coverage using opencover.console")]
        public static void RunTestsWithCoverage()
        {
            Process process = Process.GetCurrentProcess();
            FileInfo main = new FileInfo(process.MainModule.FileName);
            string testType = "Unit";
            if (Arguments.Contains("type"))
            {
                testType = Arguments["type"];
            }
            if (!testType.Equals("Unit") && !testType.Equals("Integration"))
            {
                OutLineFormat("Invalid test type specified: {0}", testType);
                Exit(-1);
            }
            string testReportHost = GetArgument("testReportHost", "What server/hostname should tests report to?");
            string testReportPort = GetArgument("testReportPort", "What port is the test report service listening on?");
            string tag = string.Empty;
            OutLine("Checking for commit file");
            string commitFile = Path.Combine(main.Directory.FullName, "commit");
            if (File.Exists(commitFile))
            {
                OutLine("commit file found; reading commit hash to use as tag");
                tag = File.ReadAllText(commitFile).First(6);
                OutLine(tag);
            }
            if (string.IsNullOrEmpty(tag))
            {
                tag = GetArgument("tag", "Enter a tag to use to identify test results");
            }
            DirectoryInfo outputDirectory = EnsureOutputDirectories(tag);
            FileInfo[] testAssemblies = GetTestFiles(GetTestDirectory());
            foreach(FileInfo file in testAssemblies)
            {
                OutLineFormat("{0}:Running tests in: {1}", ConsoleColor.Cyan, DateTime.Now.ToLongTimeString(), file.FullName);
                string testFileName = Path.GetFileNameWithoutExtension(file.Name);
                string xmlFile = Path.Combine(Paths.Tests, TestConstants.CoverageXmlFolder, $"{testFileName}_{tag}_coverage.xml");
                string outputFile = Path.Combine(Paths.Tests, TestConstants.OutputFolder, $"{testFileName}_{tag}_output.txt");
                string errorFile = Path.Combine(Paths.Tests, TestConstants.OutputFolder, $"{testFileName}_{tag}_error.txt");
                string commandLine = $"{OpenCover} -target:\"{main.FullName}\" -targetargs:\"/type:{testType} /{testType}Tests:{file.FullName} /testReportHost:{testReportHost} /testReportPort:{testReportPort} /tag:{tag}\" -register -threshold:10 -filter:\"+[Bam.Net*]* -[*].Data.* -[*].Testing.* -[*Test*].Tests.*\" -output:{xmlFile}";
                OutLineFormat("CommandLine: {0}", ConsoleColor.Yellow, commandLine);
                ProcessOutput output = commandLine.Run(7200000); // timeout after 2 hours
                output.StandardError.SafeWriteToFile(errorFile, true);
                output.StandardOutput.SafeWriteToFile(outputFile, true);
            }
        }

        /// <summary>
        /// Runs the unit tests in file.
        /// </summary>
        /// <exception cref="InvalidOperationException">UnitTest file not specified</exception>
        [ConsoleAction("UnitTests", "[path_to_test_assembly]", "Run unit tests in the specified assembly")]
        public static void RunUnitTestsInFile()
        {
            string assemblyPath = Arguments["UnitTests"];
            if (string.IsNullOrEmpty(assemblyPath))
            {
                throw new InvalidOperationException("UnitTest file not specified");
            }
            RunUnitTestsInFile(assemblyPath, Environment.CurrentDirectory);
        }

        [ConsoleAction("Group", "[name of test group]",
            "Run tests with the specified TestGroup attribute name in assemblies found for the given search pattern.")]
        public static void RunTestGroupsInFolder()
        {
            string testGroupName = GetArgument("Group", "Please enter the name of the test group to run.");
            string searchPattern = GetArgumentOrDefault("search", "*Tests.*");
            string testDir = GetArgumentOrDefault("dir", BamPaths.BamHome);
            DirectoryInfo directory = new DirectoryInfo(testDir);
            FileInfo[] files = directory.GetFiles(searchPattern);
            if (files.Length > 0)
            {
                List<UnitTestMethod> succeeded = new List<UnitTestMethod>();
                Dictionary<UnitTestMethod, Exception> failed = new Dictionary<UnitTestMethod, Exception>();
                foreach (FileInfo file in files)
                {
                    Assembly testAssembly = Assembly.Load(file.FullName);
                    List<UnitTestMethod> testMethods = UnitTestMethod.FromAssembly(testAssembly).Where(unitTestMethod =>
                    {
                        if (unitTestMethod.Method.HasCustomAttributeOfType<TestGroupAttribute>(out TestGroupAttribute testGroupAttribute))
                        {
                            return testGroupAttribute.Groups.Contains(testGroupName);
                        }

                        return false;
                    }).ToList();
                    OutLineFormat("Found ({0}) tests in group ({1}) in assembly ({2})", ConsoleColor.Blue, testMethods.Count, testGroupName, testAssembly.FullName);
                    testMethods.Each(testMethod =>
                    {
                        if (testMethod.TryInvoke(ex =>
                        {
                            OutLineFormat("{0} failed: {1}", testMethod.Description, ex.Message);
                            failed.Add(testMethod, ex);
                        }))
                        {
                            succeeded.Add(testMethod);
                        };
                    });
                }

                if (succeeded.Count > 0)
                {
                    OutLineFormat("{0} tests passed", ConsoleColor.Green, succeeded.Count);
                    succeeded.Each(unitTest=> OutLineFormat("{0} passed", ConsoleColor.Green, unitTest.Description));
                }
                
                if (failed.Count > 0)
                {
                    StringBuilder failures = new StringBuilder();
                    failed.Keys.Each(unitTest =>  failures.AppendLine($"{unitTest.Description}: {failed[unitTest].Message}\r\n{failed[unitTest].StackTrace}\r\n"));
                    OutLineFormat("There were {0} failures", failed.Count);
                    OutLine(failures.ToString(), ConsoleColor.Magenta);
                    Exit(1);
                }
                else
                {
                    Exit(0);
                }
            }
            OutLineFormat("No files found in ({0}) for search pattern ({1})", testDir, searchPattern);
            Exit(1);
        }
        
        /// <summary>
        /// Runs the unit tests in specified assemlby.
        /// </summary>
        /// <param name="assemblyPath">The assembly path.</param>
        /// <param name="endDirectory">The end directory.</param>
        public static void RunUnitTestsInFile(string assemblyPath, string endDirectory)
        {
            OutLineFormat("Running UnitTests: {0}", ConsoleColor.DarkGreen, assemblyPath);
            assemblyPath = assemblyPath ?? Arguments["UnitTests"];
            endDirectory = endDirectory ?? Environment.CurrentDirectory;
            try
            {
                Setup();
                Assembly assembly = Assembly.LoadFrom(assemblyPath);
                RunAllUnitTests(assembly, Log.Default, (o, a) => _passedCount++, (o, a) => _failedCount++);
                Environment.CurrentDirectory = endDirectory;
                OutLineFormat("Test run complete: {0}", ConsoleColor.DarkYellow, assemblyPath);
            }
            catch (Exception ex)
            {
                Environment.CurrentDirectory = endDirectory;
                HandleException(ex);
            }
        }

        /// <summary>
        /// Runs the integration tests in the specified file.
        /// </summary>
        /// <param name="assemblyPath">The assembly path.</param>
        /// <param name="endDirectory">The end directory.</param>
        [ConsoleAction("IntegrationTests", "[path_to_test_assembly]", "Run integration tests in the specified assemlby")]
        public static void RunIntegrationTestsInFile(string assemblyPath = null, string endDirectory = null)
        {
            assemblyPath = assemblyPath ?? Arguments["IntegrationTests"];
            try
            {
                Setup();
                Assembly assembly = Assembly.LoadFrom(assemblyPath);
                IntegrationTestRunner.RunIntegrationTests(assembly);
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }
        }

        private static DirectoryInfo EnsureOutputDirectories(string tag)
        {
            OutLineFormat("Creating output directories as necessary: OutputRoot={0}, tag={1}", ConsoleColor.Cyan, OutputRoot, tag);
            DirectoryInfo outputDirectory = new DirectoryInfo(Path.Combine(OutputRoot, tag));
            OutLineFormat("Checking for output directory: {0}", ConsoleColor.Cyan, outputDirectory);
            if (!outputDirectory.Exists)
            {
                OutLineFormat("Directory doesn't exist, creating it: {0}", outputDirectory.FullName);
                outputDirectory.Create();
            }
            string coverageDir = Path.Combine(Paths.Tests, TestConstants.CoverageXmlFolder);
            OutLineFormat("Checking for coverage directory: {0}", ConsoleColor.Cyan, coverageDir);
            if (!Directory.Exists(coverageDir))
            {
                OutLineFormat("Coverage directory doesn't exist, creating it: {0}", coverageDir);
                Directory.CreateDirectory(coverageDir);
            }

            return outputDirectory;
        }
    }
}
