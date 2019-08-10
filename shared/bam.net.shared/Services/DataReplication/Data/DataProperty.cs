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
        public string InstanceIdentifier { get; set; }
        public string Name { get; set; }
        public object Value { get; set; }
        public override bool Equals(object obj)
        {
            if (obj is DataProperty dataProp)
            {
                return dataProp.Name.Equals(Name) && Value.Equals(dataProp.Value) && InstanceIdentifier.Equals(dataProp.InstanceIdentifier);
            }
            return false;
        }
        public override int GetHashCode()
        {
            return this.GetHashCode(InstanceIdentifier, Name, Value);
        }
        
        public virtual ulong CreateOperationId { get; set; }
        public virtual ulong DeleteEventId { get; set; }
        public virtual ulong DeleteOperationId { get; set; }
        public virtual ulong UpdateOperationId { get; set; }
        public virtual ulong WriteEventId { get; set; }
        public virtual ulong SaveOperationId { get; set; }
    }
}
