using System;
using System.IO;
using System.Linq;
using System.Reflection;
using Bam.Net;
using Bam.Net.Application;
using Bam.Net.CommandLine;


namespace Bam.Shell.Conf
{
    public class ConfigProvider : Bam.Shell.Conf.ConfProvider
    {
        public override void RegisterArguments(string[] args)
        {            
            base.RegisterArguments(args);
            AddValidArgument("name", "Config: The name of the config value");
            AddValidArgument("value", "Config: The value to set");
        }

        public override void Get(Action<string> output = null, Action<string> error = null)
        {
            try
            {
                output(Config.Current[GetArgument("name", "Please enter the name of the config value to get")]);
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
                string name = GetArgument("name", "Please enter the name of the config value to set");
                string value = GetArgument("value", "Please enter the value to set");
                Config.Current[name] = value;
            }
            catch (Exception ex)
            {
                error($"Failed to set config value: {ex.Message}");
                Exit(1);
            }
            Exit(0);
        }        
    }
}