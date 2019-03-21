using System;

namespace Bam.Shell
{
    [AttributeUsage(AttributeTargets.Method)]
    public class ArgZeroAttribute : Attribute
    {
        public ArgZeroAttribute(string argument)
        {
            Argument = argument;
        }
        
        public string Argument { get; set; }
    }
}