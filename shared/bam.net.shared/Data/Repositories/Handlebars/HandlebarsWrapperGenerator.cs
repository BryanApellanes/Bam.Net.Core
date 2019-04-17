using Bam.Net.Presentation.Handlebars;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;

namespace Bam.Net.Data.Repositories.Handlebars
{
    public class HandlebarsWrapperGenerator : WrapperGenerator
    {
        public HandlebarsWrapperGenerator()
        {
        }

        public HandlebarsWrapperGenerator(string wrapperNamespace, string daoNamespace) : base(wrapperNamespace, daoNamespace)
        {
            HandlebarsDirectory = new HandlebarsDirectory("./Templates");
            HandlebarsEmbeddedResources = new HandlebarsEmbeddedResources(this.GetType().Assembly);
        }

        public HandlebarsDirectory HandlebarsDirectory { get; set; }
        public HandlebarsEmbeddedResources HandlebarsEmbeddedResources { get; set; }

        object _generateLock = new object();
        public override GeneratedAssemblyInfo GenerateAssembly()
        {
            lock (_generateLock)
            {
                RoslynCompiler compiler = new RoslynCompiler();
                Assembly assembly = compiler.CompileAssembly($"{WrapperNamespace}.Wrapper.dll", new System.IO.DirectoryInfo(WriteSourceTo));
                GeneratedAssemblyInfo result = new GeneratedAssemblyInfo($"{WrapperNamespace}.Wrapper.dll", assembly);
                result.Save();
                return result;
            }
        }

        public override void WriteSource(string writeSourceDir)
        {
            WriteSourceTo = writeSourceDir;
            foreach (Type type in TypeSchema.Tables)
            {
                HandlebarsWrapperModel model = new HandlebarsWrapperModel(type, TypeSchema, WrapperNamespace, DaoNamespace);
                model.Renderer = new HandlebarsTemplateRenderer(HandlebarsEmbeddedResources, HandlebarsDirectory);
                string fileName = "{0}Wrapper.cs"._Format(type.Name.TrimNonLetters());
                using (StreamWriter sw = new StreamWriter(Path.Combine(writeSourceDir, fileName)))
                {
                    sw.Write(model.Render());
                }
            }
        }
    }
}
