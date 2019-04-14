using System;
using System.Linq;
using System.Reflection;
using Bam.Net;
using Bam.Net.Data;
using Bam.Net.Data.Repositories;
using Bam.Shell.ShellGeneration.Data;

namespace Bam.Shell.ShellGeneration
{
    public class ShellGenerationProvider: ShellProvider
    {
        public ShellGenerationProvider()
        {
            ShellGenerationDatabase = DataProvider.Current.GetAppDatabaseFor(ProcessApplicationNameProvider.Current, this);
            ShellGenerationRepository = new DaoRepository(ShellGenerationDatabase);
            ShellGenerationRepository.AddType<ShellDescriptor>();
        }

        public Config Config
        {
            get
            {
                return Config.Current;
            }
        }

        public Database ShellGenerationDatabase { get; private set; }
        public DaoRepository ShellGenerationRepository { get; private set; }

        public override void RegisterArguments(string[] args)
        {
            base.RegisterArguments(args);
            AddValidArgument("typeName", "When generating a shell provider, the name of the type to wrap.");
        }

        public override void List(Action<string> output = null, Action<string> error = null)
        {
            try
            {
                ShellGenerationRepository.BatchRetrieveAll(typeof(ShellDescriptor), 1000,
                sd => { output(sd.ToCsvLine()); });
            }
            catch (Exception ex)
            {
                OutLineFormat("{0}", ConsoleColor.DarkRed, ex.Message);
                error(ex.Message);
                Exit(1);
            }
        }

        public override void Add(Action<string> output = null, Action<string> error = null)
        {
            try
            {
                string typeName = GetArgument("typeName");
                Type type = null;
                foreach (Assembly assembly in AppDomain.CurrentDomain.GetAssemblies())
                {
                    type = FindType(assembly, typeName);
                    if (type != null)
                    {
                        Message("Found type ({0}) in assembly ({1})", output, typeName, assembly.FullName);
                        break;
                    }
                }

                if (type != null)
                {
                    ShellDescriptor descriptor = new ShellDescriptor(type);
                    ShellGenerationRepository.Save(descriptor);

                    // TODO: Generate provider and delegator
                    
                    throw new NotImplementedException();
                }
            }
            catch (Exception ex)
            {
                OutLineFormat("{0}", ConsoleColor.DarkRed, ex.Message);
                error(ex.Message);
                Exit(1);
            }
        }

        public override void Show(Action<string> output = null, Action<string> error = null)
        {
            throw new NotImplementedException();
        }

        public override void Remove(Action<string> output = null, Action<string> error = null)
        {
            throw new NotImplementedException();
        }

        public override void Run(Action<string> output = null, Action<string> error = null)
        {
            throw new NotImplementedException();
        }

        private Type FindType(Assembly assembly, string namespaceQualifiedTypeName)
        {
            return assembly.GetTypes().FirstOrDefault(t => $"{t.Namespace}.{t.Name}".Equals(namespaceQualifiedTypeName));
        }

        private void Message(string messageFormat, Action<string> output, params string[] args)
        {
            string message = string.Format(messageFormat, args);
            OutLineFormat(messageFormat, ConsoleColor.Cyan, args);
            output(message);
        }
    }
}