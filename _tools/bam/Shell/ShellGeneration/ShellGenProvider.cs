using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using Bam.Net;
using Bam.Net.Data;
using Bam.Net.Data.Repositories;
using Bam.Net.Presentation.Handlebars;
using Bam.Net.Testing;
using Bam.Shell.ShellGeneration.Configuration;
using Bam.Shell.ShellGeneration.Data;
using Bam.Templates.Shell.Models;

namespace Bam.Shell.ShellGeneration
{
    public class ShellGenProvider: ShellProvider
    {
        public ShellGenProvider()
        {
            ShellGenerationDatabase = DataProvider.Current.GetAppDatabaseFor(ProcessApplicationNameProvider.Current, this);
            ShellGenerationRepository = new DaoRepository(ShellGenerationDatabase);
            ShellGenerationRepository.BaseNamespace = typeof(ShellDescriptor).Namespace;
            ShellGenerationRepository.RequireCuid = true;
            ShellGenerationRepository.AddType<ShellDescriptor>();
        }

        public Config Config => Config.Current;

        public Database ShellGenerationDatabase { get; private set; }
        public DaoRepository ShellGenerationRepository { get; private set; }

        public override void RegisterArguments(string[] args)
        {
            base.RegisterArguments(args);
            AddValidArgument("baseNamespace", "When generating a shell provider, the namespace where the base type is found.");
            AddValidArgument("baseTypeName", "When generating a shell provider, the name of the base type to delegate.");
            AddValidArgument("methodNames", "When generating a shell provider, a comma separated list of method names.");
            AddValidArgument("generateBaseType", true,false,  "When generating a shell provider, if specified, will generate source for the base type instead of searching for it.");
            AddValidArgument("delegatorNamespace", "When generating a shell provider, specifies the namespace to write generated types to.");
            AddValidArgument("writeTo", "When generating a shell provider, specifies where to write source code.");
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
                HashSet<FileInfo> codeFiles = new HashSet<FileInfo>();
                HandlebarsTemplateRenderer renderer = new HandlebarsTemplateRenderer(new HandlebarsEmbeddedResources(GetType().Assembly));
                RoslynCompiler compiler = new RoslynCompiler();
                ShellGenConfig config = GetConfig();
                
                Type baseType = null;
                if (!Arguments.Contains("generateBaseType"))
                {
                    Message("Searching current AppDomain for specified base type ({0})", output, config.BaseTypeName);
                    foreach (Assembly assembly in AppDomain.CurrentDomain.GetAssemblies())
                    {
                        baseType = FindType(assembly, config.BaseTypeNamespace, config.BaseTypeName);
                        if (baseType != null)
                        {
                            Message("Found base type ({0}) in assembly ({1})", output, config.BaseTypeName, assembly.FullName);
                            break;
                        }
                    }
                }
                
                CommandLineDelegatedClassModel model = CommandLineDelegatedClassModel.Create(config.ConcreteTypeName, config.BaseTypeName);
                model.Namespace = config.Namespace;
                SetConcreteModelProperties(model.ConcreteClass, config);
                
                ShellDescriptor descriptor = new ShellDescriptor();
                if (baseType != null)
                {
                    descriptor = new ShellDescriptor(baseType);
                    model.SetMethods(baseType.GetMethods().Where(m => m.IsAbstract).Select(m => m.Name).ToArray());
                }
                else
                {
                    model.SetMethods(GetArgument("methodNames",
                            "Please enter the method names to define on the base type (comma separated).")
                        .DelimitSplit(","));

                    compiler.AddAssemblyReference(typeof(IRegisterArguments));
                    EnsureBaseType(config.BaseTypeName, config.WriteTo, renderer, model, codeFiles);
                }
                model.SetBaseTypeName(config.BaseTypeName);
                
                GenerateDelegator(config.WriteTo, model.DelegatorClass, renderer, codeFiles);

                GenerateConcreteType(config.WriteTo, config.ConcreteTypeName, renderer, model.ConcreteClass, codeFiles);

                byte[] delegatorAssembly = compiler.Compile(config.ConcreteTypeName, codeFiles.ToArray());
                
                descriptor = ShellGenerationRepository.Save(descriptor);
                ShellWrapperAssembly wrapperAssembly = new ShellWrapperAssembly {ShellDescriptorKey = descriptor.Key(), Base64Assembly = delegatorAssembly.ToBase64()};
                ShellGenerationRepository.Save(wrapperAssembly);
            }
            catch (Exception ex)
            {
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

        private ShellGenConfig GetConfig()
        {
            return new ShellGenConfig()
            {
                BaseTypeNamespace = GetArgument("baseNamespace", "Please enter the namespace the base type is in (or where to write it if it doesn't exist)."),
                BaseTypeName = GetArgument("baseTypeName", "Please enter the name of the base type."),
                ConcreteTypeName = GetArgument("concreteTypeName", "Please enter the name of the concreteType"),
                Namespace = GetArgument("delegatorNamespace",  "Please enter the namespace to write shell delegator code to."),
                WriteTo = GetArgument("writeTo", "Please enter the path to the directory to write source code to.")
            };
        }
        
        private Type FindType(Assembly assembly, string baseTypeNamespace, string baseTypeName)
        {
            return assembly.GetTypes().FirstOrDefault(t => $"{t.Namespace}.{t.Name}".Equals($"{baseTypeNamespace}.{baseTypeName}"));
        }

        private void Message(string messageFormat, Action<string> output, params string[] args)
        {
            string message = string.Format(messageFormat, args);
            output(message);
        }
        
        private static void SetDelegatorModelProperties(DelegatorClassModel model, ShellGenConfig config)
        {
            model.NameSpace = config.Namespace;
            model.BaseTypeName = config.BaseTypeName;
        }
        
        private static void SetConcreteModelProperties(ConcreteClassModel model, ShellGenConfig config)
        {
            model.NameSpace = config.Namespace;
            model.ConcreteTypeName = config.ConcreteTypeName;
            model.BaseTypeNamespace = config.BaseTypeNamespace;
            model.BaseTypeName = config.BaseTypeName;
        }
        
        private static void GenerateConcreteType(string writeTo, string concreteTypeName, HandlebarsTemplateRenderer renderer,
            ConcreteClassModel model, HashSet<FileInfo> codeFiles)
        {
            FileInfo concreteTypeCodeFile = new FileInfo(Path.Combine(writeTo, $"{concreteTypeName}.cs"));
            string concreteTypeCode = renderer.Render("ConcreteClass", model);
            concreteTypeCode.SafeWriteToFile(concreteTypeCodeFile.FullName, true);
            codeFiles.Add(concreteTypeCodeFile);
        }

        private static void GenerateDelegator(string writeTo, DelegatorClassModel model,
            HandlebarsTemplateRenderer renderer, HashSet<FileInfo> codeFiles)
        {
            FileInfo delegatorTypeCodeFile =
                new FileInfo(Path.Combine(writeTo, $"{model.BaseTypeName}Delegator.cs"));
            string delegatorCode = renderer.Render("Delegator", model);
            delegatorCode.SafeWriteToFile(delegatorTypeCodeFile.FullName, true);
            codeFiles.Add(delegatorTypeCodeFile);
        }

        private static void EnsureBaseType(string baseTypeName, string writeTo, HandlebarsTemplateRenderer renderer,
            CommandLineDelegatedClassModel model, HashSet<FileInfo> codeFiles)
        {
            FileInfo baseTypeCodeFile = new FileInfo(Path.Combine(writeTo, $"{baseTypeName}.cs"));
            string baseTypeCode = renderer.Render("ProviderBaseClass", model.ProviderBaseClass);
            baseTypeCode.SafeWriteToFile(baseTypeCodeFile.FullName, true);
            codeFiles.Add(baseTypeCodeFile);
        }

    }
}