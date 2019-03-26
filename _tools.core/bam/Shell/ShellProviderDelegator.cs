using System;
using System.Collections.Generic;
using bam.Shell;
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
            provider?.List(o => OutLine(o), e => OutLine(e));
            Exit(provider != null ? 0: 1);
        }
        
        [ArgZero("add")]
        public void Add()
        {
            ShellProvider provider = Construct();
            provider?.Add(o => OutLine(o), e => OutLine(e));
            Exit(provider != null ? 0: 1);
        }

        [ArgZero("show")]
        public void Show()
        {
            ShellProvider provider = Construct();
            provider?.Show(o => OutLine(o), e => OutLine(e));
            Exit(provider != null ? 0: 1);
        }

        [ArgZero("remove")]
        public void Remove()
        {
            ShellProvider provider = Construct();
            provider?.Remove(o => OutLine(o), e => OutLine(e));
            Exit(provider != null ? 0: 1);
        }
        
        [ArgZero("run")]
        public void Run()
        {
            ShellProvider provider = Construct();
            provider?.Run(o => OutLine(o), e => OutLine(e));
            Exit(provider != null ? 0: 1);
        }

        [ArgZero("pack")]
        public void Pack()
        {
            ShellProvider provider = Construct();
            provider?.Pack(o => OutLine(o), e => OutLine(e));
            Exit(provider != null ? 0: 1);
        }
        
        [ArgZero("edit")]
        public void Edit()
        {
            ShellProvider provider = Construct();
            provider?.Edit(o => OutLine(o), e => OutLine(e));
            Exit(provider != null ? 0 : 1);
        }
    }
}