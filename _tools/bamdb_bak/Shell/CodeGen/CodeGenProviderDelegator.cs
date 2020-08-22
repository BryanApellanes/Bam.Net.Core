namespace Bam.Shell.CodeGen
{
    public class CodeGenProviderDelegator : ArgZeroDelegator<CodeGenProvider>
    {
        [ArgZero("generate")]
        public void Generate()
        {
            CodeGenProvider provider = Construct();
            provider?.Generate(StandardOut, StandardError);
            Exit(provider != null ? 0 : 1);
        }
    }
}