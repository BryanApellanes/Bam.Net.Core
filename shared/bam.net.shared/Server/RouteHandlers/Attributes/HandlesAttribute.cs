
using System;

namespace Bam.Net.Server
{
    /// <summary>
    /// Attribute used to adorn a class as a route handler that handles the specified name as path prefix.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class)]
    public class HandlesAttribute: Attribute
    {
        public HandlesAttribute(string name)
        {
            Name = name;
        }
        public string Name { get; set; }
    }
}