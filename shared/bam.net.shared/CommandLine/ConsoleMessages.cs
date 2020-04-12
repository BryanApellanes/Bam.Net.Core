using System;
using System.Collections.Generic;

namespace Bam.Net.CommandLine
{
    public static class ConsoleMessages
    {
        public static List<ConsoleMessage> Print(this string message, params object[] args)
        {
            return Print(message, ConsoleColor.Cyan, args);
        }
        
        public static List<ConsoleMessage> Print(this string message, ConsoleColor textColor, params object[] args)
        {
            List<ConsoleMessage> messages = new List<ConsoleMessage>();
            return Add(messages, message, textColor, args);
        }
        
        public static List<ConsoleMessage> Add(this List<ConsoleMessage> list, string message, params object[] args)
        {
            return Add(list, message, ConsoleColor.Cyan, args);
        }
        
        public static List<ConsoleMessage> Add(this List<ConsoleMessage> list, string message, ConsoleColor textColor, params object[] args)
        {
            list.Add(new ConsoleMessage(message, textColor, args));
            return list;
        }

        public static void Print(this List<ConsoleMessage> list)
        {
            ConsoleMessage.Print(list);
        }
    }
}