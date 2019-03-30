using System;

namespace Bam.Shell
{
    [AttributeUsage(AttributeTargets.Method)]
    public class ArgZeroAttribute : Attribute
    {
        public ArgZeroAttribute(string argument, string description = null)
        {
            Argument = argument;
            Description = description;
        }

        public string Argument { get; set; }
        public string Description { get; set; }
    }
}