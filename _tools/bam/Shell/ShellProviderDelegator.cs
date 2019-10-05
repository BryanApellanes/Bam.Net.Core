using System;
using System.Collections.Generic;
using Bam.Shell;
using Bam.Net;
using Bam.Net.Testing;

namespace Bam.Shell
{
    public class ShellProviderDelegator : ArgZeroDelegator<ShellProvider>
    {
        [ArgZero("list", typeof(ShellProvider))]
        public void List()
        {
            ShellProvider provider = Construct();
            provider?.List(StandardOut, StandardError);
            Exit(provider != null ? 0: 1);
        }
        
        [ArgZero("add", typeof(ShellProvider))]
        public void Add()
        {
            ShellProvider provider = Construct();
            provider?.Add(StandardOut, StandardError);
            Exit(provider != null ? 0: 1);
        }
        
        [ArgZero("copy", typeof(ShellProvider))]
        public void Copy()
        {
            ShellProvider provider = Construct();
            provider?.Copy(StandardOut, StandardError);
            Exit(provider != null ? 0: 1);
        }
        
        [ArgZero("rename", typeof(ShellProvider))]
        public void Rename()
        {
            ShellProvider provider = Construct();
            provider?.Rename(StandardOut, StandardError);
            Exit(provider != null ? 0: 1);
        }

        [ArgZero("show", typeof(ShellProvider))]
        public void Show()
        {
            ShellProvider provider = Construct();
            provider?.Show(StandardOut, StandardError);
            Exit(provider != null ? 0: 1);
        }

        [ArgZero("remove", typeof(ShellProvider))]
        public void Remove()
        {
            ShellProvider provider = Construct();
            provider?.Remove(StandardOut, StandardError);
            Exit(provider != null ? 0: 1);
        }
        
        [ArgZero("run", typeof(ShellProvider))]
        public void Run()
        {
            ShellProvider provider = Construct();
            provider?.Run(StandardOut, StandardError);
            Exit(provider != null ? 0: 1);
        }
        
        [ArgZero("edit", typeof(ShellProvider))]
        public void Edit()
        {
            ShellProvider provider = Construct();
            provider?.Edit(StandardOut, StandardError);
            Exit(provider != null ? 0 : 1);
        }
    }
}