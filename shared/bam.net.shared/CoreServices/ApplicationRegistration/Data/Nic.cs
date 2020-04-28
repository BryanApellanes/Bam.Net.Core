using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Bam.Net.Data.Repositories;

namespace Bam.Net.CoreServices.ApplicationRegistration.Data
{
    [Serializable]
    public class Nic: KeyedRepoData
    {
        public ulong MachineId { get; set; }
        
        public virtual Machine Machine { get; set; }
        
        [CompositeKey]
        public string AddressFamily { get; set; }
        
        [CompositeKey]
        public string Address { get; set; }
        
        [CompositeKey]
        public string MacAddress { get; set; }
        
        public override int GetHashCode()
        {
            return $"{AddressFamily}:{Address}:{MacAddress}".GetHashCode();
        }
        
        public override bool Equals(object obj)
        {
            if(obj is Nic input)
            {
                return input.AddressFamily.Equals(AddressFamily) && input.Address.Equals(Address);
            }
            return false;
        }
    }
}
