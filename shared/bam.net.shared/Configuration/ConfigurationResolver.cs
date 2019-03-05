using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration;
using System.Text;

namespace Bam.Net.Configuration
{
    public partial class ConfigurationResolver
    {
        static object _currentLock = new object();
        static ConfigurationResolver _current;
        public static ConfigurationResolver Current
        {
            get
            {
                return _currentLock.DoubleCheckLock(ref _current, () => new ConfigurationResolver());
            }
            set
            {
                _current = value;
            }
        }
        // TODO: add configurationService
    }
}
