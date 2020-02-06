using System;

namespace Bam.Net.Server
{
    [AttributeUsage(AttributeTargets.Class)]
    public class AppAttribute : Attribute
    {
        /// <summary>
        /// The name of the application that the component applies to.
        /// </summary>
        public string Name { get; set; }
    }
}