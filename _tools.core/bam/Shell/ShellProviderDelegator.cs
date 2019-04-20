using System;
using System.Collections.Generic;
using Bam.Shell;
using Bam.Net;
using Bam.Net.Testing;

namespace Bam.Shell
{
    public class ShellProviderDelegator : ArgZeroDelegator<ShellProvider>
    {
        [ArgZero("list")]
        public void List()
        {
            ShellProvider provider = Construct();
            provider?.List(StandardOut, StandardError);
            Exit(provider != null ? 0: 1);
        }
        
        [ArgZero("add")]
        public void Add()
        {
            ShellProvider provider = Construct();
            provider?.Add(StandardOut, StandardError);
            Exit(provider != null ? 0: 1);
        }

        [ArgZero("show")]
        public void Show()
        {
            ShellProvider provider = Construct();
            provider?.Show(StandardOut, StandardError);
            Exit(provider != null ? 0: 1);
        }

        [ArgZero("remove")]
        public void Remove()
        {
            ShellProvider provider = Construct();
            provider?.Remove(StandardOut, StandardError);
            Exit(provider != null ? 0: 1);
        }
        
        [ArgZero("run")]
        public void Run()
        {
            ShellProvider provider = Construct();
            provider?.Run(StandardOut, StandardError);
            Exit(provider != null ? 0: 1);
        }
        
        [ArgZero("edit")]
        public void Edit()
        {
            ShellProvider provider = Construct();
            provider?.Edit(StandardOut, StandardError);
            Exit(provider != null ? 0 : 1);
        }
    }
}