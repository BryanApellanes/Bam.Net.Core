using System;

namespace Bam.Shell.CodeGen
{
    public class ExtractionProvider : CodeGenProvider
    {
        public override void RegisterArguments(string[] args)
        {
            base.RegisterArguments(args);
            
        }

        public override void Gen(Action<string> output = null, Action<string> error = null)
        {
            OutLine("Extraction!!", ConsoleColor.Cyan);
        }
    }
}