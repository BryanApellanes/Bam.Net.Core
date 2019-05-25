using System;
using Bam.Net.Testing;

namespace Bam.Shell.Conf
{
    public abstract class ConfProvider: CommandLineTestInterface, IRegisterArguments
    {
        public string[] RawArguments { get; private set; }

        public virtual void RegisterArguments(string[] args)
        {
            RawArguments = args;
        }


        public abstract void Get(Action<string> output = null, Action<string> error = null);
        public abstract void Set(Action<string> output = null, Action<string> error = null);
        public abstract void Print(Action<string> output = null, Action<string> error = null);
    }
}