using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Bam.Net.Logging;

namespace Bam.Net.CommandLine
{
    public class ConsoleMessage
    {
        public ConsoleMessage()
        {
        }

        public ConsoleMessage(string msg)
        {
            Text = msg;
        }

        public ConsoleMessage(string msg, ConsoleColorCombo colors): this(msg)
        {
            Colors = colors;
        }

        public ConsoleMessage(string msg, ConsoleColor textColor, ConsoleColor background = ConsoleColor.Black) : this(msg, new ConsoleColorCombo(textColor, background))
        {
        }

        public ConsoleMessage(string messageSignature, ConsoleColor textColor, params object[] msgArgs) : this(messageSignature, new ConsoleColorCombo(textColor, ConsoleColor.Black))
        {
            MessageSignature = messageSignature;
            MessageArgs = msgArgs;
        }

        private string _text;

        public string Text
        {
            get => GetText();
            set => _text = value;
        }

        public void SetText(string messageSignature, params object[] messageSignatureArgs)
        {
            _text = null;
            MessageSignature = messageSignature;
            MessageArgs = messageSignatureArgs;
        }
        
        public ConsoleColorCombo Colors { get; set; }
        
        protected string MessageSignature { get; set; }
        
        protected object[] MessageArgs { get; set; }
        protected string GetText()
        {
            if (!string.IsNullOrEmpty(_text))
            {
                return _text;
            }

            try
            {
                _text = string.Format(MessageSignature, MessageArgs);
            }
            catch (Exception ex)
            {
                _text = ex.Message;
            }

            return _text;
        }

        public void Log()
        {
            Log(Logging.Log.Default, this);
        }

        public void Log(ILogger logger)
        {
            Log(logger, this);
        }
        
        public static void Log(string message, params object[] messageArgs)
        {
            Log(Logging.Log.Default, message, messageArgs);
        }

        public static void Log(ILogger logger, string message, params object[] messageArgs)
        {
            Log(logger, message, ConsoleColor.Cyan, messageArgs);
        }

        public static void Log(ILogger logger, string messageSignature, ConsoleColor textColor, params object[] messageArgs)
        {
            Log(logger, new ConsoleMessage(messageSignature, textColor, messageArgs));
        }

        public static void Log(params ConsoleMessage[] consoleMessages)
        {
            foreach (ConsoleMessage consoleMessage in consoleMessages)
            {
                Log(Logging.Log.Default, consoleMessage);
            }
        }

        public static void Log(ILogger logger, params ConsoleMessage[] consoleMessages)
        {
            foreach (ConsoleMessage consoleMessage in consoleMessages)
            {
                Log(logger, consoleMessage);
            }
        }
        
        public static void Log(ConsoleMessage consoleMessage)
        {
            Log(Logging.Log.Default, consoleMessage);
        }
        
        public static void Log(ILogger logger, ConsoleMessage consoleMessage)
        {
            Print(consoleMessage);
            Task.Run(() =>
            {
                HashSet<ConsoleColor> errorBackgrounds = new HashSet<ConsoleColor>(new ConsoleColor[]{ConsoleColor.Red, ConsoleColor.DarkRed, ConsoleColor.Magenta, ConsoleColor.DarkMagenta});
                if (errorBackgrounds.Contains(consoleMessage.Colors.BackgroundColor))
                {
                    logger.Error(consoleMessage.MessageSignature ?? consoleMessage.Text, consoleMessage.MessageArgs ?? new object[]{});
                    return;
                }
                
                switch (consoleMessage.Colors.ForegroundColor)
                {    
                    case ConsoleColor.Red:
                    case ConsoleColor.DarkRed:
                    case ConsoleColor.Magenta:
                    case ConsoleColor.DarkMagenta:
                        logger.Error(consoleMessage.MessageSignature ?? consoleMessage.Text, consoleMessage.MessageArgs ?? new object[]{});
                        break;
                    case ConsoleColor.Yellow:
                    case ConsoleColor.DarkYellow:
                        logger.Warning(consoleMessage.MessageSignature ?? consoleMessage.Text, consoleMessage.MessageArgs ?? new object[]{});
                        break;
                    case ConsoleColor.Black:
                    case ConsoleColor.White:
                    case ConsoleColor.DarkBlue:
                    case ConsoleColor.DarkGreen:
                    case ConsoleColor.Gray:
                    case ConsoleColor.DarkGray:
                    case ConsoleColor.Blue:
                    case ConsoleColor.Green:
                    case ConsoleColor.Cyan:
                    case ConsoleColor.DarkCyan:
                    default:
                        logger.Info(consoleMessage.MessageSignature ?? consoleMessage.Text, consoleMessage.MessageArgs ?? new object[]{});
                        break;
                }
            });
        }
        
        public void Print()
        {
            PrintMessage(this);
        }

        public static void Print(string messageSignature, params object[] messageArgs)
        {
            Print(messageSignature, ConsoleColor.Cyan, messageArgs);
        }
        
        // TODO: Add signature Print(string, ConsoleColorCombo, params object[])
        public static void Print(string messageSignature, ConsoleColor textColor, params object[] messageArgs)
        {
            Print(new ConsoleMessage(messageSignature, textColor, messageArgs));
        }
        
        public static void Print(List<ConsoleMessage> messages)
        {
            Print(messages.ToArray());
        }
        
        public static void Print(params ConsoleMessage[] messages)
        {
            if (messages != null)
            {
                foreach (ConsoleMessage message in messages)
                {
                    PrintMessage(message);
                }
            }
        }

        private static void PrintMessage(ConsoleMessage message)
        {
            Console.ForegroundColor = message.Colors.ForegroundColor;
            Console.BackgroundColor = message.Colors.BackgroundColor;
            Console.Write(message.Text);
            Console.ResetColor();
        }
    }
}
