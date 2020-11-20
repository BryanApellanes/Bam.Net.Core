using System;
using Bam.Net.Logging;

namespace Bam.Net.CommandLine
{
    public class Message
    {
        public static void Log(params ConsoleMessage[] messages)
        {
            ConsoleMessage.Log(messages);
        }
        
        public static void LogLine(string messageSignature, params object[] messageSignatureArgs)
        {
            Log($"{messageSignature}\r\n", messageSignatureArgs);
        }
        
        public static void Log(string messageSignature, params object[] messageSignatureArgs)
        {
            ConsoleMessage.Log(messageSignature, messageSignatureArgs);
        }

        public static void LogLine(ILogger logger, string messageSignature, params object[] messageSignatureArgs)
        {
            Log(logger, $"{messageSignature}\r\n", messageSignatureArgs);
        }
        
        public static void Log(ILogger logger, string messageSignature, params object[] messageSignatureArgs)
        {
            ConsoleMessage.Log(logger, messageSignature, messageSignatureArgs);
        }

        public static void LogLine(string messageSignature, ConsoleColor textColor, params object[] messageSignatureArgs)
        {
            Log($"{messageSignature}", textColor, messageSignatureArgs);
        }
        
        public static void Log(string messageSignature, ConsoleColor textColor, params object[] messageSignatureArgs)
        {
            ConsoleMessage.Log(Logging.Log.Default, messageSignature, textColor, messageSignatureArgs);
        }

        public static void Log(string messageSignature, ConsoleColorCombo colors, params object[] messageSignatureArgs)
        {
            ConsoleMessage.Log(messageSignature, colors, messageSignatureArgs);
        }

        public static void LogLine(ILogger logger, string messageSignature, ConsoleColor textColor, params object[] messageSignatureArgs)
        {
            Log(logger, $"{messageSignature}\r\n", textColor, messageSignatureArgs);
        }
        
        public static void Log(ILogger logger, string messageSignature, ConsoleColor textColor, params object[] messageSignatureArgs)
        {
            ConsoleMessage.Log(logger, messageSignature, textColor, messageSignatureArgs);
        }

        public static void PrintLine()
        {
            PrintLine("");
        }

        public static void Print(object instance)
        {
            PrintLine(instance.ToJson(true));
        }
        
        public static void PrintLine(string messageSignature, params object[] messageSignatureArgs)
        {
            Print($"{messageSignature}\r\n", messageSignatureArgs);
        }
        
        public static void Print(string messageSignature, params object[] messageSignatureArgs)
        {
            ConsoleMessage.Print(messageSignature, messageSignatureArgs);
        }
        
        public static void PrintLine(string messageSignature, ConsoleColor textColor, params object[] messageSignatureArgs)
        {
            Print($"{messageSignature}\r\n", textColor, messageSignatureArgs);
        }

        public static void PrintLine(string messageSignature, ConsoleColorCombo colors, params object[] messageSignatureArgs)
        {
            Print($"{messageSignature}\r\n", colors, messageSignatureArgs);
        }
        
        public static void Print(string messageSignature, ConsoleColorCombo colors, params object[] messageSignatureArgs)
        {
            ConsoleMessage.Print(messageSignature, colors, messageSignatureArgs);
        }
        
        public static void Print(string messageSignature, ConsoleColor textColor, params object[] messageSignatureArgs)
        {
            ConsoleMessage.Print(messageSignature, textColor, messageSignatureArgs);
        }
    }
}