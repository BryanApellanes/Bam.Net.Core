using System.Reflection;

namespace Bam.Net.ServiceProxy
{
    public class AppServiceAssembly
    {
        public static implicit operator Assembly(AppServiceAssembly appServiceAssembly)
        {
            return appServiceAssembly?.Assembly;
        }

        public static implicit operator byte[](AppServiceAssembly appServiceAssembly)
        {
            return appServiceAssembly?.AssemblyData;
        }
        
        public string Name { get; set; }

        public Assembly Assembly => Assembly.Load(AssemblyData);

        protected internal byte[] AssemblyData { get; set; }
    }
}