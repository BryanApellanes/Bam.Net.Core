using System;
using System.Collections.Generic;
using Bam.Net;
using Bam.Net.Testing;
using Bam.Shell;

namespace bam.Shell
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
        public static string[] Arguments { get; set; }
        public static void Register(string[] args)
        {
            Arguments = args;
            HashSet<string> exclude = new HashSet<string> {"External"};

            foreach (string typeName in ArgZero.ProviderTypes.Keys)
            {
                if (!exclude.Contains(typeName))
                {
                    AddValidArgument(typeName, $"Add a new {typeName}");
                }
            }
        }
        
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
            if (Arguments.Length >= 2)
            {
                string typeName = Arguments[1];
                if (typeName.EndsWith("s"))
                {
                    typeName = typeName.Truncate(1);
                }

                return typeName;
            }

            return string.Empty;
        }
    }
}