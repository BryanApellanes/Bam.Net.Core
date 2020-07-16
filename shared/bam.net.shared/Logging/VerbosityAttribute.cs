/*
	Copyright © Bryan Apellanes 2015  
*/
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bam.Net.Logging
{
    [AttributeUsage(AttributeTargets.Event)]
    public class VerbosityAttribute: Attribute
    {
		public VerbosityAttribute(VerbosityLevel eventType)
        {
            this.Value = eventType;
        }

		public VerbosityAttribute(LogEventType eventType)
		{
			this.Value = (VerbosityLevel)eventType;
		}

        public VerbosityLevel Value { get; private set; }

        public string MessageFormat
        {
            get => SenderMessageFormat ?? EventArgsMessageFormat;
            set
            {
                SenderMessageFormat = value;
                EventArgsMessageFormat = value;
            }
        }

		/// <summary>
		/// The "NamedFormat" message format to use when outputting messages; named variables are the names of properties
		/// on the current Loggable instance and should be enclosed in curly braces {}.  The sender argument
		/// to the registered event handler is used to resolve message variables.
		/// </summary>
        public string SenderMessageFormat { get; set; }
		
		/// <summary>
		/// The "NamedFormat" message to use when outputting messages.  The EventArgs argument
		/// to the registered event handler is used to resolve message variables.
		/// </summary>
        public string EventArgsMessageFormat { get; set; }

        public bool TryGetSenderMessage(object value, out string message)
        {
            if (string.IsNullOrEmpty(SenderMessageFormat))
            {
                message = string.Empty;
                return false;
            }
            return TryGetMessage(value, SenderMessageFormat, out message);
        }

        public bool TryGetEventArgsMessage(EventArgs args, out string message)
        {
            if (string.IsNullOrEmpty(EventArgsMessageFormat))
            {
                message = string.Empty;
                return false;
            }
            return TryGetMessage(args, EventArgsMessageFormat, out message);
        }

        public string GetMessage(object sender, EventArgs args)
        {
            TryGetSenderMessage(sender, out string senderMessage);
            TryGetEventArgsMessage(args, out string eventArgsMessage);
            if (!string.IsNullOrEmpty(senderMessage) && !senderMessage.Equals(MessageFormat))
            {
                return senderMessage;
            }

            if (!string.IsNullOrEmpty(eventArgsMessage) && !eventArgsMessage.Equals(MessageFormat))
            {
                return eventArgsMessage;
            }

            return string.Empty;
        }
        
        public bool TryGetMessage(object value, string format, out string message)
        {
            try
            {
                if (!string.IsNullOrEmpty(format))
                {
                    message = format.NamedFormat(value);
                }
                else
                {
                    message = value.TryPropertiesToString();
                }

                return true;
            }
            catch (Exception ex)
            {
                message = ex.Message;
                return false;
            }
        }
    }
}
