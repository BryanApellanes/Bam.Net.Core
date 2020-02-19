using System;
using System.Collections.Generic;
using System.Text;

namespace Bam.Net.Services
{
    /// <summary>
    /// Used to adorn a class that is used as an application service.
    /// </summary>
    public class AppServiceAttribute: Attribute
    {
        /// <summary>
        /// Gets or sets the name of the application the adorned class 
        /// is a service for.  May be null or blank.
        /// </summary>
        public string ApplicationName { get; set; }
    }
}
