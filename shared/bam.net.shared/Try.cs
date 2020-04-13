using System;

namespace Bam.Net
{
    public static class Try
    {
        public static T Execute<T>(Func<T> func, Action<Exception> exceptionHandler = null)
        {
            return To<T>(func, exceptionHandler);
        }
        
        public static T To<T>(Func<T> func, Action<Exception> exceptionHandler = null)
        {
            return Exec.Try(func, exceptionHandler);
        }
        
        public static void Execute(Action action, Action<Exception> exceptionHandler = null)
        {
            To(action, exceptionHandler);
        }
        
        public static void To(Action action, Action<Exception> exceptionHandler = null)
        {
            Exec.Try(action, exceptionHandler);
        }
    }
}