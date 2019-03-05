using System;
using System.Collections.Generic;
using System.Text;

namespace Bam.Net.Configuration
{
    public class ConfigurationConflictEventArgs: EventArgs
    {
        public string Key { get; set; }
        public string WinningValue { get; set; }
        public string OverriddenValue { get; set; }
        public Type WinningConfigurationServiceType { get; set; }
        public Type OverriddenConfigurationServiceType { get; set; }
    }
}
