using System;
using Bam.Net.Testing;
using Bam.Shell;

namespace Bam.Shell.Data
{
    public abstract class DbShellProvider : CommandLineTestInterface, IRegisterArguments
    {
        public abstract void New(Action<string> output = null, Action<string> error = null);
        public abstract void Get(Action<string> output = null, Action<string> error = null);
        public abstract void Set(Action<string> output = null, Action<string> error = null);
        public abstract void Del(Action<string> output = null, Action<string> error = null);
        public abstract void Find(Action<string> output = null, Action<string> error = null);
        
        public virtual void RegisterArguments()
        {
        }
    }
}