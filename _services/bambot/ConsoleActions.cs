using System;
using System.IO;
using System.Threading;
using Bam.Net.CommandLine;
using Bam.Net.Testing;

namespace Bam.Net.Application
{
    [Serializable]
    public class ConsoleActions : CommandLineTestInterface
    {
        [ConsoleAction("init", "Write the default bambot.yaml file")]
        public void Init()
        {
            BuildSettings settings = new BuildSettings();
            settings.ToYamlFile("./bambot.yaml");
        }
        
        [ConsoleAction("build", "Build the BamFramework")]
        public void Build()
        {
            Bambot bambot = new Bambot();
            if (!bambot.TryGetBuildRunner(out FileInfo buildRunner))
            {
                OutLineFormat("Failed to get build runner");
                Exit(1);
            }

            string buildSettingsPath = GetArgument("build", "Please enter the path to the build settings file to use").Or("./bambot.yaml");
            BuildSettings buildSettings = buildSettingsPath.FromYamlFile<BuildSettings>();
            bambot.Build(buildSettings, output => OutLine(output, ConsoleColor.DarkCyan),
                error => OutLine(error, ConsoleColor.DarkMagenta));
        }

        [ConsoleAction("test", "Test the BamFramework")]
        public void Test()
        {
            // run tests bte
        }
    }
}