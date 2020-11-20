using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Bam.Net.Logging;

namespace Bam.Net.CommandLine
{
    public class ConsoleMessage
    {
        public ConsoleMessage()
        {
        }

        public ConsoleMessage(string message)
        {
            Text = message;
        }
        
        public ConsoleMessage(string message, ConsoleColorCombo colors, params object[] messageSignatureArgs)
        {
            Colors = colors;
            SetText(message, messageSignatureArgs);
        }

        public ConsoleMessage(string message, ConsoleColor textColor, ConsoleColor background = ConsoleColor.Black) : this(message, new ConsoleColorCombo(textColor, background))
        {
        }

        public ConsoleMessage(string messageSignature, ConsoleColor textColor, params object[] messageSignatureArgs) : this(messageSignature, new ConsoleColorCombo(textColor, ConsoleColor.Black))
        {
            SetText(messageSignature, messageSignatureArgs);
        }

        private string _text;

        public string Text
        {
            get => GetText();
            private set => _text = value;
        }

        public void SetText(string messageSignature, params object[] messageSignatureArgs)
        {
            _text = null;
            MessageSignature = messageSignature;
            MessageArgs = messageSignatureArgs;
        }

        public ConsoleColor ForegroundColor
        {
            get => Colors.ForegroundColor;
            set => Colors = new ConsoleColorCombo(value);
        }

        public ConsoleColor BackgroundColor
        {
            get => Colors.BackgroundColor;
            set => Colors = new ConsoleColorCombo(ForegroundColor, value);
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

        public static void Log(string messageSignature, ConsoleColorCombo colors, params object[] messageSignatureArgs)
        {
            Log(Logging.Log.Default, messageSignature, colors, messageSignatureArgs);
        }
        
        public static void Log(ILogger logger, string messageSignature, ConsoleColorCombo colors, params object[] messageSignatureArgs)
        {
            Log(logger, new ConsoleMessage(messageSignature, colors, messageSignatureArgs));
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
        
        static readonly HashSet<ConsoleColor> _errorBackgrounds = new HashSet<ConsoleColor>(new ConsoleColor[]{ConsoleColor.Red, ConsoleColor.DarkRed, ConsoleColor.Magenta, ConsoleColor.DarkMagenta});
        static readonly HashSet<ConsoleColor> _warningBackgrounds = new HashSet<ConsoleColor>(new ConsoleColor[]{ConsoleColor.Yellow, ConsoleColor.DarkYellow});
        public static void Log(ILogger logger, ConsoleMessage consoleMessage)
        {
            Print(consoleMessage);
            Task.Run(() =>
            {
                if (_errorBackgrounds.Contains(consoleMessage.Colors.BackgroundColor))
                {
                    logger.Error(consoleMessage.MessageSignature ?? consoleMessage.Text, consoleMessage.MessageArgs ?? new object[]{});
                    return;
                }

                if (_warningBackgrounds.Contains(consoleMessage.Colors.BackgroundColor))
                {
                    logger.Warning(consoleMessage.MessageSignature ?? consoleMessage.Text, consoleMessage.MessageArgs ?? new object[]{});
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
            if (messageArgs != null && messageArgs.Length == 0)
            {
                PrintMessage(messageSignature, ConsoleColor.Cyan);
            }
            else
            {
                Print(messageSignature, ConsoleColor.Cyan, messageArgs);
            }
        }
        
        public static void Print(string messageSignature, ConsoleColorCombo colors, params object[] messageArgs)
        {
            Print(new ConsoleMessage(messageSignature, colors, messageArgs));
        }
        
        public static void Print(string messageSignature, ConsoleColor textColor, params object[] messageArgs)
        {
            if (messageArgs == null || messageArgs.Length == 0)
            {
                PrintMessage(new ConsoleMessage(messageSignature) {ForegroundColor = textColor});
            }
            else
            {
                Print(new ConsoleMessage(messageSignature, textColor, messageArgs));
            }
        }
        
        public static void Print(List<ConsoleMessage> messages)
        {
            Print(messages.ToArray());
        }

        private static IConsoleMessageHandler _consoleMessageHandler;
        private static readonly object _consoleMessageHandlerLock = new object();
        public static IConsoleMessageHandler ConsoleMessageHandler
        {
            get
            {
                return _consoleMessageHandlerLock.DoubleCheckLock(ref _consoleMessageHandler, () => new DefaultConsoleMessageHandler());
            }
            set => _consoleMessageHandler = value;
        }
        
        private static ConsoleMessageDelegate _printProvider;
        private static readonly object _printProviderLock = new object();
        public static ConsoleMessageDelegate PrintProvider
        {
            get
            {
                return _printProviderLock.DoubleCheckLock(ref _printProvider, () => ConsoleMessageHandler.Print);
            }
            set => _printProvider = value;
        } 
        
        public static void Print(params ConsoleMessage[] messages)
        {
            PrintProvider(messages);
        }

        private static void PrintMessage(string message, ConsoleColor foregroundColor, ConsoleColor backgroundColor = ConsoleColor.Black)
        {
            DefaultConsoleMessageHandler.PrintMessage(message, foregroundColor, backgroundColor);
        }
        
        private static void PrintMessage(ConsoleMessage message)
        {
            DefaultConsoleMessageHandler.PrintMessage(message);
        }
    }
}
