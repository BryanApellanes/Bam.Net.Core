using System.Collections.Generic;
using Bam.Net.CoreServices.ApplicationRegistration.Data;
using Bam.Net.Data.Repositories;

namespace Bam.Net.Services.DataReplication.Data
{
    public class DataPointOrigin : KeyedAuditRepoData
    {
        public DataPointOrigin()
        {
            MachineName = $"{Machine.Current.DnsName}({Machine.Current.Name})";
        }
        [CompositeKey]
        public string MachineName { get; set; }
        [CompositeKey]
        public string AssemblyPath { get; set; }
        
        public virtual List<DataPoint> DataPoints { get; set; }
    }
}