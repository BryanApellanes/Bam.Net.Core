using System;
using Bam.Shell;
using Bam.Shell;

namespace Bam.Shell.Data
{
    public class DataShellProviderDelegator : ArgZeroDelegator<DataShellProvider>
    {
        [ArgZero("new")]
        public void New()
        {
            DataShellProvider provider = Construct();
            provider?.New(StandardOut, StandardError);
            Exit(provider != null ? 0 : 1);
        }
        
        [ArgZero("get")]
        public void Get()
        {
            DataShellProvider provider = Construct();
            provider?.Get(StandardOut, StandardError);
            Exit(provider != null ? 0 : 1);
        }
        
        [ArgZero("set")]
        public void Set()
        {
            DataShellProvider provider = Construct();
            provider?.Set(StandardOut, StandardError);
            Exit(provider != null ? 0 : 1);
        }
        
        [ArgZero("del")]
        public void Del()
        {
            DataShellProvider provider = Construct();
            provider?.Del(StandardOut, StandardError);
            Exit(provider != null ? 0 : 1);
        }
        
        [ArgZero("find")]
        public void Find()
        {
            DataShellProvider provider = Construct();
            provider?.Find(StandardOut, StandardError);
            Exit(provider != null ? 0 : 1);
        }
    }
}