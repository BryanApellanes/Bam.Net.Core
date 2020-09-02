using Bam.Net.CommandLine;

namespace Bam.Net
{
    public interface IConsoleMessageHandler
    {
        void Log(params ConsoleMessage[] consoleMessages);
        void Print(params ConsoleMessage[] messages);
    }
}