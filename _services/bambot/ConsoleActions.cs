using System;
using System.Threading;
using Bam.Net.CommandLine;
using Bam.Net.Testing;

namespace Bam.Net.Application
{
    [Serializable]
    public class ConsoleActions : CommandLineTestInterface
    {
        [ConsoleAction("init", "Write the default BuildSettings.yaml file")]
        public void Init()
        {
            BuildSettings settings = new BuildSettings();
            settings.ToYamlFile("./bambot.yaml");
        }
        
        [ConsoleAction("build", "Build the BamFramework")]
        public void Build()
        {
            Bambot bambot = new Bambot();
            if (!bambot.Bake.Exists)
            {
                OutLineFormat("Bake doesn't exist in workspace tools {0}", bambot.Bake.FullName);
                return;
            }

            string buildSettingsPath = GetArgument("build").Or("./bambot.yaml");
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