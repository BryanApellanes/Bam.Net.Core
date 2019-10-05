using System;
using System.Reflection;

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

        public ArgZeroAttribute(string argument, Type providerBaseType) : this(argument)
        {
            BaseType = providerBaseType;
        }
        
        /// <summary>
        /// The base type of the provider.  Extenders of this type are registered as
        /// delegate providers.
        /// </summary>
        public Type BaseType { get; set; }
        public string Argument { get; set; }
        public string Description { get; set; }
    }
}