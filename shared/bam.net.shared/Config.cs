using System.Collections.Generic;
using System.IO;
using System.Reflection;
using Lucene.Net.Analysis.Hunspell;

namespace Bam.Net
{
    public class Config
    {
        public static Dictionary<string, string> Read()
        {
            FileInfo configFile = GetConfigFile();
            return configFile.FromYamlFile<Dictionary<string, string>>() ?? new Dictionary<string, string>();
        }

        public static void Write(Dictionary<string, string> appSettings)
        {
            foreach (string key in appSettings.Keys)
            {
                BamEnvironmentVariables.SetBamVariable(key, appSettings[key]);
            }
            FileInfo configFile = GetConfigFile();
            if (configFile.Exists)
            {
                Dictionary<string, string> existing = configFile.FullName.FromYamlFile<Dictionary<string, string>>() ?? new Dictionary<string, string>();
                foreach (string key in existing.Keys)
                {
                    appSettings.AddMissing(key, existing[key]);
                }
            }
            appSettings.ToYaml().SafeWriteToFile(configFile.FullName, true);
        }

        private static FileInfo GetConfigFile()
        {
            string assemblyFile = Assembly.GetEntryAssembly().GetFileInfo().FullName;
            string configName = Path.GetFileNameWithoutExtension(assemblyFile);
            FileInfo configFile = new FileInfo(Path.Combine(BamPaths.ConfPath, configName, $"{configName}.yaml"));
            if (!configFile.Exists)
            {
                configFile.Create();
            }
            return configFile;
        }
    }
}