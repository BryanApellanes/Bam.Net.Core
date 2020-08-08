using System.Collections.Generic;
using System.IO;
using System.Reflection;
using Bam.Net.Logging;

namespace Bam.Net
{
    public class ProfileConfig: Config
    {
        public ProfileConfig() : base()
        {
            AppSettings = GetBamProfileConfigFile()?.FromYamlFile<Dictionary<string, string>>();
        }

        public static void Save<T>(T instance, string configName = "config")
        {
            string path = GetPath<T>(configName);
            instance.ToJsonFile(path);
        }
        
        public static T Load<T>(string configName = "config")
        {
            string path = GetPath<T>(configName);
            Log.Trace("config file path = {0}", path);
            FileInfo configFile = EnsureFile(path);

            return configFile.FromJsonFile<T>();
        }

        private static string GetPath<T>(string configName)
        {
            string assemblyFile = Assembly.GetEntryAssembly().GetFileInfo().FullName;
            string assemblyName = Path.GetFileNameWithoutExtension(assemblyFile);
            string path = Path.Combine(BamProfile.Config, assemblyName,
                $"{assemblyName}.{typeof(T).Namespace}.{typeof(T).Name}_{configName}.yaml");
            return path;
        }
    }
}