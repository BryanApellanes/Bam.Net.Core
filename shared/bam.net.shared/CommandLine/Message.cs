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
        
        public static void LogLine(string messageSignature, params object[] messageArgs)
        {
            Log($"{messageSignature}\r\n", messageArgs);
        }
        
        public static void Log(string messageSignature, params object[] messageArgs)
        {
            ConsoleMessage.Log(messageSignature, messageArgs);
        }

        public static void LogLine(ILogger logger, string messageSignature, params object[] messageArgs)
        {
            Log(logger, $"{messageSignature}\r\n", messageArgs);
        }
        
        public static void Log(ILogger logger, string messageSignature, params object[] messageArgs)
        {
            ConsoleMessage.Log(logger, messageSignature, messageArgs);
        }

        public static void LogLine(string messageSignature, ConsoleColor textColor, params object[] messageArgs)
        {
            Log($"{messageSignature}", textColor, messageArgs);
        }
        
        public static void Log(string messageSignature, ConsoleColor textColor, params object[] messageArgs)
        {
            ConsoleMessage.Log(Logging.Log.Default, messageSignature, textColor, messageArgs);
        }

        public static void LogLine(ILogger logger, string messageSignature, ConsoleColor textColor, params object[] messageArgs)
        {
            Log(logger, $"{messageSignature}\r\n", textColor, messageArgs);
        }
        
        public static void Log(ILogger logger, string messageSignature, ConsoleColor textColor, params object[] messageArgs)
        {
            ConsoleMessage.Log(logger, messageSignature, textColor, messageArgs);
        }

        public static void PrintLine()
        {
            PrintLine("");
        }
        
        public static void PrintLine(string messageSignature, params object[] messageArgs)
        {
            Print($"{messageSignature}\r\n", messageArgs);
        }
        
        public static void Print(string messageSignature, params object[] messageArgs)
        {
            ConsoleMessage.Print(messageSignature, messageArgs);
        }
        
        // TODO: Add signature PrintLine(string, ConsoleColorCombo, params object[])
        public static void PrintLine(string messageSignature, ConsoleColor textColor, params object[] messageArgs)
        {
            Print($"{messageSignature}\r\n", textColor, messageArgs);
        }
        
        // TODO: Add signature Print(string, ConsoleColorCombo, params object[])
        public static void Print(string messageSignature, ConsoleColor textColor, params object[] messageArgs)
        {
            ConsoleMessage.Print(messageSignature, textColor, messageArgs);
        }
    }
}