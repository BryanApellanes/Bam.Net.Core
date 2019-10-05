using System;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using Bam.Net;
using Bam.Net.Application;
using Bam.Net.CommandLine;
using Bam.Net.Testing;


namespace Bam.Shell.Conf
{
    /// <summary>
    /// Provides read write access to the process level configuration file for applications.
    /// </summary>
    public class ConfigProvider : Bam.Shell.Conf.ConfProvider
    {
        public override void RegisterArguments(string[] args)
        {            
            base.RegisterArguments(args);
            AddValidArgument("app", $"Config: The name of the application, the default is {ProcessApplicationNameProvider.Current.GetApplicationName()}");
            AddValidArgument("name", "Config: The name of the config value");
            AddValidArgument("value", "Config: The value to set");
        }

        public override void Get(Action<string> output = null, Action<string> error = null)
        {
            try
            {
                Config config = GetTargetConfig();
                output(config.File.FullName);
                output(config[GetArgument("name", "Please enter the name of the config value to get")]);
            }
            catch (Exception ex)
            {
                error($"Failed to get config value: {ex.Message}");
                Exit(1);
            }
            Exit(0);
        }        
        
        public override void Set(Action<string> output = null, Action<string> error = null)
        {
            try
            {
                Config config = GetTargetConfig();
                output(config.File.FullName);
                string name = GetArgument("name", "Please enter the name of the config value to set");
                string value = GetArgument("value", "Please enter the value to set");
                config[name] = value;
            }
            catch (Exception ex)
            {
                error($"Failed to set config value: {ex.Message}");
                Exit(1);
            }
            Exit(0);
        }

        public override void Print(Action<string> output = null, Action<string> error = null)
        {
            try
            {
                var config = GetTargetConfig();
                output(config.File.FullName);
                StringBuilder msg = new StringBuilder();
                foreach(string key in config.AppSettings.Keys)
                {
                    msg.AppendLine($"{key} = {config[key]}");
                }

                output(msg.ToString());
            }
            catch (Exception ex)
            {
                error($"Failed to list config values: {ex.Message}");
                Exit(1);
            }
        }

        private static Config GetTargetConfig()
        {
            string applicationName = ProcessApplicationNameProvider.Current.GetApplicationName();
            if (Arguments.Contains("app"))
            {
                applicationName = GetArgument("app", "Please enter the name of the application to configure");
            }

            Config config = Net.Config.For(applicationName);
            return config;
        }
    }
}