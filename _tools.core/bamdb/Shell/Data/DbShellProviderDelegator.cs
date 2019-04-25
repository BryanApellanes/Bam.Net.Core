using System;
using Bam.Shell;
using Bam.Shell;

namespace Bam.Shell.Data
{
    public class DbShellProviderDelegator : ArgZeroDelegator<DbShellProvider>
    {
        [ArgZero("new")]
        public void New()
        {
            DbShellProvider provider = Construct();
            provider?.New(StandardOut, StandardError);
            Exit(provider != null ? 0 : 1);
        }
        
        [ArgZero("get")]
        public void Get()
        {
            DbShellProvider provider = Construct();
            provider?.Get(StandardOut, StandardError);
            Exit(provider != null ? 0 : 1);
        }
        
        [ArgZero("set")]
        public void Set()
        {
            DbShellProvider provider = Construct();
            provider?.Set(StandardOut, StandardError);
            Exit(provider != null ? 0 : 1);
        }
        
        [ArgZero("del")]
        public void Del()
        {
            DbShellProvider provider = Construct();
            provider?.Del(StandardOut, StandardError);
            Exit(provider != null ? 0 : 1);
        }
        
        [ArgZero("find")]
        public void Find()
        {
            DbShellProvider provider = Construct();
            provider?.Find(StandardOut, StandardError);
            Exit(provider != null ? 0 : 1);
        }
    }
}