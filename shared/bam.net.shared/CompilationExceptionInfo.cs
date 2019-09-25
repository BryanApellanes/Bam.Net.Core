namespace Bam.Net
{
    public class CompilationExceptionInfo
    {
        public CompilationExceptionInfo()
        {
            ProcessInfo = new ProcessInfo();
        }

        public CompilationExceptionInfo(RoslynCompilationException ex): this()
        {
            Message = ex.Message;
        }
        
        public ProcessInfo ProcessInfo { get; set; }
        public string Message { get; set; }
    }
}