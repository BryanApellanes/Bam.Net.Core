using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Reflection;
using System.Diagnostics;
using Bam.Net.CoreServices;
using Bam.Net.Logging;

namespace Bam.Net
{
    public static class Resolver
    {
        static Resolver()
        {
            AppDomain.CurrentDomain.AssemblyResolve += (sender, args) =>
            {
                try
                {
                    WriteLog($"Resolving assembly {args.Name}");
                    return Assembly.Load(ResolveAssembly(args));
                }
                catch (Exception ex)
                {
                    WriteLog($"Exception resolving assembly ({args?.Name ?? "null"}): {ex.Message}");
                }

                return null;
            };
        }
        
        public static void Register()
        {
            AssemblyResolver = AssemblyResolver ?? ((rea) =>
            {
                WriteLog($"Couldn't resolve assembly: {rea.Name}\r\nRequesting assembly: {rea.RequestingAssembly?.FullName}\r\nRequesting assembly path: {rea.RequestingAssembly?.GetFilePath()}");
                return null;
            });
        }

        public static void Register(AssemblyResolutionStrategy strategy)
        {
            switch (strategy)
            {
                case AssemblyResolutionStrategy.Local:
                    Register();
                    break;
                case AssemblyResolutionStrategy.Heart:
                    Register();
                    // TODO: use CoreClient to call assemblyservice 
                    break;
            }
            // TODO: finish this
            // Resolver.AssemblyResolver
            //
            // resolve assembly passes the assembly.fullname
            // use that to ask the AssemblyService.AssemblyManagementRepository for the 
            // AssemblyDescriptor by fullname
            // then AssemblyService.ResolveAssembly by assemblyDescriptor.Name
            // read the assembly bytes and send them in response Resolver.AssemblyResolver
        }

        private static void RegisterHeart()
        {
            throw new NotImplementedException();
        }

        private static void RegisterPartners()
        {
            throw new NotImplementedException();
        }

        private static void RegisterRecursive()
        {
            throw new NotImplementedException();
        }
        
        private static byte[] ResolveAssembly(ResolveEventArgs rea)
        {
            return AssemblyResolver(rea);
        }
        
        private static Func<ResolveEventArgs, byte[]> AssemblyResolver { get; set; }

        private static byte[] ResolveAssembly(string assemblyName, params IAssemblyResolver[] resolvers)
        {
            foreach (IAssemblyResolver resolver in resolvers)
            {
                Assembly assembly = resolver.ResolveAssembly(assemblyName);
                if (assembly != null)
                {
                    return File.ReadAllBytes(assembly.GetFileInfo().FullName);
                }
            }

            return null;
        }
        
        private static void WriteTrace(string message, Exception ex, bool writeLog = true)
        {
            Trace.WriteLine(ex.Message);
            Trace.WriteLine(message);
            Trace.WriteLine(ex.StackTrace);
            if (writeLog)
            {
                WriteLog(message);
            }
        }

        private static void WriteLog(string message)
        {
            try
            {
                DateTime now = DateTime.UtcNow;
                DateTime local = now.ToLocalTime();
                FileInfo logFile = new FileInfo("./Bam.Net.Resolver.log");
                string line = $"[LocalTime({local.ToString()} ms {local.Millisecond}), UtcTime({now.ToString()} ms {now.Millisecond})]::Bam.Net.Resolver::{message}";
                using (StreamWriter sw = new StreamWriter(logFile.FullName))
                {
                    sw.WriteLine(line);
                }
                Console.WriteLine($"{logFile.FullName}::{line}");
            }
            catch (Exception ex)
            {
                WriteTrace(message, ex, false);
            }
        }
    }

}
