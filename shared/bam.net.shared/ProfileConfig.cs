using System;
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

        public static T LoadJson<T>()
        {
            string path = GetPath(typeof(T), "json");
            return LoadJsonConfig<T>(path);
        }
        
        public static T LoadYaml<T>()
        {
            string path = GetPath(typeof(T), "yaml");
            return LoadYamlConfig<T>(path);
        }

        public static void SaveJson<T>(T instance)
        {
            string path = GetPath(typeof(T), "json");
            instance.ToJsonFile(path);
        }
        
        public static void SaveYaml<T>(T instance)
        {
            string path = GetPath(typeof(T), "yaml");
            instance.ToYamlFile(path);
        }
        
        public static void SaveAssemblyConfig<T>(T instance, string configName = "config")
        {
            string path = GetAssemblyConfigPath<T>(configName);
            instance.ToJsonFile(path);
        }

        public static T Load<T>(string configName = "config")
        {
            return LoadAssemblyConfig<T>(configName);
        }
        
        public static T LoadAssemblyConfig<T>(string configName = "config")
        {
            string path = GetAssemblyConfigPath<T>(configName);
            return LoadJsonConfig<T>(path);
        }

        private static T LoadJsonConfig<T>(string path)
        {
            Log.Trace("config file path = {0}", path);
            FileInfo configFile = EnsureFile(path);

            return configFile.FromJsonFile<T>();
        }
        
        private static T LoadYamlConfig<T>(string path)
        {
            Log.Trace("config file path = {0}", path);
            FileInfo configFile = EnsureFile(path);

            return configFile.FromYamlFile<T>();
        }

        private static string GetAssemblyConfigPath<T>(string configName)
        {
            string assemblyFile = Assembly.GetEntryAssembly().GetFileInfo().FullName;
            string assemblyName = Path.GetFileNameWithoutExtension(assemblyFile);
            string path = Path.Combine(BamProfile.Config, assemblyName,
                $"{assemblyName}.{typeof(T).Namespace}.{typeof(T).Name}_{configName}.yaml");
            return path;
        }
        
        private static string GetYamlPath(Type t)
        {
            return GetPath(t, "yaml");
        }

        private static string GetJsonPath(Type t)
        {
            return GetPath(t);
        }
        
        private static string GetPath(Type t, string fileExtension = "json")
        {
            return Path.Combine(BamProfile.Config, $"{t.Name}.{fileExtension}");
        }
    }
}