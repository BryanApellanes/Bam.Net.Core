using System;
using Bam.Net.Testing;

namespace Bam.Shell.CodeGen
{
    public abstract class CodeGenProvider : CommandLineTestInterface, IRegisterArguments
    {
        public abstract void Gen(Action<string> output = null, Action<string> error = null);
        
        public virtual void RegisterArguments()
        {
        }
    }
}