using Bam.Net.Data.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Bam.Net.CoreServices.ApplicationRegistration.Data
{
    /// <summary>
    /// A domain associated with an application
    /// </summary>
    [Serializable]
    public class HostDomain: KeyedAuditRepoData
    {
        public HostDomain()
        {
            Port = 80;
        }
        
        [CompositeKey]
        public string DefaultApplicationName { get; set; }
        [CompositeKey]
        public string DomainName { get; set; }
        [CompositeKey]
        public int Port { get; set; }
        public bool Authorized { get; set; }
        public virtual List<Application> Applications { get; set; }
    }
}
