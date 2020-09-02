using System;
using Bam.Net.CommandLine;

namespace Bam.Net
{
    public class DefaultConsoleMessageHandler : IConsoleMessageHandler
    {
        public void Log(params ConsoleMessage[] consoleMessages)
        {
            ConsoleMessage.Log(consoleMessages);
        }

        public void Print(params ConsoleMessage[] messages)
        {
            if (messages != null)
            {
                foreach (ConsoleMessage message in messages)
                {
                    PrintMessage(message);
                }
            }
        }
        
        public static void PrintMessage(string message, ConsoleColor foregroundColor, ConsoleColor backgroundColor = ConsoleColor.Black)
        {
            Console.ForegroundColor = foregroundColor;
            Console.BackgroundColor = backgroundColor;
            Console.WriteLine(message);
            Console.ResetColor();
        }
        
        public static void PrintMessage(ConsoleMessage message)
        {
            Console.ForegroundColor = message.Colors.ForegroundColor;
            Console.BackgroundColor = message.Colors.BackgroundColor;
            Console.Write(message.Text);
            Console.ResetColor();
        }
    }
}