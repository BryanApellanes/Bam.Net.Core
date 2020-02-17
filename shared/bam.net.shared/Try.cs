using System;

namespace Bam.Net
{
    public static class Try
    {
        public static void Execute(Action action, Action<Exception> exceptionHandler = null)
        {
            Exec.Try(action, exceptionHandler);
        }
    }
}