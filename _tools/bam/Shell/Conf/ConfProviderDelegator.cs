namespace Bam.Shell.Conf
{
    public class ConfProviderDelegator : ArgZeroDelegator<ConfProvider>
    {
        [ArgZero("get", typeof(ConfProvider))]
        public void Get()
        {
            ConfProvider provider = Construct();
            provider?.Get(StandardOut, StandardError);
            Exit(provider != null ? 0 : 1);
        }
        
        [ArgZero("set", typeof(ConfProvider))]
        public void Set()
        {
            ConfProvider provider = Construct();
            provider?.Set(StandardOut, StandardError);
            Exit(provider != null ? 0 : 1);
        }

        [ArgZero("print", typeof(ConfProvider))]
        public void Print()
        {
            ConfProvider provider = Construct();
            provider?.Print(StandardOut, StandardError);
            Exit(provider != null ? 0 : 1);
        }
    } 
}