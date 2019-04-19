

namespace Bam.Shell.Pack
{
    public class PackageProviderDelegator : ArgZeroDelegator<PackageProvider>
    {
    

        [ArgZero("build")]
        public void Build()
        {
            PackageProvider provider = Construct();
            provider?.Build(StandardOut, StandardError);
            Exit(provider != null ? 0 : 1);
        }
        
        [ArgZero("pack")]
        public void Pack()
        {
            PackageProvider provider = Construct();
            provider?.Pack(StandardOut, StandardError);
            Exit(provider != null ? 0 : 1);
        }
        
        [ArgZero("push")]
        public void Push()
        {
            PackageProvider provider = Construct();
            provider?.Push(StandardOut, StandardError);
            Exit(provider != null ? 0 : 1);
        }
        
        [ArgZero("pull")]
        public void Pull()
        {
            PackageProvider provider = Construct();
            provider?.Pull(StandardOut, StandardError);
            Exit(provider != null ? 0 : 1);
        }
            
    } 
}