using System;
using Bam.Net;
using Bam.Net.Testing;
using Bam.Shell;

namespace Bam.Shell.Data
{
    public abstract class DataShellProvider : CommandLineTool, IRegisterArguments
    {
        public abstract void New(Action<string> output = null, Action<string> error = null);
        public abstract void Get(Action<string> output = null, Action<string> error = null);
        public abstract void Set(Action<string> output = null, Action<string> error = null);
        public abstract void Del(Action<string> output = null, Action<string> error = null);
        public abstract void Find(Action<string> output = null, Action<string> error = null);
        
        public string[] RawArguments { get; private set; }
        public virtual void RegisterArguments(string[] args)
        {
            RawArguments = args;
        }
    }
}