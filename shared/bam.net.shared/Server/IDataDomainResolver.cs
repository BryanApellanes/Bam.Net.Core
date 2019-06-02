namespace Bam.Net.Server
{
    public interface IDataDomainResolver
    {
        /// <summary>
        /// When implemented by a derived class, returns an array of
        /// fully qualified domain names served for the specified AppConf.
        /// For example: myDataDomain.myPrimaryDomain.com 
        /// </summary>
        /// <param name="appConf"></param>
        /// <returns></returns>
        string[] ResolveDataSubdomains(AppConf appConf);
    }
}