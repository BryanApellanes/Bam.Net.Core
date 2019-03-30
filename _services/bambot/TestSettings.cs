namespace Bam.Net.Application
{
    public class TestSettings
    {
        public TestSettings()
        {
            TestRunner = "bamtest";
            TestArguments = "see bamtest for more information";
        }
        
        public string TestRunner { get; set; }
        public string TestArguments { get; set; }
        public int TestSuccessExitCode { get; set; }
        
        
        
        /*ProcessOutput testOutput = settings.TestRunner.Start(settings.TestArguments, output, error);
            if (testOutput.ExitCode != settings.TestSuccessExitCode)
        {
            return new BambotActionResult()
            {
                Success = false,
                Message = $"Test exited with code {testOutput.ExitCode}"
            };
        }*/
    }
}