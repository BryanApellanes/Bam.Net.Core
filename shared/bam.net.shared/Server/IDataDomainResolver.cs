using System.Collections;
using System.Collections.Generic;

namespace Bam.Net.Server
{
    public interface IDataDomainResolver
    {
        string PrimaryHostName { get; set; }
        int Port { get; set; }
        /// <summary>
        /// When implemented by a derived class, returns an array of
        /// fully qualified domain names served for the specified AppConf.
        /// For example: myDataDomain.myPrimaryDomain.com 
        /// </summary>
        /// <param name="appConf"></param>
        /// <returns></returns>
        IEnumerable<HostPrefix> ResolveDataSubdomains(AppConf appConf);
    }
}