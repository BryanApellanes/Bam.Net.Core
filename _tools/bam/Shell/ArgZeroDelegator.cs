using System;
using System.Collections.Generic;
using Bam.Net;
using Bam.Net.Testing;
using Bam.Shell;

namespace Bam.Shell
{
    public class ArgZeroDelegator<T> : ArgZeroDelegator
    {
        public virtual T Construct()
        {
            string typeName = GetTypeName();
            Type type = GetType(typeName);
            if (type != null)
            {
                return type.Construct<T>();
            }

            return default(T);
        }
    }
    
    public class ArgZeroDelegator: CommandLineTestInterface
    {   
        public static string[] CommandLineArguments { get; set; }
        
        public Type GetType(string typeName)
        {
            if (!ArgZero.ProviderTypes.ContainsKey(typeName))
            {
                typeName = typeName.PascalCase();
            }

            if (ArgZero.ProviderTypes.ContainsKey(typeName))
            {
                return ArgZero.ProviderTypes[typeName];
            }

            return null;
        }
        
        protected string GetTypeName()
        {
            if (CommandLineArguments.Length >= 2)
            {
                string typeName = CommandLineArguments[1];
                if (typeName.EndsWith("s"))
                {
                    typeName = typeName.Truncate(1);
                }

                return typeName;
            }

            return string.Empty;
        }

        protected void StandardOut(string output)
        {
            OutLine(output, ConsoleColor.Cyan);
        }

        protected void StandardError(string output)
        {
            OutLine(output, ConsoleColor.DarkYellow); // would make this red or magenta but git outputs a bunch of useful non error stuff to error out
        }
    }
}