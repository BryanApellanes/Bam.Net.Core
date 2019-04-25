namespace Bam.Shell.CodeGen
{
    public class CodeGenProviderDelegator : ArgZeroDelegator<CodeGenProvider>
    {
        [ArgZero("gen")]
        public void Gen()
        {
            CodeGenProvider provider = Construct();
            provider?.Gen(StandardOut, StandardError);
            Exit(provider != null ? 0 : 1);
        }
    }
}