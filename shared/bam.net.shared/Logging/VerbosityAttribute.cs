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

        public VerbosityLevel Value { get; set; }

		/// <summary>
		/// The "NamedFormat" message format to use when outputting messages.  The sender argument
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
