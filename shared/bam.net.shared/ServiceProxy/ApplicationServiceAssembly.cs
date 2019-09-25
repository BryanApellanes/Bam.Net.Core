using System.Reflection;
using Bam.Net.Server;

namespace Bam.Net.ServiceProxy
{
    public class ApplicationServiceAssembly
    {
        public static implicit operator Assembly(ApplicationServiceAssembly applicationServiceAssembly)
        {
            return applicationServiceAssembly?.Assembly;
        }

        public static implicit operator byte[](ApplicationServiceAssembly applicationServiceAssembly)
        {
            return applicationServiceAssembly?.AssemblyData;
        }
        
        public AppConf AppConf { get; set; }
        
        public string Name { get; set; }

        public Assembly Assembly => Assembly.Load(AssemblyData);

        protected internal byte[] AssemblyData { get; set; }
    }
}