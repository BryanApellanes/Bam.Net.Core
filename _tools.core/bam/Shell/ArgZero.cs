using System;
using System.Collections.Generic;
using System.Reflection;
using Bam.Net;
using Bam.Net.CommandLine;
using Bam.Net.Data.Dynamic;
using Bam.Net.Testing;
using CsQuery.ExtensionMethods;

namespace Bam.Shell
{
    /// <summary>
    /// A class representing the first argument to the entry assembly.  Exists primarily to allow
    /// processing of arguments without a prefix and to route execution to a method specified on
    /// the command line.
    /// </summary>
    public class ArgZero : CommandLineTestInterface
    {
        static object _targetsLock = new object();
        static Dictionary<string, MethodInfo> _targets; 
        public static Dictionary<string, MethodInfo> Targets
        {
            get
            {
                return _targetsLock.DoubleCheckLock(ref _targets,
                    () => new Dictionary<string, MethodInfo>());
            }
        }

        public static void Register(string arg, MethodInfo method)
        {
            if (!Targets.AddMissing(arg, method))
            {
                Args.Throw<InvalidOperationException>("The specified ArgZero is already registered: {0}", arg);
            }
        }

        /// <summary>
        /// Scan the current assembly for ArgZero methods.
        /// </summary>
        public static void Scan()
        {
            Assembly current = Assembly.GetExecutingAssembly();
            current.GetTypes().ForEach(t =>
            {
                t.GetMethods().ForEach(m =>
                {
                    if (m.HasCustomAttributeOfType<ArgZeroAttribute>(out ArgZeroAttribute arg))
                    {
                        Register(arg.Argument, m);
                    }
                });
            });
        }

        /// <summary>
        /// Execute any ArgZero arguments specified on the command line then exit.  Has no effect if no relevant arguments
        /// are detected.
        /// </summary>
        public static void ExecuteArgZero(string[] arguments)
        {
            if (arguments.Length == 0)
            {
                return;
            }
            
            Scan();
            if (Targets.ContainsKey(arguments[0]))
            {
                List<string> targetArguments = new List<string>();
                List<ArgumentInfo> argumentInfos = new List<ArgumentInfo>();
                arguments.Rest(1, (val) =>
                {
                    targetArguments.Add(val);
                    if (val.StartsWith("/"))
                    {
                        string argName = val.TruncateFront(1).ReadUntil(':', out string argVal);
                        argumentInfos.Add(new ArgumentInfo(argName, true));
                    }
                });
                Arguments = new ParsedArguments(targetArguments.ToArray(), argumentInfos.ToArray());
                MethodInfo method = Targets[arguments[0]];
                object instance = null;
                if (!method.IsStatic)
                {
                    instance = method.DeclaringType.Construct();
                }

                try
                {
                    method.Invoke(instance, null);
                }
                catch (Exception ex)
                {
                    OutLineFormat("Exception executing ArgZero: {0}", ConsoleColor.Magenta, ex.Message);
                }
                Exit(0);
            }
        }
    }
}