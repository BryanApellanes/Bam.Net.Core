using System;
using Bam.Net;
using Bam.Net.Testing;

namespace Bam.Shell.CodeGen
{
    public abstract class CodeGenProvider : CommandLineTool, IRegisterArguments
    {
        public const string AppDataFolderName = "AppData";
        public const string GenerationOutputFolderName = "_gen";
        
        public abstract void Generate(Action<string> output = null, Action<string> error = null);
        
        public string[] RawArguments { get; private set; }
        public virtual void RegisterArguments(string[] args)
        {
            RawArguments = args;
        }
    }
}