using System;
using System.Collections.Generic;
using Bam.Net;
using Bam.Net.Testing;

namespace Bam.Shell
{
    public class ArgZeroDelegator : CommandLineTestInterface
    {   
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

        public static string[] Arguments { get; set; }
        
        [ArgZero("list")]
        public void List()
        {
            string typeName = GetTypeName();
            ShellProvider provider = GetType(typeName)?.Construct<ShellProvider>();
            provider?.List(o => OutLine(o), e => OutLine(e));
            Exit(provider != null ? 0: 1);
        }
        
        [ArgZero("add")]
        public void Add()
        {
            string typeName = GetTypeName();
            ShellProvider provider = GetType(typeName)?.Construct<ShellProvider>();
            provider?.Add(o => OutLine(o), e => OutLine(e));
            Exit(provider != null ? 0: 1);
        }

        [ArgZero("show")]
        public void Show()
        {
            string typeName = GetTypeName();
            ShellProvider provider = GetType(typeName)?.Construct<ShellProvider>();
            provider?.Show(o => OutLine(o), e => OutLine(e));
            Exit(provider != null ? 0: 1);
        }

        [ArgZero("set")]
        public void Set()
        {
            string typeName = GetTypeName();
            ShellProvider provider = GetType(typeName)?.Construct<ShellProvider>();
            provider?.Set(o => OutLine(o), e => OutLine(e));
            Exit(provider != null ? 0: 1);
        }

        [ArgZero("remove")]
        public void Remove()
        {
            string typeName = GetTypeName();
            ShellProvider provider = GetType(typeName)?.Construct<ShellProvider>();
            provider?.Remove(o => OutLine(o), e => OutLine(e));
            Exit(provider != null ? 0: 1);
        }
        
        [ArgZero("run")]
        public void Run()
        {
            string typeName = GetTypeName();
            ShellProvider provider = GetType(typeName)?.Construct<ShellProvider>();
            provider?.Run(o => OutLine(o), e => OutLine(e));
            Exit(provider != null ? 0: 1);
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
        
        private string GetTypeName()
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