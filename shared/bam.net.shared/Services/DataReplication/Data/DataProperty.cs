using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Bam.Net.Data.Repositories;

namespace Bam.Net.Services.DataReplication.Data
{
    [Serializable]
    public class DataProperty: AuditRepoData
    {
        public string InstanceCuid { get; set; }
        public string Name { get; set; }
        public object Value { get; set; }
        public override bool Equals(object obj)
        {
            if (obj is DataProperty dataProp)
            {
                return dataProp.Name.Equals(Name) && Value.Equals(dataProp.Value) && InstanceCuid.Equals(dataProp.InstanceCuid);
            }
            return false;
        }
        public override int GetHashCode()
        {
            return this.GetHashCode(InstanceCuid, Name, Value);
        }
    }
}
