using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Bam.Net.Data.Repositories;

namespace Bam.Net.CoreServices.ApplicationRegistration.Data
{
    [Serializable]
    public class ConfigurationSetting: KeyedAuditRepoData
    {
        [CompositeKey]
        public virtual ulong ConfigurationId { get; set; }
        public virtual Configuration Configuration { get; set; }

        [CompositeKey]
        public string Key { get; set; }
        public string Value { get; set; }

        public override bool Equals(object obj)
        {
            if(!(obj is ConfigurationSetting conf))
            {
                return false;
            }

            return conf.Key.Equals(Key) && conf.Value.Equals(Value);
        }

        public override int GetHashCode()
        {
            return $"{Key}.{Value}".GetHashCode();
        }
    }
}
