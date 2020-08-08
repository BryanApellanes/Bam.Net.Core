using System;
using Bam.Net.CommandLine;

namespace Bam.Net.Testing
{
    public class SettingsConsoleActions: CommandLineTool
    {
        [ConsoleAction("config", "Set a config value")]
        public void SetApplicationSetting()
        {
            string configArg = GetArgument("config");
            Config config = Config.Current;
            if (configArg.Equals("show"))
            {
                ShowConfig();
                return;
            }
            string[] keyValue = configArg.DelimitSplit("=");
            if (keyValue.Length != 2)
            {
                Message.PrintLine("Unrecognized config argument specified, should be in the form /config:[key]=[value]: /config:{0}", configArg);
                return;
            }
            string key = keyValue[0];
            string value = keyValue[1];
            config[key, value] = value;
            ShowConfig();
        }

        private void ShowConfig()
        {
            Message.PrintLine("{0}", Config.Current.ToYaml());
        }
    }
}