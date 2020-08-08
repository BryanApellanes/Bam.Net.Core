using System;
using Bam.Net;
using Bam.Net.Testing;

namespace Bam.Shell.Pack
{
    public abstract class PackageProvider: CommandLineTool, IRegisterArguments
    {
        public string[] RawArguments { get; private set; }

        public virtual void RegisterArguments(string[] args)
        {
            RawArguments = args;
        }


        public abstract void Build(Action<string> output = null, Action<string> error = null);
        public abstract void Pack(Action<string> output = null, Action<string> error = null);
        public abstract void Push(Action<string> output = null, Action<string> error = null);
        public abstract void Pull(Action<string> output = null, Action<string> error = null);        
    }
}